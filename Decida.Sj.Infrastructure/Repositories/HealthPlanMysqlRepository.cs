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
   public class HealthPlanMysqlRepository: IHealthPlanMysqlRepository
    {
        private readonly string _connectionString;

        public HealthPlanMysqlRepository(IOptions<ConnectionStringsOptionsDTO> connOption)
        {
            _connectionString = connOption.Value.MyDb;
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<List<HealthPlanEntity>>GetHelthPlanList()
        {
             var convenios = new List<HealthPlanEntity>();
            try
            {
                string sql = @"SELECT ID_CONVENIO,CD_CONVENIO,DS_CONVENIO FROM
                                CONVENIOS order by ID_CONVENIO";


                using (var conn = new MySqlConnection(_connectionString))
                {
                    convenios = (await conn.QueryAsync<HealthPlanEntity>(sql)).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return convenios;
        }





    }
}
