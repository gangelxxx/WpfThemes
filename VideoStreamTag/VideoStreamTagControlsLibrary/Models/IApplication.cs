using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoStreamTagControlsLibrary.Models
{
    public interface IApplication
    {
        ILineUpsRepository LineUpsRepository { get; }
    }
}
