using System;

namespace Campionato
{
    public class PartitaEliminazione : Partita
    {
        public int RigoriCasa { get; set; }
        public int RigoriOspite { get; set; }

        public PartitaEliminazione(int giornata, string casa, string ospite, string risultato, DateTime data, int rigoriCasa, int rigoriOspite) : base(giornata, casa, ospite, risultato, data)
        {
            RigoriCasa = rigoriCasa;
            RigoriOspite = rigoriOspite;
        }

        public override string CalcolaVincitore()
        {
            int casa = GetGolCasa();
            int ospite = GetGolOspite();

            if (casa > ospite) return Casa;
            if (ospite > casa) return Ospite;

            if (RigoriCasa >= 0 && RigoriOspite >= 0)
            {
                if (RigoriCasa > RigoriOspite) return Casa;
                if (RigoriOspite > RigoriCasa) return Ospite;
            }

            return null;
        }

        public override string Descrizione()
        {
            string baseDesc = base.Descrizione();
            if (CalcolaEsito() == Esito.Pareggio && RigoriCasa >= 0 && RigoriOspite >= 0)
            {
                string vincitore = CalcolaVincitore();
                return $"{baseDesc} {RigoriCasa}-{RigoriOspite}, vince {vincitore}";
            }
            return baseDesc;
        }
    }
}