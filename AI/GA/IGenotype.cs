namespace KernelDeeps.AI.GA
{
	public interface IGenotype
	{
		IGenotype Crossover(IGenotype parent1, IGenotype parent2);
		void Mutate(float mutationRate);
		IGenotype Clone();
	}
}
