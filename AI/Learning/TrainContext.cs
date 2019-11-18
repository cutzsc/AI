using System;

namespace KernelDeeps.AI.Learning
{
	public class TrainContext<T> : EventArgs
	{
		public T[] Inputs { get; }
		public T[] Outputs { get; }
		public T[] Targets { get; }
		public double Error { get; }

		public TrainContext() { }

		public TrainContext(T[] inputs, T[] outputs)
		{
			Inputs = new T[inputs.Length];
			Outputs = new T[outputs.Length];
			Array.Copy(inputs, Inputs, inputs.Length);
			Array.Copy(outputs, Outputs, outputs.Length);
		}

		public TrainContext(T[] inputs, T[] outputs, T[] targets)
			: this(inputs, outputs)
		{
			Targets = new T[targets.Length];
			Array.Copy(targets, Targets, targets.Length);
		}
	}
}
