using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimal_api.Dominio.ModelViews
{
    public class AdministradorModelView
    {
        public int Id { get; set; }
        public string email { get; set; }
        public string Perfil { get; set; }

    }
}