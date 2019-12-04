using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI.Learning
{
	public class DataBatch
	{
		public readonly int inputSize;
		public readonly int outputSize;

		List<DataSample> sets = new List<DataSample>();
		int position = 0;

		public DataSample this[int index]
		{
			get { return sets[index]; }
		}

		public DataBatch(int inputSize, int outputSize)
		{
			this.inputSize = inputSize;
			this.outputSize = outputSize;
		}

		public void Add(float[] inputs, float[] outputs)
		{
			int count = inputs.Length / inputSize;
			if (count != outputs.Length / outputSize ||
				inputs.Length % inputSize != 0 ||
				outputs.Length % outputSize != 0)
				throw new ArgumentException();

			for (int i = 0; i < count; i++)
			{
				sets.Add(new DataSample(inputs, i * inputSize, inputSize,
					outputs, i * outputSize, outputSize));
			}
		}

		public void Add(DataSample set)
		{
			if (set.inputs.Length != inputSize ||
				set.outputs.Length != outputSize)
				throw new ArgumentException();

			sets.Add(set);
		}

		public void Remove(int index)
		{
			sets.RemoveAt(index);
		}

		public void Remove(int index, int count)
		{
			sets.RemoveRange(index, count);
		}

		public object Clone()
		{
			DataBatch cpy = new DataBatch(inputSize, outputSize);
			for (int i = 0; i < sets.Count; i++)
			{
				cpy.Add(new DataSample(sets[i].inputs, i * inputSize, inputSize,
					sets[i].outputs, i * outputSize, outputSize));
			}
			return cpy;
		}

		public DataSample Next(bool random)
		{
			if (random)
				return sets[Mathf.random.Next(0, sets.Count)];
			if (position >= sets.Count)
				position = 0;
			return sets[position++];
		}
	}
}
