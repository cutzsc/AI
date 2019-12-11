namespace KernelDeeps.AI.GA
{
	public interface IGenotype
	{
		(IGenotype, IGenotype) Crossover(IGenotype partner, XOptions options);
		void Mutate(MutationType type, float rate);
		IGenotype Clone();
	}
}
