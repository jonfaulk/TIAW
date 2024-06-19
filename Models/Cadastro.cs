using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TIAW.Models
{
    public class Cadastro
    {
        private readonly static string _conn = @"Data Source=localhost\MSSQLSERVER01;Initial Catalog=db_Academia;Integrated Security=True;Encrypt=False";
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
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                string query = "INSERT INTO usuarios (nome, email, senha, sexo, data_nascimento, lesao, treina, condicao, objetivo, tipo) " +
                               "VALUES (@Nome, @Email, @Senha, @Sexo, @DataNascimento, @Lesao, @Treina, @Condicao, @Objetivo, @Tipo)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nome", cliente.FullName);
                    cmd.Parameters.AddWithValue("@Email", cliente.Email);
                    cmd.Parameters.AddWithValue("@Senha", cliente.Password);
                    cmd.Parameters.AddWithValue("@Sexo", cliente.Sexo);
                    cmd.Parameters.AddWithValue("@DataNascimento", DateTime.Now.AddYears(-cliente.Idade)); // Calcula a data de nascimento
                    cmd.Parameters.AddWithValue("@Lesao", cliente.Injury ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Treina", cliente.Conte == "Sim");
                    cmd.Parameters.AddWithValue("@Condicao", cliente.Conte == "Sim");
                    cmd.Parameters.AddWithValue("@Objetivo", cliente.InjuryDetails ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tipo", cliente.Role);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AtualizarCliente(ClienteModel cliente)
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                string query = "UPDATE usuarios SET nome = @Nome, email = @Email, senha = @Senha, sexo = @Sexo, data_nascimento = @DataNascimento, " +
                               "lesao = @Lesao, treina = @Treina, condicao = @Condicao, objetivo = @Objetivo, tipo = @Tipo WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nome", cliente.FullName);
                    cmd.Parameters.AddWithValue("@Email", cliente.Email);
                    cmd.Parameters.AddWithValue("@Senha", cliente.Password);
                    cmd.Parameters.AddWithValue("@Sexo", cliente.Sexo);
                    cmd.Parameters.AddWithValue("@DataNascimento", DateTime.Now.AddYears(-cliente.Idade)); // Calcula a data de nascimento
                    cmd.Parameters.AddWithValue("@Lesao", cliente.Injury ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Treina", cliente.Conte == "Sim");
                    cmd.Parameters.AddWithValue("@Condicao", cliente.Conte == "Sim");
                    cmd.Parameters.AddWithValue("@Objetivo", cliente.InjuryDetails ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tipo", cliente.Role);
                    cmd.Parameters.AddWithValue("@Id", cliente.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoverCliente(int id)
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                string query = "DELETE FROM usuarios WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<ClienteModel> ListarClientes()
        {
            List<ClienteModel> listaClientes = new List<ClienteModel>();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                string query = "SELECT * FROM usuarios";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ClienteModel cliente = new ClienteModel
                            {
                                FullName = reader["nome"].ToString(),
                                Email = reader["email"].ToString(),
                                Password = reader["senha"].ToString(),
                                Sexo = reader["sexo"].ToString(),
                                Idade = CalculateAge(Convert.ToDateTime(reader["data_nascimento"])),
                                Injury = reader["lesao"].ToString(),
                                Conte = Convert.ToBoolean(reader["treina"]) ? "Sim" : "Não",
                                InjuryDetails = reader["objetivo"].ToString(),
                                Role = reader["tipo"].ToString()
                            };

                            listaClientes.Add(cliente);
                        }
                    }
                }
            }

            return listaClientes;
        }

        private int CalculateAge(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
                age--;
            return age;
        }
    }
}