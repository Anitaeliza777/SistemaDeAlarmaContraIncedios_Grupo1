using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_Registro
{
    public static class Registro
    {
        private static List<string> eventos = new List<string>();

        public static void Guardar(string mensaje)
        {
            string log = $"[{DateTime.Now:HH:mm:ss}] {mensaje}";
            eventos.Add(log);
        }

        public static void Mostrar()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("      HISTORIAL DE INCIDENTES");
            Console.WriteLine("╚════════════════════════════════════╝\n");
            Console.ResetColor();
            if (eventos.Count == 0)
                Console.WriteLine("No se registran incidentes hasta el momento.");
            else
            {
                foreach (var e in eventos)
                {
                    if (e.Contains("INCENDIO"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (e.Contains("RIESGO"))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else
                    {
                        Console.ResetColor();
                    }

                    Console.WriteLine(e);
                    Console.ResetColor();
                }
            }
            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }
    }
}
