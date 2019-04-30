using Newtonsoft.Json;
using System.Collections.Generic;

namespace WsDireccionamientoMiPresClass.Modelos
{

    public class MinRespuesta
    {
        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("ModelState")]
        public ModelState ModelState { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("IdDireccionamiento")]
        public int IdDireccionamiento { get; set; }

        [JsonProperty("Errors")]
        public IList<string> Errors { get; set; }
    }
    public class ModelState
    {
        [JsonProperty("direccionamiento.NoPrescripcion")]
        public IList<string> DireccionamientoNoPrescripcion { get; set; }
        [JsonProperty("direccionamiento.TipoTec")]
        public IList<string> DireccionamientoTipoTec { get; set; }

        [JsonProperty("direccionamiento.ConTec")]
        public IList<string> DireccionamientoConTec { get; set; }

        [JsonProperty("direccionamiento.TipoIDPaciente")]
        public IList<string> DireccionamientoTipoIDPaciente { get; set; }

        [JsonProperty("direccionamiento.NoIDPaciente")]
        public IList<string> DireccionamientoNoIDPaciente { get; set; }

        [JsonProperty("direccionamiento.NoEntrega")]
        public IList<string> DireccionamientoNoEntrega { get; set; }

        [JsonProperty("direccionamiento.NoSubEntrega")]
        public IList<string> DireccionamientoNoSubEntrega { get; set; }

        [JsonProperty("direccionamiento.TipoIDProv")]
        public IList<string> DireccionamientoTipoIDProv { get; set; }

        [JsonProperty("direccionamiento.NoIDProv")]
        public IList<string> DireccionamientoNoIDProv { get; set; }

        [JsonProperty("direccionamiento.CodMunEnt")]
        public IList<string> DireccionamientoCodMunEnt { get; set; }

        [JsonProperty("direccionamiento.FecMaxEnt")]
        public IList<string> DireccionamientoFecMaxEnt { get; set; }

        [JsonProperty("direccionamiento.CantTotAEntregar")]
        public IList<string> DireccionamientoCantTotAEntregar { get; set; }

        [JsonProperty("direccionamiento.DirPaciente")]
        public IList<string> DireccionamientoDirPaciente { get; set; }

        [JsonProperty("direccionamiento.CodSerTecAEntregar")]
        public IList<string> DireccionamientoCodSerTecAEntregar { get; set; }

        [JsonProperty("direccionamiento")]
        public IList<string> Direccionamiento { get; set; }
    }
}

