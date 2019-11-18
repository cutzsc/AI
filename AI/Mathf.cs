using System;

namespace KernelDeeps.AI
{
	public static class Mathf
	{
		public static float NextSingle(float min, float max)
		{
			return (float)Mathd.NextDouble(min, max);
		}

		public static float Sigmoid(float x)
		{
			return (float)(1 / (1 + Math.Exp(-x)));
		}

		public static float SigmoidDerivative(float x)
		{
			return x * (1 - x);
		}
	}
}
