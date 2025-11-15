using System;
using System.Collections.Generic;
using System.Linq;

namespace PametnaBiblioteka
{
    public class Posudba
    {
        public int Id { get; set; }
        public Korisnik Korisnik { get; set; }
        public Knjiga Knjiga { get; set; }
        public DateTime DatumPosudbe { get; set; }
        public DateTime RokVracanja { get; set; }
        public DateTime? DatumVracanja { get; set; }
        public decimal Kazna { get; set; }

        public bool Vracena => DatumVracanja.HasValue;

        public string Status
        {
            get
            {
                if (Vracena) return "VRACENA";
                if (DateTime.Today.Date > RokVracanja.Date) return "KASNJENJE";
                return "AKTIVNA";
            }
        }

        public override string ToString()
        {
            string osnovno = $"{Id}. {Knjiga?.Naslov} -> {Korisnik?.ImePrezime} | " +
                             $"Posudjeno: {DatumPosudbe:dd.MM.yyyy}, Rok: {RokVracanja:dd.MM.yyyy}, Status: {Status}";

            if (Vracena)
            {
                osnovno += $", Vraćeno: {DatumVracanja:dd.MM.yyyy}, Kazna: {Kazna:0.00} KM";
            }

            return osnovno;
        }
    }

    public class SistemPosudbe
    {
        private readonly List<Posudba> _posudbe;
        private readonly List<Korisnik> _korisnici;
        private readonly List<Knjiga> _knjige;
        private int _sljedeciId = 1;

        private const int BROJ_DANA_POSUDBE = 14;
        private const decimal KAZNA_PO_DANU = 0.50m;
        private const int MAX_POSUDBI_PO_KORISNIKU = 3;

        public SistemPosudbe(List<Posudba> posudbe, List<Korisnik> korisnici, List<Knjiga> knjige)
        {
            _posudbe = posudbe;
            _korisnici = korisnici;
            _knjige = knjige;
            if (_posudbe.Any()) _sljedeciId = _posudbe.Max(p => p.Id) + 1;
        }

