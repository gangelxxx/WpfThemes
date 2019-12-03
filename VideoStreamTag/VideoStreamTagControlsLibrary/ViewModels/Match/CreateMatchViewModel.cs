using System;
using System.Threading.Tasks;
using ControlsLibrary.Helpers;
using ControlsLibrary.VirtualList.Interface;
using Uniso.InStat.ClickAndTag;
using Uniso.InStat.Web;
using VideoStreamTagControlsLibrary.Providers;
using VirtlistLib;

namespace VideoStreamTagControlsLibrary.ViewModels.Match
{
    public class CreateMatchViewModel : ViewModelBase, ICreateMatchViewModel
    {
        public CreateMatchViewModel()
        {
            _provider = new TeamDataProvider();
            HomeTeams = new VirtualList<VirtualListItem>(_provider, 20);
            AwayTeams = new VirtualList<VirtualListItem>(_provider, 20);
        }

        private bool _gender;
        private TeamDataProvider _provider;
        private VirtualListItem _selectedHomeTeam;
        private VirtualListItem _selectedAwayTeam;
        private string _textHomeTeam;

        public VirtualList<VirtualListItem> HomeTeams { get; }
        public VirtualList<VirtualListItem> AwayTeams { get; }

        public bool Gender
        {
            get => _gender;
            set
            {
                SetProperty(ref _gender, value);

                _provider.Gender = value ? 1 : 2;
                HomeTeams.OwnerUpdateAction?.Invoke();
            }
        }

        public VirtualListItem SelectedHomeTeam
        {
            get => _selectedHomeTeam;
            set => SetProperty(ref _selectedHomeTeam, value);
        }

        public VirtualListItem SelectedAwayTeam
        {
            get => _selectedAwayTeam;
            set => SetProperty(ref _selectedAwayTeam, value);
        }

        public string TextHomeTeam
        {
            get => _textHomeTeam;
            set => SetProperty(ref _textHomeTeam, value); 
        }

        public void SetTeam(TeamData team)
        {
            TextHomeTeam = team.Name;
        }

        public async Task<int?> CreateMatchAsync()
        {
            if (SelectedHomeTeam != null && SelectedAwayTeam != null)
            {
                return await Task<int>.Factory.StartNew(() =>
                    MsSqlService.CreateMatchForClkTag(DateTime.Now, 2926, 1, true, 1, 999999, 45,
                        SelectedHomeTeam.Idx, SelectedAwayTeam.Idx, 1));
            }

            return null;
        }
    }
}