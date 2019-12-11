using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI.GA
{
	public struct XOptions
	{
		public XType xType;
		public int kPoints;

		public XOptions(XType xType, int kPoints)
		{
			this.xType = xType;
			this.kPoints = kPoints;
		}
	}
}
