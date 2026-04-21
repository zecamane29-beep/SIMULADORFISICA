using SimuladorFisica.Models;

namespace SimuladorFisica.Utils
{
    public class Ordenacao
    {
        public static void BubbleSortParticulas(List<Particula> particulas)
        {
            int n = particulas.Count;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (string.Compare(particulas[j].Nome, particulas[j + 1].Nome, StringComparison.Ordinal) > 0)
                    {
                        Particula temp = particulas[j];
                        particulas[j] = particulas[j + 1];
                        particulas[j + 1] = temp;
                    }
                }
            }
        }
    }
}