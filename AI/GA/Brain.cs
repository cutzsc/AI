using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KernelDeeps.AI;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.Distributions;

namespace KernelDeeps.AI.GA
{
	public class Brain : NeuralNetwork, IGenotype
	{
		public Brain(params LayerOptions[] layers)
			: base(layers) { }

		public Brain(IEnumerable<LayerOptions> layers)
			: base(layers) { }

		public (IGenotype, IGenotype) Crossover(IGenotype partner)
		{
			Brain child = new Brain(layers);

			// TODO

			// crossover

			return (child, child);
		}

		public void Mutate(MutationType type, float chance)
		{
			List<Matrix<float>> data = new List<Matrix<float>>();
			for (int i = 0; i < weights.Length; i++)
			{
				data.Add(weights[i]);
				data.Add(b_weights[i]);
			}
			float mean = Statistics.Mean(data);
			float stddev = Statistics.Stddev(data, mean);

			switch (type)
			{
				case MutationType.PDF:
					PDF(data, chance, mean, stddev);
					break;
				case MutationType.BoxMuller:
					BoxMuller(data, chance, mean, stddev);
					break;
				default:
					throw new ArgumentException();
			}
		}

		private void BoxMuller(List<Matrix<float>> data, float chance, float mean, float stddev)
		{
			var boxMuller = Statistics.BoxMuller(mean, stddev);
			int toggle = 0;

			foreach (Matrix<float> matrix in data)
			{
				for (int y = 0; y < matrix.RowCount; y++)
				{
					for (int x = 0; x < matrix.ColumnCount; x++)
					{
						if (Mathf.random.NextDouble() < chance)
						{
							toggle++;
							if (toggle % 2 == 0)
							{
								matrix[y, x] += boxMuller.Item1;
								toggle = 0;
								boxMuller = Statistics.BoxMuller(mean, stddev);
							}
							else
								matrix[y, x] += boxMuller.Item2;
						}
					}
				}
			}
		}

		private void PDF(List<Matrix<float>> data, float chance, float mean, float stddev)
		{
			foreach (Matrix<float> matrix in data)
			{
				for (int y = 0; y < matrix.RowCount; y++)
				{
					for (int x = 0; x < matrix.ColumnCount; x++)
					{
						if (Mathf.random.NextDouble() < chance)
						{
							matrix[y, x] += Statistics.PDF(mean, stddev, Mathf.NextSingle(-1f, 1f));
						}
					}
				}
			}
		}

		public override object Clone()
		{
			Brain copy = new Brain(layers);
			copy.Build();
			for (int i = 0; i < weights.Length; i++)
			{
				copy.weights[i].SetSubMatrix(0, 0, weights[i]);
				copy.b_weights[i].SetSubMatrix(0, 0, b_weights[i]);
			}
			return copy;
		}

		IGenotype IGenotype.Clone()
		{
			return Clone() as Brain;
		}
	}
}
