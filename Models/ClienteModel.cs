using System;
using System.Collections.Generic;

namespace TIAW.Models
{
    public class ClienteModel
    {
        private static HashSet<int> ids = new HashSet<int>();
        private static Random random = new Random();

        public int Id { get; private set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public ClienteModel(string nome, string email)
        {
            this.Id = GenerateUniqueId();
            this.Nome = nome;
            this.Email = email;
        }

        private int GenerateUniqueId()
        {
            int newId;
            do
            {
                newId = random.Next(1, 201); // Gera um ID entre 1 e 200
            } while (ids.Contains(newId));

            ids.Add(newId);
            return newId;
        }
    }
}
