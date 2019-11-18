using System;

namespace KernelDeeps.AI.Learning
{
	public class LearningSet<T>
		where T : struct, IEquatable<T>, IFormattable
	{
		public readonly T[] inputs;
		public readonly T[] outputs;

		public LearningSet(T[] inputs, T[] outputs)
			: this(inputs, 0, inputs.Length, outputs, 0, outputs.Length) { }

		public LearningSet(T[] inputs, int insputs_start, int inputs_length,
			T[] outputs, int outputs_start, int outputs_length)
		{
			this.inputs = new T[inputs_length];
			this.outputs = new T[outputs_length];

			int i = 0;
			if (inputs_length > outputs_length)
			{
				for (; i < outputs_length; i++)
				{
					this.inputs[i] = inputs[i + insputs_start];
					this.outputs[i] = outputs[i + outputs_start];
				}
				for (; i < inputs_length; i++)
				{
					this.inputs[i] = inputs[i + insputs_start];
				}
			}
			else
			{
				for (; i < inputs_length; i++)
				{
					this.inputs[i] = inputs[i + insputs_start];
					this.outputs[i] = outputs[i + outputs_start];
				}
				for (; i < outputs_length; i++)
				{
					this.outputs[i] = outputs[i + outputs_start];
				}
			}
		}
	}
}
