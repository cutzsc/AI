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
			LearningBegan?.Invoke(this, new LearningEventArgs());

			for (int epoch = 0; epoch < epochs; epoch++)
			{
				for (int i = 0; i < (numSamples / batchSize); i++)
				{
					ProcessBatch(reader.ReadNext(batchSize), learningRate, momentumRate);
				}

				if (numSamples % batchSize > 0)
				{
					ProcessBatch(reader.ReadNext(numSamples % batchSize), learningRate, momentumRate);
				}
			}

			LearningEnded?.Invoke(this, new LearningEventArgs());
		}

		private void ProcessBatch(Sample[] samples, float eta, float alpha)
		{
			int i = 0;
			foreach (Sample sample in samples)
			{
				net.ProcessInputs(sample.inputs);
				net.Learn(sample.outputs, eta, alpha);

				if (++i % samples.Length == 0)
				{
					Cost(sample.outputs);
					LearningPerformed?.Invoke(this, new LearningEventArgs(sample.inputs, net.Prediction, sample.outputs, MSE));
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
