// Decompiled with JetBrains decompiler
// Type: SMClient.Api.MetadataApi
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Dropbox.Api.Files;
using Dropbox.Api.Stone;
using Newtonsoft.Json;
using SMClient.Data.Dropbox;
using SMClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SMClient.Api
{
    public class MetadataApi
    {
        public static async Task<string> GetBaseMetadata() => await BaseApi.GetStringAsync("changelog.txt");

        public static async Task<List<Package>> LoadMetadata()
        {
            var packages = new List<Package>();

            try
            {
                var fileInfo = await BaseApi.GetFileMetadata("/branchMetadata.json");
                var file = await BaseApi.DownloadFile(fileInfo);
                var metadata = Encoding.UTF8.GetString(file.File);
                packages = string.IsNullOrEmpty(metadata) ? null : JsonConvert.DeserializeObject<List<Package>>(metadata);
            }
            catch (Exception ex)
            {

            }
            

            return packages;
        }
    }
}
