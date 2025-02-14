using Dapper;
using Decida.Sj.Applications.Interfaces.Dto;
using Decida.Sj.Applications.Interfaces.Repositories;
using Decida.Sj.Core.Entities;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Options;
using System.Data;
namespace Decida.Sj.Infrastructure.Repositories
{
    public class PacientOracleRepository : IPacientOracleRepository
    {
        private readonly string _connectionString;

        public PacientOracleRepository(IOptions<ConnectionStringsOptionsDTO> connOptions)
        {
            _connectionString = connOptions.Value.OracleDb;
        }

        public async Task<PacienteEntity?> GetPessoaFisicaByCpfAsync(string cpf)
        {
            var pessoa = new PacienteEntity();

            // Query com param nomeado :pCpf para Oracle
            const string sql = @"
           SELECT 
                pf.CD_PESSOA_FISICA AS PacienteId,
                pf.NM_PESSOA_FISICA AS PacientName,
                c.ds_convenio AS PacientCare,
                c.cd_convenio AS PacientIdCare
            FROM 
                TASY.PESSOA_FISICA pf
            LEFT JOIN (
                SELECT 
                    ac.cd_pessoa_fisica,
                    ac.cd_convenio,
                    cv.ds_convenio
                FROM 
                    tasy.agenda_consulta ac
                JOIN 
                    tasy.convenio cv ON ac.cd_convenio = cv.cd_convenio
                WHERE 
                    ac.nr_sequencia = (
                        SELECT MAX(ac2.nr_sequencia)
                        FROM tasy.agenda_consulta ac2
                        WHERE ac2.cd_pessoa_fisica = ac.cd_pessoa_fisica
                    )
            ) c ON c.cd_pessoa_fisica = pf.CD_PESSOA_FISICA
            WHERE 
                pf.NR_CPF = :pCpf
                    ";

            try
            {
                // Abre conexão
                using (IDbConnection connection = new OracleConnection(_connectionString))
                {
                    // Consulta usando QuerySingleOrDefaultAsync para trazer 0 ou 1 registro
                    pessoa = (await connection.QueryAsync<PacienteEntity>(sql, new { pCpf = cpf })).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return pessoa; // Retornará null se não achar registro
        }
    }
}