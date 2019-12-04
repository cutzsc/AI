using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KernelDeeps.IO;
using KernelDeeps.IO.MNIST;

namespace KernelDeeps.AI.Learning
{
	public class Teacher
	{
		NeuralNetwork net;
		BDReader reader;

		public delegate void LearningHandler(object sender, LearningEventArgs e);
		public event LearningHandler LearningBegan;
		public event LearningHandler LearningEnded;
		public event LearningHandler LearningPerformed;

		public double MSE { get; protected set; }

		public Teacher(NeuralNetwork net, BDReader reader)
		{
			this.net = net;
			this.reader = reader;
		}

		public virtual void Train(int epochs, int numSamples, int batchSize,
			float learningRate, float momentumRate)
		{
			for (int epoch = 0; epoch < epochs; epoch++)
			{
				Sample[] samples;

				for (int i = 0; i < (numSamples / batchSize); i++)
				{
					samples = reader.ReadNext(batchSize);
					foreach (Sample sample in samples)
					{
						net.ProcessInputs(sample.inputs);
						net.Learn(sample.outputs, 0.15f, 0.75f);
					}
				}

				samples = reader.ReadNext(numSamples % batchSize);
				foreach (Sample sample in samples)
				{
					net.ProcessInputs(sample.inputs);
					net.Learn(sample.outputs, 0.15f, 0.75f);
				}
			}
		}

		private void Cost(float[] targets)
		{
			float sum = 0;
			float[] outputs = net.Prediction;
			for (int i = 0; i < targets.Length; i++)
			{
				float sub = targets[i] - outputs[i];
				sum += sub * sub;
			}
			MSE = sum / targets.Length;
		}
	}
}
