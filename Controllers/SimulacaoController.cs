using SimuladorFisica.Models;

namespace SimuladorFisica.Controllers
{
    public class SimulacaoController
    {
        private const double Gravidade = 9.80;

        public string SimularCinematica(Projeto? projetoAtivo, double duracao, double passo)
        {
            if (projetoAtivo == null)
                return "Nenhum projeto selecionado.";

            List<Particula> particulas = projetoAtivo.Particulas.ToList();

            if (particulas.Count == 0)
                return "Nenhuma partícula registada.";

            if (duracao <= 0 || passo <= 0)
                return "O valor da duração da simulação ou do passo temporal inválido.";

            if (passo > duracao)
                return "Passo invalido. O passo não pode ser superior ao tempo total.";

            List<string> linhas = new List<string>();

            linhas.Add("Simulação cinemática iniciada.");
            linhas.Add($"Tempo total: {duracao:F2}s");
            linhas.Add($"Passo temporal: {passo:F2}s");

            for (double t = 0; t <= duracao + 0.0001; t += passo)
            {
                linhas.Add("----------------------------------------");
                linhas.Add($"Tempo: {t:F2}s");

                foreach (Particula p in particulas)
                {
                    Vetor2D aceleracao = ObterAceleracaoCinematica(p, projetoAtivo.GravidadeAtiva);

                    Vetor2D posicao = CalcularPosicao(p.PosicaoInicial, p.VelocidadeInicial, aceleracao, t);
                    Vetor2D velocidade = CalcularVelocidade(p.VelocidadeInicial, aceleracao, t);

                    double tempoAnterior = Math.Max(0, t - passo);
                    Vetor2D posicaoAnterior = CalcularPosicao(p.PosicaoInicial, p.VelocidadeInicial, aceleracao, tempoAnterior);
                    Vetor2D deslocamentoIntervalo = posicao.Subtrair(posicaoAnterior);

                    linhas.Add($"Partícula: {p.Nome}");
                    linhas.Add($"Posição: {FormatarVetor(posicao, "m")}");
                    linhas.Add($"Velocidade: {FormatarVetor(velocidade, "m/s")}");
                    linhas.Add($"Aceleração: {FormatarVetor(aceleracao, "m/s²")}");
                    linhas.Add($"Deslocamento no intervalo: {FormatarVetor(deslocamentoIntervalo, "m")}");
                    linhas.Add($"Distância percorrida no intervalo: {deslocamentoIntervalo.Magnitude():F2} m");
                }
            }

            linhas.Add("Simulação cinemática concluída.");
            return string.Join(Environment.NewLine, linhas);
        }

