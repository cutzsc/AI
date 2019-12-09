using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI.GA
{
	public interface IIndividual
	{
		IGenotype Genotype { get; set; }
		float Score { get; set; }
		float Fitness { get; set; }
	}
}
