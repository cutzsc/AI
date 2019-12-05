using KernelDeeps.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI.Learning
{
	public class LearningEventArgs : EventArgs
	{
		public Sample Sample { get; }
		public float[] Prediction { get; }

		public int EpochNum { get; }
		public int SampleNum { get; }
		public int BatchNum { get; }

		public double MSE { get; }

		public LearningEventArgs() { }

		public LearningEventArgs(Sample sample, float[] prediction, double MSE)
		{
			float[] inputs = new float[sample.inputs.Length];
			float[] outputs = new float[sample.outputs.Length];
			Array.Copy(sample.inputs, inputs, inputs.Length);
			Array.Copy(sample.outputs, outputs, outputs.Length);

			Sample = new Sample(inputs, outputs);
			Prediction = prediction;
			this.MSE = MSE;
		}

		public LearningEventArgs(int epochNum, double MSE)
		{
			EpochNum = epochNum;
			this.MSE = MSE;
		}
	}
}
