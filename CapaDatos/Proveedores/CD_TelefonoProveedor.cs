using CapaEntidad.Proveedores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Proveedores
{
    public class CD_TelefonoProveedor
    {
        public List<TelefonoProveedor> Listar()
        {
            List<TelefonoProveedor> lista = new List<TelefonoProveedor>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_TelefonoProveedor_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new TelefonoProveedor()
                            {
                                IdentificacionProveedor = dr["IdentificacionProveedor"].ToString(),
                                RazonSocial = dr["RazonSocial"].ToString(),
                                NumeroTelefono = dr["NumeroTelefono"].ToString(),
                                IdTipoTelefono = Convert.ToInt32(dr["IdTipoTelefono"]),
                                TipoTelefonoNombre = dr["TipoTelefonoNombre"].ToString(),
                                EsPrincipal = Convert.ToBoolean(dr["EsPrincipal"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<TelefonoProveedor>();
            }

            return lista;
        }

        public int Registrar(TelefonoProveedor obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_TelefonoProveedor_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", obj.IdentificacionProveedor);
                    cmd.Parameters.AddWithValue("@NumeroTelefono", obj.NumeroTelefono);
                    cmd.Parameters.AddWithValue("@IdTipoTelefono", obj.IdTipoTelefono);
                    cmd.Parameters.AddWithValue("@EsPrincipal", obj.EsPrincipal);

                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Editar(string numeroAnterior, TelefonoProveedor obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_TelefonoProveedor_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", obj.IdentificacionProveedor);
                    cmd.Parameters.AddWithValue("@NumeroTelefonoAnterior", numeroAnterior);
                    cmd.Parameters.AddWithValue("@NumeroTelefonoNuevo", obj.NumeroTelefono);
                    cmd.Parameters.AddWithValue("@IdTipoTelefono", obj.IdTipoTelefono);
                    cmd.Parameters.AddWithValue("@EsPrincipal", obj.EsPrincipal);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);

                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Inactivar(string identificacionProveedor, string numeroTelefono, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_TelefonoProveedor_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", identificacionProveedor);
                    cmd.Parameters.AddWithValue("@NumeroTelefono", numeroTelefono);

                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }
    }
}