using System.Globalization;

namespace Ujeby.Common.Tools.Types
{
	public class v2
	{
		public double X = 0.0;
		public double Y = 0.0;

		public v2()
		{

		}

		public v2(v2 v) : this(v.X, v.Y)
		{

		}

		public v2(double x, double y)
		{
			X = x;
			Y = y;
		}

		public v2(double d) : this(d, d)
		{

		}

		public override string ToString()
		{
			return $"{ X.ToString("F4", CultureInfo.InvariantCulture) }, { Y.ToString("F4", CultureInfo.InvariantCulture) }";
		}
	}

	public class v3 : v2
	{
		public double Z = 0.0;

		public v3()
		{

		}

		public v3(v3 v) : this(v.X, v.Y, v.Z)
		{

		}

		public v3(double x, double y, double z) : base(x, y)
		{
			Z = z;
		}

		public v3(double d) : this(d, d, d)
		{

		}

		public v3(v2 v) : base(v.X, v.Y)
		{

		}

		public override string ToString()
		{
			return $"{ base.ToString() }, { Z.ToString("F4", CultureInfo.InvariantCulture) }";
		}
	}

	public class v4 : v3
	{
		public double W = 0.0;

		public v4()
		{

		}

		public v4(v4 v) : this(v.X, v.Y, v.Z, v.Z)
		{

		}

		public v4(double x, double y, double z, double w) : base(x, y, z)
		{
			W = w;
		}

		public v4(double d) : this(d, d, d, d)
		{

		}

		public v4(v2 v) : base(v)
		{

		}

		public v4(v3 v) : base(v.X, v.Y, v.Z)
		{

		}

		public override string ToString()
		{
			return $"{ base.ToString() }, { W.ToString("F4", CultureInfo.InvariantCulture) }";
		}
	}
}
