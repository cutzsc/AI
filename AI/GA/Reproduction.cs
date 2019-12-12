using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI.GA
{
	public static class Reproduction
	{
		public static (float[], float[]) OnePointX(float[] parent1, float[] parent2)
		{
			if (parent1.Length != parent2.Length)
				throw new ArgumentException();

			int len = parent1.Length;
			float[] offspringX = new float[len];
			float[] offspringY = new float[len];
			int k = Mathf.random.Next(0, len);

			int i = 0;
			for (; i < k; i++)
			{
				offspringX[i] = parent1[i];
				offspringY[i] = parent2[i];
			}
			for (; i < len; i++)
			{
				offspringX[i] = parent2[i];
				offspringY[i] = parent1[i];
			}

			return (offspringX, offspringY);
		}

		public static (float[], float[]) KPointX(float[] parent1, float[] parent2, int points)
		{
			if (parent1.Length != parent2.Length ||
				parent1.Length <= points ||
				points < 1)
				throw new ArgumentException();

			int len = parent1.Length;
			float[] offspringX = new float[len];
			float[] offspringY = new float[len];
			int[] kPoints = new int[points];

			for (int k = 0; k < kPoints.Length; k++)
			{
				bool found = false;
				while (!found)
				{
					kPoints[k] = Mathf.random.Next(0, len);
					found = true;
					for (int i = 0; i < k; i++)
						if (kPoints[i] == kPoints[k])
						{
							found = false;
							break;
						}
				}
			}

			Array.Sort(kPoints);

			for (int i = 0; i < kPoints[0]; i++)
			{
				offspringX[i] = parent1[i];
				offspringY[i] = parent2[i];
			}
			for (int k = 1; k < points; k++)
			{
				if (k % 2 == 0)
					for (int i = kPoints[k - 1]; i < kPoints[k]; i++)
					{
						offspringX[i] = parent1[i];
						offspringY[i] = parent2[i];
					}
				else
					for (int i = kPoints[k - 1]; i < kPoints[k]; i++)
					{
						offspringX[i] = parent2[i];
						offspringY[i] = parent1[i];
					}

			}
			if (points % 2 == 0)
				for (int i = kPoints[points - 1]; i < len; i++)
				{
					offspringX[i] = parent1[i];
					offspringY[i] = parent2[i];
				}
			else
				for (int i = kPoints[points - 1]; i < len; i++)
				{
					offspringX[i] = parent2[i];
					offspringY[i] = parent1[i];
				}

			return (offspringX, offspringY);
		}
	}
}
