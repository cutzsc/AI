namespace KernelDeeps.AI.GA
{
	public interface IOrganism<T>
		where T : IGenotype
	{
		T Genotype { get; set; }
		float Score { get; set; }
		float Fitness { get; set; }
	}
}
