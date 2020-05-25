using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ujeby.Common.Tools
{
	public class Hashing
	{
		public static string GetHash(string input)
		{
			if (input == null)
				return null;

			var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
			return string.Concat(hash.Select(b => b.ToString("x2")));
		}
	}
}
