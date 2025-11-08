using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using Biblioteca_Registro;
using Biblioteca_Zonas;
using Microsoft.Win32;
using NAudio.Wave;

namespace Biblioteca_PanelCentral
{
    public class PanelCentral
    {
        private List<List<Zona>> turbogeneradores = new List<List<Zona>>();
        private Random random = new Random();

        public PanelCentral()
        {
            for (int i = 1; i <= 5; i++)
            {
                turbogeneradores.Add(new List<Zona>()
                {
                    new Zona("SALA GENERAL", TipoZona.SALA_GENERAL),
                    new Zona("COJINETES", TipoZona.COJINETES),
                    new Zona("GENERADOR", TipoZona.GENERADOR),
                    new Zona("TRANSFORMADOR", TipoZona.TRANSFORMADOR)
                });
            }
        }

        public void VerificarZonas()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();
            Console.WriteLine("⇒ Accediendo al monitoreo de planta\n");
            Thread.Sleep(1000);
            ConsoleKey key;
            do
            {
                bool alerta = false;
                foreach (var turbo in turbogeneradores)
                {
                    foreach (var z in turbo)
                    {
                        z.ActualizarSensores();
                        if (z.Estado == EstadoZona.RIESGO)
                        {
                            alerta = true;
                            Registro.Guardar($"[RIESGO] Turbogenerador {turbogeneradores.IndexOf(turbo) + 1} - Zona: {z.Nombre} | T: {z.Temperatura}°C H: {z.Humo}%/m");

                        }
                        else if (z.Estado == EstadoZona.INCENDIO)
                        {
                            alerta = true;
                            Registro.Guardar($"[INCENDIO] Turbogenerador {turbogeneradores.IndexOf(turbo) + 1} - Zona: {z.Nombre} | T: {z.Temperatura}°C H: {z.Humo}%/m");
                        }
                    }
                }
                DibujarEstructura();
                if (alerta)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("¡ALERTA! "); Console.ResetColor(); Console.Write("Se han detectado condiciones de");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" RIESGO");
                    Console.ResetColor();
                    Console.Write(" o ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("INCENDIO.\n");
                    Console.ResetColor();
                    ReproducirSonido("incendio.mp3");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Todas las zonas operan dentro de parámetros seguros.");
                    Console.ResetColor();
                }
                Console.WriteLine("\nPresione ESC o ENTER para detener el monitoreo\n");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                for (int i = 5; i > 0; i--)
                {
                    Console.Write($"Actualizando en {i} segundos...  ");
                    Thread.Sleep(1000);
                    Console.SetCursorPosition(0, Console.CursorTop);
                }
                Console.ResetColor();
                Console.WriteLine();
            } while (!Console.KeyAvailable || ((key = Console.ReadKey(true).Key) != ConsoleKey.Escape && key != ConsoleKey.Enter));


