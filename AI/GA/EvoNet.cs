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
	public class EvoNet : NeuralNetwork, IGenotype
	{
		public EvoNet(params LayerOptions[] layers)
			: base(layers) { }

		public EvoNet(IEnumerable<LayerOptions> layers)
			: base(layers) { }

		public override bool Equals(object obj)
		{
			EvoNet other = obj as EvoNet;
			if (other == null ||
				layers.Count != other.layers.Count)
				return false;
			for (int i = 0; i < layers.Count; i++)
			{
				if (layers[i].neuronCount != other.layers[i].neuronCount)
					return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public (IGenotype, IGenotype) Crossover(IGenotype partner, XOptions options = default)
		{
			if (!Equals(partner))
				throw new ArgumentException();

			EvoNet offspringX = new EvoNet(layers);
			EvoNet offspringY = new EvoNet(layers);
			offspringX.Build();
			offspringY.Build();

			(float[] offspringX, float[] offspringY) offsprings = (new float[0], new float[0]);
			switch (options.xType)
			{
				case XType.OnePointX:
					offsprings = Reproduction.OnePointX(ToRowMajorArray(), ((EvoNet)partner).ToRowMajorArray());
					break;
				case XType.KPointX:
					offsprings = Reproduction.KPointX(ToRowMajorArray(), ((EvoNet)partner).ToRowMajorArray(), options.kPoints);
					break;
			}

			int gene = 0;
			for (int w = 0; w < weights.Length; w++)
			{
				for (int y = 0; y < weights[w].RowCount; y++)
				{
					for (int x = 0; x < weights[w].ColumnCount; x++)
					{
						offspringX.weights[w][y, x] = offsprings.offspringX[gene];
						offspringY.weights[w][y, x] = offsprings.offspringY[gene];
						gene++;
					}
				}
				for (int x = 0; x < b_weights[w].ColumnCount; x++)
				{
					offspringX.b_weights[w][0, x] = offsprings.offspringX[gene];
					offspringY.b_weights[w][0, x] = offsprings.offspringY[gene];
					gene++;
				}
			}

			return (offspringX, offspringY);
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
							matrix[y, x] += Statistics.PDF(mean, stddev, Mathf.NextSingle(-1, 1));
						}
					}
				}
			}
		}

		public override object Clone()
		{
			EvoNet copy = new EvoNet(layers);
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
			return Clone() as EvoNet;
		}
	}
}
