namespace SimuladorFisica.Models
{
    public class Forca
    {
        public Vetor2D Valor { get; set; }

        public Forca(double fx, double fy)
        {
            Valor = new Vetor2D(fx, fy);
        }
    }
}