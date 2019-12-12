using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI.GA
{
	public static class Selection
	{
		public static T RouletteWheelSelection<T, V>(IEnumerable<T> organisms)
			where T : IOrganism<V>
			where V : IGenotype
		{
			int index = 0;
			float pointer = Mathf.NextSingle(0, 1);
			while (pointer >= 0)
			{
				pointer -= organisms.ElementAt(index++).SelectionProbability;
				if (index == organisms.Count() && pointer > 0)
					index = 0;
			}
			return organisms.ElementAt(index - 1);
		}
	}
}
