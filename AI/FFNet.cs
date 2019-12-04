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
	}
}
