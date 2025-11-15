using System;
using System.Collections.Generic;
using System.Linq;

namespace PametnaBiblioteka
{
    public class Knjiga
    {
        public int KnjigaID { get; set; }
        public string Naslov { get; set; }
        public string Autor { get; set; }
        public string Zanr { get; set; }
        public bool Dostupna { get; set; }

        public Knjiga()
        {
            Dostupna = true;
        }
    }

    public class InventarKnjiga
    {
        private List<Knjiga> knjige = new List<Knjiga>();
        private int sledeciID = 1;

        public void DodajKnjigu()
        {
            Console.WriteLine("\n=== DODAVANJE KNJIGE ===");
            Console.Write("Naslov: ");
            string naslov = Console.ReadLine();
            Console.Write("Autor: ");
            string autor = Console.ReadLine();
            Console.Write("Žanr: ");
            string zanr = Console.ReadLine();

            knjige.Add(new Knjiga
            {
                KnjigaID = sledeciID++,
                Naslov = naslov,
                Autor = autor,
                Zanr = zanr
            });

            Console.WriteLine("✅ Knjiga '" + naslov + "' dodana.");
        }

        public void AzurirajKnjigu()
        {
            Console.WriteLine("\n=== AŽURIRANJE KNJIGE ===");
            if (knjige.Count == 0)
            {
                Console.WriteLine("❌ Inventar je prazan.");
                return;
            }

            PrikaziSveKnjige();
            Console.Write("Unesite ID knjige: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("❌ Pogrešan unos ID-a!");
                return;
            }

            Knjiga knjiga = knjige.FirstOrDefault(k => k.KnjigaID == id);
            if (knjiga == null)
            {
                Console.WriteLine("❌ Knjiga nije pronađena!");
                return;
            }

            Console.Write("Novi naslov (Enter za preskok): ");
            string unos = Console.ReadLine();
            if (!string.IsNullOrEmpty(unos)) knjiga.Naslov = unos;

            Console.Write("Novi autor (Enter za preskok): ");
            unos = Console.ReadLine();
            if (!string.IsNullOrEmpty(unos)) knjiga.Autor = unos;

            Console.Write("Novi žanr (Enter za preskok): ");
            unos = Console.ReadLine();
            if (!string.IsNullOrEmpty(unos)) knjiga.Zanr = unos;

            Console.Write("Dostupna? (da/ne): ");
            unos = Console.ReadLine();
            if (unos != null && (unos.ToLower() == "da" || unos.ToLower() == "d"))
                knjiga.Dostupna = true;
            else if (unos != null && (unos.ToLower() == "ne" || unos.ToLower() == "n"))
                knjiga.Dostupna = false;

            Console.WriteLine("✅ Knjiga ažurirana!");
        }

        public void ObrisiKnjigu()
        {
            Console.WriteLine("\n BRISANJE KNJIGE");
            if (knjige.Count == 0)
            {
                Console.WriteLine("❌ Inventar je prazan!");
                return;
            }

            PrikaziSveKnjige();
            Console.Write("Unesite ID knjige za brisanje: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("❌ Pogrešan ID!");
                return;
            }

            Knjiga knjiga = knjige.FirstOrDefault(k => k.KnjigaID == id);
            if (knjiga == null)
            {
                Console.WriteLine("❌ Knjiga nije pronađena!");
                return;
            }

            knjige.Remove(knjiga);
            Console.WriteLine("✅ Knjiga '" + knjiga.Naslov + "' obrisana.");
        }

        public void PretraziKnjige(string kriterijum)
        {
            Console.WriteLine("\n=== PRETRAGA PO " + kriterijum.ToUpper() + " ===");
            Console.Write("Unesite pojam (" + kriterijum + "): ");
            string pojam = Console.ReadLine().ToLower();

            List<Knjiga> rezultati = new List<Knjiga>();
            if (kriterijum == "naslov")
                rezultati = knjige.Where(k => k.Naslov.ToLower().Contains(pojam)).ToList();
            else if (kriterijum == "autor")
                rezultati = knjige.Where(k => k.Autor.ToLower().Contains(pojam)).ToList();
            else if (kriterijum == "žanr" || kriterijum == "zanr")
                rezultati = knjige.Where(k => k.Zanr.ToLower().Contains(pojam)).ToList();

            if (rezultati.Count == 0)
            {
                Console.WriteLine("❌ Nema pronađenih knjiga!");
                return;
            }

            foreach (var k in rezultati)
            {
                string status = k.Dostupna ? "DOSTUPNA" : "NIJE DOSTUPNA";
                Console.WriteLine("ID: " + k.KnjigaID + " | " + k.Naslov + " | Autor: " + k.Autor + " | Žanr: " + k.Zanr + " | " + status);
            }
        }

        public void PrikaziDostupnost()
        {
            Console.WriteLine("\n DOSTUPNOST KNJIGA");
            if (knjige.Count == 0)
            {
                Console.WriteLine("❌Nema knjiga u inventaru!");
                return;
            }

            Console.WriteLine("\nDOSTUPNE:");
            foreach (var k in knjige.Where(x => x.Dostupna))
                Console.WriteLine("ID: " + k.KnjigaID + " | " + k.Naslov);

            Console.WriteLine("\nNEDOSTUPNE:");
            foreach (var k in knjige.Where(x => !x.Dostupna))
                Console.WriteLine("ID: " + k.KnjigaID + " | " + k.Naslov);
        }

        public void PrikaziSveKnjige()
        {
            Console.WriteLine("\n=== INVENTAR KNJIGA ===");
            if (knjige.Count == 0)
            {
                Console.WriteLine("Inventar je prazan.");
                return;
            }

            foreach (var k in knjige)
            {
                string status = k.Dostupna ? "DOSTUPNA" : "NIJE DOSTUPNA";
                Console.WriteLine("ID: " + k.KnjigaID + " | " + k.Naslov + " | Autor: " + k.Autor + " | Žanr: " + k.Zanr + " | " + status);
            }
        }

        public void MeniInventara()
        {
            while (true)
            {
                Console.WriteLine("\n=== MENI INVENTARA ===");
                Console.WriteLine("1) Dodaj knjigu");
                Console.WriteLine("2) Ažuriraj knjigu");
                Console.WriteLine("3) Obriši knjigu");
                Console.WriteLine("4) Pretraga po naslovu");
                Console.WriteLine("5) Pretraga po autoru");
                Console.WriteLine("6) Pretraga po žanru");
                Console.WriteLine("7) Prikaži dostupnost");
                Console.WriteLine("8) Prikaži sve knjige");
                Console.WriteLine("0) Povratak u glavni meni");
                Console.Write("Odabir: ");

                string izbor = Console.ReadLine();

                if (izbor == "1") DodajKnjigu();
                else if (izbor == "2") AzurirajKnjigu();
                else if (izbor == "3") ObrisiKnjigu();
                else if (izbor == "4") PretraziKnjige("naslov");
                else if (izbor == "5") PretraziKnjige("autor");
                else if (izbor == "6") PretraziKnjige("žanr");
                else if (izbor == "7") PrikaziDostupnost();
                else if (izbor == "8") PrikaziSveKnjige();
                else if (izbor == "0") return;
                else Console.WriteLine("❌ Neispravan izbor!");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            InventarKnjiga inventar = new InventarKnjiga();
            inventar.MeniInventara();
        }
    }
}
