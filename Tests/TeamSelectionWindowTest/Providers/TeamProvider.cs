using System.Collections.Concurrent;
using TeamSelectionWindowTest.Model;

namespace TeamSelectionWindowTest.Providers
{
    public class TeamProvider : WaitComboProvider<Team>
    {
        public override void CreateMainList()
        {
            MainList = new ConcurrentStack<Team>();

            for (int i = 1000; i < 2000; i++)
            {
                MainList.Push(new Team(i.ToString()));
            }
        }
    }
}