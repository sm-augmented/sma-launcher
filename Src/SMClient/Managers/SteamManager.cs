using Steamworks;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMClient.Managers
{
    public class SteamManager
    {
        protected static SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

        public static bool Initialized { get; protected set; }

        protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText) => Logger.LogWarning(pchDebugText.ToString());

        protected static CancellationTokenSource callbackToken;

        public static bool Initialize()
        {
            try
            {
                if (!Packsize.Test())
                    throw new Exception("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");

                if (!DllCheck.Test())
                    throw new Exception("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");

                Initialized = SteamAPI.Init();

                if (!Initialized)
                {
                    throw new Exception("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.");
                }

                if (m_SteamAPIWarningMessageHook == null)
                {
                    m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
                    SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
                }

                RunCallbacks();
            }
            catch (Exception ex)
            {
                Logger.LogError(new Exception("Steam initialization exception", ex));
            }

            return Initialized;
        }

        private static void RunCallbacks()
        {
            callbackToken = new CancellationTokenSource();

            Task.Run(() =>
            {
                while (!callbackToken.IsCancellationRequested)
                {
                    SteamAPI.RunCallbacks();
                    Thread.Sleep(TimeSpan.FromSeconds(1.0));
                }
            });
        }

        public static void Shutdown()
        {
            if (!Initialized)
                return;

            callbackToken?.Cancel();
            SteamAPI.Shutdown();
        }
    }
}
