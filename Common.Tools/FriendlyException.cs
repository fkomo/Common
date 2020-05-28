using System;

namespace Ujeby.Common.Tools
{
	public class FriendlyException : Exception
	{
		public FriendlyException(string message) : base(message)
		{

		}
	}
}
