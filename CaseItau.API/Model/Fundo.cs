using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaseItau.API.Model
{
    public class Fundo
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public int CodigoTipo { get; set; }
        public string NomeTipo { get; set; }
        public decimal? Patrimonio { get; set; }
        public List<TipoFundo> TipoFundos { get; set; }
    }
}
