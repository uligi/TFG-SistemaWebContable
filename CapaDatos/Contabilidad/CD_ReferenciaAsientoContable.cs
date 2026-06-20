using CapaEntidad.Contabilidad;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos.Contabilidad
{
    public class CD_ReferenciaAsientoContable
    {
        public bool Existe(
            string moduloOrigen,
            string documentoOrigen,
            string tipoMovimiento,
            out string numeroAsiento
        )
        {
            bool existe = false;
            numeroAsiento = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_ReferenciaAsientoContable_Existe", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ModuloOrigen", moduloOrigen);
                    cmd.Parameters.AddWithValue("@DocumentoOrigen", documentoOrigen);
                    cmd.Parameters.AddWithValue("@TipoMovimiento", tipoMovimiento);

                    SqlParameter parametroExiste = new SqlParameter("@Existe", SqlDbType.Bit);
                    parametroExiste.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parametroExiste);

                    SqlParameter parametroNumeroAsiento = new SqlParameter("@NumeroAsiento", SqlDbType.VarChar, 45);
                    parametroNumeroAsiento.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parametroNumeroAsiento);

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    existe = Convert.ToBoolean(parametroExiste.Value);
                    numeroAsiento = parametroNumeroAsiento.Value == null
                        ? string.Empty
                        : parametroNumeroAsiento.Value.ToString();
                }
            }
            catch
            {
                existe = false;
                numeroAsiento = string.Empty;
            }

            return existe;
        }

        public int Registrar(
            ReferenciaAsientoContable obj,
            out string mensaje
        )
        {
            int resultado = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Contabilidad.sp_ReferenciaAsientoContable_Registrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroAsiento", obj.NumeroAsiento);
                    cmd.Parameters.AddWithValue("@ModuloOrigen", obj.ModuloOrigen);
                    cmd.Parameters.AddWithValue("@DocumentoOrigen", obj.DocumentoOrigen);
                    cmd.Parameters.AddWithValue("@TipoMovimiento", obj.TipoMovimiento);

                    SqlParameter parametroResultado = new SqlParameter("@Resultado", SqlDbType.Int);
                    parametroResultado.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parametroResultado);

                    SqlParameter parametroMensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500);
                    parametroMensaje.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(parametroMensaje);

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(parametroResultado.Value);
                    mensaje = parametroMensaje.Value == null
                        ? string.Empty
                        : parametroMensaje.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                mensaje = ex.Message;
            }

            return resultado;
        }
    }
}