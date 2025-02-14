using Decida.Sj.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Interfaces.Services
{
    public interface IMedicService
    {
        Task<List<MedicEntity>> GetMedicListAsyncService();
    }
}
