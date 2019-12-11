using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KernelDeeps.AI;
using KernelDeeps.AI.GA;

namespace KernelDeeps.AI.Test
{
	class Ant : IOrganism<Brain>
	{
		public Brain Genotype { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public float Score { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public float Fitness { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	}
}
