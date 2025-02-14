using Decida.Sj.Applications.Interfaces.Repositories;
using Decida.Sj.Applications.Interfaces.Services;
using Decida.Sj.Core.Entities;
using Decida.Sj.Core.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decida.Sj.Infrastructure.Services
{
    public class MedicalSpecialtyServices : IMedicalSpecialtyServices
    {
        
        private readonly IMSpecialtyMysqlRepository _medSpec;

        public MedicalSpecialtyServices(IMSpecialtyMysqlRepository medSpec)
        {

            _medSpec = medSpec;
        }
        public async Task<List<MedicalSpecialtyEntity>> GetHMedicalSpecialtyListAsyncService()
        {
            var MedSpecialPlanRepoReturn = new List<MedicalSpecialtyEntity>();

            try
            {
                MedSpecialPlanRepoReturn = await _medSpec.GetMedSpecListRepsitory();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return MedSpecialPlanRepoReturn;
        }
    }
}
