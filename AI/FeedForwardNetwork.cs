using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Random;
using MathNet.Numerics.LinearAlgebra;

namespace KernelDeeps.AI
{
	public class FeedForwardNetwork<T> : INeuralNetwork<T>
		where T : struct, IEquatable<T>, IFormattable
	{
		// inits
		int[] dimension;
		int indexOfLayers_last;
		int indexOfWeights_last;

		// neural network params
		Matrix<T>[] outputs;
		Matrix<T>[] weights;
		Matrix<T>[] deltaWeights;
		Matrix<T>[] b_weights;
		Matrix<T>[] b_deltaWeights;
		
		public Func<T, T> Transfer { get; set; }
		public Func<T, T> TransferDerivative { get; set; }
		public T[] Prediction => outputs[dimension.Length - 1].ToRowMajorArray();

		public FeedForwardNetwork(params int[] dimension)
		{
			this.dimension = new int[dimension.Length];
			indexOfLayers_last = dimension.Length - 1;
			indexOfWeights_last = dimension.Length - 2;
			Array.Copy(dimension, this.dimension, dimension.Length);
			outputs = new Matrix<T>[dimension.Length];
			weights = new Matrix<T>[dimension.Length - 1];
			deltaWeights = new Matrix<T>[dimension.Length - 1];
			b_weights = new Matrix<T>[dimension.Length - 1];
			b_deltaWeights = new Matrix<T>[dimension.Length - 1];
		}

		public void Init(T minWeight, T maxWeight, Func<T, T, T> f)
		{
			for (int i = 0; i < dimension.Length; i++)
			{
				outputs[i] = Matrix<T>.Build.Dense(1, dimension[i]);
				if (i < dimension.Length - 1)
				{
					weights[i] = Matrix<T>.Build.Dense(dimension[i], dimension[i + 1]);
					deltaWeights[i] = Matrix<T>.Build.Dense(dimension[i], dimension[i + 1]);
					b_weights[i] = Matrix<T>.Build.Dense(1, dimension[i + 1]);
					b_deltaWeights[i] = Matrix<T>.Build.Dense(1, dimension[i + 1]);
					weights[i].MapInplace(e => e = f(minWeight, maxWeight));
					b_weights[i].MapInplace(e => e = f(minWeight, maxWeight));
				}
			}
		}

		public void ProcessInputs(T[] inputs)
		{
			if (inputs.Length != dimension[0])
				throw new ArgumentException();

			outputs[0].SetRow(0, inputs);

			for (int i = 0; i < outputs.Length - 1; i++)
			{
				outputs[i + 1] = outputs[i] * weights[i];
				outputs[i + 1] += b_weights[i];
				outputs[i + 1].MapInplace(Transfer);
			}
		}

		public void Learn(T[] targets, T eta, T alpha)
		{
			// Output layer
			Matrix<T> error = Matrix<T>.Build.Dense(1, dimension[indexOfLayers_last], targets) - outputs[indexOfLayers_last]; // НЕ СОЗДАТЬ А РАСЧИТАТЬ
			Matrix<T> gradient = outputs[indexOfLayers_last].Map(TransferDerivative).PointwiseMultiply(error);
			Matrix<T> momentum = deltaWeights[indexOfWeights_last].Multiply(alpha);
			Matrix<T> b_momentum = b_deltaWeights[indexOfWeights_last].Multiply(alpha);

			deltaWeights[indexOfWeights_last] = outputs[indexOfLayers_last - 1].TransposeThisAndMultiply(gradient)
				.Multiply(eta)
				.Add(momentum);
			weights[indexOfWeights_last] += deltaWeights[indexOfWeights_last];

			b_deltaWeights[indexOfWeights_last] = gradient
				.Multiply(eta)
				.Add(b_momentum);
			b_weights[indexOfWeights_last] += b_deltaWeights[indexOfWeights_last];

			// Other layers
			for (int layer_index = indexOfLayers_last - 1, weights_index = indexOfWeights_last - 1;
				layer_index > 0;
				layer_index--, weights_index--)
			{
				error = gradient.TransposeAndMultiply(weights[weights_index + 1]);
				gradient = outputs[layer_index].Map(TransferDerivative).PointwiseMultiply(error);
				momentum = deltaWeights[weights_index].Multiply(alpha);
				b_momentum = b_deltaWeights[weights_index].Multiply(alpha);

				deltaWeights[weights_index] = outputs[layer_index - 1].TransposeThisAndMultiply(gradient)
					.Multiply(eta)
					.Add(momentum);
				weights[weights_index] += deltaWeights[weights_index];

				b_deltaWeights[weights_index] = gradient
					.Multiply(eta)
					.Add(b_momentum);
				b_weights[weights_index] += b_deltaWeights[weights_index];
			}
		}
	}
}