        public string SimularDinamica(Projeto? projetoAtivo, string nomeParticula, double duracao, double passo)
        {
            if (projetoAtivo == null)
                return "Nenhum projeto selecionado.";

            if (duracao <= 0 || passo <= 0)
                return "O valor da duração da simulação ou do passo temporal inválido.";

            if (passo > duracao)
                return "Passo invalido. O passo não pode ser superior ao tempo total.";

            Particula? particula = projetoAtivo.Particulas.Find(p => p.Nome == nomeParticula);

            if (particula == null)
                return $"Partícula {nomeParticula} não encontrada.";

            Vetor2D forcaResultante = CalcularForcaResultante(particula, projetoAtivo.GravidadeAtiva);
            Vetor2D aceleracaoDinamica = new Vetor2D(
                forcaResultante.X / particula.Massa,
                forcaResultante.Y / particula.Massa
            );

            List<string> linhas = new List<string>();

            linhas.Add("Simulação dinâmica iniciada.");
            linhas.Add($"Partícula: {particula.Nome}");
            linhas.Add($"Massa: {particula.Massa:F2} kg");
            linhas.Add($"Força resultante: {FormatarVetor(forcaResultante, "N")}");
            linhas.Add($"Aceleração pela 2ª Lei de Newton: {FormatarVetor(aceleracaoDinamica, "m/s²")}");

            for (double t = 0; t <= duracao + 0.0001; t += passo)
            {
                Vetor2D posicao = CalcularPosicao(
                    particula.PosicaoInicial,
                    particula.VelocidadeInicial,
                    aceleracaoDinamica,
                    t
                );

                Vetor2D velocidade = CalcularVelocidade(
                    particula.VelocidadeInicial,
                    aceleracaoDinamica,
                    t
                );

                double tempoAnterior = Math.Max(0, t - passo);

                Vetor2D posicaoAnterior = CalcularPosicao(
                    particula.PosicaoInicial,
                    particula.VelocidadeInicial,
                    aceleracaoDinamica,
                    tempoAnterior
                );

                Vetor2D deslocamentoIntervalo = posicao.Subtrair(posicaoAnterior);

                double energiaCinetica = CalcularEnergiaCinetica(particula.Massa, velocidade);
                double energiaPotencial = CalcularEnergiaPotencial(particula.Massa, posicao.Y);
                double energiaMecanica = energiaCinetica + energiaPotencial;
                double trabalho = CalcularTrabalho(forcaResultante, deslocamentoIntervalo);
                double potenciaMedia = passo > 0 ? trabalho / passo : 0;

                linhas.Add("----------------------------------------");
                linhas.Add($"Tempo: {t:F2}s");
                linhas.Add($"Posição: {FormatarVetor(posicao, "m")}");
                linhas.Add($"Velocidade: {FormatarVetor(velocidade, "m/s")}");
                linhas.Add($"Aceleração: {FormatarVetor(aceleracaoDinamica, "m/s²")}");
                linhas.Add($"Deslocamento no intervalo: {FormatarVetor(deslocamentoIntervalo, "m")}");
                linhas.Add($"Energia cinética: {energiaCinetica:F2} J");
                linhas.Add($"Energia potencial gravítica: {energiaPotencial:F2} J");
                linhas.Add($"Energia mecânica: {energiaMecanica:F2} J");
                linhas.Add($"Trabalho no intervalo: {trabalho:F2} J");
                linhas.Add($"Potência média no intervalo: {potenciaMedia:F2} W");
            }

            linhas.Add("Simulação dinâmica concluída.");
            return string.Join(Environment.NewLine, linhas);
        }

        private Vetor2D ObterAceleracaoCinematica(Particula particula, bool gravidadeAtiva)
        {
            double ax = particula.Aceleracao.X;
            double ay = particula.Aceleracao.Y;

            if (gravidadeAtiva)
                ay -= Gravidade;

            return new Vetor2D(ax, ay);
        }

        private Vetor2D CalcularForcaResultante(Particula particula, bool gravidadeAtiva)
        {
            Vetor2D resultante = new Vetor2D(0, 0);

            foreach (Forca forca in particula.Forcas.ToList())
            {
                resultante = resultante.Somar(forca.Valor);
            }

            if (gravidadeAtiva)
            {
                Vetor2D forcaPeso = new Vetor2D(0, -particula.Massa * Gravidade);
                resultante = resultante.Somar(forcaPeso);
            }

            return resultante;
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

        private double CalcularEnergiaCinetica(double massa, Vetor2D velocidade)
        {
            double v = velocidade.Magnitude();
            return 0.5 * massa * v * v;
        }

        private double CalcularEnergiaPotencial(double massa, double altura)
        {
            return massa * Gravidade * altura;
        }

        private double CalcularTrabalho(Vetor2D forca, Vetor2D deslocamento)
        {
            return forca.ProdutoEscalar(deslocamento);
        }

        private string FormatarVetor(Vetor2D vetor, string unidade)
        {
            return $"({vetor.X:F2}, {vetor.Y:F2}) {unidade} | módulo = {vetor.Magnitude():F2} {unidade} | ângulo = {vetor.AnguloGraus():F2}°";
        }
    }
}