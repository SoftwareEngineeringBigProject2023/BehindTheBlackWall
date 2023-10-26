using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleDistributor.EntityFrameworkCore
{
    public abstract class OptionsActionWrapper
    {
        public abstract Action<IServiceProvider, DbContextOptionsBuilder>? OptionsAction { get; }

        public virtual ServiceLifetime ContextLifetime { get; } = ServiceLifetime.Scoped;

        public virtual ServiceLifetime OptionsLifetime { get; } = ServiceLifetime.Scoped;
    }
}
