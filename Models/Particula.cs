using SimuladorFisica.DataStructures;

namespace SimuladorFisica.Models
{
    public class Particula
    {
        public string Nome { get; set; }
        public Vetor2D PosicaoInicial { get; set; }
        public Vetor2D VelocidadeInicial { get; set; }
        public Vetor2D Aceleracao { get; set; }
        public double Massa { get; set; }
        public ListaLigada<Forca> Forcas { get; set; }

        public Particula(
            string nome,
            Vetor2D posicaoInicial,
            Vetor2D velocidadeInicial,
            Vetor2D aceleracao,
            double massa)
        {
            Nome = nome;
            PosicaoInicial = posicaoInicial;
            VelocidadeInicial = velocidadeInicial;
            Aceleracao = aceleracao;
            Massa = massa;
            Forcas = new ListaLigada<Forca>();
        }
    }
}