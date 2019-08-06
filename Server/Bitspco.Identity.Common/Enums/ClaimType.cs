using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitspco.Identity.Common.Enums
{
    public enum ClaimType
    {
        None = 0,
        Text = 1,
        Number = 2,
        Boolean = 3,
        Date = 4,
        Time = 5,
        DateTime = 6,
        Complex = 10,
    }
}
