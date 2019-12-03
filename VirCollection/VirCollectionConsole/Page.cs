using System.Collections.Generic;

namespace VirCollectionConsole
{
    public class Page<T> where T: IVirtItem
    {
        public int Idx;
        public IList<IVirtItem> Items;

        public T GetItem(int i)
        {
            if (i < Items.Count)
            {
                return (T) Items[i];
            }

            return default(T);
        }

    }
}