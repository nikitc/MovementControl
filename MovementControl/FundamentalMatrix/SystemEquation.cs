namespace MovementControl.FundamentalMatrix
{
    public class SystemEquation
    {
        public double[][] Equation { get; set; }
        public double[] FreeMembers { get; set; }

        public SystemEquation(int n)
        {
            Equation = new double[n][];
            FreeMembers = new double[n];
        }
    }
}