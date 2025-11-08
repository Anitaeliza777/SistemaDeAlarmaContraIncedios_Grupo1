using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Biblioteca_PanelCentral;
using Biblioteca_Registro;
using Microsoft.Win32;
using NAudio;

namespace AlarmaContraIncendios_2025
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PanelCentral panel = new PanelCentral();
            int opcion;
            Interfaz();
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("╭───────────────────────────────────────────────╮");
                Thread.Sleep(100);
                Console.WriteLine("│                 PANEL CENTRAL                 │");
                Console.WriteLine("│   SISTEMA DE ALARMA CONTRA INCENDIOS - SCI    │");
                Thread.Sleep(100);
                Console.WriteLine("╰───────────────────────────────────────────────╯\n");
                Thread.Sleep(100);
                Console.ResetColor();
                Console.WriteLine("1. Verificar turbogeneradores");
                Console.WriteLine("2. Ver registro de incidentes");
                Console.WriteLine("3. Activar modo comisionado (prueba)");
                Console.WriteLine("4. Activar estación manual (por zona)");
                Console.WriteLine("5. Salir\n");
                Console.Write("Seleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                    opcion = 0;

                switch (opcion)
                {
                    case 1: panel.VerificarZonas(); break;
                    case 2: Registro.Mostrar(); break;
                    case 3: panel.ModoComisionado(); break;
                    case 4: panel.EstacionManual(); break;
                    case 5:
                        Console.WriteLine("Saliendo del sistema...");
                        Thread.Sleep(800);
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente nuevamente.");
                        Thread.Sleep(800);
                        break;
                }
            } while (opcion != 5);
        }
        static void Interfaz()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("╭───────────────────────────────────────────────────────────────────────╮");
            string[] dibujo =
            {
            "│            ++++     ++++   +++++++++++    ++++      ++++              │",
            "│            ++++     ++++   ++++     +++   ++++++    ++++              │",
            "│            ++++     ++++   ++++     +++   ++++ ++   ++++              │",
            "│            ++++     ++++   +++++++++++    ++++  ++  ++++              │",
            "│            ++++     ++++   ++++           ++++   ++ ++++              │",
            "│            +++++++++++++   ++++           ++++    ++++++              │",
            "│            +++++++++++++   ++++           ++++     +++++              │"
            };

            foreach (var linea in dibujo)
            {
                foreach (char c in linea)
                {
                    if (c == '+')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(c);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(c);
                    }
                }
                Console.WriteLine();
                Thread.Sleep(100);
            }
            Console.WriteLine("│                                                                       │");
            Console.WriteLine("│                                                                       │");
            string texto = "'SISTEMA DE ALARMA CONTRA INCENDIOS'";
            int anchoTotal = 71;
            int margenIzq = (anchoTotal - texto.Length) / 2;

            Console.Write("│");
            Console.Write(new string(' ', margenIzq));
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(texto);
            Console.ResetColor();
            Console.WriteLine(new string(' ', anchoTotal - margenIzq - texto.Length) + "│");
            Console.WriteLine("│                                                                       │");
            Console.WriteLine("│            Ing. Encarnacion Otiniano, Erick Sebastian                 │");
            Console.WriteLine("│              Ing. Mercado Pereyra, Renato Fabricio                    │");
            Console.WriteLine("│              Ing. Sánchez Hernández, Hilter Alexis                    │");
            Console.WriteLine("│                Ing. Yopla Huamán, Ana Elizabeth                       │");
            Console.WriteLine("│                                                                       │");
            Console.WriteLine("│                                                                       │");
            Console.WriteLine("│                           GRUPO FDA SAC ®                             │");
            Console.WriteLine("│                                                                       │");
            Console.WriteLine("╰───────────────────────────────────────────────────────────────────────╯");
            Thread.Sleep(5000);
        }
    }
}
