using Decida.Sj.Applications.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Decida.Sj.Applications.Interfaces.Dto;
using Microsoft.Extensions.Options;
using System.Globalization;
namespace Decida.Sj.Applications.Services
{
    public  class UtilitiesService
    {
        private readonly MensagensOptionsDTO _mensagens;

        public UtilitiesService(IOptions<MensagensOptionsDTO> mensagensOptions)
        {
            _mensagens = mensagensOptions.Value;
        }

        public  (bool status, string jsonOrError) DecodeAgendaHash(int id, string hash)
        {
            try
            {
                // Verifica se a hash não é nula ou vazia
                if (string.IsNullOrEmpty(hash))
                    return (false, JsonSerializer.Serialize(new { error = "Hash inválida ou vazia." }));

                // Decodifica a string Base64 para JSON
                string jsonString = Encoding.UTF8.GetString(Convert.FromBase64String(hash));

                // Converte para uma lista de objetos
                var agendas = JsonSerializer.Deserialize<List<AgendaDTO>>(jsonString);

                if (agendas == null || agendas.Count == 0)
                    return (false, JsonSerializer.Serialize(new { error = "Nenhuma agenda encontrada." }));

                // Busca o registro correspondente ao ID
                var agendaSelecionada = agendas.Find(a => a.Index == id);

                // Verifica se o template de mensagem existe
                if (string.IsNullOrEmpty(_mensagens.ConfirmaAgenda))
                    return (false, JsonSerializer.Serialize(new { error = "Template de mensagem não configurado." }));

                // Se não encontrar, retorna erro
                if (agendaSelecionada == null)
                    return (false, JsonSerializer.Serialize(new { error = "ID não encontrado na agenda." }));
                string msg = string.Format(_mensagens.ConfirmaAgenda, 
                    agendaSelecionada.NmPessoaFisicaMedico, agendaSelecionada.DsEspecialidade, agendaSelecionada.Dia, agendaSelecionada.DiaSemana, agendaSelecionada.Hora);
             
                
                // Retorna o JSON do registro encontrado
                return (true, msg);
            }
            catch (Exception ex)
            {
                return (false, JsonSerializer.Serialize(new { error = "Erro ao processar os dados.", details = ex.Message }));
            }
        }

        public (bool status, AgendaDTO agendaConfirmada) DecodeAgendaoConfirmaHash(int id, string hash)
        {
            try
            {
                // Verifica se a hash não é nula ou vazia
                if (string.IsNullOrEmpty(hash))
                    return (false, new AgendaDTO());

                // Decodifica a string Base64 para JSON
                string jsonString = Encoding.UTF8.GetString(Convert.FromBase64String(hash));

                // Converte para uma lista de objetos
                var agendas = JsonSerializer.Deserialize<List<AgendaDTO>>(jsonString);

                if (agendas == null || agendas.Count == 0)
                    return (false, new AgendaDTO());

                // Busca o registro correspondente ao ID
                var agendaSelecionada = agendas.Find(a => a.Index == id);

                // Verifica se o template de mensagem existe
                if (string.IsNullOrEmpty(_mensagens.ConfirmaAgenda))
                    return (false, new AgendaDTO());

                // Se não encontrar, retorna erro
                if (agendaSelecionada == null)
                    return (false, new AgendaDTO());

           

                // Retorna o JSON do registro encontrado
                return (true, agendaSelecionada);
            }
            catch (Exception ex)
            {
                return (false, new AgendaDTO());
            }
        }


 
        /// <summary>
        /// Valida se a data informada está no formato "dd/MM/yyyy".
        /// </summary>
        /// <param name="dataNascimento">Data de nascimento como string.</param>
        /// <returns>True se a data for válida e estiver no formato correto; caso contrário, false.</returns>
        public bool ValidarData(string dataNascimento)
        {
            if (string.IsNullOrWhiteSpace(dataNascimento))
                return false;

            // Tenta fazer o parse da data com o formato exato "dd/MM/yyyy"
            DateTime parsedDate;
            bool isValid = DateTime.TryParseExact(
                dataNascimento,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out parsedDate);

            return isValid;
        }
   



}



}

