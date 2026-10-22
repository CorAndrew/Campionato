using System;

namespace Campionato
{
    public class PartitaGirone : Partita
    {
        public PartitaGirone(int giornata, string casa, string ospite, string risultato, DateTime data) : base (giornata, casa, ospite, risultato, data)
        {
        }

        public int PuntiPer(string squadra)
        {
            bool isCasa = string.Equals(squadra, Casa);
            bool isOspite = string.Equals(squadra, Ospite);

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