using SMClient;
using SMClient.Api;
using SMClient.Models;
using Steamworks;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMClient.Managers
{
    public class AuthManager
    {
        public static User Account { get; set; }

        public static string GetUsername(bool useSteam)
        {
            var username = Account?.Login;
            if (useSteam && SteamManager.Initialized)
                username = SteamFriends.GetPersonaName();
            return username;
        }

        private static async Task<bool> CheckAccess(CSteamID steamid)
        {
            var initialized = true;
            try
            {
                var listMeta = await BaseApi.GetFileMetadata("/GitGid.txt");
                var listFile = await BaseApi.DownloadFile(listMeta);
                var list = Encoding.UTF8.GetString(listFile.File);

                if (!string.IsNullOrEmpty(list))
                {
                    var csteamIdList = list
                        .Replace("\r\n", "\n")
                        .Split('\n')
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(x => new CSteamID(Convert.ToUInt64(x)));

                    initialized = !csteamIdList.Contains(steamid);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(new Exception("Unable to reach file server", ex));
            }

            return initialized;
        }

        public static async Task<bool> Authenticate()
        {
            var role = "user";
            try
            {
                var steamid = SteamUser.GetSteamID();
                var hasAccess = await CheckAccess(steamid);
                if (hasAccess)
                {
                    try
                    {
                        var listMeta = await BaseApi.GetFileMetadata("/Testers.txt");
                        var listFile = await BaseApi.DownloadFile(listMeta);
                        var list = Encoding.UTF8.GetString(listFile.File);

                        if (!string.IsNullOrEmpty(list))
                        {
                            var csteamIdList = list
                                .Replace("\r\n", "\n")
                                .Split('\n')
                                .Where(x => !string.IsNullOrEmpty(x))
                                .Select(x => new CSteamID(Convert.ToUInt64(x)));

                            role = csteamIdList.Contains(SteamUser.GetSteamID()) ? "tester" : "user";
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(new Exception("Unable to reach file server", ex));
                    }

                    Account = new User()
                    {
                        Login = steamid.ToString(),
                        SteamID = steamid.ToString(),
                        Role = role
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(new Exception("Authentication exception", ex));
            }

            return Account != null;
        }
    }
}
