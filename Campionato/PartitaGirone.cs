using System;

namespace Campionato
{
    public class PartitaGirone : Partita
    {
        public PartitaGirone(int giornata, string casa, string ospite, string risultato) : base(giornata, casa, ospite, risultato)
        {
        }

        public int PuntiPer(string squadra)
        {
            bool isCasa = string.Equals(squadra, Casa, StringComparison.OrdinalIgnoreCase);
            bool isOspite = string.Equals(squadra, Ospite, StringComparison.OrdinalIgnoreCase);

            if (!isCasa && !isOspite) return 0;

            Esito esito = CalcolaEsito();

            if (esito == Esito.Pareggio) return 1;

            if (esito == Esito.VittoriaCasa)
            {
                return isCasa ? 3 : 0;
            }
            else
            {
                return isOspite ? 3 : 0;
            }
        }
    }
}