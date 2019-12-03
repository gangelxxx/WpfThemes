using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoStreamTagControlsLibrary.Models
{
    public interface ILineUpsTable
    {
        int Id { get; set; }

        int PlayerId { get; set; }

        int TeamId { get; set; }

        int Number { get; set; }

        int MatchId { get; set; }
    }
}
