using System.Globalization;
using SimuladorFisica.Controllers;
using SimuladorFisica.Models;
using SimuladorFisica.Utils;

namespace SimuladorFisica
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo cultura = CultureInfo.InvariantCulture;

            ProjetoController projetoController = new ProjetoController();
            SimulacaoController simulacaoController = new SimulacaoController();

            while (true)
            {
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input == "Exit")
                    break;

                string[] partes = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string comando = partes[0].ToUpper();

                switch (comando)
                {
                    case "RPJ":
                        if (partes.Length != 2)
                        {
                            Console.WriteLine("Comando inválido. Sintaxe correta: RPJ <NomeProjeto>.");
                            break;
                        }

                        Console.WriteLine(projetoController.RegistarProjeto(partes[1]));
                        break;

                    case "LPJ":
                        if (partes.Length != 1)
                        {
                            Console.WriteLine("Comando inválido: Sintaxe correta: LPJ.");
                            break;
                        }

                        List<Projeto> projetos = projetoController.ListarProjetos();

                        if (projetos.Count == 0)
                        {
                            Console.WriteLine("Não existem projetos registados.");
                        }
                        else
                        {
                            Console.WriteLine("Lista de projetos:");
                            foreach (Projeto projeto in projetos)
                            {
                                string estado = projeto.Ativo ? "Ativo" : "Inativo";
                                Console.WriteLine($"{projeto.Nome} | Estado: {estado}");
                            }
                        }
                        break;

                    case "SPJ":
                        if (partes.Length != 2)
                        {
                            Console.WriteLine("Comando inválido: Sintaxe correta: SPJ <NomeProjeto>.");
                            break;
                        }

                        Console.WriteLine(projetoController.SelecionarProjeto(partes[1]));
                        break;

                    case "RP":
                        if (partes.Length != 9)
                        {
                            Console.WriteLine("Comando invalido. Sintaxe correta: RP NomeParticula PosicaoInicialX PosicaoInicialY VelocidadeInicialX VelocidadeInicialY AceleracaoX AceleracaoY Massa");
                            break;
                        }

                        if (!double.TryParse(partes[2], NumberStyles.Any, cultura, out double posX) ||
                            !double.TryParse(partes[3], NumberStyles.Any, cultura, out double posY) ||
                            !double.TryParse(partes[4], NumberStyles.Any, cultura, out double velX) ||
                            !double.TryParse(partes[5], NumberStyles.Any, cultura, out double velY) ||
                            !double.TryParse(partes[6], NumberStyles.Any, cultura, out double accX) ||
                            !double.TryParse(partes[7], NumberStyles.Any, cultura, out double accY) ||
                            !double.TryParse(partes[8], NumberStyles.Any, cultura, out double massa))
                        {
                            Console.WriteLine("Parâmetros numéricos inválidos.");
                            break;
                        }

                        Console.WriteLine(
                            projetoController.RegistarParticula(
                                partes[1], posX, posY, velX, velY, accX, accY, massa
                            )
                        );
                        break;

                    case "RF":
                        if (partes.Length != 4)
                        {
                            Console.WriteLine("Comando inválido. Sintaxe correta: RF NomeParticula ForcaX ForcaY.");
                            break;
                        }

                        if (!double.TryParse(partes[2], NumberStyles.Any, cultura, out double forcaX) ||
                            !double.TryParse(partes[3], NumberStyles.Any, cultura, out double forcaY))
                        {
                            Console.WriteLine("Parâmetros numéricos inválidos.");
                            break;
                        }

                        Console.WriteLine(projetoController.RegistarForca(partes[1], forcaX, forcaY));
                        break;

                    case "LP":
                        if (partes.Length != 1)
                        {
                            Console.WriteLine("Comando inválido. Sintaxe correta: LP.");
                            break;
                        }

                        List<Particula>? particulas = projetoController.ListarParticulasProjetoAtivo();

                        if (particulas == null)
                        {
                            Console.WriteLine("Nenhum projeto selecionado.");
                            break;
                        }

                        if (particulas.Count == 0)
                        {
                            Console.WriteLine("Não existem partículas registadas no projeto atual.");
                            break;
                        }

                        Ordenacao.BubbleSortParticulas(particulas);

                        Console.WriteLine("Lista de partículas do projeto atualmente selecionado:");

                        foreach (Particula particula in particulas)
                        {
                            Console.WriteLine($"Partícula: {particula.Nome}");
                            Console.WriteLine($"Posicao inicial: ({particula.PosicaoInicial.X:F2}, {particula.PosicaoInicial.Y:F2}) m");
                            Console.WriteLine($"Velocidade inicial: ({particula.VelocidadeInicial.X:F2}, {particula.VelocidadeInicial.Y:F2}) m/s");
                            Console.WriteLine($"Aceleração: ({particula.Aceleracao.X:F2}, {particula.Aceleracao.Y:F2}) m/s^2");
                            Console.WriteLine($"Massa: {particula.Massa:F2} kg");
                            Console.WriteLine($"Número de forcas aplicadas: {particula.Forcas.Count}");
                        }
                        break;

                    case "TG":
                        if (partes.Length != 2)
                        {
                            Console.WriteLine("Comando inválido. Sintaxe correta: TG ON ou TG OFF.");
                            break;
                        }

                        Console.WriteLine(projetoController.ToggleGravidade(partes[1].ToUpper()));
                        break;

                    case "SMC":
                        if (partes.Length != 3)
                        {
                            Console.WriteLine("Comando inválido. Sintaxe correta: SMC <DuracaoSimulacao> <PassoTemporal>.");
                            break;
                        }

                        if (!double.TryParse(partes[1], NumberStyles.Any, cultura, out double duracaoSimulacao) ||
                            !double.TryParse(partes[2], NumberStyles.Any, cultura, out double passoTemporal))
                        {
                            Console.WriteLine("O valor da duração da simulação ou do passo temporal inválido.");
                            break;
                        }

                        Console.WriteLine(
                            simulacaoController.SimularCinematica(
                                projetoController.ObterProjetoAtivo(),
                                duracaoSimulacao,
                                passoTemporal
                            )
                        );
                        break;

                    default:
                        Console.WriteLine("Instrução inválida.");
                        break;
                }
            }
        }
    }
}