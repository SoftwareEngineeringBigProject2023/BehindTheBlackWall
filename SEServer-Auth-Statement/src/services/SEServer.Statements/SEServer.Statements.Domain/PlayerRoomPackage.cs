using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEServer.Statements.Domain
{
    public sealed class PlayerRoomPackage
    {
        public List<WeaponModule>? Modules { get; set; }
        public long Score { get; set; }
        public long KDA { get; set; }
    }
}
