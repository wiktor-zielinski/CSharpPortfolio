using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CSharpPortfolio
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            bool running = true;

            // Menu
            while (running)
            {
                // Title box
                int boxWidth = 50;
                string title = "PORTFOLIO - WIKTOR ZIELIŃSKI";
                string border = new string('-', boxWidth);
                int leftPadding = (boxWidth - 2 + title.Length) / 2;

                Console.WriteLine(border);
                Console.WriteLine("|" + title.PadLeft(leftPadding).PadRight(boxWidth - 2) + "|");
                Console.WriteLine(border);
                Console.WriteLine("0 - Zakończ program");
                Console.WriteLine("1 - Kalkulator emerytalny - z konsoli");
                Console.WriteLine("2 - Kalkulator emerytalny - z pliku csv");
                Console.WriteLine("3 - Objętość stożka");
                Console.WriteLine("4 - Równanie kwadratowe");
                Console.WriteLine("5 - Parametry trójkąta");
                Console.WriteLine(border);
                Console.WriteLine("Twój wybór: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "0": running = false; break;
                    case "1": RetirementCalculator(); Thread.Sleep(3000); break;
                    case "2": RetirementCalculatorCSV("users.csv"); Thread.Sleep(3000); break;
                    case "3": ConeVolume(); Thread.Sleep(3000); break;
                    case "4": Quadratic(); Thread.Sleep(3000); break;
                    case "5": TriangleParameters(); Thread.Sleep(3000); break;
                }
            }


            void RetirementCalculator()
            {
                bool validInput = false;
                Console.WriteLine(" --- ZADANIE 1: KALKULATOR EMERYTALNY --- ");

                while (!validInput)
                {
                    Console.WriteLine("Podaj Nazwisko, Aktualny wiek oraz próg emerytalny (oddzielone spacją)");

                    var input = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (input == null || input.Length < 3)
                    {
                        Console.WriteLine("Błąd: Nieprawidłowa ilość danych wejściowych. Spróbuj ponownie.");
                        continue;
                    }

                    string surname = input[0];
                    if (surname.Any(char.IsDigit) || !int.TryParse(input[1], out int age) || !int.TryParse(input[2], out int threshold))
                    {
                        Console.WriteLine("Błąd: Nieprawidłowy typ danych. Spróbuj ponownie.");
                        continue;
                    }

                    if (age < 0 || threshold < 0)
                    {
                        Console.WriteLine("Błąd: Wiek nie może być ujemny. Spróbuj ponownie.");
                        continue;
                    }

                    else
                    {
                        int difference = threshold - age;
                        int lastDigit = difference % 10;
                        int lastTwoDigits = difference % 100;
                        string output = (difference == 1) ? "rok!" : (lastDigit >= 2 && lastDigit <= 4 && (lastTwoDigits < 12 || lastTwoDigits > 14)) ? "lata!" : "lat!";
                        Console.WriteLine($"Do emerytury zostało ci: {difference} {output}");
                        validInput = true;
                    }

                }
            }
            static void RetirementCalculatorCSV(string filePath)
            {
                Console.WriteLine(" --- ZADANIE 2: KALKULATOR EMERYTALNY CSV --- ");
                try
                {
                    string[] lines = File.ReadAllLines(filePath);
                    Console.WriteLine($"Znaleziono {lines.Length - 1} wpisów do przetworzenia.");

                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] data = lines[i].Split(',');

                        if (data.Length < 3)
                        {
                            Console.WriteLine($"Błąd: Nieprawidłowa ilość danych wejściowych. Pominięto wiersz {i}");
                            continue;
                        }
                        string surname = data[0];
                        if (surname.Any(char.IsDigit) || !int.TryParse(data[1], out int age) || !int.TryParse(data[2], out int threshold))
                        {
                            Console.WriteLine($"Błąd: Nieprawidłowy typ danych. Pominięto wiersz {i}");
                            continue;
                        }
                        if (age < 0 || threshold < 0)
                        {
                            Console.WriteLine($"Błąd: Wiek nie może być ujemny. Spróbuj ponownie. Pominięto wiersz {i}");
                            continue;
                        }

                        else
                        {
                            int difference = threshold - age;
                            int lastDigit = difference % 10;
                            int lastTwoDigits = difference % 100;
                            string output = (difference == 1) ? "rok!" : (lastDigit >= 2 && lastDigit <= 4 && (lastTwoDigits < 12 || lastTwoDigits > 14)) ? "lata!" : "lat!";
                            Console.WriteLine($"[{i}] ({surname}, {age}) brakuje do emerytury: {difference} {output}");
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($"Błąd krytyczny: Nie znaleziono pliku '{filePath}'. Upewnij się, że plik istnieje.");
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine($"Błąd krytyczny: Brak uprawnień do odczytu pliku '{filePath}'.");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Błąd krytyczny: Plik jest używany przez inny program lub wystąpił błąd dysku. Szczegóły: {ex.Message}");
                }
            }

            void ConeVolume()
            {
                bool validInput = false;
                Console.WriteLine(" --- ZADANIE 3: OBJĘTOŚĆ STOŻKA --- ");

                while (!validInput)
                {
                    Console.WriteLine("Podaj promień podstawy \"r\" oraz tworzącą \"l\" z przedziału [0 - 1 000 000] (oddzielone spacją)");
                    var input = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (input == null || input.Length < 2)
                    {
                        Console.WriteLine("Nieprawidłowa ilość danych wejściowych. Spróbuj ponownie.");
                        continue;
                    }
                    if (!int.TryParse(input[0], out int radius) || !int.TryParse(input[1], out int slant))
                    {
                        Console.WriteLine("Nieprawidłowy typ danych. Spróbuj ponownie.");
                        continue;
                    }
                    if (radius <= 0 || slant <= 0)
                    {
                        Console.WriteLine("Stożek nie istnieje. Argumenty muszą być większe od 0. ");
                        continue;
                    }

                    if (!(radius > 0 && radius < 1000000) || !(slant > 0 && slant < 1000000))
                    {
                        Console.WriteLine("Argumenty nie mieszczą się w przedziale.");
                        continue;
                    }

                    if (slant < radius)
                    {
                        Console.WriteLine("Stożek nie istnieje");
                        continue;
                    }

                    double height = Math.Sqrt((double)slant * slant - (double)radius * radius);
                    double volume = (Math.PI * radius * radius * height) / 3.0;
                    Console.WriteLine($"Dokładna objętość: {volume}");
                    Console.WriteLine($"Objętość zaokrąglona w dół: {Math.Floor(volume)}");
                    Console.WriteLine($"Objętość zaokrąglona w górę: {Math.Ceiling(volume)}");

                    validInput = true;
                }
            }


            void Quadratic()
            {
                bool validInput = false;
                Console.WriteLine(" --- ZADANIE 4: RÓWNANIE KWADRATOWE --- ");

                while (!validInput)
                {
                    Console.WriteLine("Podaj współczynniki równania kwadratowego [ax² + bx + c] (oddzielone spacją)");
                    var input = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (input == null || input.Length < 3)
                    {
                        Console.WriteLine("Nieprawidłowa ilość danych wejściowych. Spróbuj ponownie.");
                        continue;
                    }
                    if (!double.TryParse(input[0], out double a) || !double.TryParse(input[1], out double b) || !double.TryParse(input[2], out double c))
                    {
                        Console.WriteLine("Nieprawidłowy typ danych. Spróbuj ponownie.");
                        continue;
                    }
                    if (a == 0 && b == 0 && c == 0)
                    {
                        Console.WriteLine("Równanie tożsamościowe. Nieskończenie wiele rozwiązań");
                        continue;
                    }

                    else if (a == 0 && b == 0 && c != 0)
                    {
                        Console.WriteLine("Równanie sprzeczne. Brak rozwiązań.");
                        continue;
                    }

                    else if (a == 0 && b != 0)
                    {
                        Console.WriteLine($"Równanie liniowe. Jedno rozwiązanie: x = {Math.Round((-c / b), 2):F2}");
                        continue;
                    }

                    double delta = b * b - (4 * a * c);
                    if (delta < 0) Console.WriteLine("Delta ujemna. Brak rozwiązań");
                    else if (delta == 0)
                    {
                        Console.WriteLine($"Jedno rozwiązanie: x = {Math.Round((-b / (2 * a)), 2):F2}");
                    }
                    else
                    {
                        double sqrtDelta = Math.Sqrt(delta);
                        double x1 = Math.Round((-b - sqrtDelta) / (2 * a), 2);
                        double x2 = Math.Round((-b + sqrtDelta) / (2 * a), 2);
                        Console.WriteLine($"Wynik: x1={x1:F2}, x2={x2:F2}");
                    }
                    validInput = true;
                }
            }

            void TriangleParameters()
            {
                bool validInput = false;
                Console.WriteLine(" --- ZADANIE 5: PARAMETRY TRÓJKĄTA --- ");

                while (!validInput)
                {
                    Console.WriteLine("Podaj wszystkie 3 boki trójkąta (oddzielone spacją)");
                    var input = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

                    if (input == null || input.Length < 3)
                    {
                        Console.WriteLine("Nieprawidłowa ilość danych wejściowych. Spróbuj ponownie.");
                        continue;
                    }
                    if (!double.TryParse(input[0], out double a) || !double.TryParse(input[1], out double b) || !double.TryParse(input[2], out double c))
                    {
                        Console.WriteLine("Nieprawidłowy typ danych. Spróbuj ponownie.");
                        continue;
                    }
                    if (a <= 0 || b <= 0 || c <= 0)
                    {
                        Console.WriteLine("Nieprawidłowe dane. Długość boków musi być większa od zera. Spróbuj ponownie.");
                        continue;
                    }

                    double max = Math.Max(Math.Max(a, b), c);
                    if(a + b + c - max <= max)
                    {
                        Console.WriteLine("Błąd. Nie da się utworzyć trójkąta. Spróbuj ponownie.");
                        continue;
                    }
                    double perimeter = a + b + c;
                    double p = perimeter / 2;
                    double area = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
                    double sumOfShorterSides = a * a + b * b + c * c - max * max;
                    string angleType = "";

                    if (sumOfShorterSides > c * c)
                    {
                        angleType = "Ostrokątny";
                    }
                    else if (sumOfShorterSides == c * c)
                    {
                        angleType = "Prostokątny";
                    }
                    else angleType = "Rozwartokątny";

                    string triangleType = "";
                    if (a == b && b == c) triangleType = "Równoboczny";
                    else if (a == b || b == c || a == c) triangleType = "Równoramienny";
                    else triangleType = "Różnoboczny";

                    Console.WriteLine($"Typ trójkąta: {triangleType},{angleType}\nObwód: {Math.Round(perimeter,2):F2}\nPole: {Math.Round(area,2):F2}\n");
                    validInput = true;
                }
            }
        }
    }
}
