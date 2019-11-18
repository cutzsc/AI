using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace KernelDeeps.AI.Learning
{
	public class TrainSet<T>
		where T : struct, IEquatable<T>, IFormattable
	{
		public readonly int numInputs;
		public readonly int numOutputs;

		List<Vector<T>> inputs;
		List<Vector<T>> outputs;

		public int Size => inputs.Count;

		public TrainSet(int numInputs, int numOutputs)
		{
			this.numInputs = numInputs;
			this.numOutputs = numOutputs;
			inputs = new List<Vector<T>>();
			outputs = new List<Vector<T>>();
		}

		#region Initialization

		public void SetData(T[,] inputs, T[,] outputs)
		{
			if (inputs.GetLength(0) != outputs.GetLength(0) ||
				inputs.GetLength(1) != numInputs ||
				outputs.GetLength(1) != numOutputs)
				throw new ArgumentException();

			for (int i = 0; i < inputs.GetLength(0); i++)
			{
				this.inputs.Add(Vector<T>.Build
					.DenseOfEnumerable(Enumerable.Range(0, numInputs).Select(e => inputs[i, e])));
				this.outputs.Add(Vector<T>.Build
					.DenseOfEnumerable(Enumerable.Range(0, numOutputs).Select(e => outputs[i, e])));
			}
		}

		public void SetData(T[] inputs, T[] outputs)
		{
			int datasetSize = inputs.Length / numInputs;
			if (datasetSize == outputs.Length / numOutputs ||
				inputs.Length % numInputs != 0)
				throw new ArgumentException();

			T[] inputsBuffer = new T[numInputs];
			T[] outputsBuffer = new T[numOutputs];
			for (int i = 0; i < datasetSize; i++)
			{
				Array.Copy(inputs, i * numInputs, inputsBuffer, 0, numInputs);
				Array.Copy(outputs, i * numOutputs, outputsBuffer, 0, numOutputs);
				this.inputs.Add(Vector<T>.Build.DenseOfArray(inputsBuffer));
				this.outputs.Add(Vector<T>.Build.DenseOfArray(outputsBuffer));
			}
		}

		public void AddData(T[] inputs, T[] outputs)
		{
			if (inputs.Length != numInputs ||
				outputs.Length != numOutputs)
				throw new ArgumentException();

			this.inputs.Add(Vector<T>.Build.Dense(inputs));
			this.outputs.Add(Vector<T>.Build.Dense(outputs));
		}

		public void RemoveDataAt(int index)
		{
			inputs.RemoveAt(index);
			outputs.RemoveAt(index);
		}

		#endregion
	}
}
