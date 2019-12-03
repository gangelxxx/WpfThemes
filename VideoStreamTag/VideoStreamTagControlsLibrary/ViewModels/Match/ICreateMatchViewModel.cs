using System.Threading.Tasks;
using Uniso.InStat.ClickAndTag;

namespace VideoStreamTagControlsLibrary.ViewModels.Match
{
    public interface ICreateMatchViewModel
    {
        void SetTeam(TeamData team);
        Task<int?> CreateMatchAsync();
    }
}