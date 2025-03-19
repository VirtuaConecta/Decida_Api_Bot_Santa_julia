using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Decida.Sj.Applications.Model
{
    public class AgendaDTO
    {
        [JsonPropertyName("nr_sequencia")]
        public int NrSequencia { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("cd_agenda")]
        public int CdAgenda { get; set; }

        [JsonPropertyName("cd_pessoa_fisica")]
        public int CdPessoaFisica { get; set; }

        [JsonPropertyName("cd_pessoa_fisica_medico")]
        public int CdPessoaFisicaMedico { get; set; }

        [JsonPropertyName("nm_pessoa_fisica")]
        public string NmPessoaFisica { get; set; }

        [JsonPropertyName("nm_pessoa_fisica_medico")]
        public string NmPessoaFisicaMedico { get; set; }

        

        [JsonPropertyName("dia")]
        public string Dia { get; set; }

        [JsonPropertyName("hora")]
        public string Hora { get; set; }

        [JsonPropertyName("DiaSemana")]
        public string DiaSemana { get; set; }

        [JsonPropertyName("ds_especialidade")]
        public string DsEspecialidade { get; set; }


        [JsonPropertyName("cd_convenio")]
        public string DsConvenio { get; set; }
    }
}
