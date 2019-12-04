using System;
using MathNet.Numerics.Random;

namespace KernelDeeps.AI
{
	public static class Mathf
	{
		public static Random random;

		static Mathf()
		{
			random = new Random();
		}

		public static float NextSingle(float min, float max)
		{
			return (float)random.NextDouble() * (max - min) + min;
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
