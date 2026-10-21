using System;

namespace Campionato
{
    public class StatisticheSquadra : IComparable<StatisticheSquadra>, IEquatable<StatisticheSquadra>
    {
        public string NomeSquadra { get; set; }
        public int Giocate { get; set; }
        public int Vinte { get; set; }
        public int Pareggiate { get; set; }
        public int Perse { get; set; }
        public int GolFatti { get; set; }
        public int GolSubiti { get; set; }
        public int DifferenzaReti => GolFatti - GolSubiti;
        public int Punti { get; set; }

        public StatisticheSquadra(string nomeSquadra)
        {
            NomeSquadra = nomeSquadra;
        }

        // Due righe sono uguali se riguardano la stessa squadra
        public bool Equals(StatisticheSquadra other)
        {
            if (other == null) return false;
            return string.Equals(NomeSquadra, other.NomeSquadra, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StatisticheSquadra);
        }


        public int CompareTo(StatisticheSquadra other)
        {
            if (other == null) return 1;

            // ordinamento punti decrescente
            int compPunti = other.Punti.CompareTo(this.Punti);
            if (compPunti != 0) return compPunti;

            // differenza reti
            int compDr = other.DifferenzaReti.CompareTo(this.DifferenzaReti);
            if (compDr != 0) return compDr;

            // Gol fatti
            int compGf = other.GolFatti.CompareTo(this.GolFatti);
            if (compGf != 0) return compGf;

            // Nome squadra
            return string.Compare(NomeSquadra, other.NomeSquadra, StringComparison.OrdinalIgnoreCase);
        }
    }
}