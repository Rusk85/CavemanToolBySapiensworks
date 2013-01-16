namespace CavemanTools.Model
{
    public class LinkedItems<T, V>
    {
        public T Item1 { get; set; }
        public V Item2 { get; set; }

        public LinkedItems(T item1, V item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
}