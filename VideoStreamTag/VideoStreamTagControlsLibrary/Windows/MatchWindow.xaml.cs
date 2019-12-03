using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlsLibrary.Windows;
using Uniso.InStat.ClickAndTag;
using Uniso.InStat.Web;

namespace VideoStreamTagControlsLibrary.Windows
{
    public partial class MatchWindow : WindowTypeOne
    {
        public MatchWindow()
        {
            InitializeComponent();

//            List<List<TeamData>> listss = new List<List<TeamData>>();
//
//            for (int i = 0; i < 10; i++)
//            {
//                var teams = MsSqlService.SearchTeamsForClkTag("цска", (int)1, 2);
//
//                listss.Add(teams);
//            }
//
//            for (int i = 0; i < listss.Count; i++)
//            {
//                var list1 = listss[i];
//                for (int j = 0; j < listss.Count; j++)
//                {
//                    var list2 = listss[j];
//
//                    if (list1.Count != list2.Count)
//                    {
//                        Console.WriteLine();
//                    }
//
//                    for (int k = 0; k < list1.Count; k++)
//                    {
//                        for (int l = k + 1; l < list1.Count; l++)
//                        {
//                            if (list1[k].Id == list1[l].Id)
//                            {
//                                Console.WriteLine();
//                            }
//                        }
//                    }
//
//                    for (int k = 0; k < list1.Count; k++)
//                    {
//                        if (list1[k].Id != list2[k].Id)
//                        {
//                            Console.WriteLine();
//                        }
//                    }
//                }
//            }


//            GoToSelectMatchState();
//            GoToCreateMatchState();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NSelectMatchView.CreateMatchOnClickEvent += NSelectMatchViewOnCreateMatchOnClickEvent;
        }

        private void NSelectMatchViewOnCreateMatchOnClickEvent(object sender, TeamData team)
        {
            NCreateMatchView.ViewModel.SetTeam(team);
            GoToCreateMatchState();
        }

        private void GoToCreateMatchState()
        {
            VisualStateManager.GoToElementState(grid, "CreateMatchState", true);
        }

        private void GoToSelectMatchState()
        {
            VisualStateManager.GoToElementState(grid, "SelectMatchState", true);
        }

        private void Button11_OnClick(object sender, RoutedEventArgs e)
        {
            NSelectMatchView.GoToShowWaitMatchSearchState();
        }

        private void Button22_OnClick(object sender, RoutedEventArgs e)
        {
            NSelectMatchView.GoToHideWaitMatchSearchState();
        }
    }
}
