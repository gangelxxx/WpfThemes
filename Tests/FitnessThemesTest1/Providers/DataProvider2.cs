using ThemesLib.InStat.Helpers.VirtualizingCollection;
using ThemesLib.InStat.Providers;
using Uniso.InStat;

namespace FitnessThemesTest1.Providers
{
    public class DataProvider2 : WaitComboProvider<Customer>
    {
        public override void CreateMainList()
        {
            Uniso.InStat.Web.MsSqlService.GetUserList();
            for (var index = 0; index < User.List.Count; index++)
            {
                User item = User.List[index];
                if (!string.IsNullOrEmpty(item.Login))
                {
                    Customer ct = new Customer(index, item.Login);
                    MainList.Add(ct);
                    _filterList.Add(ct);
                }
            }

//            for (var index = 0; index < 10000; index++)
//            {
//                Customer pi = new Customer(index, "asdf");
//                MainList.Add(pi);
//                _filterList.Add(pi);
//            }
        }
    }
}