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
                            lista.Add(new Gasto()
                            {
                                IdGasto = Convert.ToInt32(dr["IdGasto"]),

                                IdCuentaPorPagar = dr["IdCuentaPorPagar"] == DBNull.Value
        ? (int?)null
        : Convert.ToInt32(dr["IdCuentaPorPagar"]),

                                NumeroDocumento = dr["NumeroDocumento"] == DBNull.Value
        ? ""
        : dr["NumeroDocumento"].ToString(),

                                IdAbonoCuentaPorPagar = dr["IdAbonoCuentaPorPagar"] == DBNull.Value
        ? (int?)null
        : Convert.ToInt32(dr["IdAbonoCuentaPorPagar"]),

                                IdCuentaContable = dr["IdCuentaContable"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IdCuentaContable"]),
                                CodigoCuenta = dr["CodigoCuenta"] == DBNull.Value ? "" : dr["CodigoCuenta"].ToString(),
                                NombreCuenta = dr["NombreCuenta"] == DBNull.Value ? "" : dr["NombreCuenta"].ToString(),

                                FechaGasto = Convert.ToDateTime(dr["FechaGasto"]),
                                Descripcion = dr["Descripcion"].ToString(),

                                IdTipoGasto = Convert.ToInt32(dr["IdTipoGasto"]),
                                TipoGastoNombre = dr["TipoGastoNombre"].ToString(),

                                Monto = Convert.ToDecimal(dr["Monto"]),

                                IdentificacionProveedor = dr["IdentificacionProveedor"] == DBNull.Value
        ? ""
        : dr["IdentificacionProveedor"].ToString(),

                                ProveedorNombre = dr["ProveedorNombre"] == DBNull.Value
        ? ""
        : dr["ProveedorNombre"].ToString(),

                                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),

                                FechaModificacion = dr["FechaModificacion"] == DBNull.Value
        ? (DateTime?)null
        : Convert.ToDateTime(dr["FechaModificacion"]),

                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
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

        public int Registrar(Gasto obj, out string Mensaje)
        {
            int resultado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Gasto_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdCuentaPorPagar",
                        obj.IdCuentaPorPagar == null ? (object)DBNull.Value : obj.IdCuentaPorPagar);

                    cmd.Parameters.AddWithValue("@IdAbonoCuentaPorPagar",
                        obj.IdAbonoCuentaPorPagar == null ? (object)DBNull.Value : obj.IdAbonoCuentaPorPagar);

                    cmd.Parameters.AddWithValue("@IdCuentaContable", obj.IdCuentaContable);
                    cmd.Parameters.AddWithValue("@FechaGasto", obj.FechaGasto);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("@IdTipoGasto", obj.IdTipoGasto);
                    cmd.Parameters.AddWithValue("@Monto", obj.Monto);

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

                    cmd.Parameters.AddWithValue("@IdGasto", obj.IdGasto);
                    cmd.Parameters.AddWithValue("@IdCuentaContable", obj.IdCuentaContable);
                    cmd.Parameters.AddWithValue("@FechaGasto", obj.FechaGasto);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("@IdTipoGasto", obj.IdTipoGasto);
                    cmd.Parameters.AddWithValue("@Monto", obj.Monto);
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

        public bool Inactivar(int idGasto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Movimientos.sp_Gasto_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdGasto", idGasto);

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