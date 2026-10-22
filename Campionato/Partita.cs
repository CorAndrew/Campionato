using System;

namespace Campionato
{
    public class Partita : IComparable<Partita>, IEquatable<Partita>, IRisultato, IStampabile
    {
        public int Giornata { get; set; }
        public string Casa { get; set; }
        public string Ospite { get; set; }
        public string Risultato { get; set; }
        public DateTime Data { get; set; }

        public static string Criterio = "naturale";

        public Partita(int giornata, string casa, string ospite, string risultato, DateTime data)
        {
            Giornata = giornata;
            Casa = casa;
            Ospite = ospite;
            Risultato = risultato;
            Data = data;
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
            return Math.Abs(GetGolCasa() - GetGolOspite());
        }

        public Esito CalcolaEsito()
        {
            int casa = GetGolCasa();
            int ospite = GetGolOspite();
            if (casa > ospite) return Esito.VittoriaCasa;
            if (ospite > casa) return Esito.VittoriaOspite;
            return Esito.Pareggio;
        }

        public virtual string CalcolaVincitore()
        {
            int casa = GetGolCasa();
            int ospite = GetGolOspite();
            if (casa > ospite) return Casa;
            if (ospite > casa) return Ospite;
            return string.Empty;
        }

        public virtual string Descrizione()
        {
            return $"{Data.ToShortDateString()} - Giornata {Giornata} - {Casa} vs {Ospite} | Risultato: {Risultato}";
        }

        // Metodo di IEquatable
        public bool Equals(Partita altra)
        {
            if (altra == null) return false;
            return Giornata == altra.Giornata &&
                   string.Equals(Casa, altra.Casa, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Ospite, altra.Ospite, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Partita);
        }

        public static bool operator ==(Partita a, Partita b)
        {
            if (object.ReferenceEquals(a, null) && object.ReferenceEquals(b, null)) return true;
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Partita a, Partita b)
        {
            return !(a == b);
        }

        // Metodo di IComparable
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

            if (Criterio == "data")
                return Data.CompareTo(altra.Data);

            if (Giornata != altra.Giornata)
                return Giornata.CompareTo(altra.Giornata);

            return string.Compare(Casa, altra.Casa, StringComparison.OrdinalIgnoreCase);
        }
    }
}