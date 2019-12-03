using ThemesLib.InStat.Helpers.VirtualizingCollection;
using ThemesLib.InStat.Providers;

namespace FitnessThemesTest1
{
    public class Customer : IVirtualItem
    {
        public int Id { get; set; }
        public string FindingName { get; set; }

        public Customer(int id, string findingName)
        {
            Id = id;
            FindingName = findingName;
        }

        public override string ToString()
        {
            return FindingName;
        }
    }
}