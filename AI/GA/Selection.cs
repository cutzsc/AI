using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI.GA
{
	public static class Selection
	{
		public static void LinearRankSelection() { }

		public static T RouletteWheelSelection<T, V>(IEnumerable<T> organisms)
			where T : IOrganism<V>
			where V : IGenotype
		{
			int index = 0;
			float roulette = Mathf.NextSingle(0, 1);
			while (roulette >= 0)
			{
				roulette -= organisms.ElementAt(index++).SelectionProbability;
			}
			return organisms.ElementAt(index - 1);
		}

		public static void StochasticUniversalSampling() { }

		public static void TruncationSelection() { }

		public static void TournamentSelection() { }
	}
}
