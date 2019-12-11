using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI
{
	public class LayerOptions
	{
		public Func<float, float> Transfer { get; }
		public Func<float, float> TransferDerivative { get; }
		public int NeuronCount { get; }

		public LayerOptions(int neuronCount, Func<float, float> transfer, Func<float, float> transferDerivative)
		{
			NeuronCount = neuronCount;
			Transfer = transfer;
			TransferDerivative = transferDerivative;
		}
	}
}
