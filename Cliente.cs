using System;

namespace WPF_GymProManager
{
    internal class Cliente
    {
        public DateTime FechaRegistro { get; set; }
        public string TipoMembresia { get; set; }
        public int DuracionMeses { get; set; }

        public int CalcularDiasRestantes()
        {
            // Calcular la fecha de vencimiento de la membresía
            DateTime fechaVencimiento = FechaRegistro.AddMonths(DuracionMeses);

            // Calcular la diferencia de días entre la fecha de vencimiento y la fecha actual
            int diasRestantes = (int)(fechaVencimiento - DateTime.Now).TotalDays;

            return diasRestantes;
        }
    }
}
