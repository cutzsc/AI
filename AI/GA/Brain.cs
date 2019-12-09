using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KernelDeeps.AI;

namespace KernelDeeps.AI.GA
{
	public class Brain : NeuralNetwork, IGenotype
	{
		public Brain(params int[] dimension)
			: base(dimension) { }

		public IGenotype Crossover(IGenotype parent1, IGenotype parent2)
		{
			Brain p1 = parent1 as Brain;
			Brain p2 = parent2 as Brain;
			Brain child = new Brain(dimension);

			// TODO
			// crossover

			return child;
		}

		public void Mutate(float mutationRate)
		{
			for (int i = 0; i < weights.Length; i++)
			{
				weights[i].MapInplace(e =>
				{
					return Mathf.random.NextDouble() < mutationRate ? Mathf.NextSingle(-1, 1) : e;
				});

				b_weights[i].MapInplace(e =>
				{
					return Mathf.random.NextDouble() < mutationRate ? Mathf.NextSingle(-1, 1) : e;
				});
			}
		}

		public override object Clone()
		{
			Brain copy = new Brain(dimension);
			copy.Initialize();
			copy.Transfer = Transfer;
			copy.TransferDerivative = TransferDerivative;
			for (int i = 0; i < weights.Length; i++)
			{
				copy.weights[i].SetSubMatrix(0, 0, weights[i]);
				copy.b_weights[i].SetSubMatrix(0, 0, b_weights[i]);
			}
			return copy;
		}

		IGenotype IGenotype.Clone()
		{
			Brain copy = new Brain(dimension);
			copy.Initialize();
			copy.Transfer = Transfer;
			copy.TransferDerivative = TransferDerivative;
			for (int i = 0; i < weights.Length; i++)
			{
				copy.weights[i].SetSubMatrix(0, 0, weights[i]);
				copy.b_weights[i].SetSubMatrix(0, 0, b_weights[i]);
			}
			return copy;
		}
	}
}
