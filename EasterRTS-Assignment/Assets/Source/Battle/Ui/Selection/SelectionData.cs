using EasterRts.Battle.World;

namespace EasterRts.Battle.Ui.Selection {

    public class SelectionData {

        private readonly UnitEntity _selected;
        public UnitEntity Unit => _selected;

        public SelectionData() {
            _selected = null;
        }

        public SelectionData(UnitEntity unit) {
            _selected = unit;
        }
    }
}
