using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CapaNegocio.Facturacion
{
    public class CN_CorreoFactura
    {
        public bool EnviarFactura(
            string correoDestino,
            string numeroFactura,
            string cliente,
            string fecha,
            string cajero,
            string detalleHtml,
            string subtotal,
            string impuesto,
            string descuento,
            string total,
            out string mensaje)
        {
            mensaje = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(correoDestino))
                {
                    mensaje = "El cliente no tiene correo registrado.";
                    return false;
                }

                string correoSistema = ConfigurationManager.AppSettings["CorreoSistema"];
                string claveCorreoSistema = ConfigurationManager.AppSettings["ClaveCorreoSistema"];
                string smtpServidor = ConfigurationManager.AppSettings["SmtpServidor"];
                string nombreEmpresa = ConfigurationManager.AppSettings["NombreEmpresaCorreo"];

                int smtpPuerto = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPuerto"]);

                if (string.IsNullOrWhiteSpace(correoSistema) ||
                    string.IsNullOrWhiteSpace(claveCorreoSistema) ||
                    string.IsNullOrWhiteSpace(smtpServidor))
                {
                    mensaje = "La configuración del correo del sistema está incompleta.";
                    return false;
                }

                string asunto = "Factura " + numeroFactura + " - " + nombreEmpresa;

                string cuerpo = ConstruirCuerpoCorreo(
                    numeroFactura,
                    cliente,
                    fecha,
                    cajero,
                    detalleHtml,
                    subtotal,
                    impuesto,
                    descuento,
                    total,
                    nombreEmpresa
                );

                MailMessage correo = new MailMessage();
                correo.From = new MailAddress(correoSistema, nombreEmpresa);
                correo.To.Add(correoDestino);
                correo.Subject = asunto;
                correo.Body = cuerpo;
                correo.IsBodyHtml = true;
                correo.BodyEncoding = Encoding.UTF8;
                correo.SubjectEncoding = Encoding.UTF8;

                SmtpClient smtp = new SmtpClient(smtpServidor, smtpPuerto);
                smtp.Credentials = new NetworkCredential(correoSistema, claveCorreoSistema);
                smtp.EnableSsl = true;

                smtp.Send(correo);

                mensaje = "Factura enviada correctamente al correo del cliente.";
                return true;
            }
            catch (Exception ex)
            {
                mensaje = "No se pudo enviar el correo. Detalle: " + ex.Message;
                return false;
            }
        }

        private string ConstruirCuerpoCorreo(
            string numeroFactura,
            string cliente,
            string fecha,
            string cajero,
            string detalleHtml,
            string subtotal,
            string impuesto,
            string descuento,
            string total,
            string nombreEmpresa)
        {
            return @"
                <html>
                <body style='font-family: Arial, sans-serif; color:#222;'>
                    <h2>" + nombreEmpresa + @"</h2>
                    <h3>Factura " + numeroFactura + @"</h3>

                    <p><strong>Fecha:</strong> " + fecha + @"</p>
                    <p><strong>Cliente:</strong> " + cliente + @"</p>
                    <p><strong>Cajero:</strong> " + cajero + @"</p>

                    <hr />

                    <h4>Detalle de factura</h4>

                    <table style='width:100%; border-collapse:collapse;'>
                        <thead>
                            <tr>
                                <th style='border:1px solid #ccc; padding:6px; text-align:left;'>Producto</th>
                                <th style='border:1px solid #ccc; padding:6px; text-align:right;'>Cantidad</th>
                                <th style='border:1px solid #ccc; padding:6px; text-align:right;'>Precio</th>
                                <th style='border:1px solid #ccc; padding:6px; text-align:right;'>Total</th>
                            </tr>
                        </thead>
                        <tbody>
                            " + detalleHtml + @"
                        </tbody>
                    </table>

                    <br />

                    <table style='width:300px; margin-left:auto;'>
                        <tr>
                            <td><strong>Subtotal:</strong></td>
                            <td style='text-align:right;'>" + subtotal + @"</td>
                        </tr>
                        <tr>
                            <td><strong>IVA:</strong></td>
                            <td style='text-align:right;'>" + impuesto + @"</td>
                        </tr>
                        <tr>
                            <td><strong>Descuento:</strong></td>
                            <td style='text-align:right;'>" + descuento + @"</td>
                        </tr>
                        <tr>
                            <td><strong>Total:</strong></td>
                            <td style='text-align:right; font-size:18px;'><strong>" + total + @"</strong></td>
                        </tr>
                    </table>

                    <hr />

                    <p>Gracias por su compra.</p>
                </body>
                </html>";
        }
    }
}