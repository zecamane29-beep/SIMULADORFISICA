using SimuladorFisica.DataStructures;
using SimuladorFisica.Models;

namespace SimuladorFisica.Controllers
{
    public class ProjetoController
    {
        private ListaLigada<Projeto> projetos = new ListaLigada<Projeto>();

        public string RegistarProjeto(string nomeProjeto)
        {
            Projeto? existente = projetos.Find(p => p.Nome == nomeProjeto);

            if (existente != null)
                return $"Já existe um projeto registado com o nome {nomeProjeto}.";

            projetos.AddLast(new Projeto(nomeProjeto));
            return "Projeto registado com sucesso.";
        }

        public string SelecionarProjeto(string nomeProjeto)
        {
            Projeto? projeto = projetos.Find(p => p.Nome == nomeProjeto);

            if (projeto == null)
                return $"Projeto {nomeProjeto} não encontrado.";

            if (projeto.Ativo)
                return $"Projeto {nomeProjeto} já se encontra selecionado.";

            foreach (Projeto p in projetos.ToList())
                p.Ativo = false;

            projeto.Ativo = true;
            return $"Projeto {nomeProjeto} selecionado com sucesso.";
        }

        public Projeto? ObterProjetoAtivo()
        {
            return projetos.Find(p => p.Ativo);
        }

        public List<Projeto> ListarProjetos()
        {
            return projetos.ToList();
        }

        public string RegistarParticula(
            string nomeParticula,
            double posX,
            double posY,
            double velX,
            double velY,
            double accX,
            double accY,
            double massa)
        {
            Projeto? projetoAtivo = ObterProjetoAtivo();

            if (projetoAtivo == null)
                return "Nenhum projeto selecionado.";

            if (massa <= 0)
                return "Massa invalida. O valor da massa deve se superior a 0.";

            Particula? existente = projetoAtivo.Particulas.Find(p => p.Nome == nomeParticula);

            if (existente != null)
                return $"Já existe uma partícula neste projeto registada com o nome {nomeParticula}.";

            Particula novaParticula = new Particula(
                nomeParticula,
                new Vetor2D(posX, posY),
                new Vetor2D(velX, velY),
                new Vetor2D(accX, accY),
                massa
            );

            projetoAtivo.Particulas.AddLast(novaParticula);
            return $"Partícula {nomeParticula} registada com sucesso.";
        }

        public string RegistarForca(string nomeParticula, double forcaX, double forcaY)
        {
            Projeto? projetoAtivo = ObterProjetoAtivo();

            if (projetoAtivo == null)
                return "Nenhum projeto selecionado.";

            Particula? particula = projetoAtivo.Particulas.Find(p => p.Nome == nomeParticula);

            if (particula == null)
                return $"Partícula {nomeParticula} não encontrada.";

            particula.Forcas.AddLast(new Forca(forcaX, forcaY));
            return $"Força registada na partícula: {nomeParticula}.";
        }

        public List<Particula>? ListarParticulasProjetoAtivo()
        {
            Projeto? projetoAtivo = ObterProjetoAtivo();

            if (projetoAtivo == null)
                return null;

            return projetoAtivo.Particulas.ToList();
        }

        public string ToggleGravidade(string estado)
        {
            Projeto? projetoAtivo = ObterProjetoAtivo();

            if (projetoAtivo == null)
                return "Nenhum projeto selecionado.";

            if (estado == "ON")
            {
                projetoAtivo.GravidadeAtiva = true;
                return "Gravidade ativada (g = 9.80 m/s^2).";
            }

            if (estado == "OFF")
            {
                projetoAtivo.GravidadeAtiva = false;
                return "Gravidade desativada.";
            }

            return "Comando inválido. Sintaxe correta: TG ON ou TG OFF.";
        }
    }
}