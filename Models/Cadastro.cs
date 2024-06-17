using Microsoft.Data.SqlClient;
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
            CarregarClientesDoBanco();
        }

        private void CarregarClientesDoBanco()
        {
            using (SqlConnection connection = new SqlConnection(_conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT Id, FullName, Email, Password, Idade, Sexo, Injury, Conte, InjuryDetails, Role FROM Clientes", connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ClienteModel cliente = new ClienteModel
                    {
                        FullName = reader.GetString(1),
                        Email = reader.GetString(2),
                        Password = reader.GetString(3),
                        Idade = reader.GetInt32(4),
                        Sexo = reader.GetString(5),
                        Injury = reader.GetString(6),
                        Conte = reader.GetString(7),
                        InjuryDetails = reader.GetString(8),
                        Role = reader.GetString(9)
                    };
                    clientes.Add(cliente);
                }
            }
        }

        public void AdicionarCliente(ClienteModel cliente)
        {
            using (SqlConnection connection = new SqlConnection(_conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Clientes (FullName, Email, Password, Idade, Sexo, Injury, Conte, InjuryDetails, Role) VALUES (@FullName, @Email, @Password, @Idade, @Sexo, @Injury, @Conte, @InjuryDetails, @Role); SELECT SCOPE_IDENTITY();", connection);
                cmd.Parameters.AddWithValue("@FullName", cliente.FullName);
                cmd.Parameters.AddWithValue("@Email", cliente.Email);
                cmd.Parameters.AddWithValue("@Password", cliente.Password);
                cmd.Parameters.AddWithValue("@Idade", cliente.Idade);
                cmd.Parameters.AddWithValue("@Sexo", cliente.Sexo);
                cmd.Parameters.AddWithValue("@Injury", cliente.Injury);
                cmd.Parameters.AddWithValue("@Conte", cliente.Conte);
                cmd.Parameters.AddWithValue("@InjuryDetails", cliente.InjuryDetails);
                cmd.Parameters.AddWithValue("@Role", cliente.Role);

                clientes.Add(cliente);
            }
        }

        public void RemoverCliente(ClienteModel cliente)
        {
            using (SqlConnection connection = new SqlConnection(_conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Clientes WHERE Id = @Id", connection);
                cmd.Parameters.AddWithValue("@Id", cliente.Id);
                cmd.ExecuteNonQuery();
            }

            Clientes.Remove(cliente);
        }

        public List<ClienteModel> ListarClientes()
        {
            return clientes;
        }
    }
}
