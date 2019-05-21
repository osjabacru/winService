using System;
using DataLayer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace WsDireccionamientoMiPresClass
{
    public class ClsDireccionamientoMiPres
    {
        public DataTable ConsultarDireccionamientosPendientes(ref string MsjError)
        {

            clsDataServices objDatalayer = new clsDataServices();
            DataTable ObjDT = new DataTable();

            try
            {
                ObjDT = (DataTable)objDatalayer.ExecuteStoredProcedure("P_PS_MIPRES_INFORMACION_ENVIO", ReturnType.DatatableType);

                return ObjDT;
            }
            catch (Exception ex)
            {
                MsjError = ex.Message;
            }

            return ObjDT;
        }

        public void RegistrarREsultado(string strNoPrescripcion, string strCodigoRespuesta, string strRespuesta
                                     , string NoPrescripcionMsg, string TipoTec, string ConTec, string TipoIDPaciente
                                     , string NoIDPaciente, string NoEntrega, string NoSubEntrega, string TipoIDProv
                                     , string NoIDProv, string CodMunEnt, string FecMaxEnt, string CantTotAEntregar
                                     , string DirPaciente, string CodSerTecAEntregar, string Id, string IdDireccionamiento
                                     , string strErrors, string strNUA, string Fechadireccionamiento, string strEstado)
        {
            string MsjError = "";
            clsDataServices objDatalayer = new clsDataServices();
            DataTable ObjDT = new DataTable();

            try
            {
                objDatalayer.AddGenericParameter("@NOPRESCRIPCION", DbType.String, ParameterDirection.Input, strNoPrescripcion);
                objDatalayer.AddGenericParameter("@CODIGORESPUESTA", DbType.String, ParameterDirection.Input, strCodigoRespuesta);
                objDatalayer.AddGenericParameter("@RESPUESTA", DbType.String, ParameterDirection.Input, strRespuesta);

                objDatalayer.AddGenericParameter("@NoPrescripcionMsg", DbType.String, ParameterDirection.Input, NoPrescripcionMsg);
                objDatalayer.AddGenericParameter("@TipoTec", DbType.String, ParameterDirection.Input, TipoTec);
                objDatalayer.AddGenericParameter("@ConTec", DbType.String, ParameterDirection.Input, ConTec);
                objDatalayer.AddGenericParameter("@TipoIDPaciente", DbType.String, ParameterDirection.Input, TipoIDPaciente);
                objDatalayer.AddGenericParameter("@NoIDPaciente", DbType.String, ParameterDirection.Input, NoIDPaciente);
                objDatalayer.AddGenericParameter("@NoEntrega", DbType.String, ParameterDirection.Input, NoEntrega);
                objDatalayer.AddGenericParameter("@NoSubEntrega", DbType.String, ParameterDirection.Input, NoSubEntrega);
                objDatalayer.AddGenericParameter("@TipoIDProv", DbType.String, ParameterDirection.Input, TipoIDProv);
                objDatalayer.AddGenericParameter("@NoIDProv", DbType.String, ParameterDirection.Input, NoIDProv);
                objDatalayer.AddGenericParameter("@CodMunEnt", DbType.String, ParameterDirection.Input, CodMunEnt);
                objDatalayer.AddGenericParameter("@FecMaxEnt", DbType.String, ParameterDirection.Input, FecMaxEnt);
                objDatalayer.AddGenericParameter("@CantTotAEntregar", DbType.String, ParameterDirection.Input, CantTotAEntregar);
                objDatalayer.AddGenericParameter("@DirPaciente", DbType.String, ParameterDirection.Input, DirPaciente);
                objDatalayer.AddGenericParameter("@CodSerTecAEntregar", DbType.String, ParameterDirection.Input, CodSerTecAEntregar);
                objDatalayer.AddGenericParameter("@Id", DbType.String, ParameterDirection.Input, Id);
                objDatalayer.AddGenericParameter("@IdDireccionamiento", DbType.String, ParameterDirection.Input, IdDireccionamiento);
                objDatalayer.AddGenericParameter("@Errors", DbType.String, ParameterDirection.Input, strErrors);
                objDatalayer.AddGenericParameter("@AUTORIZACION", DbType.String, ParameterDirection.Input, strNUA);
                objDatalayer.AddGenericParameter("@FechaDireccionamiento", DbType.String, ParameterDirection.Input, Fechadireccionamiento); 

                objDatalayer.AddGenericParameter("@ESTADO", DbType.String, ParameterDirection.Input, strEstado);
                ObjDT = (DataTable)objDatalayer.ExecuteStoredProcedure("P_PS_MIPRES_RESULTADO_ENVIO", ReturnType.DatatableType);

            }
            catch (Exception ex)
            {
                MsjError = ex.Message;
            }

        }

        public DataTable ConsultarDireccionamientosAnular(ref string MsjError)
        {

            clsDataServices objDatalayer = new clsDataServices();
            DataTable ObjDT = new DataTable();

            try
            {
                ObjDT = (DataTable)objDatalayer.ExecuteStoredProcedure("P_PS_MIPRES_INFORMACION_ANULACION", ReturnType.DatatableType);

                return ObjDT;
            }
            catch (Exception ex)
            {
                MsjError = ex.Message;
            }

            return ObjDT;
        }

        public void RegistrarResultadoAnular(string strNoPrescripcion, string strIdDireccionamiento, string strIdAnulacion, string strError
                                            , string strMensaje, string strNUA, string strEstado)
        {
            string MsjError = "";
            clsDataServices objDatalayer = new clsDataServices();
            DataTable ObjDT = new DataTable();

            try
            {
                objDatalayer.AddGenericParameter("@NOPRESCRIPCION", DbType.String, ParameterDirection.Input, strNoPrescripcion);
                objDatalayer.AddGenericParameter("@IdDireccionamiento", DbType.String, ParameterDirection.Input, strIdDireccionamiento);
                objDatalayer.AddGenericParameter("@IdAnulacion", DbType.String, ParameterDirection.Input, strIdAnulacion);
                objDatalayer.AddGenericParameter("@Error", DbType.String, ParameterDirection.Input, strError);
                objDatalayer.AddGenericParameter("@Mensaje", DbType.String, ParameterDirection.Input, strMensaje);
                objDatalayer.AddGenericParameter("@ESTADO", DbType.String, ParameterDirection.Input, strEstado);
                objDatalayer.AddGenericParameter("@AUTORIZACION", DbType.String, ParameterDirection.Input, strNUA);
                ObjDT = (DataTable)objDatalayer.ExecuteStoredProcedure("P_PS_MIPRES_RESULTADO_ENVIO_ANULACION", ReturnType.DatatableType);

            }
            catch (Exception ex)
            {
                MsjError = ex.Message;
            }

        }

        public DataTable ConsultarParametrizacionMipres(string strMetodo, ref string MsjError)
        {

            clsDataServices objDatalayer = new clsDataServices();
            DataTable ObjDT = new DataTable();

            try
            {
                objDatalayer.AddGenericParameter("@METODO", DbType.String, ParameterDirection.Input, strMetodo);
                ObjDT = (DataTable)objDatalayer.ExecuteStoredProcedure("P_PS_MIPRES_CONSULTAR_PARAMETRIZACION_DIRECCIONAMIENTO", ReturnType.DatatableType);

                return ObjDT;
            }
            catch (Exception ex)
            {
                MsjError = ex.Message;
            }

            return ObjDT;
        }


        public DataTable ConsultarNUAMiPresPendiente(string strNoPrescripcion,string strConOrden, ref string MsjError)
        {

            clsDataServices objDatalayer = new clsDataServices();
            DataTable ObjDT = new DataTable();

            try
            {
                objDatalayer.AddGenericParameter("@NOPRESCRIPCION", DbType.String, ParameterDirection.Input, strNoPrescripcion);
                objDatalayer.AddGenericParameter("@CONORDEN", DbType.String, ParameterDirection.Input, strConOrden);
                ObjDT = (DataTable)objDatalayer.ExecuteStoredProcedure("P_PS_MIPRES_CONSULTAR_NUA_PENDIENTE", ReturnType.DatatableType);

                return ObjDT;
            }
            catch (Exception ex)
            {
                MsjError = ex.Message;
            }

            return ObjDT;
        }

        public DataTable ConsultarNUAMiPresAnular(string strNoPrescripcion, ref string MsjError)
        {

            clsDataServices objDatalayer = new clsDataServices();
            DataTable ObjDT = new DataTable();

            try
            {
                objDatalayer.AddGenericParameter("@NOPRESCRIPCION", DbType.String, ParameterDirection.Input, strNoPrescripcion);
                ObjDT = (DataTable)objDatalayer.ExecuteStoredProcedure("P_PS_MIPRES_CONSULTAR_NUA_ANULAR", ReturnType.DatatableType);

                return ObjDT;
            }
            catch (Exception ex)
            {
                MsjError = ex.Message;
            }

            return ObjDT;
        }
    }
}
