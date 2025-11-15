using System;
using System.Collections.Generic;
using System.Linq;

namespace PametnaBiblioteka
{
    public enum UlogaKorisnika
    {
        Administrator = 1,
        Clan = 2
    }

    public class Korisnik
    {
        public int Id { get; set; }
        public string ImePrezime { get; set; }
        public string Email { get; set; }
        public UlogaKorisnika Uloga { get; set; }

        public override string ToString()
        {
            return $"{Id}. {ImePrezime} ({Email}) - {Uloga}";
        }
    }

    public class UpravljanjeKorisnicima
    {
        private readonly List<Korisnik> _korisnici;
        private int _sljedeciId;

        public UpravljanjeKorisnicima(List<Korisnik> korisnici)
        {
            _korisnici = korisnici;
            _sljedeciId = _korisnici.Any() ? _korisnici.Max(k => k.Id) + 1 : 1;
        }

        public void Meni()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("UPRAVLJANJE KORISNICIMA");
                Console.WriteLine("1) Prikaz svih korisnika");
                Console.WriteLine("2) Registracija korisnika");
                Console.WriteLine("3) Ažuriranje korisnika");
                Console.WriteLine("4) Brisanje korisnika");
                Console.WriteLine("5) Dodjela uloge korisniku");
                Console.WriteLine("0) Povratak u glavni meni");
                Console.Write("Odabir: ");
                string izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1": PrikaziKorisnike(); break;
                    case "2": RegistrujKorisnika(); break;
                    case "3": AzurirajKorisnika(); break;
                    case "4": ObrisiKorisnika(); break;
                    case "5": DodijeliUlogu(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Neispravan izbor!");
                        Pauza();
                        break;
                }
            }
        }

        private void PrikaziKorisnike()
        {
            Console.Clear();
            Console.WriteLine("SVI KORISNICI");

            if (!_korisnici.Any())
            {
                Console.WriteLine("Nema registrovanih korisnika.");
            }
            else
            {
                foreach (var k in _korisnici)
                    Console.WriteLine(k);
            }

            Pauza();
        }

        private void RegistrujKorisnika()
        {
            Console.Clear();
            Console.WriteLine("REGISTRACIJA KORISNIKA");

            Console.Write("Ime i prezime: ");
            string ime = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(ime))
            {
                Console.WriteLine("Ime je obavezno.");
                Pauza();
                return;
            }

            Console.Write("Email: ");
            string email = Console.ReadLine();
            if (!ValidanEmail(email))
            {
                Console.WriteLine("Email nije ispravan.");
                Pauza();
                return;
            }

            Console.WriteLine("Uloga:");
            Console.WriteLine("1) Administrator");
            Console.WriteLine("2) Član");
            Console.Write("Odabir: ");
            string ulogaStr = Console.ReadLine();

            UlogaKorisnika uloga;
            if (ulogaStr == "1") uloga = UlogaKorisnika.Administrator;
            else if (ulogaStr == "2") uloga = UlogaKorisnika.Clan;
            else
            {
                Console.WriteLine("Neispravan izbor uloge.");
                Pauza();
                return;
            }

            var novi = new Korisnik
            {
                Id = _sljedeciId++,
                ImePrezime = ime.Trim(),
                Email = email.Trim(),
                Uloga = uloga
            };

            _korisnici.Add(novi);
            Console.WriteLine("Korisnik uspjesno registrovan.");
            Pauza();
        }

        private void AzurirajKorisnika()
        {
            Console.Clear();
            Console.WriteLine("AŽURIRANJE KORISNIKA");

            if (!_korisnici.Any())
            {
                Console.WriteLine("Nema korisnika");
                Pauza();
                return;
            }

            PrikaziKorisnikeBezPauze();
            Console.Write("Unesi ID korisnika za azuriranje: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Neispravan ID.");
                Pauza();
                return;
            }

            var korisnik = _korisnici.FirstOrDefault(k => k.Id == id);
            if (korisnik == null)
            {
                Console.WriteLine("Korisnik nije pronadjen.");
                Pauza();
                return;
            }

            Console.Write($"Novo ime i prezime ({korisnik.ImePrezime}): ");
            string novoIme = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoIme))
                korisnik.ImePrezime = novoIme.Trim();

            Console.Write($"Novi email ({korisnik.Email}): ");
            string noviEmail = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(noviEmail))
            {
                if (!ValidanEmail(noviEmail))
                {
                    Console.WriteLine("Email nije ispravan. Promjena emaila nije sacuvana");
                }
                else
                {
                    korisnik.Email = noviEmail.Trim();
                }
            }

            Console.WriteLine("Korisnik je azuriran.");
            Pauza();
        }

        private void ObrisiKorisnika()
        {
            Console.Clear();
            Console.WriteLine("BRISANJE KORISNIKA");

            if (!_korisnici.Any())
            {
                Console.WriteLine("Nema korisnika");
                Pauza();
                return;
            }

            PrikaziKorisnikeBezPauze();
            Console.Write("Unesi ID korisnika za brisanje: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Neispravan ID");
                Pauza();
                return;
            }

            var korisnik = _korisnici.FirstOrDefault(k => k.Id == id);
            if (korisnik == null)
            {
                Console.WriteLine("Korisnik nije pronadjen.");
                Pauza();
                return;
            }

            Console.Write($"Da li sigurno zelis obrisati korisnika {korisnik.ImePrezime}? (y/N): ");
            string potvrda = Console.ReadLine();
            if (potvrda.ToLower() == "y")
            {
                _korisnici.Remove(korisnik);
                Console.WriteLine("Korisnik obrisan");
            }
            else
            {
                Console.WriteLine("Brisanje otkazano");
            }

            Pauza();
        }

        private void DodijeliUlogu()
        {
            Console.Clear();
            Console.WriteLine("DODJELA ULOGE");

            if (!_korisnici.Any())
            {
                Console.WriteLine("Nema korisnika.");
                Pauza();
                return;
            }

            PrikaziKorisnikeBezPauze();
            Console.Write("Unesi ID korisnika: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Neispravan ID.");
                Pauza();
                return;
            }

            var korisnik = _korisnici.FirstOrDefault(k => k.Id == id);
            if (korisnik == null)
            {
                Console.WriteLine("Korisnik nije pronadjen");
                Pauza();
                return;
            }

            Console.WriteLine($"Trenutna uloga: {korisnik.Uloga}");
            Console.WriteLine("Nova uloga:");
            Console.WriteLine("1) Administrator");
            Console.WriteLine("2) Član");
            Console.Write("Odabir: ");
            string ulogaStr = Console.ReadLine();

            if (ulogaStr == "1") korisnik.Uloga = UlogaKorisnika.Administrator;
            else if (ulogaStr == "2") korisnik.Uloga = UlogaKorisnika.Clan;
            else
            {
                Console.WriteLine("Neispravan izbor. Uloga nije promijenjena.");
                Pauza();
                return;
            }

            Console.WriteLine("Uloga je azurirana.");
            Pauza();
        }

        private bool ValidanEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return email.Contains("@") && email.Contains(".");
        }

        private void PrikaziKorisnikeBezPauze()
        {
            foreach (var k in _korisnici)
                Console.WriteLine(k);
        }

        private void Pauza()
        {
            Console.WriteLine("\nPritisnite Enter za nastavak");
            Console.ReadLine();
        }
    }
}

