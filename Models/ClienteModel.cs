using System;
using System.Collections.Generic;

namespace TIAW.Models
{
    public class ClienteModel
    {
        private static HashSet<int> ids = new HashSet<int>();
        private static Random random = new Random();
        public int Id { get; private set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Idade { get; set; }
        public string Sexo { get; set; }
        public string Injury { get; set; }
        public string Conte { get; set; }
        public string InjuryDetails { get; set; } 
        public string cargo { get; set; }

        public ClienteModel(string fullName, string email, string password, int idade, string sexo, string injury, string conte, string injuryDetails)
        {
            this.Id = GenerateUniqueId();
            this.FullName = fullName;
            this.Email = email;
            this.Password = password;
            this.Idade = idade;
            this.Sexo = sexo;
            this.Injury = injury;
            this.Conte = conte;
            this.InjuryDetails = injuryDetails;
            this.cargo = "";
        }

        public ClienteModel()
        {
            FullName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Idade = 0;
            Sexo = string.Empty;
            Injury = string.Empty;
            Conte = string.Empty;
            InjuryDetails = string.Empty;
            this.cargo = "";
        }


        private static int GenerateUniqueId()
        {
            int newId;
            do
            {
                newId = random.Next(1, 201);
            } while (ids.Contains(newId));

            ids.Add(newId);
            return newId;
        }
    }
}
