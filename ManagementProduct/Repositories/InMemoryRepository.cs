namespace ManagementProduct.Repositories
{
    public class InMemoryRepository<T> where T : class
    {
        private readonly List<T> _items = new List<T>();

        public List<T> GetAll() => _items;

        public void Add(T item) => _items.Add(item);

        public void Remove(T item) => _items.Remove(item);

        public void Update(T oldItem, T newItem)
        {
            var index = _items.IndexOf(oldItem);
            if (index >= 0)
                _items[index] = newItem;
        }
    }
}
