using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Contabilidad
{
    public class CD_ConfiguracionCuentaContable
    {
        public List<ConfiguracionCuentaContable> Listar()
        {
            List<ConfiguracionCuentaContable> lista = new List<ConfiguracionCuentaContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_ConfiguracionCuentaContable_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearConfiguracion(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ConfiguracionCuentaContable>();
            }

            return lista;
        }

        public ConfiguracionCuentaContable ObtenerPorOperacion(string codigoOperacion)
        {
            ConfiguracionCuentaContable obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_ConfiguracionCuentaContable_ObtenerPorOperacion", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoOperacion", codigoOperacion ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearConfiguracion(dr);
                        }
                    }
                }
            }
            catch
            {
                obj = null;
            }

            return obj;
        }

        public ConfiguracionCuentaContable ObtenerActivaPorOperacion(string codigoOperacion)
        {
            ConfiguracionCuentaContable obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_ConfiguracionCuentaContable_ObtenerActivaPorOperacion", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoOperacion", codigoOperacion ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearConfiguracion(dr);
                        }
                    }
                }
            }
            catch
            {
                obj = null;
            }

            return obj;
        }

        public int Registrar(ConfiguracionCuentaContable obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_ConfiguracionCuentaContable_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoOperacion", obj.CodigoOperacion ?? "");
                    cmd.Parameters.AddWithValue("@NombreOperacion", obj.NombreOperacion ?? "");
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? "");
                    cmd.Parameters.AddWithValue("@CodigoCuenta", obj.CodigoCuenta ?? "");

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

        public bool Editar(ConfiguracionCuentaContable obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_ConfiguracionCuentaContable_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoOperacion", obj.CodigoOperacion ?? "");
                    cmd.Parameters.AddWithValue("@NombreOperacion", obj.NombreOperacion ?? "");
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? "");
                    cmd.Parameters.AddWithValue("@CodigoCuenta", obj.CodigoCuenta ?? "");
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

        public bool Inactivar(string codigoOperacion, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_ConfiguracionCuentaContable_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoOperacion", codigoOperacion ?? "");

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

        private ConfiguracionCuentaContable MapearConfiguracion(SqlDataReader dr)
        {
            return new ConfiguracionCuentaContable()
            {
                CodigoOperacion = dr["CodigoOperacion"].ToString(),
                NombreOperacion = dr["NombreOperacion"].ToString(),
                Descripcion = dr["Descripcion"].ToString(),

                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                NombreCuenta = dr["NombreCuenta"].ToString(),

                AceptaMovimientos = Convert.ToBoolean(dr["AceptaMovimientos"]),
                CuentaActiva = Convert.ToBoolean(dr["CuentaActiva"]),

                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),

                Activo = Convert.ToBoolean(dr["Activo"])
            };
        }
    }
}