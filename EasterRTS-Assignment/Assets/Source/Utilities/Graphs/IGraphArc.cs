using EasterRts.Utilities.FixedFloats;

namespace EasterRts.Utilities.Graphs {
    
    public interface IGraphArc<TVertex> {

        TVertex To { get; }
        FixedSingle Distance { get; }
    }
}
