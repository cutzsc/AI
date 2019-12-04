using System;

namespace KernelDeeps.AI.Learning
{
	public class DataSample : IDisposable
	{
		public readonly float[] inputs;
		public readonly float[] outputs;

		public DataSample(float[] inputs, float[] outputs)
			: this(inputs, 0, inputs.Length, outputs, 0, outputs.Length) { }

		public DataSample(float[] inputs, int insputs_start, int inputs_length,
			float[] outputs, int outputs_start, int outputs_length)
		{
			this.inputs = new float[inputs_length];
			this.outputs = new float[outputs_length];

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

		public void Dispose()
		{

		}
	}
}
