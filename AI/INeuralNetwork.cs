using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace KernelDeeps.AI
{
    public interface INeuralNetwork<T>
		where T : struct, IEquatable<T>, IFormattable
	{
		T[] Prediction { get; }
		Func<T, T> Transfer { get; set; }
		Func<T, T> TransferDerivative { get; set; }
		void Init(T minWeight, T maxWeight, Func<T, T, T> f);
		void ProcessInputs(T[] inputs);
		void Learn(T[] targets, T eta, T alpha);
    }
}
