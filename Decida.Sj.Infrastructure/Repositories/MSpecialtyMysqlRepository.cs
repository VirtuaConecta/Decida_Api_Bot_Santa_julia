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
   public class MSpecialtyMysqlRepository : IMSpecialtyMysqlRepository
    {
        private readonly string _connectionString;

        public MSpecialtyMysqlRepository(IOptions<ConnectionStringsOptionsDTO> connOption)
        {
            _connectionString = connOption.Value.MyDb;
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<List<MedicalSpecialtyEntity>>GetMedSpecListRepsitory()
        {
             var MadicalEspcRepoList = new List<MedicalSpecialtyEntity>();
            try
            {
                string sql = @"SELECT ID_ESPECIALIDADE,CD_ESPECIALIDADE,DS_ESPECIALIDADE FROM
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





    }
}
