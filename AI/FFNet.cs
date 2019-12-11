using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Random;
using MathNet.Numerics.LinearAlgebra;

namespace KernelDeeps.AI
{
	public class FFNet : NeuralNetwork
	{
		public FFNet(params LayerOptions[] layers)
			: base(layers) { }

		public FFNet(IEnumerable<LayerOptions> layers)
			: base(layers) { }

		public override object Clone()
		{
			FFNet copy = new FFNet(layers);
			copy.Build();
			for (int i = 0; i < weights.Length; i++)
			{
				copy.weights[i].SetSubMatrix(0, 0, weights[i]);
				copy.b_weights[i].SetSubMatrix(0, 0, b_weights[i]);
			}
			return copy;
		}
	}
}
