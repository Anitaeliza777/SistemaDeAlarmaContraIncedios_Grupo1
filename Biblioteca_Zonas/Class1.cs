using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_Zonas
{
    public enum EstadoZona
    {
        NORMAL,
        RIESGO,
        INCENDIO
    }
    public enum TipoZona
    {
        SALA_GENERAL,
        COJINETES,
        GENERADOR,
        TRANSFORMADOR
    }

    public class Zona
    {
        public string Nombre { get; set; }
        public int Temperatura { get; set; }
        public int Humo { get; set; }
        public EstadoZona Estado { get; set; }
        public TipoZona Tipo { get; set; }

        private static readonly Random rangos = new Random();

        public Zona(string nombre, TipoZona tipoZona)
        {
            Nombre = nombre;
            Tipo = tipoZona;
            Temperatura = rangos.Next(20, 80);
            Humo = rangos.Next(0, 4);
            Estado = EstadoZona.NORMAL;
        }

        public void ActualizarSensores()
        {
            Temperatura = rangos.Next(20, 40);
            Humo = rangos.Next(0, 6);
            Evaluar(Tipo);
        }

        public void Evaluar(TipoZona tipoZona)
        {
            switch (Tipo)
            {
                case TipoZona.SALA_GENERAL:
                    if (Temperatura > 60 || Humo > 6)
                        Estado = EstadoZona.INCENDIO;
                    else if ((Temperatura > 40 && Temperatura <= 60) || (Humo > 3 && Humo <= 6))
                        Estado = EstadoZona.RIESGO;
                    else
                        Estado = EstadoZona.NORMAL;
                    break;

                case TipoZona.COJINETES:
                    if (Temperatura > 70 || Humo > 4)
                        Estado = EstadoZona.INCENDIO;
                    else if ((Temperatura > 55 && Temperatura <= 70) || (Humo > 3 && Humo <= 4))
                        Estado = EstadoZona.RIESGO;
                    else
                        Estado = EstadoZona.NORMAL;
                    break;

                case TipoZona.GENERADOR:
                    if (Temperatura > 100 || Humo > 4)
                        Estado = EstadoZona.INCENDIO;
                    else if ((Temperatura > 80 && Temperatura <= 100) || (Humo > 3 && Humo <= 4))
                        Estado = EstadoZona.RIESGO;
                    else
                        Estado = EstadoZona.NORMAL;
                    break;

                case TipoZona.TRANSFORMADOR:
                    if (Temperatura > 85 || Humo > 4)
                        Estado = EstadoZona.INCENDIO;
                    else if ((Temperatura > 65 && Temperatura <= 85) || (Humo > 3 && Humo <= 4))
                        Estado = EstadoZona.RIESGO;
                    else
                        Estado = EstadoZona.NORMAL;
                    break;

            }


        }
    }
}