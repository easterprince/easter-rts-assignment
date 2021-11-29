using EasterRts.Battle.Data.Map;
using EasterRts.Common.Cores;
using EasterRts.Utilities;
using EasterRts.Utilities.Collections;
using EasterRts.Utilities.FixedFloats;
using System;
using System.Linq;
using UnityEngine;
using VoronoiLib;
using VoronoiLib.Structures;

namespace EasterRts.Battle.Generation {

    public class MapGenerator {

        public Vector2Int Size { get; set; }
        public int SegmentSize { get; set; }

        public MapData Generate(int? seed = null) {

            if (Size.x % SegmentSize != 0 || Size.y % SegmentSize != 0) {
                throw new InvalidOperationException($"{nameof(Size)} must be multiple of {nameof(SegmentSize)}!");
            }
            if (Size.x < 0 || Size.y < 0) {
                throw new InvalidOperationException($"{nameof(Size)} must have non-negative components!");
            }

            var data = new MapData();
            var random = new System.Random(seed ?? 0);

            // Generate generative points.
            data.Size = Size;
            data.SiteSquareSize = SegmentSize;
            var coreId = new CoreId(200000);
            var sites = new Array2<MapSiteData>(Size / SegmentSize);
            foreach (var index in sites.Indexes) {
                var traversable = random.NextDouble() < 0.8 && (index.x != 0 && index.y != 0 && index.x != sites.Size.x - 1 && index.y != sites.Size.y - 1);
                var x = FixedSingle.FromThousandths(100 + random.Next(800 + 1));
                var y = FixedSingle.FromThousandths(100 + random.Next(800 + 1));
                var point = new FixedVector2(x, y);
                var site = new MapSiteData() {
                    Id = coreId,
                    InternalPoint = point,
                    Traversable = traversable,
                };
                sites[index] = site;
                coreId = coreId.GetNext();
            }

            // Generate site transitions.
            var fortuneSites = sites.Indexes
                .Select(index => {
                    var site = sites[index];
                    var sitePosition = site.GetPointPosition(index, SegmentSize);
                    var fortuneSite = new FortuneSite((float) sitePosition.X, (float) sitePosition.Y);
                    return fortuneSite;
                })
                .ToArray2(sites.Size);
            var fortuneSiteToIndex = fortuneSites.Indexes
                .ToDictionary(index => fortuneSites[index]);
            var fortuneEdges = FortunesAlgorithm.Run(fortuneSites.ToList(), 0, 0, Size.x, Size.y);
            foreach (var edge in fortuneEdges) {
                
                var leftIndex = fortuneSiteToIndex[edge.Left];
                var rightIndex = fortuneSiteToIndex[edge.Right];
                var leftSite = sites[leftIndex];
                var rightSite = sites[rightIndex];
                var leftSitePosition = leftSite.GetPointPosition(leftIndex, SegmentSize);
                var rightSitePosition = rightSite.GetPointPosition(rightIndex, SegmentSize);

                var distance = (leftSitePosition - rightSitePosition).Magnitude;
                var edgeStart = (FixedVector2) new Vector2((float) edge.Start.X, (float) edge.Start.Y);
                var edgeEnd = (FixedVector2) new Vector2((float) edge.End.X, (float) edge.End.Y);
                var edgeSegment = new FixedSegment2(edgeStart, edgeEnd);
                
                leftSite.Transitions.Add(new MapSiteData.Transition {
                    neighbour = rightIndex,   
                    distance = distance,
                    edge = edgeSegment,
                });
                rightSite.Transitions.Add(new MapSiteData.Transition {
                    neighbour = leftIndex,
                    distance = distance,
                    edge = edgeSegment
                });
            }

            data.Sites = sites;

            return data;
        }
    }
}
