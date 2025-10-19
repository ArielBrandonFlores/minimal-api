using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimal_api.Dominio.ModelViews
{
    public class ErrosDeValidacao
    {
        public List<string> Mensagem { get; set; } = new List<string>();

    }
}