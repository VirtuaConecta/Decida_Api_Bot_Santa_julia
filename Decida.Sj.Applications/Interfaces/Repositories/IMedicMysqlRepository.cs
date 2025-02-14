using Decida.Sj.Core.Entities;
using Decida.Sj.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Interfaces.Repositories
{
    public interface IMedicMysqlRepository
    {
        Task<List<MedicEntity>> GetMedListRepsitory();
    }
}