        public void Meni()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=SISTEM POSUDBE+");
                Console.WriteLine("1) Posudba knjige");
                Console.WriteLine("2) Vraćanje knjige");
                Console.WriteLine("3) Prikaz svih posudbi");
                Console.WriteLine("4) Prikaz aktivnih posudbi");
                Console.WriteLine("5) Prikaz prekoračenih posudbi");
                Console.WriteLine("6) Historija posudbi za korisnika");
                Console.WriteLine("0) Povratak u glavni meni");
                Console.Write("Odabir: ");
                string izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1": PosudiKnjigu(); break;
                    case "2": VratiKnjigu(); break;
                    case "3": PrikaziSve(); break;
                    case "4": PrikaziAktivne(); break;
                    case "5": PrikaziPrekoracene(); break;
                    case "6": HistorijaZaKorisnika(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Neispravan izbor!");
                        Pauza();
                        break;
                }
            }
        }

        private void PosudiKnjigu()
        {
            Console.Clear();
            Console.WriteLine("= POSUDBA KNJIGE=");

            if (!_korisnici.Any())
            {
                Console.WriteLine("Nema registrovanih korisnika");
                Pauza();
                return;
            }

            if (!_knjige.Any())
            {
                Console.WriteLine("Nema knjiga u inventaru.");
                Pauza();
                return;
            }

            var korisnik = OdaberiKorisnika();
            if (korisnik == null) return;

            int aktivnePosudbe = _posudbe.Count(p => p.Korisnik.Id == korisnik.Id && !p.Vracena);
            if (aktivnePosudbe >= MAX_POSUDBI_PO_KORISNIKU)
            {
                Console.WriteLine($"Korisnik već ima {aktivnePosudbe} aktivnih posudbi (max {MAX_POSUDBI_PO_KORISNIKU}).");
                Pauza();
                return;
            }

            var dostupneKnjige = _knjige.Where(k => k.Dostupna).ToList();
            if (!dostupneKnjige.Any())
            {
                Console.WriteLine("Nema dostupnih knjiga za posudbu.");
                Pauza();
                return;
            }

            var knjiga = OdaberiKnjigu(dostupneKnjige);
            if (knjiga == null) return;

            var posudba = new Posudba
            {
                Id = _sljedeciId++,
                Korisnik = korisnik,
                Knjiga = knjiga,
                DatumPosudbe = DateTime.Today,
                RokVracanja = DateTime.Today.AddDays(BROJ_DANA_POSUDBE),
                DatumVracanja = null,
                Kazna = 0
            };

            knjiga.Dostupna = false;
            _posudbe.Add(posudba);

            Console.WriteLine("\nPosudba evidentirana:");
            Console.WriteLine(posudba);
            Pauza();
        }

        private void VratiKnjigu()
        {
            Console.Clear();
            Console.WriteLine("VRAĆANJE KNJIGE");

            var aktivne = _posudbe.Where(p => !p.Vracena).ToList();
            if (!aktivne.Any())
            {
                Console.WriteLine("Nema aktivnih posudbi.");
                Pauza();
                return;
            }

            var posudba = OdaberiPosudbu(aktivne);
            if (posudba == null) return;

            posudba.DatumVracanja = DateTime.Today;

            if (posudba.DatumVracanja.Value.Date > posudba.RokVracanja.Date)
            {
                int kasnjenje = (int)(posudba.DatumVracanja.Value.Date - posudba.RokVracanja.Date).TotalDays;
                if (kasnjenje < 0) kasnjenje = 0;
                posudba.Kazna = kasnjenje * KAZNA_PO_DANU;
            }

            posudba.Knjiga.Dostupna = true;

            Console.WriteLine("\nKnjiga je vraćena:");
            Console.WriteLine(posudba);
            Pauza();
        }

        private void PrikaziSve()
        {
            Console.Clear();
            Console.WriteLine("SVE POSUDBE");
            if (!_posudbe.Any())
            {
                Console.WriteLine("Nema posudbi.");
            }
            else
            {
                foreach (var p in _posudbe)
                    Console.WriteLine(p);
            }
            Pauza();
        }

        private void PrikaziAktivne()
        {
            Console.Clear();
            Console.WriteLine("=AKTIVNE POSUDBE=");
            var aktivne = _posudbe.Where(p => !p.Vracena).ToList();
            if (!aktivne.Any())
            {
                Console.WriteLine("Nema aktivnih posudbi.");
            }
            else
            {
                foreach (var p in aktivne)
                    Console.WriteLine(p);
            }
            Pauza();
        }

        private void PrikaziPrekoracene()
        {
            Console.Clear();
            Console.WriteLine("=PREKORAČENE POSUDBE=");
            var prek = _posudbe.Where(p => !p.Vracena && DateTime.Today.Date > p.RokVracanja.Date).ToList();
            if (!prek.Any())
            {
                Console.WriteLine("Nema prekoracenih posudbi.");
            }
            else
            {
                foreach (var p in prek)
                {
                    int kasnjenje = (int)(DateTime.Today.Date - p.RokVracanja.Date).TotalDays;
                    if (kasnjenje < 0) kasnjenje = 0;
                    Console.WriteLine(p);
                    Console.WriteLine($"   -> Kasnjenje: {kasnjenje} dana");
                }
            }
            Pauza();
        }

        private void HistorijaZaKorisnika()
        {
            Console.Clear();
            Console.WriteLine("= HISTORIJA POSUDBI KORISNIKA =");

            if (!_korisnici.Any())
            {
                Console.WriteLine("Nema korisnika.");
                Pauza();
                return;
            }

            var korisnik = OdaberiKorisnika();
            if (korisnik == null) return;

            var lista = _posudbe.Where(p => p.Korisnik.Id == korisnik.Id).ToList();
            if (!lista.Any())
            {
                Console.WriteLine("Korisnik nema posudbi.");
            }
            else
            {
                Console.WriteLine($"\nPosudbe za {korisnik.ImePrezime}:");
                foreach (var p in lista)
                    Console.WriteLine(p);
            }
            Pauza();
        }

        private Korisnik OdaberiKorisnika()
        {
            Console.WriteLine("\nKorisnici:");
            for (int i = 0; i < _korisnici.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {_korisnici[i]}");
            }

            Console.Write("Odaberi korisnika ( 0 za otkaz): ");
            if (!int.TryParse(Console.ReadLine(), out int izbor) || izbor < 0 || izbor > _korisnici.Count)
            {
                Console.WriteLine("Neispravan izbor.");
                Pauza();
                return null;
            }

            if (izbor == 0) return null;

            return _korisnici[izbor - 1];
        }

        private Knjiga OdaberiKnjigu(List<Knjiga> lista)
        {
            Console.WriteLine("\nKnjige:");
            for (int i = 0; i < lista.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {lista[i]}");
            }

            Console.Write("Odaberi knjigu (broj, 0 za otkaz): ");
            if (!int.TryParse(Console.ReadLine(), out int izbor) || izbor < 0 || izbor > lista.Count)
            {
                Console.WriteLine("Neispravan izbor.");
                Pauza();
                return null;
            }

            if (izbor == 0) return null;

            return lista[izbor - 1];
        }

        private Posudba OdaberiPosudbu(List<Posudba> lista)
        {
            Console.WriteLine("\nPosudbe:");
            for (int i = 0; i < lista.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {lista[i]}");
            }

            Console.Write("Odaberi posudbu ( 0 za otkaz): ");
            if (!int.TryParse(Console.ReadLine(), out int izbor) || izbor < 0 || izbor > lista.Count)
            {
                Console.WriteLine("Neispravan izbor.");
                Pauza();
                return null;
            }

            if (izbor == 0) return null;

            return lista[izbor - 1];
        }

        private void Pauza()
        {
            Console.WriteLine("\nPritisnite Enter za nastavak");
            Console.ReadLine();
        }
    }
}

