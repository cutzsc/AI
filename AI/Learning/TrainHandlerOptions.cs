namespace KernelDeeps.AI.Learning
{
	public struct TrainHandlerOptions
	{
		public CallTime call;
		public int calls;
	}

	public enum CallTime
	{
		Never = 0,
		AtTheEnd,
		EveryFewTimes,
	}
}
