using CapaEntidad.Proveedores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Proveedores
{
    public class CD_Proveedor
    {
        public List<Proveedor> Listar()
        {
            List<Proveedor> lista = new List<Proveedor>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Proveedor_Listar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Proveedor()
                            {
                                IdentificacionProveedor = dr["IdentificacionProveedor"].ToString(),
                                RazonSocial = dr["RazonSocial"].ToString(),
                                NombreContacto = dr["NombreContacto"] == DBNull.Value ? "" : dr["NombreContacto"].ToString(),

                                IdDistrito = dr["IdDistrito"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdDistrito"]),
                                DistritoNombre = dr["DistritoNombre"] == DBNull.Value ? "" : dr["DistritoNombre"].ToString(),

                                IdCanton = dr["IdCanton"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdCanton"]),
                                CantonNombre = dr["CantonNombre"] == DBNull.Value ? "" : dr["CantonNombre"].ToString(),

                                IdProvincia = dr["IdProvincia"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdProvincia"]),
                                ProvinciaNombre = dr["ProvinciaNombre"] == DBNull.Value ? "" : dr["ProvinciaNombre"].ToString(),

                                DireccionExacta = dr["DireccionExacta"] == DBNull.Value ? "" : dr["DireccionExacta"].ToString(),
                                DiasCredito = dr["DiasCredito"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["DiasCredito"]),

                                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                                FechaModificacion = dr["FechaModificacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaModificacion"]),

                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Proveedor>();
            }

            return lista;
        }

        public List<Proveedor> ListarActivos()
        {
            List<Proveedor> lista = new List<Proveedor>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Proveedor_ListarActivos", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Proveedor()
                            {
                                IdentificacionProveedor = dr["IdentificacionProveedor"].ToString(),
                                RazonSocial = dr["RazonSocial"].ToString(),
                                NombreContacto = dr["NombreContacto"] == DBNull.Value ? "" : dr["NombreContacto"].ToString(),
                                IdDistrito = dr["IdDistrito"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["IdDistrito"]),
                                DireccionExacta = dr["DireccionExacta"] == DBNull.Value ? "" : dr["DireccionExacta"].ToString(),
                                DiasCredito = dr["DiasCredito"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["DiasCredito"]),
                                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                                FechaModificacion = dr["FechaModificacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaModificacion"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Proveedor>();
            }

            return lista;
        }

        public int Registrar(Proveedor obj, out string Mensaje, out string identificacionProveedorGenerada)
        {
            int resultado = 0;
            Mensaje = string.Empty;
            identificacionProveedorGenerada = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Proveedor_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RazonSocial", obj.RazonSocial);
                    cmd.Parameters.AddWithValue("@NombreContacto", string.IsNullOrWhiteSpace(obj.NombreContacto) ? (object)DBNull.Value : obj.NombreContacto);
                    cmd.Parameters.AddWithValue("@IdDistrito", obj.IdDistrito.HasValue ? (object)obj.IdDistrito.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DireccionExacta", string.IsNullOrWhiteSpace(obj.DireccionExacta) ? (object)DBNull.Value : obj.DireccionExacta);
                    cmd.Parameters.AddWithValue("@DiasCredito", obj.DiasCredito.HasValue ? (object)obj.DiasCredito.Value : DBNull.Value);

                    cmd.Parameters.Add("@IdentificacionProveedorGenerada", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    identificacionProveedorGenerada = cmd.Parameters["@IdentificacionProveedorGenerada"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                Mensaje = ex.Message;
                identificacionProveedorGenerada = string.Empty;
            }

            return resultado;
        }

        public bool Editar(Proveedor obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Proveedor_Editar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", obj.IdentificacionProveedor);
                    cmd.Parameters.AddWithValue("@RazonSocial", obj.RazonSocial);
                    cmd.Parameters.AddWithValue("@NombreContacto", string.IsNullOrWhiteSpace(obj.NombreContacto) ? (object)DBNull.Value : obj.NombreContacto);
                    cmd.Parameters.AddWithValue("@IdDistrito", obj.IdDistrito.HasValue ? (object)obj.IdDistrito.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DireccionExacta", string.IsNullOrWhiteSpace(obj.DireccionExacta) ? (object)DBNull.Value : obj.DireccionExacta);
                    cmd.Parameters.AddWithValue("@DiasCredito", obj.DiasCredito.HasValue ? (object)obj.DiasCredito.Value : DBNull.Value);
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

        public bool Inactivar(string identificacionProveedor, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Persona.sp_Proveedor_Inactivar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdentificacionProveedor", identificacionProveedor);

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