using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Stone;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace SMClientUpdater
{
	internal class Program
	{
		private static bool isDev;

		private static void Main(string[] args)
		{
			foreach (string item in Directory.EnumerateFiles("."))
			{
				if (!(Path.GetExtension(item) == ".pdb") || !(Path.GetFileNameWithoutExtension(item) == "SMClient"))
				{
					continue;
				}
				int num = 0;
				while (num < 3)
				{
					try
					{
						File.Delete(item);
					}
					catch
					{
						num++;
						TimeSpan.FromSeconds(1.0);
						continue;
					}
					break;
				}
			}
			if (args.Length != 0 && args[0] == "-d")
			{
				isDev = true;
			}
			Console.WriteLine("Loading update...");
			Process[] processesByName = Process.GetProcessesByName("SMClient");
			for (int i = 0; i < processesByName.Length; i++)
			{
				processesByName[i].Kill();
			}
			DownloadFileAsync().Wait();
			Process.Start("SMClient.exe");
		}

		private static async Task DownloadFileAsync()
		{
			HttpClient httpClient = new HttpClient(new WebRequestHandler
			{
				ReadWriteTimeout = 1000000
			})
			{
				Timeout = TimeSpan.FromHours(20.0)
			};
			DropboxClientConfig config = new DropboxClientConfig
			{
				HttpClient = httpClient
			};
			IDownloadResponse<FileMetadata> downloadResponse = await new DropboxClient("secret", config).Files.DownloadAsync("/SMClient.zip");
			_ = downloadResponse.Response.Size;
			byte[] buffer2 = new byte[1048576];
			try
			{
				using Stream stream = await downloadResponse.GetContentAsStreamAsync();
				using MemoryStream memoryStream = new MemoryStream();
				for (int num = stream.Read(buffer2, 0, 1048576); num > 0; num = stream.Read(buffer2, 0, 1048576))
				{
					memoryStream.Write(buffer2, 0, num);
				}
				ZipFile zipFile = new ZipFile(memoryStream);
				foreach (ZipEntry item in zipFile)
				{
					if (!item.IsFile)
					{
						continue;
					}
					string name = item.Name;
					buffer2 = new byte[4096];
					Stream inputStream = zipFile.GetInputStream(item);
					string path = Path.Combine(Directory.GetCurrentDirectory(), name);
					string directoryName = Path.GetDirectoryName(path);
					if (directoryName.Length > 0)
					{
						Directory.CreateDirectory(directoryName);
					}
					try
					{
						using FileStream destination = File.Create(path);
						StreamUtils.Copy(inputStream, destination, buffer2);
					}
					catch
					{
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				if (ex.InnerException != null)
				{
					Console.WriteLine("INNER: " + ex.InnerException.Message);
					Console.WriteLine("INNER STACK: " + ex.InnerException.StackTrace);
				}
				Console.WriteLine("STACK:" + ex.StackTrace);
				throw ex;
			}
		}
	}
}
