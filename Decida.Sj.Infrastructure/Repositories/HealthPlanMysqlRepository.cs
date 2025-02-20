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


        public async Task<HealthPlanEntity> GetHelthPlanByID(int id)
        {
            var convenios = new HealthPlanEntity();


            try
            {
                string sql = @"SELECT ID_CONVENIO,CD_CONVENIO,DS_CONVENIO FROM
                                CONVENIOS where ID_CONVENIO=@ID_CONVENIO";

                DynamicParameters p = new DynamicParameters();
                p.Add("@ID_CONVENIO", id);

                using (var conn = new MySqlConnection(_connectionString))
                {
                    convenios = (await conn.QueryAsync<HealthPlanEntity>(sql,p)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return convenios;
        }


        public async Task<List<PlanCareEntity>> GetPlanListByCompany(int id_convenio)
        {
            var plans = new List<PlanCareEntity>();
            try
            {
                string sql = @"
            SELECT 
                p.INDICE as Sequence,
                p.CD_CONVENIO AS CdConvenio,
                p.CD_CATEGORIA AS CdCategoria,
                p.CD_PLANO AS CdPlano,
                p.DS_PLANO AS DsPlano
            FROM 
                CONVENIO_PLANOS p
                JOIN CONVENIOS c ON p.CD_CONVENIO = c.CD_CONVENIO
            WHERE 
                c.ID_CONVENIO = @ID_CONVENIO
            ORDER BY 
                p.CD_PLANO";

                var parameters = new DynamicParameters();
                parameters.Add("@ID_CONVENIO", id_convenio);

                using (var conn = new MySqlConnection(_connectionString))
                {
                    plans = (await conn.QueryAsync<PlanCareEntity>(sql, parameters)).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return plans;
        }

    }
}
