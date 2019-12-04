using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI
{
	public abstract class NeuralNetwork
	{
		protected int[] dimension;

		protected Matrix<float>[] outputs;
		protected Matrix<float>[] weights;
		protected Matrix<float>[] deltaWeights;
		protected Matrix<float>[] b_weights;
		protected Matrix<float>[] b_deltaWeights;

		public Func<float, float> Transfer { get; set; }
		public Func<float, float> TransferDerivative { get; set; }
		public float[] Prediction => outputs[outputs.Length - 1].ToRowMajorArray();

		public NeuralNetwork(params int[] dimension)
		{
			this.dimension = new int[dimension.Length];
			Array.Copy(dimension, this.dimension, dimension.Length);
			outputs = new Matrix<float>[dimension.Length];
			weights = new Matrix<float>[dimension.Length - 1];
			deltaWeights = new Matrix<float>[dimension.Length - 1];
			b_weights = new Matrix<float>[dimension.Length - 1];
			b_deltaWeights = new Matrix<float>[dimension.Length - 1];
		}

		/// <summary>
		/// Fully connected initialization
		/// </summary>
		public virtual void Init(float minWeight, float maxWeight, Func<float, float, float> f)
		{
			for (int i = 0; i < dimension.Length; i++)
			{
				outputs[i] = Matrix<float>.Build.Dense(1, dimension[i]);
				if (i < dimension.Length - 1)
				{
					weights[i] = Matrix<float>.Build.Dense(dimension[i], dimension[i + 1]);
					deltaWeights[i] = Matrix<float>.Build.Dense(dimension[i], dimension[i + 1]);
					b_weights[i] = Matrix<float>.Build.Dense(1, dimension[i + 1]);
					b_deltaWeights[i] = Matrix<float>.Build.Dense(1, dimension[i + 1]);
					weights[i].MapInplace(e => e = f(minWeight, maxWeight));
					b_weights[i].MapInplace(e => e = f(minWeight, maxWeight));
				}
			}
		}

		/// <summary>
		/// Feed forward
		/// </summary>
		public virtual void ProcessInputs(float[] inputs)
		{
			if (inputs.Length != outputs[0].ColumnCount)
				throw new ArgumentException();

			outputs[0].SetRow(0, inputs);

			for (int i = 0; i < outputs.Length - 1; i++)
			{
				outputs[i + 1] = outputs[i] * weights[i];
				outputs[i + 1] += b_weights[i];
				outputs[i + 1].MapInplace(Transfer);
			}
		}

		/// <summary>
		/// Backpropagation
		/// </summary>
		public virtual void Learn(float[] targets, float eta, float alpha)
		{
			if (targets.Length != outputs.Last().ColumnCount)
				throw new ArgumentException();

			// calculate output layer
			Matrix<float> error = Matrix<float>.Build.Dense(1, outputs[outputs.Length - 1].ColumnCount, targets) - outputs[outputs.Length - 1];
			Matrix<float> gradient = outputs[outputs.Length - 1].Map(TransferDerivative).PointwiseMultiply(error);
			Matrix<float> momentum = deltaWeights[deltaWeights.Length - 1].Multiply(alpha);
			Matrix<float> b_momentum = b_deltaWeights[b_deltaWeights.Length - 1].Multiply(alpha);

			deltaWeights[deltaWeights.Length - 1] = outputs[outputs.Length - 2].TransposeThisAndMultiply(gradient)
				.Multiply(eta)
				.Add(momentum);
			weights[weights.Length - 1] += deltaWeights[deltaWeights.Length - 1];

			b_deltaWeights[b_deltaWeights.Length - 1] = gradient
				.Multiply(eta)
				.Add(b_momentum);
			b_weights[b_weights.Length - 1] += b_deltaWeights[b_deltaWeights.Length - 1];

			// Other layers
			for (int layer_index = outputs.Length - 2, weights_index = weights.Length - 2;
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
