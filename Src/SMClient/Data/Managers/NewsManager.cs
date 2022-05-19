// Decompiled with JetBrains decompiler
// Type: SMClient.Data.Managers.NewsManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Dropbox.Api.Files;
using Dropbox.Api.Stone;
using Newtonsoft.Json;
using SMClient.Api;
using SMClient.Data.Dropbox;
using SMClient.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMClient.Data.Managers
{
    public static class NewsManager
    {
        public static async Task<string> GetChangelog()
        {
            IDownloadResponse<FileMetadata> fileMeta = await BaseApi.GetFileMetadata("/CHANGELOG.txt");
            DownloadedFile downloadedFile = await BaseApi.DownloadFile(fileMeta);
            var changelog = Encoding.UTF8.GetString(downloadedFile.File);

            return changelog;
        }

        public static async Task<List<News>> GetNewsAsync()
        {
            List<News> newsList = new List<News>();
            try
            {
                foreach (Metadata metadata in (await BaseApi.GetFolderContent("/News", new uint?(50U))).Where<Metadata>((Func<Metadata, bool>)(x => x.IsFile)))
                {
                    try
                    {
                        News news = JsonConvert.DeserializeObject<News>(Encoding.UTF8.GetString((await BaseApi.DownloadFile(await BaseApi.GetFileMetadata(metadata.PathLower))).File));
                        newsList.Add(news);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            newsList = newsList.OrderByDescending<News, DateTime>((Func<News, DateTime>)(x => DateTime.ParseExact(x.Date, "dd.MM.yyyy", (IFormatProvider)CultureInfo.InvariantCulture))).ToList<News>();
            List<News> newsAsync = newsList;
            newsList = (List<News>)null;
            return newsAsync;
        }
    }
}
