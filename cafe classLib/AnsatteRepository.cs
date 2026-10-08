using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace cafe_classLib
{
    public class AnsatteRepository
    {
        private SqlConnection _conn;
     
        public void ConnectToDatabase()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
         
                .Build();
            string connectionString = configuration["AntonConnectionString"];



            _conn = new SqlConnection(connectionString);

            _conn.Open();
        }
        public void DisconnectFromDatabase()
        {
            if (_conn != null)
            {
                _conn.Close();
            }
        }

        // CREATE
        public void AddAnsat(Ansatte ansat)
        {
            ConnectToDatabase();

            string sql = @"INSERT INTO Ansatte
                          (AnsatteID, Navn, Telefonnummer, Email, ErLeder)
                          VALUES
                          (@AnsatteID, @Navn, @Telefonnummer, @Email, @ErLeder)";

            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.AddWithValue("@AnsatteID", ansat.AnsatteID);
            cmd.Parameters.AddWithValue("@Navn", ansat.Navn);
            cmd.Parameters.AddWithValue("@Telefonnummer", ansat.Telefonnummer);
            cmd.Parameters.AddWithValue("@Email", ansat.Email);
            cmd.Parameters.AddWithValue("@ErLeder", ansat.ErLeder);

            cmd.ExecuteNonQuery();

            DisconnectFromDatabase();
        }

        // READ ALL
        public List<Ansatte> GetAllAnsatte()
        {
            ConnectToDatabase();

            List<Ansatte> ansatteListe = new List<Ansatte>();

            string sql = "SELECT * FROM Ansatte";

            SqlCommand cmd = new SqlCommand(sql, _conn);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Ansatte ansat = new Ansatte
                {
                    AnsatteID = (int)reader["AnsatteID"],
                    Navn = reader["Navn"].ToString(),
                    Telefonnummer = reader["Telefonnummer"].ToString(),
                    Email = reader["Email"].ToString(),
                    ErLeder = (bool)reader["ErLeder"]
                };

                ansatteListe.Add(ansat);
            }

            reader.Close();

            DisconnectFromDatabase();

            return ansatteListe;
        }

        // READ ONE
        public Ansatte GetAnsatById(int id)
        {
            ConnectToDatabase();

            string sql = "SELECT * FROM Ansatte WHERE AnsatteID = @Id";

            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@Id", id);

            SqlDataReader reader = cmd.ExecuteReader();

            Ansatte ansat = null;

            if (reader.Read())
            {
                ansat = new Ansatte
                {
                    AnsatteID = (int)reader["AnsatteID"],
                    Navn = reader["Navn"].ToString(),
                    Telefonnummer = reader["Telefonnummer"].ToString(),
                    Email = reader["Email"].ToString(),
                    ErLeder = (bool)reader["ErLeder"]
                };
            }

            reader.Close();

            DisconnectFromDatabase();

            return ansat;
        }

        // UPDATE
        public void UpdateAnsat(Ansatte ansat)
        {
            ConnectToDatabase();

            string sql = @"UPDATE Ansatte
                           SET Navn = @Navn,
                               Telefonnummer = @Telefonnummer,
                               Email = @Email,
                               ErLeder = @ErLeder
                           WHERE AnsatteID = @AnsatteID";

            SqlCommand cmd = new SqlCommand(sql, _conn);

            cmd.Parameters.AddWithValue("@AnsatteID", ansat.AnsatteID);
            cmd.Parameters.AddWithValue("@Navn", ansat.Navn);
            cmd.Parameters.AddWithValue("@Telefonnummer", ansat.Telefonnummer);
            cmd.Parameters.AddWithValue("@Email", ansat.Email);
            cmd.Parameters.AddWithValue("@ErLeder", ansat.ErLeder);

            cmd.ExecuteNonQuery();

            DisconnectFromDatabase();
        }

        // DELETE
        public void DeleteAnsat(int id)
        {
            ConnectToDatabase();

            string sql = "DELETE FROM Ansatte WHERE AnsatteID = @Id";

            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();

            DisconnectFromDatabase();
        }



        // Månedsplan
        public void VisMånedsplan(int år, int måned)
        {
            ConnectToDatabase();

            string sql = @"
        SELECT V.StartTid, V.SlutTid, H.Navn AS Hold, A.Navn AS Medarbejder
        FROM Månedsplan M
        INNER JOIN Ansatte A ON M.AnsatteID = A.AnsatteID
        INNER JOIN Vagt V ON M.VagtID = V.VagtID
        INNER JOIN Hold H ON V.Hold = H.HoldID
        WHERE YEAR(V.StartTid) = @År AND MONTH(V.StartTid) = @Måned
        ORDER BY V.StartTid, A.Navn";

            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@År", år);
            cmd.Parameters.AddWithValue("@Måned", måned);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{(DateTime) reader["StartTid"]} - {(DateTime) reader["SlutTid"]} | {reader["Hold"],-12} | {reader["Medarbejder"]}");
            }
            reader.Close();

            DisconnectFromDatabase();
        }

        // Belastning måned
        public void VisBelastningMåned(int år, int måned)
        {
            ConnectToDatabase();

            string sql = @"
        SELECT A.Navn, COUNT(*) AS AntalVagter, SUM(V.AntalTimer) AS AntalTimer
        FROM Månedsplan M
        INNER JOIN Ansatte A ON M.AnsatteID = A.AnsatteID
        INNER JOIN Vagt V ON M.VagtID = V.VagtID
        WHERE YEAR(V.StartTid) = @År AND MONTH(V.StartTid) = @Måned
        GROUP BY A.Navn
        ORDER BY AntalTimer DESC";

            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@År", år);
            cmd.Parameters.AddWithValue("@Måned", måned);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Navn"],-16} Vagter: {reader["AntalVagter"],2}  Timer: {reader["AntalTimer"],3}");
            }
            reader.Close();

            DisconnectFromDatabase();
        }

        // Kontakt ved sygdom
        public void VisKontaktVedSygdom(int vagtId)
        {
            ConnectToDatabase();

            string sql = @"
        SELECT Navn, Telefonnummer, Email
        FROM Ansatte
        WHERE AnsatteID NOT IN (SELECT AnsatteID FROM Månedsplan WHERE VagtID = @VagtID)
        ORDER BY Navn";

            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@VagtID", vagtId);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Navn"],-16} Tlf: {reader["Telefonnummer"]}  Email: {reader["Email"]}");
            }
            reader.Close();

            DisconnectFromDatabase();
        }

        // Belastning over året
        public void VisBelastningÅr(int år)
        {
            ConnectToDatabase();

            string sql = @"
        SELECT A.Navn, MONTH(V.StartTid) AS Måned,
               COUNT(*) AS AntalVagter, SUM(V.AntalTimer) AS AntalTimer
        FROM Månedsplan M
        INNER JOIN Ansatte A ON M.AnsatteID = A.AnsatteID
        INNER JOIN Vagt V ON M.VagtID = V.VagtID
        WHERE YEAR(V.StartTid) = @År
        GROUP BY A.Navn, MONTH(V.StartTid)
        ORDER BY A.Navn, Måned";

            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@År", år);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Navn"],-16} Måned: {reader["Måned"],2}  Vagter: {reader["AntalVagter"],2}  Timer: {reader["AntalTimer"],3}");
            }
            reader.Close();

            DisconnectFromDatabase();
        }

        // Skift medarbejder på en vagt i månedsplanen
        public void SkiftMedarbejder(int månedsplanId, int nyAnsatId)
        {
            ConnectToDatabase();

            string sql = "UPDATE Månedsplan SET AnsatteID = @Ny WHERE MånedsplanID = @Id";

            SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@Ny", nyAnsatId);
            cmd.Parameters.AddWithValue("@Id", månedsplanId);

            Console.WriteLine($"{cmd.ExecuteNonQuery()} rækker opdateret.");

            DisconnectFromDatabase();
        }
    }
}