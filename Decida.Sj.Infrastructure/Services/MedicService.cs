using Decida.Sj.Applications.Interfaces.Repositories;
using Decida.Sj.Applications.Interfaces.Services;
using Decida.Sj.Core.Entities;
using Decida.Sj.Core.Entity;
using Decida.Sj.Core.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Infrastructure.Services
{
    public class MedicService : IMedicService
    {
        
        private readonly IMedicMysqlRepository _medic;

        public MedicService(IMedicMysqlRepository medic)
        {

            _medic = medic;
        }
        public async Task<List<MedicEntity>> GetMedicListAsyncService()
        {
            var MedicRepoReturn = new List<MedicEntity>();

            try
            {
                MedicRepoReturn = await _medic.GetMedListRepsitory();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return MedicRepoReturn;
        }
    }
}
