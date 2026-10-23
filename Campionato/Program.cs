using System;
using System.Collections.Generic;

namespace Campionato
{
    internal class Program
    {
        private static IRepository<Partita> repository = new RepositoryPartite();

        public static void Main(string[] args)
        {
            repository.Carica();

            bool esci = false;

            do
            {
                Console.WriteLine("MENU");
                Console.WriteLine("1. Visualizza le partite in ordine naturale");
                Console.WriteLine("2. Visualizza le partite ordinate secondo un criterio");
                Console.WriteLine("3. Inserisci una partita");
                Console.WriteLine("4. Mostra la classifica dei gironi");
                Console.WriteLine("5. Cerca le partite di una squadra");
                Console.WriteLine("6. Elimina una partita");
                Console.WriteLine("0. Esci");

                string scelta = Console.ReadLine();
                Console.WriteLine();

                if (scelta == "1")
                {
                    VisualizzaOrdineNaturale();
                }
                else if (scelta == "2")
                {
                    VisualizzaPerCriterio();
                }
                else if (scelta == "3")
                {
                    InserisciPartita();
                }
                else if (scelta == "4")
                {
                    MostraClassifica();
                }
                else if (scelta == "5")
                {
                    CercaPartiteSquadra();
                }
                else if (scelta == "6")
                {
                    EliminaPartita();
                }
                else if (scelta == "0")
                {
                    esci = true;
                    Console.WriteLine("Programma terminato");
                }
                else
                {
                    Console.WriteLine("Opzione non valida");
                }
            }
            while (!esci);
        }

        public static void VisualizzaOrdineNaturale()
        {
            IReadOnlyList<Partita> elenco = repository.GetElenco();
            if (elenco.Count == 0)
            {
                Console.WriteLine("Nessuna partita registrata nell'archivio");
                return;
            }

            List<Partita> listaCopia = new List<Partita>(elenco);
            Partita.Criterio = "naturale";
            listaCopia.Sort();

            Console.WriteLine("elenco partite (elenco naturale)");
            foreach (Partita p in listaCopia)
            {
                Console.WriteLine(p.Descrizione());
            }
        }

        public static void VisualizzaPerCriterio()
        {
            IReadOnlyList<Partita> elenco = repository.GetElenco();
            if (elenco.Count == 0)
            {
                Console.WriteLine("Nessuna partita registrata nell'archivio.");
                return;
            }

            Console.WriteLine("Scegli il criterio di ordinamento:");
            Console.WriteLine("1. Gol totali (decrescente)");
            Console.WriteLine("2. Scarto reti (decrescente)");
            Console.WriteLine("3. Squadra di casa A-Z");
            Console.WriteLine("4. Squadra di casa Z-A");
            Console.WriteLine("5. Data");

            string sceltaCrit = Console.ReadLine();
            if (sceltaCrit == "1")
            {
                Partita.Criterio = "gol";
            }
            else if (sceltaCrit == "2")
            {
                Partita.Criterio = "scarto";
            }
            else if (sceltaCrit == "3")
            {
                Partita.Criterio = "casaAZ";
            }
            else if (sceltaCrit == "4")
            {
                Partita.Criterio = "casaZA";
            }
            else if (sceltaCrit == "5")
            {
                Partita.Criterio = "data";
            }
            else
            {
                Console.WriteLine("Criterio non valido");
                return;
            }

            List<Partita> listaCopia = new List<Partita>(elenco);
            listaCopia.Sort();

            Console.WriteLine("elenco partite (Criterio: " + Partita.Criterio + ")");
            foreach (Partita p in listaCopia)
            {
                Console.WriteLine(p.Descrizione());
            }
        }

        public static void InserisciPartita()
        {
            Console.WriteLine("Tipo di partita:");
            Console.WriteLine("1. Girone");
            Console.WriteLine("2. Eliminazione Diretta");
            Console.WriteLine("3. Finale");

            int tipo;
            if (!int.TryParse(Console.ReadLine(), out tipo) || tipo < 1 || tipo > 3)
            {
                Console.WriteLine("Scelta non valida");
                return;
            }

            Console.Write("Giornata: ");
            int giornata;
            if (!int.TryParse(Console.ReadLine(), out giornata))
            {
                Console.WriteLine("Giornata non valida");
                return;
            }

            Console.Write("Squadra di casa: ");
            string casa = Console.ReadLine();

            Console.Write("Squadra ospite: ");
            string ospite = Console.ReadLine();

            Console.Write("Risultato: ");
            string risultato = Console.ReadLine();

            Console.Write("Data (GG/MM/AAAA): ");
            DateTime dataPartita;
            if (!DateTime.TryParse(Console.ReadLine(), out dataPartita))
            {
                Console.WriteLine("Data non valida");
                dataPartita = DateTime.Now;
            }

            Partita nuovaPartita;

            if (tipo == 1)
            {
                nuovaPartita = new PartitaGirone(giornata, casa, ospite, risultato, dataPartita);
            }
            else
            {
                int rigoriCasa = -1;
                int rigoriOspite = -1;


                PartitaEliminazione prova = new PartitaEliminazione(giornata, casa, ospite, risultato, dataPartita, rigoriCasa, rigoriOspite);
                if (prova.CalcolaEsito() == Esito.Pareggio)
                {
                    Console.WriteLine("La partita è in pareggio");

                    Console.Write("Rigori casa: ");
                    int rc;
                    if (int.TryParse(Console.ReadLine(), out rc))
                    {
                        rigoriCasa = rc;
                    }

                    Console.Write("Rigori ospite: ");
                    int ro;
                    if (int.TryParse(Console.ReadLine(), out ro))
                    {
                        rigoriOspite = ro;
                    }
                }

                if (tipo == 2)
                {
                    nuovaPartita = new PartitaEliminazione(giornata, casa, ospite, risultato, dataPartita, rigoriCasa, rigoriOspite);
                }
                else
                {
                    nuovaPartita = new PartitaFinale(giornata, casa, ospite, risultato, dataPartita, rigoriCasa, rigoriOspite);
                }
            }

            bool aggiunto = repository.Aggiungi(nuovaPartita);
            if (aggiunto)
            {
                repository.Salva(); // Salvataggio sul file JSON
                Console.WriteLine("Partita inserita e salvata con successo");
            }
            else
            {
                Console.WriteLine("Errore");
            }
        }

