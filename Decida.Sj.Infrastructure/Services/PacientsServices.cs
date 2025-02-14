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
    public class PacientsServices : IPacientsServices
    {
        private readonly IPacientOracleRepository _pacientRepo;
      

        public PacientsServices(IPacientOracleRepository pacientRepo)
        {
            _pacientRepo = pacientRepo;
        }
        public async Task<PacienteEntity> GetPacientByCpfService(string cpf)
        {
            var PacientRepoReturn = new PacienteEntity();

            try
            {

                 PacientRepoReturn = await _pacientRepo.GetPessoaFisicaByCpfAsync(cpf);

               

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return PacientRepoReturn;
        }
    }
}
