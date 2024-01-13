using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt___Rabat_na_loty
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Pytania do użytkownika
            Console.Write("Podaj swoją datę urodzenia w formacie RRRR-MM-DD: ");
            DateTime dataUrodzenia;
            while (!DateTime.TryParse(Console.ReadLine(), out dataUrodzenia))
            {
                Console.WriteLine("Niepoprawny format daty. Podaj ponownie: ");
            }

            Console.Write("Podaj datę lotu w formacie RRRR-MM-DD: ");
            DateTime dataLotu;
            while (!DateTime.TryParse(Console.ReadLine(), out dataLotu))
            {
                Console.WriteLine("Niepoprawny format daty. Podaj ponownie: ");
            }


            bool lotKrajowy;
            do
            {
                Console.Write("Czy lot jest krajowy (T/N)? ");
                string odpowiedz = Console.ReadLine().Trim().ToUpper();

                if (odpowiedz == "T" || odpowiedz == "N")
                {
                    lotKrajowy = odpowiedz == "T";
                    break; // Przerwij pętlę, jeśli wprowadzono poprawną odpowiedź
                }
                else
                {
                    Console.WriteLine("Niepoprawny znak. Wprowadź T lub N: ");
                }
            } while (true); // Pętla będzie się powtarzać, dopóki nie zostanie wprowadzona poprawna odpowiedź

            bool stalymKlientem;
            if (dataUrodzenia.AddYears(18) > dataLotu)
            {
                stalymKlientem = false;
            }
            else
            { 
                do
                {
                    Console.Write("Czy jesteś stałym klientem (T/N)?");
                    string odpowiedz = Console.ReadLine().Trim().ToUpper();

                    if (odpowiedz == "T" || odpowiedz == "N")
                    {
                        //if (odpowiedz == "T" && dataUrodzenia.AddYears(18)>dataLotu) //sprawdzam czy w dniu wylotu ma 18 lat
                        //{
                        //    //Console.WriteLine("Nie przysługuje Ci rabat dla stałych klientów");
                        //    stalymKlientem = false;
                        //    break;
                        //}
                        //else
                        //{
                            stalymKlientem = odpowiedz == "T";
                            break;
                       // }
                    }
                    else
                    {
                        Console.WriteLine("Niepoprawny znak. Wprowadz T lub N: ");
                    }
                } while (true); // Petla nieskonczona. Moze ja przerwac tylko break z IFa wyzej tak samo jak przy locie krajowym
            }
            // Obliczenia rabatu
            double rabat = ObliczRabat(dataUrodzenia, dataLotu, lotKrajowy, stalymKlientem);

            // Wydruk raportu
            Console.WriteLine("\n=== Do obliczeń przyjęto:");
            Console.WriteLine($" * Data urodzenia: {dataUrodzenia.ToString("dd.MM.yyyy")}");
            Console.WriteLine($" * Data lotu: {dataLotu.ToString("dddd, d MMMM yyyy")}. Lot {(CzySezon(dataLotu) ? "w sezonie" : "poza sezonem")}");
            Console.WriteLine($" * Lot {(lotKrajowy ? "krajowy" : "międzynarodowy")}");
            Console.WriteLine($" * Stały klient: {(stalymKlientem ? "Tak" : "Nie")}\n");
            Console.WriteLine($"Przysługuje Ci rabat w wysokości: {rabat}%");
            Console.WriteLine($"Data wygenerowania raportu: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        // Funkcja do obliczenia rabatu
        static double ObliczRabat(DateTime dataUrodzenia, DateTime dataLotu, bool lotKrajowy, bool stalymKlientem)
        {
            double rabat = 0;

            // Rabat dla niemowląt
            if (dataUrodzenia.AddYears(2) > dataLotu) // Mniej niż 2 lata
                {
                rabat = lotKrajowy ? 80 : 70; // 
                rabat = Math.Min(rabat, 80); // Maksymalny rabat dla niemowląt - 80%
                return rabat;
            }
            // Rabat dla młodzieży
            else if ((dataUrodzenia.AddYears(2) <= dataLotu) && (dataUrodzenia.AddYears(16) >= dataLotu))
            {
                rabat = 10;
            }
            // Rabat dla rezerwacji 5 miesięcy przed lotem
            else if (DateTime.Now.AddMonths(5)<dataLotu)
            {
                rabat = 10;
            }
            // Rabat dla lotów międzynarodowych poza sezonem
            else if (!lotKrajowy && !CzySezon(dataLotu))
            {
                rabat = 15;
            }
            // Rabat dla stałych klientów
            else if (stalymKlientem)
            {
                rabat = 15;
            }

            // Maksymalny łączny rabat dla pozostałych - 30%
            if (!stalymKlientem)
            {
                rabat = Math.Min(rabat, 30);
            }
            return rabat;
        }

        // Funkcja sprawdzająca, czy data lotu jest w sezonie
        static bool CzySezon(DateTime dataLotu)
        {
            int rok = dataLotu.Year;
            DateTime poczatekSezonu1 = new DateTime(rok, 12, 20);
            DateTime koniecSezonu1 = new DateTime(rok + 1, 1, 10);
            DateTime poczatekSezonu2 = new DateTime(rok, 3, 20);
            DateTime koniecSezonu2 = new DateTime(rok, 4, 10);
            DateTime lipiec = new DateTime(rok, 7, 1);
            DateTime sierpien = new DateTime(rok, 8, 1);

            return (dataLotu >= poczatekSezonu1 && dataLotu <= koniecSezonu1) ||
                   (dataLotu >= poczatekSezonu2 && dataLotu <= koniecSezonu2) ||
                   (dataLotu.Month == lipiec.Month || dataLotu.Month == sierpien.Month);
        }
    }
}