        public static void MostraClassifica()
        {
            IReadOnlyList<Partita> elenco = repository.GetElenco();
            List<StatisticheSquadra> classifica = CalcolaClassifica(elenco);

            if (classifica.Count == 0)
            {
                Console.WriteLine("Nessuna statistica disponibile");
                return;
            }

            Console.WriteLine("classifica gironi");

            foreach (StatisticheSquadra s in classifica)
            {
                string nome = s.NomeSquadra != null ? s.NomeSquadra : string.Empty;

                Console.WriteLine($"{nome} | {s.Punti} | {s.Giocate} | {s.Vinte} | {s.Pareggiate} | {s.Perse} | {s.GolFatti} | {s.GolSubiti} | {s.DifferenzaReti}");
            }
        }

        public static void CercaPartiteSquadra()
        {
            IReadOnlyList<Partita> elenco = repository.GetElenco();
            if (elenco.Count == 0)
            {
                Console.WriteLine("L'archivio è vuoto");
                return;
            }

            Console.Write("Inserisci il nome della squadra da cercare");
            string nomeSquadra = Console.ReadLine();

            bool trovate = false;
            Console.WriteLine($"partite della squadra {nomeSquadra}");
            foreach (Partita p in elenco)
            {
                if (string.Equals(p.Casa, nomeSquadra) || string.Equals(p.Ospite, nomeSquadra))
                {
                    Console.WriteLine(p.Descrizione());
                    trovate = true;
                }
            }

            if (!trovate)
            {
                Console.WriteLine("Nessuna partita trovata per questa squadra");
            }
        }

        public static void EliminaPartita()
        {
            IReadOnlyList<Partita> elenco = repository.GetElenco();
            if (elenco.Count == 0)
            {
                Console.WriteLine("L'archivio è vuoto");
                return;
            }

            Console.Write("Giornata della partita da eliminare: ");
            int giornata;
            if (!int.TryParse(Console.ReadLine(), out giornata))
            {
                Console.WriteLine("Giornata non valida");
                return;
            }

            Console.Write("Squadra di casa: ");
            string casa = Console.ReadLine();

            Console.Write("Squadra ospite: ");
            string ospite = Console.ReadLine();

            // Oggetto per sfruttare il metodo Equals del repository
            Partita daRimuovere = new PartitaGirone(giornata, casa, ospite, "0-0", DateTime.Now);

            bool rimosso = repository.Rimuovi(daRimuovere);
            if (rimosso)
            {
                repository.Salva(); // Salvataggio sul file JSON
                Console.WriteLine("Partita eliminata e file aggiornato con successo");
            }
            else
            {
                Console.WriteLine("Partita non trovata nell'archivio");
            }
        }

        public static List<StatisticheSquadra> CalcolaClassifica(IReadOnlyList<Partita> partite)
        {
            List<StatisticheSquadra> classifica = new List<StatisticheSquadra>();

            foreach (Partita p in partite)
            {
                if (p is PartitaGirone girone)
                {
                    // IEquatable attraverso il metodo TrovaOCreaSquadra
                    StatisticheSquadra statCasa = TrovaOCreaSquadra(classifica, girone.Casa);
                    StatisticheSquadra statOspite = TrovaOCreaSquadra(classifica, girone.Ospite);

                    statCasa.Giocate++;
                    statOspite.Giocate++;

                    int golC = girone.GetGolCasa();
                    int golO = girone.GetGolOspite();

                    statCasa.GolFatti += golC;
                    statCasa.GolSubiti += golO;
                    statOspite.GolFatti += golO;
                    statOspite.GolSubiti += golC;

                    Esito esito = girone.CalcolaEsito();
                    if (esito == Esito.VittoriaCasa)
                    {
                        statCasa.Vinte++;
                        statCasa.Punti += 3;
                        statOspite.Perse++;
                    }
                    else if (esito == Esito.VittoriaOspite)
                    {
                        statOspite.Vinte++;
                        statOspite.Punti += 3;
                        statCasa.Perse++;
                    }
                    else
                    {
                        statCasa.Pareggiate++;
                        statCasa.Punti += 1;
                        statOspite.Pareggiate++;
                        statOspite.Punti += 1;
                    }
                }
            }

            classifica.Sort(); // Sfrutta il contratto IComparable<StatisticheSquadra>
            return classifica;
        }

        private static StatisticheSquadra TrovaOCreaSquadra(List<StatisticheSquadra> classifica, string nomeSquadra)
        {
            StatisticheSquadra ricerca = new StatisticheSquadra(nomeSquadra);

            foreach (StatisticheSquadra s in classifica)
            {
                // Utilizza il contratto IEquatable<StatisticheSquadra>
                if (s.Equals(ricerca))
                {
                    return s;
                }
            }

            StatisticheSquadra nuova = new StatisticheSquadra(nomeSquadra);
            classifica.Add(nuova);
            return nuova;
        }
    }
}