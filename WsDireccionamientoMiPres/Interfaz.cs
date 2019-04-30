using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using WsDireccionamientoMiPresClass;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using WsDireccionamientoMiPresClass.Modelos;
using Anular;
using System.Timers;

namespace WsDireccionamientoMiPres
{
    public partial class Interfaz : ServiceBase
    {
        //Control de proceso de interfaz paralelismo cero
        bool blBandera = false;
        private WebResponse m_Resp = null;
        Timer timer = new Timer(); // name space(using System.Timers;)  

        public Interfaz()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: agregar código aquí para iniciar el servicio.
            EventLog.WriteEntry("Inicio el proceso de direccionamiento! ", EventLogEntryType.Information);
            //WriteToFile("Service is started at " + DateTime.Now);

            timer.Elapsed += new ElapsedEventHandler(TimerInterfaz_Elapsed);

            timer.Interval = 5000; //number in milisecinds  

            timer.Enabled = true;
            TimerInterfaz.Start();
        }

        protected override void OnStop()
        {
            // TODO: agregar código aquí para realizar cualquier anulación necesaria para detener el servicio.
            EventLog.WriteEntry("Se detuvo el proceso de direccionamiento! ", EventLogEntryType.Information);
            TimerInterfaz.Stop();
        }

        private void TimerInterfaz_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimerInterfaz.Interval = Convert.ToDouble(ConfigurationSettings.AppSettings["TimerProceso"].ToString());
            if (blBandera) return;

            string MsjError = string.Empty;
            string Nit = "";
            string Token = "";
            string digits = "200";

