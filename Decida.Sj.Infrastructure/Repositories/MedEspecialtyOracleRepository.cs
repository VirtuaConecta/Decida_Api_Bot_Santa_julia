using Dapper;
using Decida.Sj.Applications.Interfaces.Dto;
using Decida.Sj.Applications.Interfaces.Repositories;
using Decida.Sj.Core.Entities;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Options;
using System.Data;
using Decida.Sj.Core.Entities;
using MySql.Data.MySqlClient;

namespace Decida.Sj.Infrastructure.Repositories
{
    public class MedEspecialtyOracleRepository : IMSpecialtyRepository
    {
        private readonly string _connectionString;
        private readonly string _connectionStringMy;
        private readonly string _userTasy;

        public MedEspecialtyOracleRepository(IOptions<ConnectionStringsOptionsDTO> connOptions, IOptions<ConfigApiOptionsDTO> configApiOptions)
        {
            _connectionString = connOptions.Value.OracleDb;
            _connectionStringMy = connOptions.Value.MyDb;

            _userTasy = configApiOptions.Value.UserTasy;
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

                using (var conn = new MySqlConnection(_connectionStringMy))
                {
                    MedicalEspcRepoById = (await conn.QueryAsync<MedicalSpecialtyEntity>(sql, p)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return MedicalEspcRepoById;
        }




        public async Task<List<MedicalSpecialtyEntity>> GetMedSpecListRepository(int? cd_convenio)
        {
            var especialtyList = new List<MedicalSpecialtyEntity>();

            // Query com param nomeado :pCpf para Oracle
            string sql = @"
                 select t.cd_especialidade CdEspecialidade,
                 t.ds_especialidade DsEspecialidade
                 from (
                 Select pf.nm_pessoa_fisica,c.ds_convenio,mc.cd_convenio,ag.cd_agenda,ac.dt_agenda,ag.cd_especialidade,em.ds_especialidade from 
                 tasy.medico_convenio mc
                 join tasy.pessoa_fisica pf on mc.cd_pessoa_fisica =pf.cd_pessoa_fisica
                 join tasy.agenda ag on ag.cd_pessoa_fisica= mc.cd_pessoa_fisica
                 join tasy.especialidade_medica em on ag.cd_especialidade= em.cd_especialidade
                 join tasy.agenda_consulta ac on ag.cd_agenda =ac.cd_agenda
                 join tasy.convenio c on mc.cd_convenio = c.cd_convenio
                 where 
                 mc.cd_convenio=:CD_CONVENIO
                 and ac.DT_AGENDA >= SYSDATE
                 and ac.ie_status_agenda = 'L' 
                 AND ag.CD_PESSOA_FISICA NOT IN ('344918')
                 ) t
                 group by t.cd_especialidade, t.ds_especialidade 
                order by ds_especialidade asc
 
                    ";
 

            try
            {
                DynamicParameters p = new DynamicParameters();

               
                p.Add(":CD_CONVENIO", cd_convenio);
                


                // Abre conexão
                using (IDbConnection connection = new OracleConnection(_connectionString))
                {
                    // Consulta usando QuerySingleOrDefaultAsync para trazer 0 ou 1 registro
                    especialtyList = (await connection.QueryAsync<MedicalSpecialtyEntity>(sql, p)).ToList();
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return especialtyList; // Retornará null se não achar registro
        }
    }
}