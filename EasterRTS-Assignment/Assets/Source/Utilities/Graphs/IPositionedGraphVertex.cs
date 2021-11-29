using EasterRts.Utilities.FixedFloats;

namespace EasterRts.Utilities.Graphs {
    
    public interface IPositionedGraphVertex<TVertex, TArc> : IGraphVertex<TVertex, TArc>
        where TVertex : class, IPositionedGraphVertex<TVertex, TArc>
        where TArc : IGraphArc<TVertex> {

        FixedSingle EstimateDistance(TVertex other);        
    }
}
