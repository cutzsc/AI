using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI.Learning
{
	public class LearningData<T> : ICloneable
		where T : struct, IEquatable<T>, IFormattable
	{
		public readonly int inputSize;
		public readonly int outputSize;

		List<LearningSet<T>> sets = new List<LearningSet<T>>();
		int position = 0;

		public LearningSet<T> this[int index]
		{
			get { return sets[index]; }
		}

		public LearningData(int inputSize, int outputSize)
		{
			this.inputSize = inputSize;
			this.outputSize = outputSize;
		}

		public void Add(T[] inputs, T[] outputs)
		{
			int count = inputs.Length / inputSize;
			if (count != outputs.Length / outputSize ||
				inputs.Length % inputSize != 0 ||
				outputs.Length % outputSize != 0)
				throw new ArgumentException();

			for (int i = 0; i < count; i++)
			{
				sets.Add(new LearningSet<T>(inputs, i * inputSize, inputSize,
					outputs, i * outputSize, outputSize));
			}
		}

		public void Add(LearningSet<T> set)
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
			LearningData<T> cpy = new LearningData<T>(inputSize, outputSize);
			for (int i = 0; i < sets.Count; i++)
			{
				cpy.Add(new LearningSet<T>(sets[i].inputs, i * inputSize, inputSize,
					sets[i].outputs, i * outputSize, outputSize));
			}
			return cpy;
		}

		public LearningSet<T> Next(bool random)
		{
			if (random)
				return sets[Mathd.rand.Next(0, sets.Count)];
			if (position >= sets.Count)
				position = 0;
			return sets[position++];
		}
	}
}
