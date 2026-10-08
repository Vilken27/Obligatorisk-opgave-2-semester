using System;
using System.Collections.Generic;
using System.Text;

namespace cafe_classLib
{
    public class Ansatte
    {
        public int AnsatteID { get; set; }
        public string Navn { get; set; }
        public string Telefonnummer { get; set; }
        public string Email { get; set; }
        public bool ErLeder { get; set; }

        public Ansatte()
        {
        }

        public Ansatte(int ansatteID, string navn, string telefonnummer, string email, bool erLeder)
        {
            AnsatteID = ansatteID;
            Navn = navn;
            Telefonnummer = telefonnummer;
            Email = email;
            ErLeder = erLeder;
        }

        public override string ToString()
        {
            return $"AnsatteID: {AnsatteID}, Navn: {Navn}, Telefonnummer: {Telefonnummer}, Email: {Email}, ErLeder: {ErLeder}";
        }

        
    }
}
