using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI.GA
{
	public interface IPopulation
	{
		void EvaluateFitness();
		IIndividual SelectIndividual();
		void Diversificate();
		void GeneratePopulation();
	}
}
