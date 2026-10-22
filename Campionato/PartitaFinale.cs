using System;

namespace Campionato
{
    public class PartitaFinale : PartitaEliminazione
    {
        public PartitaFinale(int giornata, string casa, string ospite, string risultato, DateTime data, int rigoriCasa, int rigoriOspite) : base(giornata, casa, ospite, risultato, data, rigoriCasa, rigoriOspite)
        {
        }

        public override string Descrizione()
        {
            string baseDesc = base.Descrizione();
            return $"[FINALE] {baseDesc}";
        }

        public int PuntiRankingStorico(string squadra)
        {
            string vincitore = CalcolaVincitore();
            if (string.Equals(squadra, vincitore, StringComparison.OrdinalIgnoreCase))
            {
                return 6;
            }
            return 0;
        }
    }
}