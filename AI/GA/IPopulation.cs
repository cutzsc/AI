using System.Collections.Generic;

namespace KernelDeeps.AI.GA
{
	public interface IPopulation<T, V>
		where T : IOrganism<V>
		where V : IGenotype
	{
		List<T> Creatures { get; }
		void CalculateFitness();
		T ChooseOrganism();
		void GeneratePopulation();
	}
}
