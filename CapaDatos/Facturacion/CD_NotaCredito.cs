using CapaEntidad.Facturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Facturacion
{
    public class CD_NotaCredito
    {
        public List<NotaCredito> Listar()
        {
            List<NotaCredito> lista = new List<NotaCredito>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_NotaCredito_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearNotaCredito(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<NotaCredito>();
            }

            return lista;
        }

        public List<NotaCredito> ListarActivas()
        {
            List<NotaCredito> lista = new List<NotaCredito>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_NotaCredito_ListarActivas", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearNotaCredito(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<NotaCredito>();
            }

            return lista;
        }

        public List<NotaCredito> ListarPorFactura(string numeroFactura)
        {
            List<NotaCredito> lista = new List<NotaCredito>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_NotaCredito_ListarPorFactura", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroFactura", numeroFactura ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(MapearNotaCredito(dr));
                        }
                    }
                }
            }
            catch
            {
                lista = new List<NotaCredito>();
            }

            return lista;
        }

        public NotaCredito Obtener(string numeroNotaCredito)
        {
            NotaCredito obj = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_NotaCredito_Obtener", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroNotaCredito", numeroNotaCredito ?? "");

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = MapearNotaCredito(dr);
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

        public int Registrar(NotaCredito obj, out string Mensaje, out string NumeroNotaCreditoGenerado)
        {
            int resultado = 0;
            Mensaje = string.Empty;
            NumeroNotaCreditoGenerado = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_NotaCredito_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroFactura", obj.NumeroFactura ?? "");
                    cmd.Parameters.AddWithValue("@IdentificacionEmpleado", obj.IdentificacionEmpleado ?? "");
                    cmd.Parameters.AddWithValue("@FechaNotaCredito", obj.FechaNotaCredito);
                    cmd.Parameters.AddWithValue("@Motivo", obj.Motivo ?? "");
                    cmd.Parameters.AddWithValue("@Subtotal", obj.Subtotal);
                    cmd.Parameters.AddWithValue("@TotalImpuesto", obj.TotalImpuesto);
                    cmd.Parameters.AddWithValue("@TotalNotaCredito", obj.TotalNotaCredito);
                    cmd.Parameters.AddWithValue("@Estado", obj.Estado ?? "");

                    cmd.Parameters.Add("@NumeroNotaCreditoGenerado", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    NumeroNotaCreditoGenerado = cmd.Parameters["@NumeroNotaCreditoGenerado"].Value.ToString();
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                NumeroNotaCreditoGenerado = string.Empty;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Editar(NotaCredito obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_NotaCredito_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroNotaCredito", obj.NumeroNotaCredito ?? "");
                    cmd.Parameters.AddWithValue("@IdentificacionEmpleado", obj.IdentificacionEmpleado ?? "");
                    cmd.Parameters.AddWithValue("@FechaNotaCredito", obj.FechaNotaCredito);
                    cmd.Parameters.AddWithValue("@Motivo", obj.Motivo ?? "");
                    cmd.Parameters.AddWithValue("@Estado", obj.Estado ?? "");
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

        public bool Inactivar(string numeroNotaCredito, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Facturacion.sp_NotaCredito_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroNotaCredito", numeroNotaCredito ?? "");

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

        private NotaCredito MapearNotaCredito(SqlDataReader dr)
        {
            return new NotaCredito()
            {
                NumeroNotaCredito = dr["NumeroNotaCredito"].ToString(),
                NumeroFactura = dr["NumeroFactura"].ToString(),

                IdentificacionCliente = dr["IdentificacionCliente"].ToString(),
                ClienteNombre = dr["ClienteNombre"].ToString(),

                IdentificacionEmpleado = dr["IdentificacionEmpleado"].ToString(),
                EmpleadoNombre = dr["EmpleadoNombre"].ToString(),

                FechaNotaCredito = Convert.ToDateTime(dr["FechaNotaCredito"]),
                Motivo = dr["Motivo"].ToString(),

                Subtotal = Convert.ToDecimal(dr["Subtotal"]),
                TotalImpuesto = Convert.ToDecimal(dr["TotalImpuesto"]),
                TotalNotaCredito = Convert.ToDecimal(dr["TotalNotaCredito"]),

                Estado = dr["Estado"].ToString(),

                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),

                Activo = Convert.ToBoolean(dr["Activo"]),

                TotalFactura = Convert.ToDecimal(dr["TotalFactura"]),
                EstadoFactura = dr["EstadoFactura"].ToString(),
                FacturaActiva = Convert.ToBoolean(dr["FacturaActiva"])
            };
        }
    }
}