using System.Collections.Generic;

namespace TIAW.Models
{
    public class Cadastro
    {
        private List<ClienteModel> clientes;

        public Cadastro()
        {
            clientes = new List<ClienteModel>();
        }

        public void AdicionarCliente(ClienteModel cliente)
        {
            clientes.Add(cliente);
        }

        public List<ClienteModel> ListarClientes()
        {
            return clientes;
        }
    }
}
