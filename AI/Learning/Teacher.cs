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
		public delegate void LearningHandler(object sender, LearningEventArgs e);
		public event LearningHandler LearningBegan;
		public event LearningHandler LearningEnded;
		public event LearningHandler LearningPerformed;

		public NeuralNetwork Net { get; set; }
		public DataStream Data { get; set; }

		public double MSE { get; protected set; }

		public Teacher() { }

		public Teacher(NeuralNetwork net) { Net = net; }

		public Teacher(NeuralNetwork net, DataStream stream)
		{
			Net = net;
			Data = stream;
		}

		public virtual void Train(int epochs, int numSamples, int batchSize,
			LearningOptions learningOptions,
			CallBackOptions callBackOptions = default)
		{
			LearningBegan?.Invoke(this, new LearningEventArgs());

			for (int epoch = 0; epoch < epochs; epoch++)
			{
				for (int i = 0; i < (numSamples / batchSize); i++)
				{
					ProcessLearning(Data.ReadNext(new StreamOptions(batchSize, learningOptions.shuffleData)),
						learningOptions.learningRate, learningOptions.momentumRate, callBackOptions);
				}

				if (numSamples % batchSize > 0)
				{
					ProcessLearning(Data.ReadNext(new StreamOptions(numSamples % batchSize, learningOptions.shuffleData)),
						learningOptions.learningRate, learningOptions.momentumRate, callBackOptions);
				}

				if (callBackOptions.call == CallTime.EveryEpoch)
					LearningPerformed?.Invoke(this, new LearningEventArgs());
			}

			LearningEnded?.Invoke(this, new LearningEventArgs());
		}

		public virtual void Test(int numSamples, int batchSize, DataStream data,
			bool shuffle, CallBackOptions callBackOptions = default)
		{
			for (int i = 0; i < (numSamples / batchSize); i++)
			{
				ProcessTest(data.ReadNext(new StreamOptions(batchSize, shuffle)), callBackOptions);
			}

			if (numSamples % batchSize > 0)
			{
				ProcessTest(data.ReadNext(new StreamOptions(numSamples % batchSize, shuffle)), callBackOptions);
			}
		}

		private void ProcessLearning(Sample[] samples, float eta, float alpha, CallBackOptions callBackOptions)
		{
			foreach (Sample sample in samples)
			{
				Net.ProcessInputs(sample.inputs);
				Net.Learn(sample.outputs, eta, alpha);

				if (callBackOptions.call == CallTime.EverySample)
				{
					Cost(sample.outputs);
					LearningPerformed?.Invoke(this, new LearningEventArgs(sample, Net.Prediction, MSE));
				}
			}

			if (callBackOptions.call == CallTime.EveryBatch)
			{
				Cost(samples[samples.Length - 1].outputs);
				LearningPerformed?.Invoke(this, new LearningEventArgs(samples[samples.Length - 1], Net.Prediction, MSE));
			}
		}

		private void ProcessTest(Sample[] samples, CallBackOptions callBackOptions)
		{
			foreach (Sample sample in samples)
			{
				Net.ProcessInputs(sample.inputs);
			}
		}

		private void Cost(float[] targets)
		{
			float sum = 0;
			float[] outputs = Net.Prediction;
			for (int i = 0; i < targets.Length; i++)
			{
				float sub = targets[i] - outputs[i];
				sum += sub * sub;
			}
			MSE = sum / targets.Length;
		}
	}
}
