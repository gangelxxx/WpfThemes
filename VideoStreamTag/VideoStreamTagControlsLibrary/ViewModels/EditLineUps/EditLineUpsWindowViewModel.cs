using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ControlsLibrary.Helpers;
using Uniso.InStat;
using Uniso.InStat.Exceptions;
using Uniso.InStat.Football;
using Uniso.InStat.Web;
using VideoStreamTagControlsLibrary.Models;
using User = Uniso.InStat.User;

namespace VideoStreamTagControlsLibrary.ViewModels.EditLineUps
{
    public class EditLineUpsWindowViewModel : ViewModelBase, IEditLineUpsWindowViewModel
    {
        private int _matchId = -1;

        private List<ILineUpsTable> _lineUpsFirstLocal = new List<ILineUpsTable>();
        private List<ILineUpsTable> _lineUpsSecondLocal = new List<ILineUpsTable>();
        
        private ILineUpsRepository _repository = null;

        private bool _closeWindow = false;
        public bool CloseWindow
        {
            get => _closeWindow;
            set => SetProperty(ref _closeWindow, value);
        }

        private bool _isEnabledUI = true;
        public bool IsEnabledUI
        {
            get => _isEnabledUI;
            set => SetProperty(ref _isEnabledUI, value);
        }

        private bool _isLoadingLineUps = false;
        public bool IsLoadingLineUps
        {
            get => _isLoadingLineUps;
            set => SetProperty(ref _isLoadingLineUps, value);
        }

        private ICommand _onLoadedCommand;
        public ICommand OnLoadedCommand
        {
            get => _onLoadedCommand;
            set => SetProperty(ref _onLoadedCommand, value);
        }

        private EditLineUpsViewModel _lineUpsFirstTeam = null;
        public EditLineUpsViewModel LineUpsFirstTeam
        {
            get => _lineUpsFirstTeam;
            set => SetProperty(ref _lineUpsFirstTeam, value);
        }

        private EditLineUpsViewModel _lineUpsSecondTeam = null;
        public EditLineUpsViewModel LineUpsSecondTeam
        {
            get => _lineUpsSecondTeam;
            set => SetProperty(ref _lineUpsSecondTeam, value);
        }

        public EditLineUpsWindowViewModel(ILineUpsRepository repository, int matchId)
        {
            LineUpsFirstTeam = new EditLineUpsViewModel();
            LineUpsSecondTeam = new EditLineUpsViewModel();

            _repository = repository;
            _matchId = matchId;
        }

        public void CompleteCommandExecute()
        {
            foreach (var pl in _lineUpsFirstLocal)
            {
                _repository.DeleteLineUps(pl);
            }
            foreach (var pl in _lineUpsSecondLocal)
            {
                _repository.DeleteLineUps(pl);
            }

            var match = Game.CurrentGame.Match;
            foreach (var pl in match.Team1Native.Default)
            {
                _repository.InsertLineUps(_repository.New(match.Id, match.Team1Native.Id, pl.Id, pl.Number));
            }
            foreach (var pl in Game.CurrentGame.Match.Team2Native.Default)
            {
                _repository.InsertLineUps(_repository.New(match.Id, match.Team2Native.Id, pl.Id, pl.Number));
            }

            CloseWindow = true;
        }

        public async void LoadLineUps()
        {
            IsLoadingLineUps = true;
            IsEnabledUI = false;

            try
            {
                var m = await MsSqlService.GetMatchAsync(null, _matchId);
                if (m == null)
                    throw new NoGameException();

                //ShowStatus(LangHelper.GetString("TEXT_LOADING_TEAMS"), 0);
                await MsSqlService.GetTeamAsync(m);

                if (m.Team1Native == null || m.Team1Native.Id == 0)
                    throw new NotFoundTeamException(1);
                if (m.Team2Native == null || m.Team2Native.Id == 0)
                    throw new NotFoundTeamException(2);

                //ShowStatus(LangHelper.GetString("TEXT_LOADING_PLAYERS_FOR_TEAM", m.Team2Native), 0);
                await MsSqlService.GetPlayersAsync(m.Team2Native);

                //ShowStatus(LangHelper.GetString("TEXT_LOADING_PLAYERS_FOR_TEAM", m.Team1Native), 0);
                await MsSqlService.GetPlayersAsync(m.Team1Native);

                Game.CurrentGame = new FootballStd { Match = m };

                User.Current = new User
                {
                    SourceType = SourceTypeEnum.LineUps
                };

                _lineUpsFirstLocal = _repository.GetAllByTeamAndMatchId(Game.CurrentGame.Match.Team1Native.Id, Game.CurrentGame.Match.Id).Result;
                _lineUpsSecondLocal = _repository.GetAllByTeamAndMatchId(Game.CurrentGame.Match.Team2Native.Id, Game.CurrentGame.Match.Id).Result;

                foreach (var pl in _lineUpsFirstLocal)
                {
                    Player player = Game.CurrentGame.Match.Team1Native.Default.FirstOrDefault(x => x.Id == pl.PlayerId);
                    if (player != null && player.Number != pl.Number)
                        player.Number = pl.Number;
                }

                foreach (var pl in _lineUpsSecondLocal)
                {
                    Player player = Game.CurrentGame.Match.Team2Native.Default.FirstOrDefault(x => x.Id == pl.PlayerId);
                    if (player != null && player.Number != pl.Number)
                        player.Number = pl.Number;
                }

                Render.Execute(() =>
                {
                    LineUpsFirstTeam.SetTeam(Game.CurrentGame.Match.Team1Native);
                    LineUpsSecondTeam.SetTeam(Game.CurrentGame.Match.Team2Native);
                });


            }
            catch { }
            finally
            {
                Render.Execute(() =>
                {
                    IsLoadingLineUps = false;
                    IsEnabledUI = true;
                });
            }
        }
    }
}