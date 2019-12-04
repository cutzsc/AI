using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI.Learning
{
	public class Trainer
	{
		public delegate void TrainHandler(TrainEventArgs ctx);
		public event TrainHandler TrainBegan;
		public event TrainHandler TrainEnded;
		public event TrainHandler TrainPerform;

		INeuralNetwork network;
		DataBatch dataBatch;

		public double MSE { get; private set; }

		public Trainer(INeuralNetwork network)
		{
			this.network = network;
		}

		public void SetLearningData(DataBatch dataBatch)
		{
			this.dataBatch = dataBatch;
		}

		public void Train(int iterations, float eta, float alpha, TrainHandlerOptions options = default, bool allowRandom = false)
		{
			TrainBegan?.Invoke(new TrainEventArgs());

			DataSample set = null;
			int time = iterations / (options.calls <= 0 ? options.call == CallTime.EveryFewTimes ? 1 : iterations + 1 : options.calls);

			for (int i = 0, dist = iterations - options.calls; i < iterations; i++)
			{
				set = dataBatch.Next(allowRandom);

				network.ProcessInputs(set.inputs);
				network.Learn(set.outputs, eta, alpha);

				switch (options.call)
				{
					case CallTime.AtTheEnd:
						if (i >= dist)
							TrainPerform?.Invoke(new TrainEventArgs(set.inputs, network.Prediction, set.outputs));
						break;
					case CallTime.EveryFewTimes:
						if (i % time == 0)
							TrainPerform?.Invoke(new TrainEventArgs(set.inputs, network.Prediction, set.outputs));
						break;
				}
			}

			TrainEnded?.Invoke(new TrainEventArgs(set.inputs, network.Prediction, set.outputs));
		}

		public void Train(int epochs, int samples, int batchSize)
		{
			// pseudo
			int iterations = samples / batchSize;

			for (int epoch = 0; epoch < epochs; epoch++)
			{

				for (int i = 0; i < iterations; i++)
				{
					//dataBatch.Begin(batchSize);
					//foreach (Sample sample in dataBatch)
					//{

					//}
					//dataBatch.End();
				}

			}
		}

		private void Cost(float[] targets)
		{
			float sum = 0;
			float[] outputs = network.Prediction;
			for (int i = 0; i < dataBatch.outputSize; i++)
			{
				float sub = targets[i] - outputs[i];
				sum += sub * sub;
			}
			MSE = sum / dataBatch.outputSize;
		}
	}
}
