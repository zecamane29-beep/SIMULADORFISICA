namespace SimuladorFisica.Models
{
    public class Vetor2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vetor2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public double AnguloGraus()
        {
            return Math.Atan2(Y, X) * 180.0 / Math.PI;
        }

        public static Vetor2D Somar(Vetor2D a, Vetor2D b)
        {
            return new Vetor2D(a.X + b.X, a.Y + b.Y);
        }

        public static Vetor2D Multiplicar(Vetor2D v, double escalar)
        {
            return new Vetor2D(v.X * escalar, v.Y * escalar);
        }

        public override string ToString()
        {
            return $"({X:F2}, {Y:F2})";
        }
    }
}