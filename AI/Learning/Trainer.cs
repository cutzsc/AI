using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI.Learning
{
	public class Trainer<T>
		where T : struct, IEquatable<T>, IFormattable
	{
		public delegate void TrainHandler(TrainContext<T> ctx);
		public event TrainHandler TrainBegan;
		public event TrainHandler TrainEnded;
		public event TrainHandler TrainPerform;

		INeuralNetwork<T> obj;
		LearningData<T> data;

		public Trainer(INeuralNetwork<T> obj)
		{
			this.obj = obj;
		}

		public void SetLearningData(LearningData<T> data)
		{
			this.data = data;
		}

		public void Train(int iterations, T eta, T alpha, TrainHandlerOptions options = default, bool allowRandom = false)
		{
			TrainBegan?.Invoke(new TrainContext<T>());

			LearningSet<T> set = null;
			int time = iterations / (options.calls <= 0 ? options.call == CallTime.EveryFewTimes ? 1 : iterations + 1 : options.calls);

			for (int i = 0, dist = iterations - options.calls; i < iterations; i++)
			{
				set = data.Next(allowRandom);

				obj.ProcessInputs(set.inputs);
				obj.Learn(set.outputs, eta, alpha);

				switch (options.call)
				{
					case CallTime.AtTheEnd:
						if (i >= dist)
							TrainPerform?.Invoke(new TrainContext<T>(set.inputs, obj.Prediction, set.outputs));
						break;
					case CallTime.EveryFewTimes:
						if (i % time == 0)
							TrainPerform?.Invoke(new TrainContext<T>(set.inputs, obj.Prediction, set.outputs));
						break;
				}
			}

			TrainEnded?.Invoke(new TrainContext<T>(set.inputs, obj.Prediction, set.outputs));
		}
	}
}
