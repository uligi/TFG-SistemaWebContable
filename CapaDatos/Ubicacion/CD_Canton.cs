using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad.Ubicacion;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos.Ubicacion
{
    public class CD_Canton
    {
        public List<Canton> Listar()
        {
            List<Canton> lista = new List<Canton>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    string query = "Select * From Canton";
                    SqlCommand cmd = new SqlCommand(query, oconexion); 
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Canton()
                                {
                                    IdCanton = Convert.ToInt32(dr["IdCanton"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                }

                                );
                        }

                    }
                }
                    
            }
            catch (Exception ex) { }
            return lista;
        }
    }
}
