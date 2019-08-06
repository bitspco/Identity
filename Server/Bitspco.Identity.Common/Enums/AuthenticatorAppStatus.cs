using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitspco.Identity.Common.Enums
{
    public enum AuthenticatorAppStatus
    {
        [Description("نا مشخص")]
        None = 0,
        [Description("غیر معتبر")]
        Deny = 1,
        [Description("معتبر")]
        Allow = 2,
    }
}
