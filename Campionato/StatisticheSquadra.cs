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
        public bool Equals(StatisticheSquadra altra)
        {
            if (altra == null) return false;
            return string.Equals(NomeSquadra, altra.NomeSquadra);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StatisticheSquadra);
        }

        public int CompareTo(StatisticheSquadra altra)
        {
            if (altra == null) return 1;

            int compPunti = altra.Punti.CompareTo(this.Punti);
            if (compPunti != 0) return compPunti;

            int compDr = altra.DifferenzaReti.CompareTo(this.DifferenzaReti);
            if (compDr != 0) return compDr;

            int compGf = altra.GolFatti.CompareTo(this.GolFatti);
            if (compGf != 0) return compGf;

            return string.Compare(NomeSquadra, altra.NomeSquadra);
        }
    }
}