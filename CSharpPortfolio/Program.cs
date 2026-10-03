using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.IO;
using System.Threading;

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
                Console.WriteLine("6 - Wypisywanie liczb");
                Console.WriteLine("7 - Rabat na loty");
                Console.WriteLine("8 - Rysowanie wzorków");
                Console.WriteLine("9 - Gra Saper");
                Console.WriteLine(border);
                Console.WriteLine("Twój wybór: ");
                Console.WriteLine();
                string choice = Console.ReadLine();
                Console.WriteLine();

                // Wywołanie wybranej aplikacji
                switch (choice)
                {
                    case "0": running = false; break;
                    case "1": RetirementCalculator(); Thread.Sleep(3000); break;
                    case "2": RetirementCalculatorCSV("users.csv"); Thread.Sleep(3000); break;
                    case "3": ConeVolume(); Thread.Sleep(3000); break;
                    case "4": Quadratic(); Thread.Sleep(3000); break;
                    case "5": TriangleParameters(); Thread.Sleep(3000); break;
                    case "6": CountNumbersWithStep(); Thread.Sleep(3000); break;
                    case "7": FlightDiscountCalc(); Thread.Sleep(10000); break;
                    case "8": DrawPatternsMenu(); Thread.Sleep(1000); break;
                    case "9": PlayMineSweeper(); Thread.Sleep(10000); break;
                }
            }

            /// <summary>
            /// ZADANIE 1: Kalkulator emerytalny
            /// Oblicza czas pozostały (liczony w latach) do osiągnięcia wieku emerytalnego.
            /// Pobiera od użytkownika jedno wierszowe wejście: Nazwisko, aktualny wiek oraz próg emerytalny.
            /// Zwraca spersonalizowane powitanie i obsługuje poprawną polską odmianę słów "rok", "lata", "lat".
            /// </summary>
            void RetirementCalculator()
            {
                Console.WriteLine(" --- ZADANIE 1: KALKULATOR EMERYTALNY --- ");

                while (true)
                {
                    Console.WriteLine("Podaj Nazwisko, Aktualny wiek oraz próg emerytalny (oddzielone spacją)");

                    var input = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    // walidacja wprowadzonych danych
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

                    // logika
                    else
                    {
                        int difference = threshold - age;
                        int lastDigit = difference % 10;
                        int lastTwoDigits = difference % 100;
                        string output = (difference == 1) ? "rok!" : (lastDigit >= 2 && lastDigit <= 4 && (lastTwoDigits < 12 || lastTwoDigits > 14)) ? "lata!" : "lat!";
                        Console.WriteLine($"Do emerytury zostało ci: {difference} {output}");
                        break;
                    }

                }
            }

            /// <summary>
            /// ZADANIE 2: Kalkulator emerytalny csv
            /// Oblicza czas pozostały (liczony w latach) do osiągnięcia wieku emerytalnego.
            /// Pobiera dane z pliku tekstowego users.csv, gdzie każdy wiersz zawiera nazwisko, aktualny wiek oraz próg emerytalny.
            /// Zwraca spersonalizowane powitanie i obsługuje poprawną polską odmianę słów "rok", "lata", "lat".
            /// </summary>
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

                        // walidacja wprowadzonych danych
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

            /// <summary>
            /// ZADANIE 3: Objętość stożka
            /// Oblicza objętość stożka na podstawie promienia podstawy (radius) i długości tworzącej (slant).
            /// Zawiera walidację danych wejściowych, sprawdza warunek istnienia stożka (slant > radius) 
            /// Zwraca wynik w postaci podłogi i sufitu (zaokrąglenia w dół i w górę) z obliczonej objętości.
            /// </summary>
            void ConeVolume()
            {
                Console.WriteLine(" --- ZADANIE 3: OBJĘTOŚĆ STOŻKA --- ");

                while (true)
                {
                    Console.WriteLine("Podaj promień podstawy \"r\" oraz tworzącą \"l\" z przedziału [0 - 1 000 000] (oddzielone spacją)");
                    var input = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    // walidacja wprowadzonych danych
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

                    break;
                }
            }

            /// <summary>
            /// ZADANIE 4: Równanie kwadratowe
            /// Rozwiązuje równanie kwadratowe postaci ax² + bx + c = 0.
            /// Obsługuje przypadki szczególne: tożsamość, brak rozwiązań, równanie liniowe 
            /// Poprawnie wylicza pierwiastki (x1, x2) używając delty, z zaokrągleniem do dwóch miejsc po przecinku.
            /// </summary>
            void Quadratic()
            {
                Console.WriteLine(" --- ZADANIE 4: RÓWNANIE KWADRATOWE --- ");

                while (true)
                {
                    Console.WriteLine("Podaj współczynniki równania kwadratowego [ax² + bx + c] (oddzielone spacją)");
                    var input = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    // walidacja wprowadzonych danych
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
                    break;
                }
            }

            /// <summary>
            /// ZADANIE 5: Parametry trójkąta
            /// Analizuje trzy boki podane przez użytkownika, sprawdzając warunek zbudowania trójkąta.
            /// Wylicza obwód i pole (przy użyciu wzoru Herona). 
            /// Rozpoznaje typ trójkąta pod względem jego kątów (ostrokątny, prostokątny, rozwartokątny) oraz długości boków (równoboczny, równoramienny).
            /// </summary>
            void TriangleParameters()
            {
                Console.WriteLine(" --- ZADANIE 5: PARAMETRY TRÓJKĄTA --- ");

                while (true)
                {
                    Console.WriteLine("Podaj wszystkie 3 boki trójkąta (oddzielone spacją)");
                    var input = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

                    // walidacja wprowadzonych danych
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

                    // Sprawdzanie nierówności trójkąta
                    double max = Math.Max(Math.Max(a, b), c);
                    if (a + b + c - max <= max)
                    {
                        Console.WriteLine("Błąd. Nie da się utworzyć trójkąta. Spróbuj ponownie.");
                        continue;
                    }

                    // Logika
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

                    Console.WriteLine($"Typ trójkąta: {triangleType},{angleType}\nObwód: {Math.Round(perimeter, 2):F2}\nPole: {Math.Round(area, 2):F2}\n");
                    break;
                }
            }

            /// <summary>
            /// ZADANIE 6: Wypisywanie liczb
            /// Generuje ciąg liczbowy na podstawie wartości początkowej, końcowej i skoku.
            /// Skok automatycznie dostosowuje kierunek (rosnący/malejący).
            /// Dla ciągów powyżej 10 elementów ukrywa środkowe wartości pod postacią wielokropka ("..."), optymalizując wyświetlanie.
            /// </summary>
            void CountNumbersWithStep()
            {
                Console.WriteLine(" --- ZADANIE 6: WYPISYWANIE LICZB --- ");

                while (true)
                {
                    Console.WriteLine("Podaj trzy liczby całkowite stanowiące początek, koniec i skok ciągu [np. 1 20 2]  (oddzielone spacją)");
                    var input = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

                    // walidacja wprowadzonych danych
                    if (input == null || input.Length < 3)
                    {
                        Console.WriteLine("Nieprawidłowa ilość danych wejściowych. Spróbuj ponownie.");
                        continue;
                    }
                    if (!int.TryParse(input[0], out int start) || !int.TryParse(input[1], out int end) || !int.TryParse(input[2], out int step))
                    {
                        Console.WriteLine("Nieprawidłowy typ danych. Spróbuj ponownie.");
                        continue;
                    }
                    if (step <= 0)
                    {
                        Console.WriteLine("Nieprawidłowe dane. Skok musi być większy od zera. Spróbuj ponownie");
                        continue;
                    }

                    // Sprawdzanie pustych ciągów
                    int max = Math.Max(start, end);
                    int min = Math.Min(start, end);
                    if (start == end || (max - min) == 1 || step > Math.Abs(max))
                    {
                        Console.WriteLine("Pusty ciąg");
                        break;
                    }

                    // Logika
                    int directionalStep = start <= end ? step : -step;
                    int count = ((max - min - 1) / step) + 1;

                    if (count < 10)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Console.Write(start + (directionalStep * i) + (i < count - 1 ? ", " : ""));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Console.Write(start + (directionalStep * i) + ", ");
                            if (i == 2) Console.Write("... , ");
                        }
                        for (int i = count - 3; i < count; i++)
                        {
                            Console.Write(start + (directionalStep * i) + (i < count - 1 ? ", " : ""));
                        }
                    }
                    Console.WriteLine();
                    break;
                }
            }

            /// <summary>
            /// ZADANIE 7: Kalkulator rabatów lotniczych
            /// Zaawansowany kalkulator wyliczający zniżkę na bilet, testujący operacje na obiektach DateOnly i flagach boolean.
            /// Precyzyjnie ustala wiek pasażera na sam dzień wylotu 
            /// Sprawdza, czy termin zahacza o tzw. wysoki sezon (czyli wakacje, święta czy ferie).
            /// Wprowadza regułę dyskwalifikującą na lot zagraniczny w sezonie dla osoby powyżej 2 r.ż. (zeruje zniżki).
            /// Sumowanie rabatów:
            /// - Niemowlę (<2 lata): +80% (krajowy) lub +70% (zagraniczny). Nie łączy się z innymi.
            /// - Dziecko (2-16 lat): +10%.
            /// - Wczesna rezerwacja (wylot za > 5 miesięcy): +10%.
            /// - Lot zagraniczny poza sezonem: +15%.
            /// - Dorosły (>= 18 lat) stały klient: +15%.
            /// * Limity: Maksymalny łączny rabat to 80% dla niemowląt i 30% dla wszystkich pozostałych pasażerów.
            /// </summary>
            void FlightDiscountCalc()
            {
                Console.WriteLine(" --- ZADANIE 7: RABAT NA LOTY --- ");

                DateTime birthDate = DateTime.MinValue;

                // Wprowadzanie daty urodzenia
                while (true)
                {
                    Console.WriteLine("Podaj swoją datę urodzenia w formacie RRRR-MM-DD: ");
                    var birthdayInput = Console.ReadLine().Trim();

                    // walidacja wprowadzonych danych
                    if (!DateTime.TryParseExact(birthdayInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate))
                    {
                        Console.WriteLine("Nieprawidłowy format daty. Spróbuj ponownie.");
                        continue;
                    }
                    if (birthDate > DateTime.Now)
                    {
                        Console.WriteLine("Data urodzenia nie może być z przyszłości. Spróbuj ponownie.");
                        continue;
                    }
                    if (birthDate <= new DateTime(1900, 1, 1))
                    {
                        Console.WriteLine("Data urodzenia nie może być wcześniejsza niż 1900-01-01. Spróbuj ponownie.");
                        continue;
                    }

                    break;
                }

                DateTime flightDate = DateTime.MinValue;

                // Wprowadzanie daty lotu
                while (true)
                {
                    Console.WriteLine("Podaj datę lotu w formacie RRRR-MM-DD: ");
                    var flightDateInput = Console.ReadLine().Trim();

                    if (!DateTime.TryParseExact(flightDateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out flightDate))
                    {
                        Console.WriteLine("Nieprawidłowy format daty. Spróbuj ponownie.");
                        continue;
                    }
                    if (flightDate < DateTime.Today)
                    {
                        Console.WriteLine("Data lotu nie może być z przeszłości. Spróbuj ponownie.");
                        continue;
                    }

                    break;
                }

                // Sprawdzanie czy data lotu jest w wysokim sezonie
                bool isInHighSeason =
                    (flightDate.Month == 7 || flightDate.Month == 8) || // sprawdzanie czy miesiąc lotu to lipiec lub sierpień
                    (flightDate.Month == 12 && flightDate.Day >= 20) || (flightDate.Month == 1 && flightDate.Day <= 10) || // sprawdzanie czy lot jest podczas świąt
                    (flightDate.Month == 3 && flightDate.Day >= 20) || (flightDate.Month == 4 && flightDate.Day <= 10); // sprawdzanie czyh lot jest podczas ferii

                string seasonText = isInHighSeason ? "Lot w sezonie" : "Lot nie w sezonie";

                // Sprawdzanie czy lot krajowy
                bool isDomestic = true;

                while (true)
                {
                    Console.WriteLine("Czy lot jest krajowy? (T/N) ");
                    var domesticFlightInput = Console.ReadLine().Trim().ToLower();
                    if (domesticFlightInput == "t")
                    {
                        isDomestic = true;
                        break;
                    }
                    else if (domesticFlightInput == "n")
                    {
                        isDomestic = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowa odpowiedź. Spróbuj ponownie");
                    }
                }
                string domesticText = isDomestic ? "Lot krajowy" : "Lot międzynarodowy";

                // Obliczanie wieku w dniu lotu
                int age = flightDate.Year - birthDate.Year;
                if (flightDate.Month < birthDate.Month || (flightDate.Month == birthDate.Month && flightDate.Day < birthDate.Day))
                {
                    age--;
                }
                Console.WriteLine();
                bool ofAge = (age >= 18);

                // Sprawdzanie czy stały klient
                bool isARegular = false;
                string regularText = "";
                if (ofAge)
                {
                    while (true)
                    {
                        Console.WriteLine("Czy jesteś stałym klientem? (T/N)");
                        var regularInput = Console.ReadLine().Trim().ToLower();

                        if (regularInput == "t")
                        {
                            isARegular = true;
                            break;
                        }
                        else if (regularInput == "n")
                        {
                            isARegular = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Nieprawidłowa odpowiedź. Spróbuj ponownie");
                        }
                    }
                    regularText = isARegular ? "Tak" : "Nie";
                }
                else
                {
                    regularText = "Nie (zniżka tylko dla pełnoletnich)";
                }

                // Wyświetlenie zebranych informacji
                Console.WriteLine("\n=== Do obliczeń przyjęto:");
                Console.WriteLine($" * Data urodzenia: {birthDate:dd.MM.yyyy}");
                Console.WriteLine($" * Data lotu: {flightDate.ToString("dddd, d MMMM yyyy", new CultureInfo("pl-PL"))}. {seasonText}");
                Console.WriteLine($" * {domesticText}");
                Console.WriteLine($" * Stały klient: {regularText}");

                // Obliczanie rabatu
                int discount = 0;
                if (!isDomestic && isInHighSeason && age >= 2) discount = 0;
                else
                {
                    if (age < 2 && isDomestic) discount += 80;
                    if (age < 2 && !isDomestic) discount += 70;
                    if (age >= 2 && age <= 16) discount += 10;
                    if (flightDate >= DateTime.Today.AddMonths(5)) discount += 10;
                    if (!isDomestic && !isInHighSeason) discount += 15;
                    if (isARegular) discount += 15;
                }

                // Nakładanie limitów
                if (age < 2 && discount > 80) discount = 80;
                if (age >= 2 && discount > 30) discount = 30;

                Console.WriteLine($"\nPrzysługuje Ci rabat w wysokości: {discount}%");
                Console.WriteLine($"Data wygenerowania raportu: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            }

            void DrawPatternsMenu()
            {
                while (true)
                {
                    string border = new string('-', 40);
                    Console.WriteLine(" --- ZADANIE 8: RYSOWANIE WZORKÓW --- ");
                    Console.WriteLine(border);
                    Console.WriteLine("1 - Rysowanie litery X");
                    Console.WriteLine("2 - Rysowanie litery Z");
                    Console.WriteLine("3 - Rysowanie odwrotnej litery Z");
                    Console.WriteLine("4 - Rysowanie klepsydry");
                    Console.WriteLine("5 - Rysowanie drzewa");
                    Console.WriteLine("0 - Powrót");
                    Console.WriteLine(border);
                    Console.WriteLine("Twój wybór: ");
                    Console.WriteLine();

                    string subChoice = Console.ReadLine();
                    Console.WriteLine();

                    if (subChoice == "0")
                    {
                        Console.WriteLine("Powrót\n");
                        return;
                    }

                    switch (subChoice)
                    {
                        case "1": DrawXPattern(GetPatternSize()); break;
                        case "2": DrawZPattern(GetPatternSize()); break;
                        case "3": DrawReverseZPattern(GetPatternSize()); break;
                        case "4": DrawHourglassPattern(GetPatternSize()); break;
                        case "5": DrawTreePattern(GetPatternSize()); break;
                        default: break;
                    }
                }
                

                // Pobieranie rozmiaru wybranego wzoru
                int GetPatternSize()
                {
                    while (true)
                    {
                        Console.WriteLine("Podaj rozmiar wzoru do narysowania. (3-20)");
                        var input = Console.ReadLine()?.Trim();

                        if (string.IsNullOrEmpty(input))
                        {
                            Console.WriteLine("Puste dane. Spróbuj ponownie.");
                            continue;
                        }
                        if (!int.TryParse(input, out int size))
                        {
                            Console.WriteLine("Nieprawidłowy format danych. Spróbuj ponownie.");
                            continue;
                        }
                        if (size < 3 || size > 20)
                        {
                            Console.WriteLine("Rozmiar wychodzi poza dozwolony przedział. Spróbuj ponownie");
                            continue;
                        }
                        return size;
                    }
                }
            }
            /// <summary>
            /// ZADANIE 8: Rysowanie wzorków
            /// Moduł rysowania figur z użyciem pętli zagnieżdżonych. 
            /// Dla podanego rozmiaru generuje wybrane wzory z gwiazdek: 
            /// Litere X, Z, Odwróconą litere Z, Klepsydrę oraz choinkę
            /// </summary>
            void DrawXPattern(int size)
            {
                for (int row = 0; row < size; row++)
                {
                    for (int col = 0; col < size; col++)
                    {
                        if (col == row || col == size - row - 1)
                            Console.Write("*");
                        else
                            Console.Write(" ");
                    }
                    Console.WriteLine();
                }
            }

            void DrawZPattern(int size)
            {
                for (int row = 0; row < size; row++)
                {
                    for (int col = 0; col < size; col++)
                    {
                        if (row == 0 || row == size - 1)
                        {
                            Console.Write("*");
                        }
                        else if (col == size - row - 1)
                        {
                            Console.Write(new string(' ', col) + "*");
                        }
                    }
                    Console.WriteLine();
                }
            }

            void DrawReverseZPattern(int size)
            {
                for (int row = 0; row < size; row++)
                {
                    for (int col = 0; col < size; col++)
                    {
                        if (row == 0 || row == size - 1)
                        {
                            Console.Write("*");
                        }
                        else if (col == row)
                        {
                            Console.Write(new string(' ', col) + "*");
                        }
                    }
                    Console.WriteLine();
                }
            }

            void DrawHourglassPattern(int size)
            {
                for (int row = 0; row < size; row++)
                {
                    for (int col = 0; col < size; col++)
                    {
                        if (row == 0 || row == size - 1)
                        {
                            Console.Write("*");
                        }
                        else if (col == row || col == size - row - 1)
                        {
                            Console.Write("*");
                        }
                        else Console.Write(" ");
                    }
                    Console.WriteLine();
                }
            }

            void DrawTreePattern(int size)
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size - i; j++)
                    {
                        Console.Write(" ");
                    }
                    for (int k = 0; k < 2 * i + 1; k++)
                    {
                        Console.Write("*");
                    }
                    Console.WriteLine();
                }

                for (int l = 0; l < 2; l++)
                {
                    for (int m = 0; m < size+(l/3); m++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine("*");
                }
            }


            /// <summary>
            /// ZADANIE 9: Gra Saper (Generator podpowiedzi)
            /// Analizuje wprowadzony przez użytkownika układ dwuwymiarowej planszy min (tablica 2D).
            /// Iteruje po każdym polu, zliczając miny w bezpośrednim sąsiedztwie (8 kierunków) 
            /// i zwraca gotową planszę z wpisanymi cyframi podpowiedzi.
            /// </summary>
            void PlayMineSweeper()
            {
                while (true)
                {
                    Console.WriteLine("Podaj ilość wierszy oraz ilość kolumn planszy sapera np. [4 5] (oddzielone spacją)");
                    var input = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    // walidacja wprowadzonych danych
                    if (input == null || input.Length < 2)
                    {
                        Console.WriteLine("Nieprawidłowa ilość danych wejściowych. Spróbuj ponownie.");
                        continue;
                    }
                    if (!int.TryParse(input[0], out int row) || !int.TryParse(input[1], out int col))
                    {
                        Console.WriteLine("Nieprawidłowy typ danych. Spróbuj ponownie.");
                        continue;
                    }
                    if (row <= 0 || col <= 0)
                    {
                        Console.WriteLine("Nieprawidłowe dane. wymiary muszą być większe od zera. Spróbuj ponownie");
                        continue;
                    }

                    char[,] board = new char[row, col];

                    Console.WriteLine("Narysuj planszę wiersz po wierszu (kropka '.' to puste pole, gwiazdka '*' to mina):");

                    for(int i = 0; i < row; i++)
                    {
                        char[] filledRow = Console.ReadLine().ToCharArray();
                        for (int j = 0; j < filledRow.Length; j++)
                        {
                            if (j < col) board[i, j] = filledRow[j];
                        }
                    }

                    Console.WriteLine("\nWynikowa plansza z podpowiedziami\n");
                    for(int i = 0; i < row; i++)
                    {
                        for(int j = 0; j < col; j++)
                        {
                            // Przepisiwanie min z wprowadzonej planszy do końcowej
                            if (board[i, j] == '*')
                            {
                                Console.Write("*");
                            }
                            else
                            {
                                // Sprawdzanie obszaru 3x3 w okół obecnego pola
                                int counter = 0;
                                for(int adjacentX = -1; adjacentX <= 1; adjacentX++) // lewo/prawo
                                {
                                    
                                    for(int adjacentY = -1; adjacentY <= 1; adjacentY++) // góra/dół
                                    {
                                        // Pomijanie obecnego (środkowego) pola
                                        if (adjacentY == 0 && adjacentX == 0) continue;

                                        int rowBeingChecked = i + adjacentX;
                                        int colBeingChecked = j + adjacentY;

                                        // Sprawdzanie czy sprawdzane pole mieści się w planszy
                                        bool insideBoard = (rowBeingChecked >= 0 && rowBeingChecked < row) && (colBeingChecked >= 0 && colBeingChecked < col);

                                        // Zliczanie ilość min
                                        if (insideBoard && board[rowBeingChecked, colBeingChecked] == '*')
                                        {
                                            counter++;
                                        }

                                    }
                                }
                                if(counter == 0) Console.Write('.');
                                else Console.Write(counter);
                            }
                        }
                        Console.WriteLine();
                    }

                    Console.WriteLine();
                    break;
                }
            }
        }
    }
}