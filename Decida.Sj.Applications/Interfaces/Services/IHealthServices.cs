using Decida.Sj.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Interfaces.Services
{
    public interface IHealthServices
    {
        Task<List<HealthPlanEntity>> GetHealthPlanListAsyncService();
    }
}
