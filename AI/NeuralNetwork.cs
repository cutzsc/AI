using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI
{
	public abstract class NeuralNetwork : ICloneable
	{
		protected readonly List<LayerOptions> layers;
		protected bool built = false;
		protected bool initialized = false;

		protected Matrix<float>[] outputs;
		protected Matrix<float>[] weights;
		protected Matrix<float>[] b_weights;
		protected Matrix<float>[] deltaWeights;
		protected Matrix<float>[] b_deltaWeights;

		public float[] Prediction => outputs[outputs.Length - 1].ToColumnMajorArray();
		public int LayerCount { get; protected set; }

		public NeuralNetwork(params LayerOptions[] layers)
		{
			this.layers = new List<LayerOptions>();
			this.layers.AddRange(layers);
		}

		public NeuralNetwork(IEnumerable<LayerOptions> layers)
		{
			this.layers = new List<LayerOptions>();
			this.layers.AddRange(layers);
		}

		#region add / remove

		public void Add(LayerOptions layer)
		{
			if (initialized)
				throw new Exception();
			layers.Add(layer);
		}

		public void Add(IEnumerable<LayerOptions> layers)
		{
			if (initialized)
				throw new Exception();
			this.layers.AddRange(layers);
		}

		public void Remove(LayerOptions layer)
		{
			if (initialized)
				throw new Exception();
			layers.Remove(layer);
		}

		public void Remove(int index)
		{
			if (initialized)
				throw new Exception();
			layers.RemoveAt(index);
		}

		#endregion

		/// <summary>
		/// Fully connected initialization, with random values.
		/// </summary>
		public void Build()
		{
			if (built)
				throw new Exception();

			built = true;
			layers.Capacity = layers.Count;
			LayerCount = layers.Count;

			outputs = new Matrix<float>[layers.Count];
			weights = new Matrix<float>[layers.Count - 1];
			deltaWeights = new Matrix<float>[layers.Count - 1];
			b_weights = new Matrix<float>[layers.Count - 1];
			b_deltaWeights = new Matrix<float>[layers.Count - 1];

			for (int i = 0; i < layers.Count; i++)
			{
				outputs[i] = Matrix<float>.Build.Dense(1, layers[i].NeuronCount);
				if (i < layers.Count - 1)
				{
					weights[i] = Matrix<float>.Build.Dense(layers[i].NeuronCount, layers[i + 1].NeuronCount);
					deltaWeights[i] = Matrix<float>.Build.Dense(layers[i].NeuronCount, layers[i + 1].NeuronCount);
					b_weights[i] = Matrix<float>.Build.Dense(1, layers[i + 1].NeuronCount);
					b_deltaWeights[i] = Matrix<float>.Build.Dense(1, layers[i + 1].NeuronCount);
				}
			}
		}

		public virtual void Initialize(float minWeight = -1f, float maxWeight = 1f)
		{
			if (initialized)
				throw new Exception();

			initialized = true;

			for (int i = 0; i < layers.Count - 1; i++)
			{
				weights[i].MapInplace(e => Mathf.NextSingle(minWeight, maxWeight));
				b_weights[i].MapInplace(e => Mathf.NextSingle(minWeight, maxWeight));
			}
		}

		/// <summary>
		/// Feed forward.
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
				outputs[i + 1].MapInplace(layers[i + 1].Transfer);
			}
		}

		/// <summary>
		/// Backpropagation.
		/// </summary>
		public virtual void Learn(float[] targets, float eta, float alpha)
		{
			if (targets.Length != outputs[outputs.Length - 1].ColumnCount)
				throw new ArgumentException();

			// calculate output layer
			Matrix<float> error = Matrix<float>.Build.Dense(1, outputs[outputs.Length - 1].ColumnCount, targets) - outputs[outputs.Length - 1];
			Matrix<float> gradient = outputs[outputs.Length - 1].Map(layers[layers.Count - 1].TransferDerivative).PointwiseMultiply(error);
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
				gradient = outputs[layer_index].Map(layers[layer_index].TransferDerivative).PointwiseMultiply(error);
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

		public virtual object Clone()
		{
			throw new NotImplementedException();
		}

		public virtual float[,] GetWeights(int layer, bool biasLayer = false)
		{
			if (biasLayer)
				return b_weights[layer].ToArray();
			return weights[layer].ToArray();
		}

		public int NeuronCount(int layer)
		{
			return layers[layer].NeuronCount;
		}
	}
}
