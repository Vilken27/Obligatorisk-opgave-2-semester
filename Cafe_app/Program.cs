using cafe_classLib;
using Microsoft.Data.SqlClient;

Console.WriteLine("Hello, World!");




Console.WriteLine();

AnsatteRepository repo = new AnsatteRepository();

try
{
    // CREATE
    Console.WriteLine("Create");

    Ansatte nyAnsat = new Ansatte(
        99,
        "Test Person",
        "12345678",
        "test@cantina.dk",
        false
    );

    repo.AddAnsat(nyAnsat);

    Console.WriteLine("Medarbejder oprettet.");
    Console.WriteLine();


    // READ ALL
    Console.WriteLine("Read All");

    List<Ansatte> ansatteListe = repo.GetAllAnsatte();

    foreach (Ansatte a in ansatteListe)
    {
        Console.WriteLine(a);
    }

    Console.WriteLine();


    // READ BY ID
    Console.WriteLine("Read ID");

    Ansatte fundet = repo.GetAnsatById(99);

    if (fundet != null)
    {
        Console.WriteLine(fundet);
    }
    else
    {
        Console.WriteLine("Ingen medarbejder fundet.");
    }

    Console.WriteLine();


    // UPDATE
    Console.WriteLine("Update");

    if (fundet != null)
    {
        fundet.Telefonnummer = "87654321";
        fundet.Email = "opdateret@cantina.dk";

        repo.UpdateAnsat(fundet);

        Console.WriteLine("Medarbejder opdateret.");
    }

    Console.WriteLine();


    // VIS EFTER UPDATE
    Console.WriteLine("Efter update");

    Ansatte opdateret = repo.GetAnsatById(99);

    if (opdateret != null)
    {
        Console.WriteLine(opdateret);
    }

    Console.WriteLine();


    // DELETE
    Console.WriteLine("Delete");

    repo.DeleteAnsat(99);

    Console.WriteLine("Medarbejder slettet.");
    Console.WriteLine();


    // VIS ALLE IGEN
    Console.WriteLine("¨Read After Delete");

    foreach (Ansatte a in repo.GetAllAnsatte())
    {
        Console.WriteLine(a);
    }
}
catch (Exception ex)
{
    Console.WriteLine("FEJL:");
    Console.WriteLine(ex.Message);
}

Console.WriteLine();

Console.WriteLine("Månedsplan");
repo.VisMånedsplan(2026, 10);

Console.WriteLine();
Console.WriteLine("Belastning måned");
repo.VisBelastningMåned(2026, 10);

Console.WriteLine();
Console.WriteLine("Kontakt ved sygdom");
repo.VisKontaktVedSygdom(5);

Console.WriteLine();
Console.WriteLine("Belastning år");
repo.VisBelastningÅr(2026);

Console.WriteLine();
Console.WriteLine("Skift medarbejder");
repo.SkiftMedarbejder(2, 7);
repo.SkiftMedarbejder(2, 3);

Console.WriteLine();
Console.WriteLine("Tryk på en tast for at afslutte...");
Console.ReadKey();

