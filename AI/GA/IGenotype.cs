namespace KernelDeeps.AI.GA
{
	public interface IGenotype
	{
		(IGenotype, IGenotype) Crossover(IGenotype partner);
		void Mutate(MutationType type, float rate);
		IGenotype Clone();
	}
}
