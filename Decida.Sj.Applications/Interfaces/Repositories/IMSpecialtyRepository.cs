using Decida.Sj.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Interfaces.Repositories
{
    public interface IMSpecialtyRepository
    {
        Task<List<MedicalSpecialtyEntity>> GetMedSpecListRepository(int? cd_convenio );
        Task<MedicalSpecialtyEntity> GetMedSpecByIdRepository(int id);
    }
}
