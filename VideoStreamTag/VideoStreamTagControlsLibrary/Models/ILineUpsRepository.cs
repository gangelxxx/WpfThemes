using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoStreamTagControlsLibrary.Models
{
    public interface ILineUpsRepository
    {
        Task<List<ILineUpsTable>> GetAll();

        Task<List<ILineUpsTable>> GetAllByTeamId(int teamId);

        Task<List<ILineUpsTable>> GetAllByTeamAndMatchId(int teamId, int matchId);

        Task InsertLineUps(ILineUpsTable lineUps);

        Task DeleteLineUps(ILineUpsTable lineUps);

        Task UpdateLineUps(ILineUpsTable lineUps);

        Task UpdateLineUps(IEnumerable<ILineUpsTable> lineUps);

        ILineUpsTable New(int matchId, int teamId, int playerId, int number);
    }
}