            #region Direccionamiento
            try
            {
                //Obtener Url Y metodo a invocar
                ClsDireccionamientoMiPres ObjClsDireccionamientoMiPres = new ClsDireccionamientoMiPres();

                var ParametrizacionDireccionamiento = ObjClsDireccionamientoMiPres.ConsultarParametrizacionMipres("Direccionamiento/", ref MsjError);

                string Url = ParametrizacionDireccionamiento.Rows[0][0].ToString();
                string Metodo = ParametrizacionDireccionamiento.Rows[0][1].ToString();
                string strNoPrescripcion = "";
                Nit = ParametrizacionDireccionamiento.Rows[0][2].ToString();
                Token = ParametrizacionDireccionamiento.Rows[0][3].ToString();

                //Obtenemos los Datos que se van a procesar
                var DtDireccionamientoPendiente = ObjClsDireccionamientoMiPres.ConsultarDireccionamientosPendientes(ref MsjError);

                if (DtDireccionamientoPendiente.Rows.Count > 0)
                {
                    //Convertir los Datos a Formato JSON
                    var strDatos = DataTableToJSONWithJavaScriptSerializer(DtDireccionamientoPendiente);
                    if (strDatos != "[]")
                    {
                        strNoPrescripcion = DtDireccionamientoPendiente.Rows[0][0].ToString();

                        //Se eliminan las llaves rectas del JSON para que sean procesados por el servicio web del ministerio
                        strDatos = strDatos.Replace("[", "");
                        strDatos = strDatos.Replace("]", "");

                        //ConsultarNUAMiPres
                        var DtNUAMiPres = ObjClsDireccionamientoMiPres.ConsultarNUAMiPresPendiente(strNoPrescripcion, ref MsjError);

                        //Se inicia la peticion hacia el ministerio
                        using (var client = new WebClient())
                        {

                            try
                            {
                                //Generacion de la url
                                Uri uri = new Uri(Url.Trim() + Metodo.Trim() + Nit.Trim() + Token.Trim());

                                //Definicion de tipo de contenido de la peticion
                                client.Headers.Add("Content-Type", "text/json");

                                //Relizamos Peticion                        

                                var rawResponse = client.UploadString(uri, "PUT", strDatos.ToString());

                                rawResponse = rawResponse.Replace("[", "");
                                rawResponse = rawResponse.Replace("]", "");

                                MinRespuesta result = JsonConvert.DeserializeObject<MinRespuesta>(rawResponse);

                                if (!string.IsNullOrEmpty(rawResponse))
                                {
                                    ObjClsDireccionamientoMiPres.RegistrarREsultado(strNoPrescripcion, digits, rawResponse
                                                                                                             , string.Empty
                                                                                                             , string.Empty
                                                                                                             , string.Empty
                                                                                                             , string.Empty
                                                                                                             , string.Empty
                                                                                                             , string.Empty
                                                                                                             , string.Empty
                                                                                                             , string.Empty
                                                                                                             , string.Empty
                                                                                                             , string.Empty
                                                                                                             , string.Empty
                                                                                                             , string.Empty
                                                                                                             , string.Empty
                                                                                                             , string.Empty
                                                                                                             , result.Id.ToString()
                                                                                                             , result.IdDireccionamiento.ToString()
                                                                                                             , string.Empty
                                                                                                             , DtNUAMiPres.Rows[0][0].ToString()
                                                                                                             , DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")
                                                                                                             , "Enviado");
                                }

                            }

                            catch (WebException ex)
                            {
                                //WriteToFile("Simple Service Error on: {0} " + ex.Message + ex.StackTrace);
                                using (StreamReader r = new StreamReader(ex.Response.GetResponseStream()))
                                {
                                    Match match;
                                    //Se obtiene el codigo de respuesta Ej (400-200)
                                    match = Regex.Match(ex.Message, @"\((\d+)\)");
                                    if (match.Success)
                                    {
                                        digits = match.Groups[1].Value;
                                    }
                                    //Mensaje que respondio el servicio
                                    string responseContent = r.ReadToEnd();

                                    MinRespuesta result = JsonConvert.DeserializeObject<MinRespuesta>(responseContent);

                                    if (result.ModelState == null)
                                    {
                                        ObjClsDireccionamientoMiPres.RegistrarREsultado(strNoPrescripcion, digits, result.Message
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , string.Empty
                                                                                                                 , result.Errors[0]
                                                                                                                 , DtNUAMiPres.Rows[0][0].ToString()
                                                                                                                 , ""
                                                                                                                 , "Fallido");

                                    }
                                    else
                                    {
                                        ObjClsDireccionamientoMiPres.RegistrarREsultado(strNoPrescripcion, digits, result.Message
                                                    , result.ModelState.DireccionamientoNoPrescripcion == null ? "" : result.ModelState.DireccionamientoNoPrescripcion[0].ToString()
                                                    , result.ModelState.DireccionamientoTipoTec == null ? "" : result.ModelState.DireccionamientoTipoTec[0].ToString()
                                                    , result.ModelState.DireccionamientoConTec == null ? "" : result.ModelState.DireccionamientoConTec[0].ToString()
                                                    , result.ModelState.DireccionamientoTipoIDPaciente == null ? "" : result.ModelState.DireccionamientoTipoIDPaciente[0].ToString()
                                                    , result.ModelState.DireccionamientoNoIDPaciente == null ? "" : result.ModelState.DireccionamientoNoIDPaciente[0].ToString()
                                                    , result.ModelState.DireccionamientoNoEntrega == null ? "" : result.ModelState.DireccionamientoNoEntrega[0].ToString()
                                                    , result.ModelState.DireccionamientoNoSubEntrega == null ? "" : result.ModelState.DireccionamientoNoSubEntrega[0].ToString()
                                                    , result.ModelState.DireccionamientoTipoIDProv == null ? "" : result.ModelState.DireccionamientoTipoIDProv[0].ToString()
                                                    , result.ModelState.DireccionamientoNoIDProv == null ? "" : result.ModelState.DireccionamientoNoIDProv[0].ToString()
                                                    , result.ModelState.DireccionamientoCodMunEnt == null ? "" : result.ModelState.DireccionamientoCodMunEnt[0].ToString()
                                                    , result.ModelState.DireccionamientoFecMaxEnt == null ? "" : result.ModelState.DireccionamientoFecMaxEnt[0].ToString()
                                                    , result.ModelState.DireccionamientoCantTotAEntregar == null ? "" : result.ModelState.DireccionamientoCantTotAEntregar[0].ToString()
                                                    , result.ModelState.DireccionamientoDirPaciente == null ? "" : result.ModelState.DireccionamientoDirPaciente[0].ToString()
                                                    , result.ModelState.DireccionamientoCodSerTecAEntregar == null ? "" : result.ModelState.DireccionamientoCodSerTecAEntregar[0].ToString()
                                                    , string.Empty
                                                    , string.Empty
                                                    , result.Errors[0]
                                                    , DtNUAMiPres.Rows[0][0].ToString()
                                                    , ""
                                                    , "Fallido");
                                    }
                                }
                            }

                        }
                    }
                }
                else
                {
                    //  WriteToFile("No se encontraron direccionamientos pendientes de envio " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                    EventLog.WriteEntry("No se encontraron direccionamientos pendientes de envio" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                //WriteToFile("Simple Service Error on: {0} " + ex.Message + ex.StackTrace);
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }

            #endregion


            #region AnularDireccionamiento

            try
            {
                //Obtener Url Y metodo a invocar
                ClsDireccionamientoMiPres ObjClsDireccionamientoMiPres = new ClsDireccionamientoMiPres();
                var ParametrizacionDireccionamiento = ObjClsDireccionamientoMiPres.ConsultarParametrizacionMipres("AnularDireccionamiento/", ref MsjError);


                string Url = ParametrizacionDireccionamiento.Rows[0][0].ToString();
                string Metodo = ParametrizacionDireccionamiento.Rows[0][1].ToString();
                string strNoPrescripcion = "";
                Nit = ParametrizacionDireccionamiento.Rows[0][2].ToString();
                Token = ParametrizacionDireccionamiento.Rows[0][3].ToString();

                //Armado de la Url para realizar la solicitud de la api
                string strNoPrescripcionAnular = "";
                string Direccionamiento = "";

                //Obtenemos los Datos que se van a procesar
                var DtDireccionamientoPendiente = ObjClsDireccionamientoMiPres.ConsultarDireccionamientosAnular(ref MsjError);
                //DtDireccionamientoPendiente.Rows[0][0]

                if (DtDireccionamientoPendiente.Rows.Count > 0)
                {
                    strNoPrescripcionAnular = (DtDireccionamientoPendiente.Rows[0][1]).ToString();

                    //ConsultarNUAMiPres
                    var DtNUAMiPres = ObjClsDireccionamientoMiPres.ConsultarNUAMiPresAnular(strNoPrescripcionAnular, ref MsjError);
                    //Se inicia la peticion hacia el ministerio
                    using (var client = new WebClient())
                    {

                        try
                        {

                            Direccionamiento = (DtDireccionamientoPendiente.Rows[0][0]).ToString();


                            //Generacion de la url
                            Uri uri = new Uri(Url.Trim() + Metodo.Trim() + Nit.Trim() + Token.Trim() + "/" + Direccionamiento);

                            //Definicion de tipo de contenido de la peticion
                            //client.Headers.Add("Content-Type", "text/json");

                            //Relizamos Peticion                        

                            var rawResponse = client.UploadString(uri, "PUT", "");


                            rawResponse = rawResponse.Replace("[", "");
                            rawResponse = rawResponse.Replace("]", "");

                            AnularDireccionamiento result = JsonConvert.DeserializeObject<AnularDireccionamiento>(rawResponse);

                            if (!string.IsNullOrEmpty(rawResponse))
                            {
                                ObjClsDireccionamientoMiPres.RegistrarResultadoAnular(strNoPrescripcionAnular, Direccionamiento, string.Empty, string.Empty, result.Mensaje, DtNUAMiPres.Rows[0][0].ToString(), "Anulado");
                            }
                        }
                        catch (WebException ex)
                        {
                            //WriteToFile("Simple Service Error on: {0} " + ex.Message + ex.StackTrace);
                            using (StreamReader r = new StreamReader(ex.Response.GetResponseStream()))
                            {
                                string responseContent = r.ReadToEnd();
                                Match match;
                                //Se obtiene el codigo de respuesta Ej (400-200)
                                match = Regex.Match(ex.Message, @"\((\d+)\)");
                                if (match.Success)
                                {
                                    digits = match.Groups[1].Value;
                                }
                                //Mensaje que respondio el servicio

                                AnularDireccionamiento result = JsonConvert.DeserializeObject<AnularDireccionamiento>(responseContent);

                                if (result.Errors != null)
                                {
                                    ObjClsDireccionamientoMiPres.RegistrarResultadoAnular(strNoPrescripcionAnular, Direccionamiento, string.Empty, result.Errors[0], result.Message, DtNUAMiPres.Rows[0][0].ToString(), "Anulado Fallido");
                                }
                            }

                            //EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                        }
                    }
                }
                else
                {
                    //WriteToFile("No se encontraron direccionamientos pendientes de anular " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                    EventLog.WriteEntry("No se encontraron direccionamientos pendientes de anular" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                //WriteToFile("Simple Service Error on: {0} " + ex.Message + ex.StackTrace);
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
            #endregion

            blBandera = false;

        }

        public string DataTableToJSONWithJavaScriptSerializer(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in table.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return jsSerializer.Serialize(rows);
        }

        private string GetToken()
        {
            string Token = new AppSettingsReader().GetValue("TOKEN", typeof(System.String)).ToString();
            string TokenSecurity = EncriptarDesencriptar(Token, false);
            return TokenSecurity;
        }

        public string EncriptarDesencriptar(string valor, bool tipo)
        {

            string valorRetornado = "";

            try
            {
                switch (tipo)
                {
                    case true:
                        valorRetornado = EncriptarClaves.clsEncriptarClases.Encrypt(valor);
                        break; // TODO: might not be correct. Was : Exit Select

                    case false:
                        valorRetornado = EncriptarClaves.clsEncriptarClases.Decrypt(valor);
                        break; // TODO: might not be correct. Was : Exit Select

                }

            }
            catch (Exception ex)
            {
                //Logger.generarLogError(ex.Message, new StackFrame(true), ex);
            }

            return valorRetornado;

        }

        private void WriteToFile(string text)
        {
            string path = "C:\\ServiceLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
            }
        }
    }
}
