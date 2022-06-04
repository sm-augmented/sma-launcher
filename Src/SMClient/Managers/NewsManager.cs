using Dropbox.Api.Files;
using Dropbox.Api.Stone;
using SMClient.Api;
using SMClient.Models.Dropbox;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SMClient.Managers
{
    public static class NewsManager
    {
        public static async Task<string> GetChangelog()
        {
            try
            {
                IDownloadResponse<FileMetadata> fileMeta = await BaseApi.GetFileMetadata("/CHANGELOG.txt");
                DownloadedFile downloadedFile = await BaseApi.DownloadFile(fileMeta);
                return Encoding.UTF8.GetString(downloadedFile.File);
            }
            catch (Exception ex)
            {
                Logger.LogError(new Exception("Get changelog exception", ex));
                throw ex;
            }
        }
    }
}
