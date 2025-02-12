using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Examen1
{
    class Program
    {
        static void Main()
        {
            bool reiniciar = true; // Variable para repetir el programa si el usuario lo desea

            while (reiniciar) // Ciclo que mantiene el programa en ejecución hasta que el usuario decida salir
            {
                Console.WriteLine("Bienvenido al sistema de cálculo de salarios.");

                // Definimos las actividades y sus tarifas por hora
                string actividad1 = "Operario";
                double tarifaOrdinaria1 = 10.0;

                string actividad2 = "Ayudante";
                double tarifaOrdinaria2 = 12.0;

                string actividad3 = "Arquitecto";
                double tarifaOrdinaria3 = 20.0;

                // Mostramos las opciones disponibles para que el usuario elija
                Console.WriteLine("Seleccione la actividad del empleado:");
                Console.WriteLine($"1- {actividad1}");
                Console.WriteLine($"2- {actividad2}");
                Console.WriteLine($"3- {actividad3}");

                // Pedimos al usuario que seleccione una actividad
                int seleccion = SolicitarEntero("Ingrese el número de la actividad: ", 1, 3);

                string actividadSeleccionada;
                double tarifaOrdinaria;

                // Asignamos la actividad y la tarifa según la selección del usuario
                if (seleccion == 1)
                {
                    actividadSeleccionada = actividad1;
                    tarifaOrdinaria = tarifaOrdinaria1;
                }
                else if (seleccion == 2)
                {
                    actividadSeleccionada = actividad2;
                    tarifaOrdinaria = tarifaOrdinaria2;
                }
                else
                {
                    actividadSeleccionada = actividad3;
                    tarifaOrdinaria = tarifaOrdinaria3;
                }

                double tarifaEspecial = tarifaOrdinaria * 1.5; // La tarifa especial es el 1.5 de la ordinaria

                // Mostramos la actividad seleccionada y las tarifas correspondientes
                Console.WriteLine($"\nActividad seleccionada: {actividadSeleccionada}");
                Console.WriteLine($"Tarifa por hora ordinaria: {tarifaOrdinaria:C}");
                Console.WriteLine($"Tarifa por hora especial: {tarifaEspecial:C}");

                // Solicitamos información adicional
                double bonoPorNoFaltas = SolicitarDouble("Ingrese el bono por no faltas en la semana: ");
                double deduccionPorRetraso = SolicitarDouble("Ingrese la deducción por retraso: ");
                int limiteRetraso = SolicitarEntero("Ingrese el límite de retraso en minutos: ");
                int diasTrabajados = SolicitarEntero("Ingrese el número de días trabajados en la semana: ");

                // Validamos que el empleado no trabaje menos de 2 días
                if (diasTrabajados < 2)
                {
                    Console.WriteLine("Error: Un empleado no puede trabajar menos de 2 días en la semana.");
                    return;
                }

                // Variables para el cálculo de horas y deducciones
                double horasOrdinarias = 0;
                double horasEspeciales = 0;
                double horasExtraPendientes = 0;
                double deducciones = 0;
                double deduccionesCCSS = 0.0917; // Deducción del 9.17% del salario bruto
                int retrasos = 0;
                bool tieneFaltas = false;

                // Recorremos cada día trabajado
                for (int i = 0; i < diasTrabajados; i++)
                {
                    Console.WriteLine($"\nDía {i + 1}:");
                    DateTime horaEntrada = SolicitarHora("Ingrese la hora de entrada (hh:mm AM/PM): ");
                    DateTime horaSalida;

                    // Validamos que la hora de salida sea después de la de entrada
                    while (true)
                    {
                        horaSalida = SolicitarHora("Ingrese la hora de salida (hh:mm AM/PM): ");
                        if (horaSalida > horaEntrada)
                        {
                            break;
                        }
                        Console.WriteLine("Error: La hora de salida no puede ser anterior o igual a la hora de entrada. Intente nuevamente.");
                    }

                    TimeSpan diferencia = horaSalida - horaEntrada;

                    // Si se supera el límite de retraso, contamos un retraso
                    if (diferencia.TotalMinutes > limiteRetraso)
                    {
                        retrasos++;
                    }

                    // Validamos que el empleado trabaje al menos 4 horas al día
                    if (diferencia.TotalHours < 4)
                    {
                        Console.WriteLine("Error: El empleado no puede trabajar menos de 4 horas al día.");
                        return;
                    }

                    double horasDia = diferencia.TotalHours;

                    // Si trabajó más de 8 horas, preguntamos si acepta las horas extra
                    if (horasDia > 8)
                    {
                        Console.Write($"El empleado trabajó {horasDia} horas. ¿Aceptas horas extraordinarias? (si/no): ");
                        string respuesta = Console.ReadLine().ToLower();
                        if (respuesta == "si")
                        {
                            horasOrdinarias += 8;
                            horasEspeciales += (horasDia - 8);
                        }
                        else
                        {
                            horasOrdinarias += 8;
                            horasExtraPendientes += horasDia - 8; // Guardamos las horas extra no aprobadas
                        }
                    }
                    else
                    {
                        horasOrdinarias += horasDia;
                    }
                }

                // Calculamos deducciones por retrasos
                if (retrasos > 0)
                {
                    deducciones += retrasos * deduccionPorRetraso;
                }

                // Calculamos el salario bruto sumando horas ordinarias y especiales
                double salarioBruto = (horasOrdinarias * tarifaOrdinaria) + (horasEspeciales * tarifaEspecial);

                // Sumamos el bono si el empleado no tuvo faltas
                if (!tieneFaltas)
                {
                    salarioBruto += bonoPorNoFaltas;
                }

                // Aplicamos la deducción del 9.17% del salario bruto
                double salarioNeto = salarioBruto - (salarioBruto * deduccionesCCSS) - deducciones;

                // Mostramos los resultados finales
                Console.WriteLine($"\nResumen para {actividadSeleccionada}:");
                Console.WriteLine($"Horas Ordinarias Trabajadas: {horasOrdinarias:F1}");
                Console.WriteLine($"Horas Especiales Trabajadas: {horasEspeciales:F1}");
                Console.WriteLine($"Horas Extraordinarias Pendientes de Aprobación: {horasExtraPendientes:F1}");
                Console.WriteLine($"Salario Bruto: {salarioBruto:C}");
                Console.WriteLine($"Deducciones: {deducciones:C}");
                Console.WriteLine($"Salario Neto: {salarioNeto:C}");

                // Preguntamos si desea calcular otro salario
                Console.Write("\n¿Desea calcular otro salario? (si/no): ");
                string reiniciarRespuesta = Console.ReadLine().ToLower();
                reiniciar = (reiniciarRespuesta == "si");
            }
        }

        // Método para solicitar un número decimal y validarlo
        static double SolicitarDouble(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                if (double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double resultado))
                {
                    return resultado;
                }
                Console.WriteLine("Entrada inválida. Por favor, ingrese un número válido.");
            }
        }

        // Método para solicitar un número entero con validación
        static int SolicitarEntero(string mensaje, int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                Console.Write(mensaje);
                if (int.TryParse(Console.ReadLine(), out int resultado) && resultado >= min && resultado <= max)
                {
                    return resultado;
                }
                Console.WriteLine($"Entrada inválida. Por favor, ingrese un número entre {min} y {max}.");
            }
        }

        // Método para solicitar una hora en formato 12 horas con validación
        static DateTime SolicitarHora(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                if (DateTime.TryParseExact(Console.ReadLine(), "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime hora))
                {
                    return hora;
                }
                Console.WriteLine("Formato inválido. Use hh:mm AM/PM (ejemplo: 09:30 AM).");
            }
        }
    }
}

