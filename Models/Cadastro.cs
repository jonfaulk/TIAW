using System.Collections.Generic;

namespace TIAW.Models
{
    public class Cadastro
    {
        private readonly static string _conn = @"Data Source=localhost\MSSQLSERVER01;Initial Catalog=db_Academia;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        private List<ClienteModel> clientes;

        public List<ClienteModel> Clientes
        {
            get { return clientes; }
        }

        public Cadastro()
        {
            clientes = new List<ClienteModel>();
        }

        public void AdicionarCliente(ClienteModel cliente)
        {
            clientes.Add(cliente);
        }

        public void RemoverCliente(ClienteModel cliente)
        {
            Clientes.Remove(cliente);
        }

        public List<ClienteModel> ListarClientes()
        {
            return clientes;
        }
    }
}
