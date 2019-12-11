using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace KernelDeeps.AI
{
	public static class Mathf
	{
		public static Random random;

		public const float Pi = 3.141593f;
		public const float Pi2 = 6.283185f;
		public const float Sqrt2Pi = 2.506628f;

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

		public static float ReLU(float x)
		{
			if (x < 0)
				return 0;
			return x;
		}
	}
}
