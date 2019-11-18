using System;

namespace KernelDeeps.AI
{
	public static class Mathd
	{
		public static Random rand;
		static Mathd()
		{
			rand = new Random();
		}

		public static double NextDouble(double min, double max)
		{
			return rand.NextDouble() * (max - min) + min;
		}

		public static double Sigmoid(double x)
		{
			return 1 / (1 + Math.Exp(-x));
		}

		public static double SigmoidDerivative(double x)
		{
			return x * (1 - x);
		}
	}
}
