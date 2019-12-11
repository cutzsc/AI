using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KernelDeeps.AI;
using KernelDeeps.AI.GA;

namespace KernelDeeps.AI.Test
{
	class Swarm : IPopulation<Ant, Brain>
	{
		public float BestScore => throw new NotImplementedException();

		public float BestFitness => throw new NotImplementedException();

		public IEnumerable<Ant> Creatures => throw new NotImplementedException();

		public void CalculateFitness()
		{
			throw new NotImplementedException();
		}

		public Ant ChooseOrganism()
		{
			throw new NotImplementedException();
		}

		public void GeneratePopulation()
		{
			throw new NotImplementedException();
		}
	}
}
