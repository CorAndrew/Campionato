using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campionato
{
    internal class Program
    {
        private static IRepository<Partita> repository = new RepositoryPartite();
        static void Main(string[] args)
        {
            repository.Carica();

            bool esci = false;

            do
            {
                Console.WriteLine("MENU CAMPIONATO");
                Console.WriteLine("1. Visualizza le partite in ordine naturale");
                Console.WriteLine("2. Visualizza le partite ordinate secondo un criterio");
                Console.WriteLine("3. Inserisci una partita");
                Console.WriteLine("4. Mostra la classifica dei gironi");
                Console.WriteLine("5. Cerca le partite di una squadra");
                Console.WriteLine("6. Elimina una partita");
                Console.WriteLine("0. Esci");
                string scelta = Console.ReadLine();
                Console.WriteLine();

                switch (scelta)
                {
                    case "1":
                        VisualizzaOrdineNaturale();
                        break;
                    case "2":
                        VisualizzaPerCriterio();
                        break;
                    case "3":
                        InserisciPartita();
                        break;
                    case "4":
                        MostraClassifica();
                        break;
                    case "5":
                        CercaPartiteSquadra();
                        break;
                    case "6":
                        EliminaPartita();
                        break;
                    case "0":
                        esci = true;
                        Console.WriteLine("Programma terminato e dati salvati");
                        break;
                    default:
                        Console.WriteLine("Opzione non valida");
                        break;
                }
            }
            while (!esci);


        }
    }
}
