using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI.Learning
{
	public enum CallTime
	{
		Never = 0,
		Certain,
		EveryEpoch,
		EveryBatch,
		EverySample
	}
}
