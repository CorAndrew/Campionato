using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Campionato
{
    public class RepositoryPartite : IRepository<Partita>
    {
        private List<Partita> _partite;
        private string _filePath;

        public RepositoryPartite(string filePath = "partite.json")
        {
            _partite = new List<Partita>();
            _filePath = filePath;
        }

        // Restituisce l'elenco in sola lettura
        public IReadOnlyList<Partita> GetElenco()
        {
            return _partite.AsReadOnly();
        }

        // Inserisce una partita solo se non è già presente
        public bool Aggiungi(Partita elemento)
        {
            if (elemento == null) return false;

            // Controlla se esiste già un elemento uguale
            foreach (var p in _partite)
            {
                if (p.Equals(elemento))
                {
                    return false; // Già presente
                }
            }

            _partite.Add(elemento);
            return true;
        }

        // Elimina una partita e restituisce false se non la trova
        public bool Rimuovi(Partita elemento)
        {
            if (elemento == null) return false;

            return _partite.Remove(elemento);
        }

        // Salva la lista delle partitee
        public void Salva()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(_partite, options);
            File.WriteAllText(_filePath, jsonString);
        }

        // Carica le partite
        public void Carica()
        {
            // Se il file non esiste lo crea
            if (!File.Exists(_filePath))
            {
                _partite = new List<Partita>();
                return;
            }

            string jsonString = File.ReadAllText(_filePath);
            var datiCaricati = JsonSerializer.Deserialize<List<Partita>>(jsonString);

            _partite.Clear();

            if (datiCaricati != null)
            {
                foreach (var partita in datiCaricati)
                {
                    Aggiungi(partita);
                }
            }
        }
    }
}