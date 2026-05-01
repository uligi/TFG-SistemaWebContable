using CapaDatos.Ubicacion;
using CapaEntidad;
using CapaEntidad.Ubicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Ubicacion
{
    public class CN_Canton
    {
        private CD_Canton objCapaDato = new CD_Canton();

        public List<Canton> Listar()
        {
            return objCapaDato.Listar();
        }
    }
}
