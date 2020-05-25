using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Ujeby.Common.Tools
{
	public class Utils
	{
		public static string Serialize<T>(T obj)
		{
			var serializer = new DataContractSerializer(obj.GetType());
			using (var writer = new StringWriter())
				using (var stm = new XmlTextWriter(writer))
				{
					serializer.WriteObject(stm, obj);
					return writer.ToString();
				}
		}

		public static T Deserialize<T>(string serialized)
		{
			var serializer = new DataContractSerializer(typeof(T));
			using (var reader = new StringReader(serialized))
				using (var stm = new XmlTextReader(reader))
					return (T)serializer.ReadObject(stm);
		}

		public static void CopyTo(Stream source, Stream destination)
		{
			var bytes = new byte[4096];

			int cnt;
			while ((cnt = source.Read(bytes, 0, bytes.Length)) != 0)
				destination.Write(bytes, 0, cnt);
		}

		public static byte[] Compress(string stringToCompress)
		{
			var bytes = Encoding.UTF8.GetBytes(stringToCompress);

			using (var msi = new MemoryStream(bytes))
			using (var mso = new MemoryStream())
			{
				using (var gs = new GZipStream(mso, CompressionMode.Compress))
					CopyTo(msi, gs);

				return mso.ToArray();
			}
		}

		public static string Decompress(byte[] bytesToDecompress)
		{
			using (var msi = new MemoryStream(bytesToDecompress))
			using (var mso = new MemoryStream())
			{
				using (var gs = new GZipStream(msi, CompressionMode.Decompress))
					CopyTo(gs, mso);

				return Encoding.UTF8.GetString(mso.ToArray());
			}
		}

		public static string GetFormattedXml(string xmlInput, string indent = "\t")
		{
			var xml = new XmlDocument();

			using (var ms = new MemoryStream())
			{
				var writerSettings = new XmlWriterSettings()
				{
					Indent = indent != null,
					IndentChars = indent,
					Encoding = System.Text.Encoding.UTF8,
					ConformanceLevel = ConformanceLevel.Document,
					OmitXmlDeclaration = true,
					//NewLineOnAttributes = true,
					NamespaceHandling = NamespaceHandling.OmitDuplicates,
				};

				using (var xmlWriter = XmlWriter.Create(ms, writerSettings))
				{
					xml.LoadXml(xmlInput);
					xml.WriteTo(xmlWriter);
				}

				ms.Position = 0;

				using (var sr = new StreamReader(ms))
					return sr.ReadToEnd();
			}
		}

		public static string GetCurrentMethodName([CallerMemberName] string caller = null)
		{
			return caller;
		}

		public static string GetTargetDirFromShortcut(string shortcut)
		{
			var shell = new IWshRuntimeLibrary.WshShell();
			var lnk = shell.CreateShortcut(shortcut) as IWshRuntimeLibrary.IWshShortcut;
			if (lnk != null)
				return lnk.WorkingDirectory;

			return string.Empty;
		}
	}
}
