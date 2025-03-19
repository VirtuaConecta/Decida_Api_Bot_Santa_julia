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
using Decida.Sj.Core.Entities;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
namespace Decida.Sj.Infrastructure.Repositories
{
   public class MedicMysqlRepository : IMedicMysqlRepository
    {
        private readonly string _connectionString;

        public MedicMysqlRepository(IOptions<ConnectionStringsOptionsDTO> connOption)
        {
            _connectionString = connOption.Value.MyDb;
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<List<MedicEntity>>GetMedListRepsitory()
        {
             var MadicalRepoList = new List<MedicEntity>();
            try
            {
                string sql = @"SELECT ID_MEDICO,CD_PESSOA_FISICA,NM_PESSOA_FISICA FROM
                                MEDICOS order by ID_MEDICO";


                using (var conn = new MySqlConnection(_connectionString))
                {
                    MadicalRepoList = (await conn.QueryAsync<MedicEntity>(sql)).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return MadicalRepoList;
        }

        public async Task<MedicEntity> GetMedByIdRepsitory(int id)
        {
            var MadicalRepoList = new  MedicEntity();
            try
            {
                string sql = @"SELECT ID_MEDICO,CD_PESSOA_FISICA,NM_PESSOA_FISICA FROM
                                MEDICOS where ID_MEDICO=@ID_MEDICO";
                DynamicParameters p = new DynamicParameters();

                p.Add("@ID_MEDICO", id);

                using (var conn = new MySqlConnection(_connectionString))
                {
                    MadicalRepoList = (await conn.QueryAsync<MedicEntity>(sql,p)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return MadicalRepoList;
        }

        public async Task<List<MedicEntity>> GetMedListByPlanEspecialtyRepository(int cd_especialidade,int id_convenio)
        {
            var MadicalRepoList = new List<MedicEntity>();
          
            try
            {
                string sql = @"
                    SELECT m.ID_MEDICO,e.CD_PESSOA_FISICA, e.NM_PESSOA_FISICA FROM api_santa_julia.ESPECIALIDADE_MEDICO e
                    join MEDICO_CONVENIO c on e.CD_PESSOA_FISICA= c.cd_pessoa_fisica
                    join MEDICOS m on e.CD_PESSOA_FISICA=m.CD_PESSOA_FISICA
                    join CONVENIOS cv on c.cd_convenio = cv.CD_CONVENIO
                    where e.CD_ESPECIALIDADE =@CD_ESPECIALIDADE and cv.ID_CONVENIO=@ID_CONVENIO
                    ";

                var p = new DynamicParameters();

                p.Add("@CD_ESPECIALIDADE", cd_especialidade);
                p.Add("@ID_CONVENIO", id_convenio);
                using (var conn = new MySqlConnection(_connectionString))
                {
                    MadicalRepoList = (await conn.QueryAsync<MedicEntity>(sql, p)).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return MadicalRepoList;
        }


    }
}
