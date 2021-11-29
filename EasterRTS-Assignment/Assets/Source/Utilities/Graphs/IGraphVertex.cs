using System.Collections.Generic;

namespace EasterRts.Utilities.Graphs {
    
    public interface IGraphVertex<TVertex, TArc>
        where TVertex : class, IGraphVertex<TVertex, TArc>
        where TArc : IGraphArc<TVertex> {

        IEnumerable<TArc> Adjacent { get; }
    }
}
