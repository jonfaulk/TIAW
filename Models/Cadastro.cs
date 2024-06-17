using System.Collections.Generic;

namespace TIAW.Models
{
    public class Cadastro
    {
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
