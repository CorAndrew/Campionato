using System;

namespace Campionato
{
    public class PartitaEliminazione : Partita
    {
        public string DettaglioRigori { get; set; }

        public PartitaEliminazione(int giornata, string casa, string ospite, string risultato, string dettaglioRigori) : base(giornata, casa, ospite, risultato)
        {
            DettaglioRigori = dettaglioRigori;
        }

        public override string Descrizione()
        {
            string baseDesc = base.Descrizione();
            if (!string.IsNullOrEmpty(DettaglioRigori))
            {
                string vincitore = CalcolaVincitore();
                return $"{baseDesc} ({DettaglioRigori}, passa {vincitore})";
            }
            return baseDesc;
        }
    }
}