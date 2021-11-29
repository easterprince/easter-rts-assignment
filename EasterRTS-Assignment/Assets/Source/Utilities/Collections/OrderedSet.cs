using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EasterRts.Utilities.Collections {
    
    public class OrderedSet<TElement> : ICollection<TElement> {

        // Fields.

        private Dictionary<TElement, LinkedListNode<TElement>> _elements = new Dictionary<TElement, LinkedListNode<TElement>>();
        private LinkedList<TElement> _order = new LinkedList<TElement>();


        // Constructors.

        public OrderedSet() {}

        public OrderedSet(IEnumerable<TElement> other) {
            foreach (var element in other) {
                Add(element);
            }
        }


        // Properties.

        public int Count => _elements.Count;

        public bool IsReadOnly => false;


        // Modification methods.

        public bool Add(TElement element) {
            if (_elements.ContainsKey(element)) {
                return false;
            }
            var node = _order.AddLast(element);
            _elements[element] = node;
            return true;
        }

        void ICollection<TElement>.Add(TElement element) => Add(element);

        public bool Remove(TElement element) {
            if (!_elements.TryGetValue(element, out var node)) {
                return false;
            }
            _elements.Remove(element);
            _order.Remove(node);
            return true;
        }

        public void Clear() {
            _elements.Clear();
            _order.Clear();
        }


        // Enumeration.

        public IEnumerator<TElement> GetEnumerator() => _order.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _order.GetEnumerator();


        // Other methods.

        public bool Contains(TElement element) => _elements.ContainsKey(element);

        public void CopyTo(TElement[] array, int arrayIndex) => _order.CopyTo(array, arrayIndex);
    }
}
