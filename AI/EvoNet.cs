using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KernelDeeps.AI.GA;

namespace KernelDeeps.AI
{
	public class EvoNet : NeuralNetwork, IGenotype
	{
		public EvoNet(params int[] dimension)
			: base(dimension) { }

		public override object Clone()
		{
			EvoNet copy = new EvoNet(dimension);
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

		public IGenotype Crossover(IGenotype parent1, IGenotype parent2)
		{
			EvoNet p1 = parent1 as EvoNet;
			EvoNet p2 = parent2 as EvoNet;
			EvoNet child = new EvoNet(dimension);

			// crossover weights

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

		IGenotype IGenotype.Clone()
		{
			return (IGenotype)Clone();
		}
	}
}
