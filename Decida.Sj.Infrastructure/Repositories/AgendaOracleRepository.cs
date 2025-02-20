using Dapper;
using Decida.Sj.Applications.Interfaces.Dto;
using Decida.Sj.Applications.Interfaces.Repositories;
using Decida.Sj.Core.Entities;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Options;
using System.Data;
using Decida.Sj.Core.Entities;
namespace Decida.Sj.Infrastructure.Repositories
{
    public class AgendaOracleRepository : IAgendaOracleRepository
    {
        private readonly string _connectionString;
        private readonly string _userTasy;

        public AgendaOracleRepository(IOptions<ConnectionStringsOptionsDTO> connOptions, IOptions<ConfigApiOptionsDTO> configApiOptions)
        {
            _connectionString = connOptions.Value.OracleDb;
            _userTasy = configApiOptions.Value.UserTasy;
        }

        public async Task<List<AgendaEntity>?> GetAgendaByFIltersRepoAsync(int id_convenio, int id_especialidade, int? cd_pessoa_fisica)
        {
            var agendas = new List<AgendaEntity>();

            // Query com param nomeado :pCpf para Oracle
              string sql = @"
           SELECT t.NR_SEQUENCIA NrSequencia, t.DIA Dia, t.HORA Hora, t.DIA_SEMANA DiaSemana, t.RANK Rank, 
               t.CD_AGENDA CdAgenda, t.HORA_INICIAL HoraInicial, t.HORA_FINAL HoraFinal,
               t.cd_pessoa_fisica AS CdPessoaFisica, f.nm_pessoa_fisica NmPessoaFisica,,e.ds_especialidade   FROM (
            SELECT NR_SEQUENCIA,to_char(DT_AGENDA,'dd/mm/yyyy') DIA, to_char(DT_AGENDA, 'hh24:mi') HORA, to_char(DT_AGENDA, 'day') DIA_SEMANA, RANK() OVER(PARTITION BY DIA,HORA_INICIAL ORDER BY DT_AGENDA) RANK,
            CD_AGENDA, HORA_INICIAL, HORA_FINAL ,cd_pessoa_fisica,cd_especialidade

            FROM (

            SELECT ac.NR_SEQUENCIA,A.CD_AGENDA, DT_AGENDA, to_char(DT_AGENDA, 'yyyy-mm-dd') DIA, to_char(DT_AGENDA, 'hh24') || ':00' HORA_INICIAL, to_char(DT_AGENDA + interval '1' hour , 'hh24') || ':00' HORA_FINAL,
            a.cd_pessoa_fisica,a.cd_especialidade
            FROM TASY.AGENDA_CONSULTA ac INNER JOIN tasy.AGENDA a ON (a.CD_AGENDA=ac.CD_AGENDA)
            WHERE ac.ie_status_agenda = 'L' and a.cd_estabelecimento = 1 AND a.IE_SITUACAO ='A'
            AND A.CD_AGENDA IN (SELECT CD_AGENDA FROM TASY.REGRA_LIB_CONV_AGENDA WHERE CD_CONVENIO = :CD_CONVENIO)
            AND ( a.CD_AGENDA NOT IN ( SELECT CD_AGENDA FROM TASY.AGENDA_CONSULTA_REGRA r WHERE r.CD_AGENDA =a.CD_AGENDA
            AND (CD_CONVENIO=:CD_CONVENIO OR CD_CONVENIO IS NULL) AND (QT_IDADE_MIN <= '40' OR QT_IDADE_MIN IS NULL) AND
            (QT_IDADE_MAX >='40' OR QT_IDADE_MAX IS NULL) AND QT_PERMISSAO = 0
            AND r.IE_CLASSIF_AGENDA ='N' ))
            AND A.CD_PESSOA_FISICA not in ('344918')
            AND a.cd_especialidade = :cd_especialidade
            AND DT_AGENDA >= sysdate ) A
            WHERE to_char(DT_AGENDA, 'd') NOT IN 
            ( SELECT IE_DIA_SEMANA FROM TASY.AGENDA_TURNO_CONV WHERE CD_AGENDA=A.CD_AGENDA AND IE_ATENDE_CONVENIO ='N' AND CD_CONVENIO IN (SELECT CD_CONVENIO from TASY.CONVENIO WHERE CD_CONVENIO =:CD_CONVENIO)

            union

            SELECT IE_DIA_SEMANA FROM TASY.AGENDA_TURNO_CONV WHERE CD_AGENDA=A.CD_AGENDA AND CD_CONVENIO IS NULL AND IE_ATENDE_CONVENIO ='N' AND IE_TIPO_CONVENIO IN 
            (SELECT IE_TIPO_CONVENIO from TASY.CONVENIO WHERE CD_CONVENIO=:CD_CONVENIO))
            AND to_char(DT_AGENDA,'YYYY-MM-DD') NOT IN (
            SELECT DATA_AGENDA FROM (
            SELECT To_char(DT_AGENDA, 'YYYY-MM-DD') DATA_AGENDA, count(*) total
            FROM TASY.AGENDA_CONSULTA
            WHERE DT_AGENDA >= sysdate AND CD_AGENDA =a.CD_AGENDA AND CD_CONVENIO =:CD_CONVENIO AND IE_STATUS_AGENDA NOT IN ('L','C')
            GROUP BY To_char(DT_AGENDA, 'YYYY-MM-DD')
            ) WHERE TOTAL > ( SELECT QT_PERMISSAO FROM TASY.AGENDA_CONSULTA_REGRA WHERE CD_AGENDA = a.CD_AGENDA
            AND (CD_CONVENIO=:CD_CONVENIO) AND (QT_IDADE_MIN <= '40' OR QT_IDADE_MIN IS NULL) AND
            (QT_IDADE_MAX >='40' OR QT_IDADE_MAX IS NULL) AND QT_PERMISSAO > 0
            AND IE_CLASSIF_AGENDA ='N')) ORDER BY DT_AGENDA) t
            join tasy.pessoa_fisica f on t.cd_pessoa_fisica=f.cd_pessoa_fisica
            join tasy.especialidade_medica e on  t.cd_especialidade=e.cd_especialidade
            WHERE RANK <=1 AND rownum<=10
                    ";
            if (cd_pessoa_fisica is not null)
            {
                sql = sql + " AND t.cd_pessoa_fisica=:cd_pessoa_fisica";
            }

            try
            {
                DynamicParameters p = new DynamicParameters();

                p.Add(":cd_especialidade", id_especialidade);
                p.Add(":cd_convenio", id_convenio);
                if (cd_pessoa_fisica is not null)
                    p.Add(":cd_pessoa_fisica", cd_pessoa_fisica);


                // Abre conexão
                using (IDbConnection connection = new OracleConnection(_connectionString))
                {
                    // Consulta usando QuerySingleOrDefaultAsync para trazer 0 ou 1 registro
                    agendas = (await connection.QueryAsync<AgendaEntity>(sql, p)).ToList();
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return agendas; // Retornará null se não achar registro
        }

        public async Task<string> UpdateAppointmentRepoAsync(AgendaEntity agenda,int cd_convenio,string cd_usuario_convenio,string dt_validade_carteira) 
        {

            string queryString = @"
        UPDATE TASY.AGENDA_CONSULTA SET 
            IE_STATUS_AGENDA = 'N',
            DT_ATUALIZACAO = sysdate,
            NM_USUARIO = :usuario,
            CD_AGENDA = :cd_agenda,
            CD_PESSOA_FISICA = :cd_pessoa_fisica,
            CD_CONVENIO = :cd_convenio,
            CD_USUARIO_CONVENIO = :cd_usuario_convenio,
            DT_VALIDADE_CARTEIRA = :dt_validade_carteira
        WHERE NR_SEQUENCIA = :nr_sequencia";

            try
            {
                using (IDbConnection connection = new OracleConnection(_connectionString))
                {
                   

                    var affectedRows = await connection.ExecuteAsync(queryString, new
                    {
                        usuario = _userTasy,
                        cd_agenda = agenda.CdAgenda,
                        cd_pessoa_fisica = agenda.CdPessoaFisica,
                        cd_convenio = cd_convenio,
                        cd_usuario_convenio = cd_usuario_convenio,
                        dt_validade_carteira = dt_validade_carteira,
                        nr_sequencia = agenda.NrSequencia
                    });

                    return affectedRows > 0 ? "OK" : "Nenhuma linha atualizada.";
                }
            }
            catch (Exception ex)
            {
                 
                throw;
            }



        }

    }
}