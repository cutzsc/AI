using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI.Learning
{
	public struct LearningOptions
	{
		public float learningRate;
		public float momentumRate;
		public bool shuffleData;
		
		public LearningOptions(float learningRate, float momentumRate, bool shuffleData)
		{
			this.learningRate = learningRate;
			this.momentumRate = momentumRate;
			this.shuffleData = shuffleData;
		}
	}
}