            Console.WriteLine("\n═══ Monitoreo detenido. Presione cualquier tecla para volver al menú principal... ═══");
            Console.ReadKey();
        }


        private void DibujarEstructura()
        {
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("╔═════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("                                         MONITOREO GENERAL DE TURBOGENERADORES                                                     ");
            Console.WriteLine("╚═════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝\n");
            int ancho = 25;
            int alto = 7;

            for (int h = 0; h < alto; h++)
            {
                Console.Write("│");
                for (int col = 0; col < turbogeneradores.Count; col++)
                {
                    var turbo = turbogeneradores[col];

                    var estadoCritico = EstadoZona.NORMAL;
                    if (turbo.Any(z => z.Estado == EstadoZona.INCENDIO)) estadoCritico = EstadoZona.INCENDIO;
                    else if (turbo.Any(z => z.Estado == EstadoZona.RIESGO)) estadoCritico = EstadoZona.RIESGO;
                    ;

                    if (h == 0 || h == alto - 1)
                    {
                        Console.Write(new string('─', ancho));
                    }
                    else
                    {
                        if (estadoCritico == EstadoZona.INCENDIO)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (estadoCritico == EstadoZona.RIESGO)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.Green;
                        }

                        string texto = "";
                        if (h == 1)
                        {
                            texto = $"Turbogenerador {col + 1}";
                        }
                        else if (h >= 2 && h < 2 + turbo.Count)
                        {
                            var sub = turbo[h - 2];
                            string abreviado = sub.Tipo == TipoZona.SALA_GENERAL ? "SL" :
                                               sub.Tipo == TipoZona.COJINETES ? "C" :
                                               sub.Tipo == TipoZona.GENERADOR ? "G" : "T";
                            texto = $"{abreviado}:T:{sub.Temperatura}°C H:{sub.Humo}%/m";
                        }

                        int espacioIzq = Math.Max(0, (ancho - texto.Length) / 2);
                        int espacioDer = Math.Max(0, ancho - espacioIzq - texto.Length);
                        Console.Write(new string(' ', espacioIzq) + texto + new string(' ', espacioDer));
                        Console.ResetColor();
                    }

                    Console.Write("│");
                }
                Console.WriteLine();
            }

            Console.WriteLine(new string('═', ancho * 5 + 6));
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\nLEYENDA: SL = SALA GENERAL | C = COJINETES | G = GENERADOR | T = TRANSFORMADOR");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("        Verde = Zona segura");
            Console.ResetColor();
            Console.Write("        ||");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("        Amarillo = Riesgo");
            Console.ResetColor();
            Console.Write("        ||");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("        Rojo = Incendio\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n╒────────────────────────────────────────────────────────────────────────────────────────────────╕");
            Console.WriteLine("│                                 TABLA DE VALORES REFERENCIALES                                 │");
            Console.WriteLine("╘────────────────────────────────────────────────────────────────────────────────────────────────╛");
            Console.WriteLine("│          │   Sala General     │     Cojinetes      │     Generador       │    Transfromador    │");
            Console.WriteLine("│----------│--------------------│--------------------│---------------------│---------------------│");
            Console.WriteLine("│  Seguro  │ T<=40°C y H<=3%/m  │  T<=55°C & H>=3%/m │  T<=80°C & H<=3%/m  │  T<=65°C & H<=3%/m  │");
            Console.WriteLine("│----------│--------------------│--------------------│---------------------│---------------------│");
            Console.WriteLine("│  Alerta  │T:40°C - 60 y H>6/%m│T:56°C - 70 y H<5%/m│T:81°C - 100 y H<5%/m│ T:66°C - 85 y H<5%/m│");
            Console.WriteLine("│----------│------------------- │--------------------│---------------------│---------------------│");
            Console.WriteLine("│ Incendio │  T>60°C y H>6%/m   │  T>70°C & H>5%/m   │  T>100°C & H>5%/m   │   T>85°C & H>5%/m   │");
            Console.WriteLine("╘────────────────────────────────────────────────────────────────────────────────────────────────╛");
            Console.ResetColor();

        }


        public void ModoComisionado()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();
            Console.WriteLine("⇒ Accediendo al modo comisionado del sistema\n");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("===== MODO COMISIONADO (PRUEBA COMPLETA DEL SISTEMA) =====\n");
            Console.WriteLine("Este modo permite verificar que todos los componentes del sistema");
            Console.WriteLine("funcionan correctamente sin generar una emergencia real.\n");
            Console.WriteLine("Iniciando prueba del sistema...\n");
            Thread.Sleep(1000);

            Console.Write("-> Verificando sensores de temperatura...");
            Thread.Sleep(800);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" OK");
            Console.Beep(800, 500);
            Console.ResetColor();

            Console.Write("-> Verificando sensores de humo...");
            Thread.Sleep(800);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" OK");
            Console.Beep(800, 500);
            Console.ResetColor();

            Console.Write("-> Probando luces estroboscópicas...");
            Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" OK");
            Console.Beep(800, 500);
            Console.ResetColor();

            Console.Write("-> Probando alarma sonora...");
            Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" OK");
            Console.Beep(800, 500);
            Console.ResetColor();

            Console.Write("-> Verificando comunicación con panel central...");
            Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" OK");
            Console.Beep(800, 500);
            Console.ResetColor();

            int opcion = 0;
            do
            {
                Console.WriteLine("\nSeleccione una acción:");
                Console.WriteLine("1. Repetir prueba de sistema");
                Console.WriteLine("2. Guardar resultado en registro");
                Console.WriteLine("3. Regresar al menú principal");
                Console.Write("Opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                    opcion = 0;

                switch (opcion)
                {
                    case 1:
                        ModoComisionado();
                        return;
                    case 2:
                        Registro.Guardar("Prueba de sistema completada exitosamente (modo comisionado).");
                        Console.WriteLine("Resultado guardado en el registro.");
                        Thread.Sleep(800);
                        break;
                    case 3:
                        Console.WriteLine("Regresando al menú principal...");
                        Thread.Sleep(1000);
                        return;
                    default:
                        Console.WriteLine("Opción no válida, intente nuevamente.");
                        break;
                }
            } while (opcion != 3);
        }
        public void EstacionManual()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            int opcion;
            Random datos = new Random();
            int[] numeros_riesgo = new int[8];
            Console.Clear();
            Console.WriteLine("⇒ Accediendo a la estacion manual del sistema\n");
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("===== ESTACION MANUAL DEL SISTEMA =====\n");
            Console.WriteLine("Seleccione la zona que desea activar manualmente:\n");
            Console.WriteLine("[1] Turbogenerador 1\n[2] Turbogenerador 2\n[3] Turbogenerador 3\n[4] Turbogenerador 4\n[5] Turbogenerador 5");
            do
            {
                Console.Write("\nEscribe el número del Turbogenerador a activar (1-5): ");
                if (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > 5)
                {
                    Console.WriteLine("Opción no válida, intenta nuevamente.");
                    continue;
                }
                var sonido = new AudioFileReader("incendio.mp3");
                var player = new WaveOutEvent();
                player.Init(sonido);
                player.Play();

                for (int i = 0; i < 5; i++)
                {

                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("\n\n\n\n\n");
                    Console.WriteLine($"          ¡¡INCENDIO!! (TURBOGENERADOR {opcion})          ");
                    Console.WriteLine("\n\n\n\n\n");
                    Thread.Sleep(500);

                    Console.ResetColor();
                    Console.Clear();
                    Thread.Sleep(300);
                }
                player.Stop();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nTurbogenerador {opcion}:");
                string[] zonas = { "Sala General", "Cojinetes", "Generador", "Transformador" };
                foreach (var zona in zonas)
                {
                    int temp = datos.Next(60, 120);
                    int humo = datos.Next(5, 10);
                    Console.WriteLine($"{zona}: Temperatura -> {temp}°C || Humo -> {humo}%/m");
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n¡Alarma de INCENDIO activada manualmente!\n");
                Registro.Guardar($"[INCENDIO] Turbogenerador {opcion} - Activación manual de alarma de incendio.");
                Console.ResetColor();

                Console.WriteLine("\n[1] Repetir Estación Manual\n[2] Salir al menú principal");
                if (!int.TryParse(Console.ReadLine(), out int opcionMenu))
                    opcionMenu = 0;

                if (opcionMenu == 2)
                {
                    Console.WriteLine("Regresando al menú principal...");
                    Thread.Sleep(1000);
                    return;
                }

            } while (true);
        }
        private void ReproducirSonido(string archivo)
        {
            try
            {
                var sonido = new AudioFileReader(archivo);
                var player = new WaveOutEvent();
                player.Init(sonido);
                player.Play();
                Thread.Sleep(4000);
                player.Stop();
            }
            catch { }
        }
    }
}