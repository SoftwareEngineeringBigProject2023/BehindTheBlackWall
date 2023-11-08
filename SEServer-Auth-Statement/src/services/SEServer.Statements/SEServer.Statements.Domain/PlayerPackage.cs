using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEServer.Statements.Domain
{
    public sealed class PlayerPackage
    {
        public bool FirstEnter { get; set; } = true;
        public RankScore? Rank { get; set; }
        public List<WeaponModule>? Modules { get; set; }
    }
}
