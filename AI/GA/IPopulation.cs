using System.Collections.Generic;

namespace KernelDeeps.AI.GA
{
	public interface IPopulation<T, V>
		where T : IOrganism<V>
		where V : IGenotype
	{
		float BestScore { get; }
		float BestFitness { get; }

		IEnumerable<T> Organisms { get; }
		void CalculateFitness();
		T ChooseOrganism();
		void GeneratePopulation();
	}
}
