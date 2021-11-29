using System;

namespace EasterRts.Common.Cores {
    
    public class IdToCore {

        private ModelBase _model;

        public IdToCore(ModelBase model) {
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public TCore Get<TCore>(CoreId id)
            where TCore : CoreBase<TCore>
            => _model.GetCoreById(id) as TCore;
    }
}
