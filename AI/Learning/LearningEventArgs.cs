using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI.Learning
{
	public class LearningEventArgs : EventArgs
	{
		public float[] Inputs { get; }
		public float[] Outputs { get; }
		public float[] Targets { get; }
		public double MSE { get; }

		public LearningEventArgs() { }

		public LearningEventArgs(float[] inputs, float[] outputs, float[] targets, double MSE)
		{
			Inputs = inputs;
			Outputs = outputs;
			Targets = targets;
			this.MSE = MSE;
		}
	}
}
