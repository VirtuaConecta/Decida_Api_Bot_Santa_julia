using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Decida.Sj.Applications.Interfaces.Dto;
using Decida.Sj.Applications.Interfaces.Repositories;
using Decida.Sj.Core.Entities;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
namespace Decida.Sj.Infrastructure.Repositories
{
   public class MSpecialtyMysqlRepository : IMSpecialtyRepository
    {
        private readonly string _connectionString;

        public MSpecialtyMysqlRepository(IOptions<ConnectionStringsOptionsDTO> connOption)
        {
            _connectionString = connOption.Value.MyDb;
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<List<MedicalSpecialtyEntity>>GetMedSpecListRepository(int? cd_convenio)
        {//int? cd_convenio não é usado aqui. está apenas para manter a compatibilidade da interface com a classe equivalente do oracle

            var MadicalEspcRepoList = new List<MedicalSpecialtyEntity>();
            try
            {
                string sql = @"SELECT ID_ESPECIALIDADE IdEspecialidade,CD_ESPECIALIDADE CdEspecialidade,DS_ESPECIALIDADE DsEspecialidade FROM
                                ESPECIALIDADES order by ID_ESPECIALIDADE";


                using (var conn = new MySqlConnection(_connectionString))
                {
                    MadicalEspcRepoList = (await conn.QueryAsync<MedicalSpecialtyEntity>(sql)).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return MadicalEspcRepoList;
        }


        public async Task<MedicalSpecialtyEntity> GetMedSpecByIdRepository(int id)
        {
            var MedicalEspcRepoById = new MedicalSpecialtyEntity();
            try
            {
                string sql = @"SELECT ID_ESPECIALIDADE IdEspecialidade,CD_ESPECIALIDADE CdEspecialidade,DS_ESPECIALIDADE DsEspecialidade FROM
                                ESPECIALIDADES where ID_ESPECIALIDADE=@ID_ESPECIALIDADE";

                DynamicParameters p = new DynamicParameters();

                p.Add("@ID_ESPECIALIDADE", id);

                using (var conn = new MySqlConnection(_connectionString))
                {
                    MedicalEspcRepoById = (await conn.QueryAsync<MedicalSpecialtyEntity>(sql,p)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return MedicalEspcRepoById;
        }





    }
}
