using System.Linq;

namespace TIAW.Models
{
    public class FichaTreino
    {
        public string NomeAluno { get; set; }
        public int id { get; set; }
        public string Tipo { get; set; }
        public List<string> TipoSelecionado { get; set; }
        public List<string> Fichas { get; set; }
        public List<string> Exercicios { get; set; }


        public FichaTreino()
        {
            Fichas = new List<string>
            {
                "Ombro e Peito",
                "Pernas",
                "Braço"
            };
        }

        public FichaTreino(string nome, List<string> tipoSelecionado, List<string> exercicios, int id)
        {
            this.NomeAluno = nome;
            this.TipoSelecionado = tipoSelecionado;
            this.Exercicios = exercicios;
            this.id = id;
        }

        public static List<string> MontarFicha(string opcoesMarcadas)
        {

            if (opcoesMarcadas == "Ombro e Peito")
            {

                List<string> treino = new List<string>
            {
                "Desenvolvimento de Ombros com Halteres: 4x8-12 | Descanso: 60-90 segundos",
                    "Elevação Lateral: 3x12-15 | Descanso: 45-60 segundos",
                    "Elevação Frontal: 3x12-15 | Descanso: 45-60 segundos",
                    "Crucifixo Invertido: 3x12-15 | Descanso: 45-60 segundos",
                    "Supino Reto com Barra: 4x8-12 | Descanso: 60-90 segundos",
                    "Supino Inclinado com Halteres: 3x10-12 | Descanso: 60-90 segundos",
                    "Crucifixo com Halteres: 3x12-15 | Descanso: 60 segundos",
                    "Crossover (no cabo): 3x12-15 | Descanso: 45-60 segundos"
                };

                return treino;

            }
            else if (opcoesMarcadas == "Pernas")
            {

                List<string> treino = new List<string>
            {
                 "Agachamento Livre: 4x8-12 | Descanso: 90-120 segundos",
                    "Leg Press: 4x10-15 | Descanso: 90-120 segundos",
                    "Afundo (com halteres): 3x12-15 (cada perna) | Descanso: 60-90 segundos",
                    "Extensão de Pernas: 3x12-15 | Descanso: 60-90 segundos",
                    "Flexão de Pernas: 3x12-15 | Descanso: 60-90 segundos",
                    "Elevação de Panturrilha: 4x15-20 | Descanso: 45-60 segundos"
                };

                return treino;
            }
            else if (opcoesMarcadas == "Braço")
            {

                List<string> treino = new List<string>
            {
                 "Rosca Direta com Barra: 4x8-12 | Descanso: 60-90 segundos",
                    "Rosca Alternada com Halteres: 3x10-12 | Descanso: 60-90 segundos",
                    "Rosca Scott (com barra EZ): 3x10-12 | Descanso: 60-90 segundos",
                    "Tríceps Pulley: 4x10-12 | Descanso: 60-90 segundos",
                    "Tríceps Francês com Halteres: 3x10-12 | Descanso: 60-90 segundos",
                    "Mergulho entre Bancos: 3x12-15 | Descanso: 60-90 segundos",
                    "Rosca Martelo: 3x12-15 | Descanso: 60 segundos"
                };

                return treino;

            }
            else
            {
                return new List<string>
                {
                    "Não foi encontrado o treino específico"
                };
            }
        }

        public static Dictionary<string, List<string>> FichaPersonalizada(List<string> Tipos, List<string> Exercicios)
        {
            List<string> OmbroPeito = new List<string>
    {
        "Desenvolvimento de Ombros com Halteres: 4x8-12 | Descanso: 60-90 segundos",
        "Elevação Lateral: 3x12-15 | Descanso: 45-60 segundos",
        "Elevação Frontal: 3x12-15 | Descanso: 45-60 segundos",
        "Crucifixo Invertido: 3x12-15 | Descanso: 45-60 segundos",
        "Supino Reto com Barra: 4x8-12 | Descanso: 60-90 segundos",
        "Supino Inclinado com Halteres: 3x10-12 | Descanso: 60-90 segundos",
        "Crucifixo com Halteres: 3x12-15 | Descanso: 60 segundos",
        "Crossover (no cabo): 3x12-15 | Descanso: 45-60 segundos"
    };

            List<string> Pernas = new List<string>
    {
        "Agachamento Livre: 4x8-12 | Descanso: 90-120 segundos",
        "Leg Press: 4x10-15 | Descanso: 90-120 segundos",
        "Afundo (com halteres): 3x12-15 (cada perna) | Descanso: 60-90 segundos",
        "Extensão de Pernas: 3x12-15 | Descanso: 60-90 segundos",
        "Flexão de Pernas: 3x12-15 | Descanso: 60-90 segundos",
        "Elevação de Panturrilha: 4x15-20 | Descanso: 45-60 segundos"
    };

            List<string> Braço = new List<string>
    {
        "Rosca Direta com Barra: 4x8-12 | Descanso: 60-90 segundos",
        "Rosca Alternada com Halteres: 3x10-12 | Descanso: 60-90 segundos",
        "Rosca Scott (com barra EZ): 3x10-12 | Descanso: 60-90 segundos",
        "Tríceps Pulley: 4x10-12 | Descanso: 60-90 segundos",
        "Tríceps Francês com Halteres: 3x10-12 | Descanso: 60-90 segundos",
        "Mergulho entre Bancos: 3x12-15 | Descanso: 60-90 segundos",
        "Rosca Martelo: 3x12-15 | Descanso: 60 segundos"
    };

            Dictionary<string, List<string>> ex = new Dictionary<string, List<string>>();

            string[] TiposLimpos = Tipos[0].Replace("[", "").Replace("]", "").Replace("\"", "").Trim().Split(',');
            string[] ExerciciosLimpos = Exercicios[0].Replace("[", "").Replace("]", "").Replace("\"", "").Trim().Split(',');

            foreach (string tipo in TiposLimpos)
            {
                if (!ex.ContainsKey(tipo.Trim()))
                {
                    ex[tipo.Trim()] = new List<string>();
                }
            }

            foreach (string tipo in TiposLimpos)
            {
                foreach (string exercicio in ExerciciosLimpos)
                {
                    if (tipo.Trim() == "Ombro e Peito" && OmbroPeito.Contains(exercicio.Trim()))
                    {
                        ex[tipo.Trim()].Add(exercicio.Trim());
                    }
                    else if (tipo.Trim() == "Pernas" && Pernas.Contains(exercicio.Trim()))
                    {
                        ex[tipo.Trim()].Add(exercicio.Trim());
                    }
                    else if (tipo.Trim() == "Braço" && Braço.Contains(exercicio.Trim()))
                    {
                        ex[tipo.Trim()].Add(exercicio.Trim());
                    }
                }
            }

            return ex;


        }

    }
}
