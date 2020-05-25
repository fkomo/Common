using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ujeby.Common.Tools
{
	public class TimedBlock : IDisposable
	{
		private string BlockName { get; set; }
		private Stopwatch Stopwatch { get; set; }

		public TimedBlock(string blockName)
		{
			BlockName = blockName;

			this.Stopwatch = new Stopwatch();
			this.Stopwatch.Start();
		}

		public void Dispose()
		{
			this.Stopwatch.Stop();
			Log.WriteLine($"{ BlockName } in { Stopwatch.ElapsedMilliseconds }ms");

			this.Stopwatch = null;
		}
	}
}
