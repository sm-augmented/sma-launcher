using Steamworks;
using System;
using System.IO;
using System.Linq;

namespace SMClient.Managers
{
    public class ProfileInfoManager
    {
        public static bool CheckVanillaPlayed()
        {
            string pchBuffer;
            SteamUser.GetUserDataFolder(out pchBuffer, 10000);
            if (Path.GetFileName(pchBuffer) == "local")
                pchBuffer = Path.Combine(Path.GetDirectoryName(pchBuffer), "remote");
            return SteamRemoteStorage.FileExists("profile_info.bin") || File.Exists(Path.Combine(pchBuffer, "profile_info.bin"));
        }

        private static bool CheckProfile(string name)
        {
            string pchBuffer;
            SteamUser.GetUserDataFolder(out pchBuffer, 10000);
            if (Path.GetFileName(pchBuffer) == "local")
                pchBuffer = Path.Combine(Path.GetDirectoryName(pchBuffer), "remote");

            return SteamRemoteStorage.FileExists(name) || File.Exists(Path.Combine(pchBuffer, name));
        }

        private static bool CheckAndCreateProfile(string from, string to)
        {
            var exists = true;

            string pchBuffer;
            SteamUser.GetUserDataFolder(out pchBuffer, 10000);
            if (Path.GetFileName(pchBuffer) == "local")
                pchBuffer = Path.Combine(Path.GetDirectoryName(pchBuffer), "remote");

            if (!SteamRemoteStorage.FileExists(to) && !File.Exists(Path.Combine(pchBuffer, to)))
            {
                exists = false;

                int fileSize1 = SteamRemoteStorage.GetFileSize(from);
                byte[] pvData = new byte[fileSize1];
                SteamRemoteStorage.FileRead(from, pvData, fileSize1);
                SteamRemoteStorage.FileWrite(to, pvData, fileSize1);
            }
            else
            {
                int fileSize1 = SteamRemoteStorage.GetFileSize(from);
                byte[] pvData = new byte[fileSize1];
                SteamRemoteStorage.FileRead(from, pvData, fileSize1);

                int fileSize2 = SteamRemoteStorage.GetFileSize(to);
                byte[] pvData2 = new byte[fileSize2];
                SteamRemoteStorage.FileRead(to, pvData2, fileSize2);

                exists = !pvData.SequenceEqual(pvData2);
            }

            return exists;
        }

        public static bool RecreateModProfile(string branch)
        {
            var from = "";
            var to = "";

            if (branch == "Versus")
            {
                from = "profile_info.bin";
                to = "profile_Vers.bin";
            }
            else if (branch == "Exterminatus")
            {
                from = "profile_info.bin";
                to = "profile_Exte.bin";
            }
            else if (branch == "Versus-test")
            {
                var hasNormal = CheckProfile("profile_Vers.bin");
                from = (hasNormal) ? "profile_Vers.bin" : "profile_info.bin";
                to = "profile_Vert.bin";
            }
            else if (branch == "Exterminatus-test")
            {
                var hasNormal = CheckProfile("profile_Exte.bin");
                from = (hasNormal) ? "profile_Exte.bin" : "profile_info.bin";
                to = "profile_Extt.bin";
            }

            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                throw new Exception("Failed to determine branch");
            }

            return CheckAndCreateProfile(from, to);
        }
    }
}
