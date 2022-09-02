using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CaseItau.API.Model
{
    public class TipoFundo
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
    }
}
