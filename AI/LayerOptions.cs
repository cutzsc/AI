using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI
{
	public struct LayerOptions
	{
		public Func<float, float> transfer;
		public Func<float, float> transferDerivative;
		public int neuronCount;

		public LayerOptions(int neuronCount, Func<float, float> transfer, Func<float, float> transferDerivative)
		{
			this.neuronCount = neuronCount;
			this.transfer = transfer;
			this.transferDerivative = transferDerivative;
		}
	}
}
