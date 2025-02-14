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
using Decida.Sj.Core.Entity;
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




    }
}
