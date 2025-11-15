using System;
using System.Collections.Generic;
using System.Text;

namespace PametnaBiblioteka
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Zajednički podaci za cijeli sistem
            var korisnici = new List<Korisnik>();
            var knjige = new List<Knjiga>();
            var posudbe = new List<Posudba>();

            // Malo testnih podataka (nije obavezno, ali pomaže)
            korisnici.Add(new Korisnik { Id = 1, ImePrezime = "Marko Marković", Email = "marko@example.com", Uloga = UlogaKorisnika.Clan });
            korisnici.Add(new Korisnik { Id = 2, ImePrezime = "Ana Anić", Email = "ana@example.com", Uloga = UlogaKorisnika.Administrator });

            knjige.Add(new Knjiga { Id = 1, Naslov = "Prokleta avlija", Autor = "Ivo Andrić", Zanr = "Roman", Dostupna = true });
            knjige.Add(new Knjiga { Id = 2, Naslov = "Na Drini ćuprija", Autor = "Ivo Andrić", Zanr = "Roman", Dostupna = true });
            knjige.Add(new Knjiga { Id = 3, Naslov = "Gospodar prstenova", Autor = "J.R.R. Tolkien", Zanr = "Fantasy", Dostupna = true });

            // Kreiranje modula
            var modulKorisnici = new UpravljanjeKorisnicima(korisnici);
            var modulInventar = new InventarKnjiga(knjige);
            var modulPosudbe = new SistemPosudbe(posudbe, korisnici, knjige);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("PAMETNA BIBLIOTEKA");
                Console.WriteLine("1) Upravljanje korisnicima");
                Console.WriteLine("2) Inventar knjiga");
                Console.WriteLine("3) Sistem posudbe");
                Console.WriteLine("0) Izlaz");
                Console.Write("Odabir: ");
                string izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        modulKorisnici.Meni();
                        break;
                    case "2":
                        modulInventar.Meni();
                        break;
                    case "3":
                        modulPosudbe.Meni();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Neispravan izbor!");
                        Pauza();
                        break;
                }
            }
        }

        private static void Pauza()
        {
            Console.WriteLine("\nPritisnite Enter za nastavak");
            Console.ReadLine();
        }
    }
}
