using Newtonsoft.Json;
using System.Collections.Generic;

namespace WsDireccionamientoMiPresClass.Modelos
{

    public class MinRespuesta
    {
        public string Message { get; set; }
        public ModelState ModelState { get; set; }
        public int Id { get; set; }
        public int IdDireccionamiento { get; set; }
        public IList<string> Errors { get; set; }
    }
    public class ModelState
    {
        public IList<string> DireccionamientoNoPrescripcion { get; set; }
        public IList<string> DireccionamientoTipoTec { get; set; }
        public IList<string> DireccionamientoConTec { get; set; }
        public IList<string> DireccionamientoTipoIDPaciente { get; set; }
        public IList<string> DireccionamientoNoIDPaciente { get; set; }
        public IList<string> DireccionamientoNoEntrega { get; set; }
        public IList<string> DireccionamientoNoSubEntrega { get; set; }
        public IList<string> DireccionamientoTipoIDProv { get; set; }
        public IList<string> DireccionamientoNoIDProv { get; set; }
        public IList<string> DireccionamientoCodMunEnt { get; set; }
        public IList<string> DireccionamientoFecMaxEnt { get; set; }
        public IList<string> DireccionamientoCantTotAEntregar { get; set; }
        public IList<string> DireccionamientoDirPaciente { get; set; }
        public IList<string> DireccionamientoCodSerTecAEntregar { get; set; }
        public IList<string> Direccionamiento { get; set; }
    }
}

