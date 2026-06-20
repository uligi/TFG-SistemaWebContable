using CapaEntidad.Movimientos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Movimientos
{
    public class CD_Gasto
    {
        public List<Gasto> Listar()
        {
            List<Gasto> lista = new List<Gasto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Gasto_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearGasto(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Gasto>();
            }

            return lista;
        }

        public List<Gasto> ListarActivos()
        {
            List<Gasto> lista = new List<Gasto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Gasto_ListarActivos", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearGasto(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Gasto>();
            }

            return lista;
        }

        public Gasto Obtener(string numeroGasto)
        {
            Gasto obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Gasto_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroGasto", numeroGasto ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearGasto(dr);
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

        public int Registrar(Gasto obj, out string Mensaje, out string NumeroGastoGenerado)
        {
            int resultado = 0;
            Mensaje = string.Empty;
            NumeroGastoGenerado = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Gasto_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FechaGasto", obj.FechaGasto);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? "");
                    cmd.Parameters.AddWithValue("@IdTipoGasto", obj.IdTipoGasto);
                    cmd.Parameters.AddWithValue("@CodigoCuenta", obj.CodigoCuenta ?? "");
                    cmd.Parameters.AddWithValue("@Monto", obj.Monto);

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", obj.IdentificacionProveedor ?? "");
                    cmd.Parameters.AddWithValue("@NumeroDocumento", obj.NumeroDocumento ?? "");

                    cmd.Parameters.AddWithValue("@IdentificacionProveedorAbono", obj.IdentificacionProveedorAbono ?? "");
                    cmd.Parameters.AddWithValue("@NumeroDocumentoAbono", obj.NumeroDocumentoAbono ?? "");
                    cmd.Parameters.AddWithValue("@NumeroAbonoCuentaPorPagar", obj.NumeroAbonoCuentaPorPagar);

                    cmd.Parameters.AddWithValue("@NumeroComprobante", obj.NumeroComprobante ?? "");
                    cmd.Parameters.AddWithValue("@NombreArchivoComprobante", obj.NombreArchivoComprobante ?? "");
                    cmd.Parameters.AddWithValue("@RutaComprobante", obj.RutaComprobante ?? "");

                    cmd.Parameters.Add("@NumeroGastoGenerado", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    NumeroGastoGenerado = cmd.Parameters["@NumeroGastoGenerado"].Value.ToString();
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                NumeroGastoGenerado = string.Empty;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Editar(Gasto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Gasto_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroGasto", obj.NumeroGasto ?? "");
                    cmd.Parameters.AddWithValue("@FechaGasto", obj.FechaGasto);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? "");
                    cmd.Parameters.AddWithValue("@IdTipoGasto", obj.IdTipoGasto);
                    cmd.Parameters.AddWithValue("@CodigoCuenta", obj.CodigoCuenta ?? "");
                    cmd.Parameters.AddWithValue("@Monto", obj.Monto);

                    cmd.Parameters.AddWithValue("@NumeroComprobante", obj.NumeroComprobante ?? "");
                    cmd.Parameters.AddWithValue("@NombreArchivoComprobante", obj.NombreArchivoComprobante ?? "");
                    cmd.Parameters.AddWithValue("@RutaComprobante", obj.RutaComprobante ?? "");
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

        public bool Inactivar(string numeroGasto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Gasto_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroGasto", numeroGasto ?? "");

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

        private Gasto MapearGasto(SqlDataReader dr)
        {
            Gasto obj = new Gasto()
            {
                NumeroGasto = dr["NumeroGasto"].ToString(),
                FechaGasto = Convert.ToDateTime(dr["FechaGasto"]),
                Descripcion = dr["Descripcion"].ToString(),

                IdTipoGasto = Convert.ToInt32(dr["IdTipoGasto"]),
                TipoGastoNombre = dr["TipoGastoNombre"].ToString(),

                CodigoCuenta = dr["CodigoCuenta"].ToString(),
                NombreCuenta = dr["NombreCuenta"].ToString(),

                Monto = Convert.ToDecimal(dr["Monto"]),

                IdentificacionProveedor = dr["IdentificacionProveedor"].ToString(),
                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                ProveedorNombre = dr["ProveedorNombre"].ToString(),

                ConceptoCuentaPorPagar = dr["ConceptoCuentaPorPagar"].ToString(),
                MontoOriginal = Convert.ToDecimal(dr["MontoOriginal"]),
                SaldoActual = Convert.ToDecimal(dr["SaldoActual"]),
                EstadoCuentaPorPagar = dr["EstadoCuentaPorPagar"].ToString(),

                IdentificacionProveedorAbono = dr["IdentificacionProveedorAbono"].ToString(),
                NumeroDocumentoAbono = dr["NumeroDocumentoAbono"].ToString(),
                NumeroAbonoCuentaPorPagar = Convert.ToInt32(dr["NumeroAbonoCuentaPorPagar"]),

                FechaAbono = Convert.ToDateTime(dr["FechaAbono"]),
                MontoAbono = Convert.ToDecimal(dr["MontoAbono"]),

                NumeroComprobante = dr["NumeroComprobante"].ToString(),
                NombreArchivoComprobante = dr["NombreArchivoComprobante"].ToString(),
                RutaComprobante = dr["RutaComprobante"].ToString(),

                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),
                Activo = Convert.ToBoolean(dr["Activo"])
            };

            return obj;
        }
    }
}