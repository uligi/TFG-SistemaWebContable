using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Contabilidad
{
    public class CD_CuentaContable
    {
        public List<CuentaContable> Listar()
        {
            List<CuentaContable> lista = new List<CuentaContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_CuentaContable_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearCuenta(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<CuentaContable>();
            }

            return lista;
        }

        public List<CuentaContable> ListarActivas()
        {
            List<CuentaContable> lista = new List<CuentaContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_CuentaContable_ListarActivas", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearCuenta(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<CuentaContable>();
            }

            return lista;
        }

        public List<CuentaContable> ListarParaMovimientos()
        {
            List<CuentaContable> lista = new List<CuentaContable>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_CuentaContable_ListarParaMovimientos", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearCuenta(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<CuentaContable>();
            }

            return lista;
        }

        public CuentaContable Obtener(string codigoCuenta)
        {
            CuentaContable obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_CuentaContable_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoCuenta", codigoCuenta ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearCuenta(dr);
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

        public int Registrar(CuentaContable obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_CuentaContable_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoCuenta", obj.CodigoCuenta ?? "");
                    cmd.Parameters.AddWithValue("@NombreCuenta", obj.NombreCuenta ?? "");
                    cmd.Parameters.AddWithValue("@IdTipoCuentaContable", obj.IdTipoCuentaContable);
                    cmd.Parameters.AddWithValue("@IdNaturalezaCuentaContable", obj.IdNaturalezaCuentaContable);
                    cmd.Parameters.AddWithValue("@AceptaMovimientos", obj.AceptaMovimientos);
                    cmd.Parameters.AddWithValue("@CodigoCuentaPadre", string.IsNullOrWhiteSpace(obj.CodigoCuentaPadre) ? "0" : obj.CodigoCuentaPadre);

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

        public bool Editar(CuentaContable obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_CuentaContable_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoCuenta", obj.CodigoCuenta ?? "");
                    cmd.Parameters.AddWithValue("@NombreCuenta", obj.NombreCuenta ?? "");
                    cmd.Parameters.AddWithValue("@IdTipoCuentaContable", obj.IdTipoCuentaContable);
                    cmd.Parameters.AddWithValue("@IdNaturalezaCuentaContable", obj.IdNaturalezaCuentaContable);
                    cmd.Parameters.AddWithValue("@AceptaMovimientos", obj.AceptaMovimientos);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);
                    cmd.Parameters.AddWithValue("@CodigoCuentaPadre", string.IsNullOrWhiteSpace(obj.CodigoCuentaPadre) ? "0" : obj.CodigoCuentaPadre);

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

        public bool Inactivar(string codigoCuenta, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_CuentaContable_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoCuenta", codigoCuenta ?? "");

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

        public bool GenerarCodigo(string codigoCuentaPadre, out string CodigoGenerado, out string Mensaje)
        {
            bool resultado = false;
            CodigoGenerado = string.Empty;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_CuentaContable_GenerarCodigo", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CodigoCuentaPadre", string.IsNullOrWhiteSpace(codigoCuentaPadre) ? "0" : codigoCuentaPadre);

                    cmd.Parameters.Add("@CodigoGenerado", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    CodigoGenerado = cmd.Parameters["@CodigoGenerado"].Value.ToString();
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                CodigoGenerado = string.Empty;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        private CuentaContable MapearCuenta(SqlDataReader dr)
        {
            return new CuentaContable()
            {
                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                NombreCuenta = dr["NombreCuenta"].ToString(),

                IdTipoCuentaContable = Convert.ToInt32(dr["IdTipoCuentaContable"]),
                TipoCuentaContableNombre = dr["TipoCuentaContableNombre"].ToString(),

                IdNaturalezaCuentaContable = Convert.ToInt32(dr["IdNaturalezaCuentaContable"]),
                NaturalezaCuentaContableNombre = dr["NaturalezaCuentaContableNombre"].ToString(),

                AceptaMovimientos = Convert.ToBoolean(dr["AceptaMovimientos"]),
                Activo = Convert.ToBoolean(dr["Activo"]),

                CodigoCuentaPadre = dr["CodigoCuentaPadre"].ToString(),
                NombreCuentaPadre = dr["NombreCuentaPadre"] == DBNull.Value
                    ? ""
                    : dr["NombreCuentaPadre"].ToString()
            };
        }
    }
}