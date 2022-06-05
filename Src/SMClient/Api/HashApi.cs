using Dropbox.Api.Files;
using Dropbox.Api.Stone;
using SMClient.Models;
using SMClient.Utils;
using System.Threading.Tasks;

namespace SMClient.Api
{
    public class HashApi
    {
        public static bool CheckUpdaterHash(string serverHash) => HashApi.CheckHash(serverHash, "SMClientUpdater.exe");

        public static async Task<bool> CheckDataHash(Package branch) => await HashApi.CheckHashByFile("/" + Settings.Instance.GetArchiveNameByBranch(branch), Settings.Instance.GetArchivePathByBranch(branch));

        public static bool CheckDataHash(Package branch, string serverHash) => HashApi.CheckHash(serverHash, Settings.Instance.GetArchivePathByBranch(branch));

        public static async Task<bool> CheckHashByFile(string fileName, string path)
        {
            if (!System.IO.File.Exists(path))
                return false;
            string hash = new DropboxContentHasher().CalculateHash(path);
            IDownloadResponse<FileMetadata> fileMetadata = await BaseApi.GetFileMetadata(fileName);
            string contentHash = fileMetadata.Response.ContentHash;
            fileMetadata.Dispose();
            return hash.CompareTo(contentHash) == 0;
        }

        private static bool CheckHash(string serverHash, string path) => System.IO.File.Exists(path) && new DropboxContentHasher().CalculateHash(path).CompareTo(serverHash) == 0;
    }
}
