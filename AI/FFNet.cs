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
		public FFNet(params int[] dimension)
			: base(dimension) { }

		public override object Clone()
		{
			FFNet copy = new FFNet(dimension);
			copy.Initialize();
			copy.Transfer = Transfer;
			copy.TransferDerivative = TransferDerivative;

			for (int i = 0; i < weights.Length; i++)
			{
				copy.weights[i].SetSubMatrix(0, 0, weights[i]);
				copy.b_weights[i].SetSubMatrix(0, 0, b_weights[i]);
			}

			return copy;
		}
	}
}
