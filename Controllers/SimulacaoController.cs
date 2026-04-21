using SimuladorFisica.Models;

namespace SimuladorFisica.Controllers
{
    public class SimulacaoController
    {
        private const double Gravidade = 9.80;

        public string SimularCinematica(Projeto? projetoAtivo, double duracaoSimulacao, double passoTemporal)
        {
            if (projetoAtivo == null)
                return "Nenhum projeto selecionado.";

            List<Particula> particulas = projetoAtivo.Particulas.ToList();

            if (particulas.Count == 0)
                return "Nenhuma partícula registada.";

            if (duracaoSimulacao <= 0 || passoTemporal <= 0)
                return "O valor da duração da simulação ou do passo temporal inválido.";

            if (passoTemporal > duracaoSimulacao)
                return "Passo invalido. O passo não pode ser superior ao tempo total.";

            int numeroIteracoes = (int)(duracaoSimulacao / passoTemporal);

            List<string> linhas = new List<string>();
            linhas.Add("Simulação cinemática iniciada.");
            linhas.Add($"Tempo total: {duracaoSimulacao:F2}s");
            linhas.Add($"Passo: {passoTemporal:F2}s");
            linhas.Add($"Número de iterações: {numeroIteracoes}");

            for (double tempo = 0.0; tempo <= duracaoSimulacao + 0.0001; tempo += passoTemporal)
            {
                linhas.Add("==================================================");
                linhas.Add($"INSTANTE DE TEMPO: {tempo:F2} s");
                linhas.Add("==================================================");

                foreach (Particula particula in particulas)
                {
                    Vetor2D aceleracaoEfetiva = ObterAceleracaoEfetiva(particula, projetoAtivo.GravidadeAtiva);
                    Vetor2D posicaoAtual = CalcularPosicao(particula.PosicaoInicial, particula.VelocidadeInicial, aceleracaoEfetiva, tempo);
                    Vetor2D velocidadeAtual = CalcularVelocidade(particula.VelocidadeInicial, aceleracaoEfetiva, tempo);

                    double tempoAnterior = Math.Max(0, tempo - passoTemporal);
                    Vetor2D posicaoAnterior = CalcularPosicao(particula.PosicaoInicial, particula.VelocidadeInicial, aceleracaoEfetiva, tempoAnterior);

                    Vetor2D deslocamentoIntervalo = new Vetor2D(
                        posicaoAtual.X - posicaoAnterior.X,
                        posicaoAtual.Y - posicaoAnterior.Y
                    );

                    double distanciaIntervalo = deslocamentoIntervalo.Magnitude();

                    linhas.Add($"Partícula: {particula.Nome}");
                    linhas.Add(FormatarGrandeza("Posição", posicaoAtual, "m"));
                    linhas.Add(FormatarGrandeza("Velocidade", velocidadeAtual, "m/s"));
                    linhas.Add(FormatarGrandeza("Aceleração", aceleracaoEfetiva, "m/s^2"));
                    linhas.Add($"Deslocamento no intervalo: {deslocamentoIntervalo.Magnitude():F2} m");
                    linhas.Add($"Distância percorrida no intervalo: {distanciaIntervalo:F2} m");
                    linhas.Add("------------------------------------------------");
                }
            }

            linhas.Add("Simulação cinemática concluída.");
            return string.Join(Environment.NewLine, linhas);
        }

        private Vetor2D ObterAceleracaoEfetiva(Particula particula, bool gravidadeAtiva)
        {
            double ax = particula.Aceleracao.X;
            double ay = particula.Aceleracao.Y;

            if (gravidadeAtiva)
                ay -= Gravidade;

            return new Vetor2D(ax, ay);
        }

        private Vetor2D CalcularPosicao(Vetor2D posicaoInicial, Vetor2D velocidadeInicial, Vetor2D aceleracao, double tempo)
        {
            double x = posicaoInicial.X + velocidadeInicial.X * tempo + 0.5 * aceleracao.X * tempo * tempo;
            double y = posicaoInicial.Y + velocidadeInicial.Y * tempo + 0.5 * aceleracao.Y * tempo * tempo;

            return new Vetor2D(x, y);
        }

        private Vetor2D CalcularVelocidade(Vetor2D velocidadeInicial, Vetor2D aceleracao, double tempo)
        {
            double vx = velocidadeInicial.X + aceleracao.X * tempo;
            double vy = velocidadeInicial.Y + aceleracao.Y * tempo;

            return new Vetor2D(vx, vy);
        }

        private string FormatarGrandeza(string nome, Vetor2D vetor, string unidade)
        {
            return $"{nome}: ({vetor.X:F2}, {vetor.Y:F2}) {unidade} | módulo = {vetor.Magnitude():F2} {unidade} | ângulo = {vetor.AnguloGraus():F2} graus";
        }
    }
}