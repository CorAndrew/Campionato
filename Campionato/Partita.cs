using System;

namespace Campionato
{
    public class Partita : IComparable<Partita>, IRisultato, IStampabile
    {
        public int Giornata { get; set; }
        public string Casa { get; set; }
        public string Ospite { get; set; }
        public string Risultato { get; set; }

        // Criterio di ordinamento
        public static string Criterio = "naturale";

        public Partita(int giornata, string casa, string ospite, string risultato)
        {
            Giornata = giornata;
            Casa = casa;
            Ospite = ospite;
            Risultato = risultato;
        }

        public int GetGolCasa()
        {
            if (string.IsNullOrEmpty(Risultato)) return 0;
            string[] parti = Risultato.Split('-');
            if (parti.Length > 0 && int.TryParse(parti[0].Trim(), out int gol))
                return gol;
            return 0;
        }

        public int GetGolOspite()
        {
            if (string.IsNullOrEmpty(Risultato)) return 0;
            string[] parti = Risultato.Split('-');
            if (parti.Length > 1 && int.TryParse(parti[1].Trim(), out int gol))
                return gol;
            return 0;
        }

        public int CalcolaGolTotali()
        {
            return GetGolCasa() + GetGolOspite();
        }

        public int CalcolaScarto()
        {
            return (GetGolCasa() - GetGolOspite());
        }

        public Esito CalcolaEsito()
        {
            int casa = GetGolCasa();
            int ospite = GetGolOspite();
            if (casa > ospite) return Esito.VittoriaCasa;
            if (ospite > casa) return Esito.VittoriaOspite;
            return Esito.Pareggio;
        }

        public string CalcolaVincitore()
        {
            int casa = GetGolCasa();
            int ospite = GetGolOspite();
            if (casa > ospite) return Casa;
            if (ospite > casa) return Ospite;
            return null; // Pareggio
        }

        public virtual string Descrizione()
        {
            return $"Giornata {Giornata} - {Casa} vs {Ospite} | Risultato: {Risultato}";
        }

        public override bool Equals(object obj)
        {
            Partita altra = obj as Partita;

            if (altra is null)
                return false;

            if (Giornata != altra.Giornata)
                return false;

            if (!string.Equals(Casa, altra.Casa, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!string.Equals(Ospite, altra.Ospite, StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        public static bool operator ==(Partita a, Partita b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Partita a, Partita b)
        {
            return !(a == b);
        }

        public int CompareTo(Partita altra)
        {
            if (altra == null) return 1;

            if (Criterio == "gol")
                return altra.CalcolaGolTotali().CompareTo(this.CalcolaGolTotali());

            if (Criterio == "scarto")
                return altra.CalcolaScarto().CompareTo(this.CalcolaScarto());

            if (Criterio == "casaAZ")
                return string.Compare(Casa, altra.Casa, StringComparison.OrdinalIgnoreCase);

            if (Criterio == "casaZA")
                return string.Compare(altra.Casa, Casa, StringComparison.OrdinalIgnoreCase);



            if (Giornata != altra.Giornata)
                return Giornata.CompareTo(altra.Giornata);

            return string.Compare(Casa, altra.Casa, StringComparison.OrdinalIgnoreCase);
        }
    }
}