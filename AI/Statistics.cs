using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelDeeps.AI
{
	public static class Statistics
	{
		/// <summary>
		/// Стандартное нормальное распределение.
		/// </summary>
		public static float PDF(float mean, float stddev, float x)
		{
			float d = (x - mean) / stddev;
			return (float)Math.Exp(-0.5 * d * d) / (Mathf.Sqrt2Pi * stddev);
		}
		/// <summary>
		/// Моделирования стандартных нормально распределённых случайных величин.
		/// </summary>
		public static (float, float) BoxMuller(float mean, float stddev)
		{
			float r;
			float v1;
			float v2;
		Mark:
			v1 = Mathf.NextSingle(-1f, 1f);
			v2 = Mathf.NextSingle(-1f, 1f);

			r = v1 * v1 + v2 * v2;
			if (r >= 1f || r == 0f)
				goto Mark;

			float fac = (float)Math.Sqrt(-2.0f * (float)Math.Log(r) / r);
			return (mean + stddev * v1 * fac,
					mean + stddev * v2 * fac);
		}
		/// <summary>
		/// Среднее значение.
		/// </summary>
		public static float Mean(float[] data)
		{
			float sum = 0;
			for (int i = 0; i < data.Length; i++)
			{
				sum += data[i];
			}
			return sum / data.Length;
		}
		/// <summary>
		/// Среднее значение.
		/// </summary>
		public static float Mean(float[,] data, int rows, int cols)
		{
			float sum = 0;
			for (int y = 0; y < rows; y++)
			{
				for (int x = 0; x < cols; x++)
				{
					sum += data[y, x];
				}
			}
			return sum / (rows * cols);
		}
		/// <summary>
		/// Среднее значение.
		/// </summary>
		public static float Mean(IEnumerable<Matrix<float>> data)
		{
			float sum = 0;
			int n = 0;
			foreach (Matrix<float> matrix in data)
			{
				for (int y = 0; y < matrix.RowCount; y++)
				{
					for (int x = 0; x < matrix.ColumnCount; x++)
					{
						sum += matrix[y, x];
						n++;
					}
				}
			}
			return sum / n;
		}
		/// <summary>
		/// Дисперсия - квадрат стандартного отклонения.
		/// </summary>
		public static float Variance(float[] data, float mean)
		{
			float sum = 0;
			for (int i = 0; i < data.Length; i++)
			{
				float diff = data[i] - mean;
				sum += diff * diff;
			}
			return sum / data.Length;
		}
		/// <summary>
		/// Дисперсия - квадрат стандартного отклонения.
		/// </summary>
		public static float Variance(float[,] data, int rows, int cols, float mean)
		{
			float sum = 0;
			for (int y = 0; y < rows; y++)
			{
				for (int x = 0; x < cols; x++)
				{
					float diff = data[y, x] - mean;
					sum += diff * diff;
				}
			}
			return sum / (rows * cols);
		}
		/// <summary>
		/// Дисперсия - квадрат стандартного отклонения.
		/// </summary>
		public static float Variance(IEnumerable<Matrix<float>> data, float mean)
		{
			float sum = 0;
			int n = 0;
			foreach (Matrix<float> matrix in data)
			{
				for (int y = 0; y < matrix.RowCount; y++)
				{
					for (int x = 0; x < matrix.ColumnCount; x++)
					{
						float diff = matrix[y, x] - mean;
						sum += diff * diff;
						n++;
					}
				}
			}
			return sum / n;
		}
		/// <summary>
		/// Среднеквадратическое отклонение.
		/// </summary>
		public static float Stddev(float[] data, float mean)
		{
			return (float)Math.Sqrt(Variance(data, mean));
		}
		/// <summary>
		/// Среднеквадратическое отклонение.
		/// </summary>
		public static float Stddev(float[,] data, int rows, int cols, float mean)
		{
			return (float)Math.Sqrt(Variance(data, rows, cols, mean));
		}
		/// <summary>
		/// Среднеквадратическое отклонение.
		/// </summary>
		public static float Stddev(IEnumerable<Matrix<float>> data, float mean)
		{
			return (float)Math.Sqrt(Variance(data, mean));
		}
	}
}
