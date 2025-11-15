using System;
using System.Collections.Generic;
using System.Linq;

namespace PametnaBiblioteka
{
    public class Knjiga
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string Autor { get; set; }
        public string Zanr { get; set; }
        public bool Dostupna { get; set; } = true;

        public override string ToString()
        {
            string status = Dostupna ? "DOSTUPNA" : "NIJE DOSTUPNA";
            return $"{Id}. {Naslov} - {Autor} [{Zanr}] ({status})";
        }
    }

    public class InventarKnjiga
    {
        private readonly List<Knjiga> _knjige;
        private int _sljedeciId;

        public InventarKnjiga(List<Knjiga> knjige)
        {
            _knjige = knjige;
            _sljedeciId = _knjige.Any() ? _knjige.Max(k => k.Id) + 1 : 1;
        }

        public void Meni()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("INVENTAR KNJIGA");
                Console.WriteLine("1) Prikaz svih knjiga");
                Console.WriteLine("2) Dodavanje knjige");
                Console.WriteLine("3) Ažuriranje knjige");
                Console.WriteLine("4) Brisanje knjige");
                Console.WriteLine("5) Pretraga knjiga");
                Console.WriteLine("0) Povratak u glavni meni");
                Console.Write("Odabir: ");
                string izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1": PrikaziSve(); break;
                    case "2": DodajKnjigu(); break;
                    case "3": AzurirajKnjigu(); break;
                    case "4": ObrisiKnjigu(); break;
                    case "5": Pretraga(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Neispravan izbor!");
                        Pauza();
                        break;
                }
            }
        }

        private void PrikaziSve()
        {
            Console.Clear();
            Console.WriteLine("SVE KNJIGE");
            if (!_knjige.Any())
            {
                Console.WriteLine("Nema unesenih knjiga.");
            }
            else
            {
                foreach (var k in _knjige)
                    Console.WriteLine(k);
            }
            Pauza();
        }

        private void DodajKnjigu()
        {
            Console.Clear();
            Console.WriteLine("DODAVANJE KNJIGE");

            Console.Write("Naslov: ");
            string naslov = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(naslov))
            {
                Console.WriteLine("Naslov je obavezan.");
                Pauza();
                return;
            }

            Console.Write("Autor: ");
            string autor = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(autor))
            {
                Console.WriteLine("Autor je obavezan");
                Pauza();
                return;
            }

            Console.Write("Zanr: ");
            string zanr = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(zanr))
            {
                Console.WriteLine("Zanr je obavezan.");
                Pauza();
                return;
            }

            var nova = new Knjiga
            {
                Id = _sljedeciId++,
                Naslov = naslov.Trim(),
                Autor = autor.Trim(),
                Zanr = zanr.Trim(),
                Dostupna = true
            };

            _knjige.Add(nova);
            Console.WriteLine("Knjiga dodana");
            Pauza();
        }

        private void AzurirajKnjigu()
        {
            Console.Clear();
            Console.WriteLine("AŽURIRANJE KNJIGE");

            if (!_knjige.Any())
            {
                Console.WriteLine("Nema knjiga.");
                Pauza();
                return;
            }

            PrikaziSveBezPauze();
            Console.Write("Unesi ID knjige za azuriranje: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Neispravan ID.");
                Pauza();
                return;
            }

            var knjiga = _knjige.FirstOrDefault(k => k.Id == id);
            if (knjiga == null)
            {
                Console.WriteLine("Knjiga nije pronadjjena");
                Pauza();
                return;
            }

            Console.Write($"Novi naslov ({knjiga.Naslov}): ");
            string noviNaslov = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(noviNaslov))
                knjiga.Naslov = noviNaslov.Trim();

            Console.Write($"Novi autor ({knjiga.Autor}): ");
            string noviAutor = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(noviAutor))
                knjiga.Autor = noviAutor.Trim();

            Console.Write($"Novi zanr ({knjiga.Zanr}): ");
            string noviZanr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(noviZanr))
                knjiga.Zanr = noviZanr.Trim();

            Console.WriteLine("Knjiga je azurirana.");
            Pauza();
        }

        private void ObrisiKnjigu()
        {
            Console.Clear();
            Console.WriteLine("BRISANJE KNJIGE");

            if (!_knjige.Any())
            {
                Console.WriteLine("Nema knjiga.");
                Pauza();
                return;
            }

            PrikaziSveBezPauze();
            Console.Write("Unesi ID knjige za brisanje: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Neispravan ID.");
                Pauza();
                return;
            }

            var knjiga = _knjige.FirstOrDefault(k => k.Id == id);
            if (knjiga == null)
            {
                Console.WriteLine("Knjiga nije pronadjena.");
                Pauza();
                return;
            }

            Console.Write($"Da li sigurno želiš obrisati '{knjiga.Naslov}'? (y/N): ");
            string potvrda = Console.ReadLine();
            if (potvrda.ToLower() == "y")
            {
                _knjige.Remove(knjiga);
                Console.WriteLine("Knjiga obrisana.");
            }
            else
            {
                Console.WriteLine("Brisanje otkazano");
            }

            Pauza();
        }

        private void Pretraga()
        {
            Console.Clear();
            Console.WriteLine("PRETRAGA KNJIGA");
            Console.WriteLine("Pretraga po:");
            Console.WriteLine("1) Naslov");
            Console.WriteLine("2) Autor");
            Console.WriteLine("3) Žanr");
            Console.Write("Odabir: ");
            string izbor = Console.ReadLine();

            Console.Write("Unesi pojam za pretragu: ");
            string pojam = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(pojam))
            {
                Console.WriteLine("Prazan pojam.");
                Pauza();
                return;
            }

            pojam = pojam.Trim().ToLower();
            IEnumerable<Knjiga> rezultat = Enumerable.Empty<Knjiga>();

            switch (izbor)
            {
                case "1":
                    rezultat = _knjige.Where(k => k.Naslov.ToLower().Contains(pojam));
                    break;
                case "2":
                    rezultat = _knjige.Where(k => k.Autor.ToLower().Contains(pojam));
                    break;
                case "3":
                    rezultat = _knjige.Where(k => k.Zanr.ToLower().Contains(pojam));
                    break;
                default:
                    Console.WriteLine("Neispravan izbor.");
                    Pauza();
                    return;
            }

            Console.WriteLine("\nRezultati:");
            if (!rezultat.Any())
            {
                Console.WriteLine("Nema rezultata");
            }
            else
            {
                foreach (var k in rezultat)
                    Console.WriteLine(k);
            }

            Pauza();
        }

        private void PrikaziSveBezPauze()
        {
            foreach (var k in _knjige)
                Console.WriteLine(k);
        }

        private void Pauza()
        {
            Console.WriteLine("\nPritisnite Enter za nastavak");
            Console.ReadLine();
        }
    }
}

