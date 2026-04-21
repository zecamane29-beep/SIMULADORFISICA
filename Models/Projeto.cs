using SimuladorFisica.DataStructures;

namespace SimuladorFisica.Models
{
    public class Projeto
    {
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public bool GravidadeAtiva { get; set; }
        public ListaLigada<Particula> Particulas { get; set; }

        public Projeto(string nome)
        {
            Nome = nome;
            Ativo = false;
            GravidadeAtiva = false;
            Particulas = new ListaLigada<Particula>();
        }
    }
}