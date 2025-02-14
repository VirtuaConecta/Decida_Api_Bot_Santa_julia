using Decida.Sj.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Interfaces.Repositories
{
    public interface IMSpecialtyMysqlRepository
    {
        Task<List<MedicalSpecialtyEntity>> GetMedSpecListRepsitory();
        Task<MedicalSpecialtyEntity> GetMedSpecByIdRepsitory(int id);
    }
}
