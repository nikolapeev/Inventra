using Inventra.Core.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Core.Contracts
{
    public interface IDashboardService
    {
        Task<HomeStatsViewModel> GetHomeStatsAsync();
    }
}
