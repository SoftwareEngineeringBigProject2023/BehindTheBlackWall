using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEServer.Statements.Domain
{
    public sealed class RankScore
    {
        public string? UserName { get; set; }
        public string? ImagePath { get; set; }
        public long TotalScore { get; set; }
        public long TotalKDA { get; set; }
    }
}
