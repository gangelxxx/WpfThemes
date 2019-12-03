using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ControlsLibrary.Helpers;
using ControlsLibrary.VirtualList.Interface;
using Uniso.InStat.ClickAndTag;
using Uniso.InStat.Web;
using VideoStreamTagControlsLibrary.Providers;
using VirtlistLib;

namespace VideoStreamTagControlsLibrary.ViewModels.Match
{
    public class SelectMatchViewModel : ViewModelBase
    {
        private readonly IContext _context;
        private TeamDataProvider _provider;
        private VirtualListItem _selectedMatch;
        private bool _isMatchSearch;
        private bool _isMatchNotFound;
        private bool _isMatchFound;
        private bool _gender;
        private ObservableCollection<Models.MatchData> _foundMatches = new ObservableCollection<Models.MatchData>();
        private bool _isCreateMatch;
        private VirtualListItem _selectedTeam;

        public SelectMatchViewModel(IContext context)
        {
            _context = context;

            _provider = new TeamDataProvider();
            Teams = new VirtualList<VirtualListItem>(_provider, 20);

            Gender = true;

            IsMatchSearch = false;
            IsMatchFound = false;

//            List<MatchData> lst = new List<MatchData>();
//
//            lst.Add(new MatchData() {FirstTeamNameRus = "11", FirstTeamNameEng = "11" });
//            lst.Add(new MatchData() {FirstTeamNameRus = "22", FirstTeamNameEng = "22" });
//            lst.Add(new MatchData() {FirstTeamNameRus = "33", FirstTeamNameEng = "33" });
//            lst.Add(new MatchData() {FirstTeamNameRus = "44", FirstTeamNameEng = "44" });
//            lst.Add(new MatchData() {FirstTeamNameRus = "55", FirstTeamNameEng = "55" });
//            //                    lst.Add(new MatchData() {FirstTeamNameRus = "3333"});
//            //                    lst.Add(new MatchData() {FirstTeamNameRus = "4444"});
//            //                    lst.Add(new MatchData() {FirstTeamNameRus = "5555"});
//            //                    lst.Add(new MatchData() {FirstTeamNameRus = "6666"});
//
//
//            foreach (var item in lst)
//            {
//                FoundMatches.Add(item);
//            }
        }

        public VirtualList<VirtualListItem> Teams { get; }

        public VirtualListItem SelectedMatch
        {
            get => _selectedMatch;
            set => SetProperty(ref _selectedMatch, value);
        }

        public bool IsMatchSearch
        {
            get => _isMatchSearch;
            set => SetProperty(ref _isMatchSearch, value);
        }

        public bool IsMatchFound
        {
            get => _isMatchFound;
            set => SetProperty(ref _isMatchFound, value);
        }

        public bool Gender
        {
            get => _gender;
            set
            {
                SetProperty(ref _gender, value);

                _provider.Gender = value ? 1 : 2;
                Teams.OwnerUpdateAction?.Invoke();
            }
        }

        public ObservableCollection<Models.MatchData> FoundMatches
        {
            get => _foundMatches;
            set => SetProperty(ref _foundMatches, value);
        }

        public VirtualListItem SelectedTeam
        {
            get => _selectedTeam;
            set
            {
                _selectedTeam = value;

                if (value == null)
                {
                    return;
                }

                SearchMatchAsync(value);
            }
        }

        private async void SearchMatchAsync(VirtualListItem val)
        {
            try
            {
                IsMatchSearch = true;

                var lst = await MsSqlService.SearchMatchsForClkTagAsync(val.Idx, DateTime.Now);

//              List < MatchData > lst = new List<MatchData>();
//              lst.Add(new MatchData() {FirstTeamNameRus = "1111", FirstTeamNameEng = "1122"});
//              lst.Add(new MatchData() {FirstTeamNameRus = "2222"});
//              lst.Add(new MatchData() {FirstTeamNameRus = "3333"});
//              lst.Add(new MatchData() {FirstTeamNameRus = "4444"});
//              lst.Add(new MatchData() {FirstTeamNameRus = "5555"});
//              lst.Add(new MatchData() {FirstTeamNameRus = "6666"});


                if (lst.Count <= 0)
                {
                    IsMatchFound = false;
                }

                _context.Invoke(() =>
                {
                    FoundMatches.Clear();

                    foreach (var item in lst)
                    {
                        var itemMatchData = new Models.MatchData(item.MatchId, item.MatchString, item);
                        itemMatchData.UpdateSelected += ModelMatchDataOnUpdateSelected;
                        FoundMatches.Add(itemMatchData);
                    }

                    var matchData = new MatchData(){MatchId = -1, FirstTeamNameEng = "Create a match", FirstTeamNameRus = "Создать матч"};
                    var createMatchData = new Models.MatchData(matchData.MatchId, matchData.MatchString, null);
                    createMatchData.UpdateSelected += ModelMatchDataOnUpdateSelected;

                    FoundMatches.Add(createMatchData);
                });

                IsMatchFound = true;
            }
            finally
            {
                IsMatchSearch = false;
            }
        }

        private void ModelMatchDataOnUpdateSelected(object sender, EventArgs e)
        {
            if (sender is Models.MatchData matchData)
            {
                if (matchData.Id == -1 && matchData.IsSelected)
                {
                    foreach (var foundMatch in FoundMatches)
                    {
                        if (foundMatch.Id != -1)
                        {
                            foundMatch.IsSelected = false;
                        }
                    }   
                }

                if (matchData.Id != -1 && matchData.IsSelected)
                {
                    foreach (var foundMatch in FoundMatches)
                    {
                        if (foundMatch.Id == -1)
                        {
                            foundMatch.IsSelected = false;
                            break;
                        }
                    }
                }
            }
        }
    }
}