using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ControlsLibrary.Helpers;
using ControlsLibrary.VirtualList.Interface;
using Uniso.InStat;
using Uniso.InStat.ClickAndTag;
using Uniso.InStat.Football;
using Uniso.InStat.Web;
using VideoStreamTagControlsLibrary.Providers;
using VirtlistLib;

namespace VideoStreamTagControlsLibrary.ViewModels.EditLineUps
{
    public class EditLineUpsViewModel : ViewModelBase, IEditLineUpsViewModel
    {
        private Team _team;

        private bool _isVisibleSearchPlayers = false;
        public bool IsVisibleSearchPlayers
        {
            get => _isVisibleSearchPlayers;
            set => SetProperty(ref _isVisibleSearchPlayers, value);
        }

        private string _teamName;
        public string TeamName
        {
            get => _teamName;
            set => SetProperty(ref _teamName, value);
        }

        private ObservableCollection<Player> _playerList = new ObservableCollection<Player>();
        public ObservableCollection<Player> PlayerList
        {
            get => _playerList;
            set => SetProperty(ref _playerList, value);
        }

        private ICommand _addPlayerCommand;
        public ICommand AddPlayerCommand
        {
            get => _addPlayerCommand;
            set => SetProperty(ref _addPlayerCommand, value);
        }

        private bool _isSelectedPlayer = false;
        public PlayerData SelectedSearchPlayer { get; set; }

        private ObservableCollection<PlayerData> _foundPlayers = new ObservableCollection<PlayerData>();
        public ObservableCollection<PlayerData> FoundPlayers
        {
            get => _foundPlayers;
            set => SetProperty(ref _foundPlayers, value);
        }

        private bool _isLoadingPlayers = false;
        public bool IsLoadingPlayers
        {
            get => _isLoadingPlayers;
            set => SetProperty(ref _isLoadingPlayers, value);
        }

        private int _selectedFoundPlayerIndex = -1;
        public int SelectedFoundPlayerIndex
        {
            get { return _selectedFoundPlayerIndex; }
            set
            {
                if (!_isSelectedPlayer)
                {
                    SelectedSearchPlayer = (value >= 0 && value < FoundPlayers.Count) ? FoundPlayers.OrderBy(x => x.FullName).ToList()[value] : null;
                    SetProperty(ref _selectedFoundPlayerIndex, value);
                    _isSelectedPlayer = true;
                }
            }
        }

        private string _searchPlayerLast;
        private string _searchPlayer;
        public string SearchPlayer
        {
            get { return _searchPlayer; }
            set
            {
                if (!_isSelectedPlayer)
                    FoundPlayers.Clear();

                SetProperty(ref _searchPlayer, value);

                if (!_isLoadingPlayers)
                {
                    if (!_isSelectedPlayer)
                        DoSearchPlayer(value);
                    else
                        _isSelectedPlayer = false;
                }
            }
        }

        #region constructor

        public EditLineUpsViewModel()
        {
        }

        #endregion

        #region methods
        public void SetTeam(Team team)
        {
            PlayerList = new ObservableCollection<Player>(team.Default);
            TeamName = team.Name;
            _team = team;
        }

        public void AddPlayerCommandExecute()
        {
            if (SelectedSearchPlayer != null && _team.Default.All(o => o.Id != SelectedSearchPlayer.Id))
            {
                Team _other = _team == Game.CurrentGame.Match.Team1Native
                    ? Game.CurrentGame.Match.Team2Native
                    : Game.CurrentGame.Match.Team1Native;

                if (_other.Default.Any(o => o.Id == SelectedSearchPlayer.Id))
                {
                    // "Player {FoundPlayer} already exists in team {_other.Name}",
                    return;
                }

                Player p = MsSqlService.GetPlayer(SelectedSearchPlayer.Id, _team);
                if (p != null)
                {
                    PlayerList.Add(p);
                    _team.Default.Add(p);
                    SelectedSearchPlayer = null;
                    FoundPlayers.Clear();
                }
            }
        }

        public void DoSearchPlayer(string str)
        {
            if (str.Length < 4)
                return;

            Render.Execute(() =>
            {
                IsLoadingPlayers = true;
            });

            Task.Factory.StartNew(() =>
            {
                try
                {
                    _searchPlayerLast = str;

                    var teams = MsSqlService.SearchPlayerForClkTag(str);
                    Render.Execute(() =>
                    {
                        if (teams != null && teams.Count > 0)
                        {
                            foreach (var team in teams)
                            {
                                FoundPlayers.Add(team);
                            }
                            IsVisibleSearchPlayers = true;
                        }
                        IsLoadingPlayers = false;

                        if (_searchPlayerLast != SearchPlayer)
                        {
                            FoundPlayers.Clear();
                            DoSearchPlayer(SearchPlayer);
                        }
                    });
                }
                catch { }
                finally
                {
                    Render.Execute(() =>
                    {
                        IsLoadingPlayers = false;
                    });
                }
            });
        }

        #endregion
    }
}
