// Decompiled with JetBrains decompiler
// Type: Discord.Discord
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe
/*
using System;
using System.Runtime.InteropServices;

namespace Discord
{
  public class Discord : IDisposable
  {
    private IntPtr EventsPtr;
    private Discord.Discord.FFIEvents Events;
    private IntPtr ApplicationEventsPtr;
    private ApplicationManager.FFIEvents ApplicationEvents;
    private ApplicationManager ApplicationManagerInstance;
    private IntPtr UserEventsPtr;
    private UserManager.FFIEvents UserEvents;
    private UserManager UserManagerInstance;
    private IntPtr ImageEventsPtr;
    private ImageManager.FFIEvents ImageEvents;
    private ImageManager ImageManagerInstance;
    private IntPtr ActivityEventsPtr;
    private ActivityManager.FFIEvents ActivityEvents;
    private ActivityManager ActivityManagerInstance;
    private IntPtr RelationshipEventsPtr;
    private RelationshipManager.FFIEvents RelationshipEvents;
    private RelationshipManager RelationshipManagerInstance;
    private IntPtr LobbyEventsPtr;
    private LobbyManager.FFIEvents LobbyEvents;
    private LobbyManager LobbyManagerInstance;
    private IntPtr NetworkEventsPtr;
    private NetworkManager.FFIEvents NetworkEvents;
    private NetworkManager NetworkManagerInstance;
    private IntPtr OverlayEventsPtr;
    private OverlayManager.FFIEvents OverlayEvents;
    private OverlayManager OverlayManagerInstance;
    private IntPtr StorageEventsPtr;
    private StorageManager.FFIEvents StorageEvents;
    private StorageManager StorageManagerInstance;
    private IntPtr StoreEventsPtr;
    private StoreManager.FFIEvents StoreEvents;
    private StoreManager StoreManagerInstance;
    private IntPtr VoiceEventsPtr;
    private VoiceManager.FFIEvents VoiceEvents;
    private VoiceManager VoiceManagerInstance;
    private IntPtr AchievementEventsPtr;
    private AchievementManager.FFIEvents AchievementEvents;
    private AchievementManager AchievementManagerInstance;
    private IntPtr MethodsPtr;
    private object MethodsStructure;
    private Discord.Discord.FFIMethods.SetLogHookCallback setLogHook;

    [DllImport("discord_game_sdk", CallingConvention = CallingConvention.Cdecl)]
    private static extern Result DiscordCreate(
      uint version,
      ref Discord.Discord.FFICreateParams createParams,
      out IntPtr manager);

    private Discord.Discord.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (Discord.Discord.FFIMethods));
        return (Discord.Discord.FFIMethods) this.MethodsStructure;
      }
    }

    public Discord(long clientId, ulong flags)
    {
      Discord.Discord.FFICreateParams createParams;
      createParams.ClientId = clientId;
      createParams.Flags = flags;
      this.Events = new Discord.Discord.FFIEvents();
      this.EventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<Discord.Discord.FFIEvents>(this.Events));
      createParams.Events = this.EventsPtr;
      createParams.EventData = (IntPtr) 0;
      this.ApplicationEvents = new ApplicationManager.FFIEvents();
      this.ApplicationEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<ApplicationManager.FFIEvents>(this.ApplicationEvents));
      createParams.ApplicationEvents = this.ApplicationEventsPtr;
      createParams.ApplicationVersion = 1U;
      this.UserEvents = new UserManager.FFIEvents();
      this.UserEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<UserManager.FFIEvents>(this.UserEvents));
      createParams.UserEvents = this.UserEventsPtr;
      createParams.UserVersion = 1U;
      this.ImageEvents = new ImageManager.FFIEvents();
      this.ImageEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<ImageManager.FFIEvents>(this.ImageEvents));
      createParams.ImageEvents = this.ImageEventsPtr;
      createParams.ImageVersion = 1U;
      this.ActivityEvents = new ActivityManager.FFIEvents();
      this.ActivityEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<ActivityManager.FFIEvents>(this.ActivityEvents));
      createParams.ActivityEvents = this.ActivityEventsPtr;
      createParams.ActivityVersion = 1U;
      this.RelationshipEvents = new RelationshipManager.FFIEvents();
      this.RelationshipEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<RelationshipManager.FFIEvents>(this.RelationshipEvents));
      createParams.RelationshipEvents = this.RelationshipEventsPtr;
      createParams.RelationshipVersion = 1U;
      this.LobbyEvents = new LobbyManager.FFIEvents();
      this.LobbyEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<LobbyManager.FFIEvents>(this.LobbyEvents));
      createParams.LobbyEvents = this.LobbyEventsPtr;
      createParams.LobbyVersion = 1U;
      this.NetworkEvents = new NetworkManager.FFIEvents();
      this.NetworkEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<NetworkManager.FFIEvents>(this.NetworkEvents));
      createParams.NetworkEvents = this.NetworkEventsPtr;
      createParams.NetworkVersion = 1U;
      this.OverlayEvents = new OverlayManager.FFIEvents();
      this.OverlayEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<OverlayManager.FFIEvents>(this.OverlayEvents));
      createParams.OverlayEvents = this.OverlayEventsPtr;
      createParams.OverlayVersion = 1U;
      this.StorageEvents = new StorageManager.FFIEvents();
      this.StorageEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<StorageManager.FFIEvents>(this.StorageEvents));
      createParams.StorageEvents = this.StorageEventsPtr;
      createParams.StorageVersion = 1U;
      this.StoreEvents = new StoreManager.FFIEvents();
      this.StoreEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<StoreManager.FFIEvents>(this.StoreEvents));
      createParams.StoreEvents = this.StoreEventsPtr;
      createParams.StoreVersion = 1U;
      this.VoiceEvents = new VoiceManager.FFIEvents();
      this.VoiceEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<VoiceManager.FFIEvents>(this.VoiceEvents));
      createParams.VoiceEvents = this.VoiceEventsPtr;
      createParams.VoiceVersion = 1U;
      this.AchievementEvents = new AchievementManager.FFIEvents();
      this.AchievementEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<AchievementManager.FFIEvents>(this.AchievementEvents));
      createParams.AchievementEvents = this.AchievementEventsPtr;
      createParams.AchievementVersion = 1U;
      this.InitEvents(this.EventsPtr, ref this.Events);
      Result result = Discord.Discord.DiscordCreate(2U, ref createParams, out this.MethodsPtr);
      if (result != Result.Ok)
      {
        this.Dispose();
        throw new ResultException(result);
      }
    }

    private void InitEvents(IntPtr eventsPtr, ref Discord.Discord.FFIEvents events) => Marshal.StructureToPtr<Discord.Discord.FFIEvents>(events, eventsPtr, false);

    public void Dispose()
    {
      if (this.MethodsPtr != IntPtr.Zero)
        this.Methods.Destroy(this.MethodsPtr);
      Marshal.FreeHGlobal(this.EventsPtr);
      Marshal.FreeHGlobal(this.ApplicationEventsPtr);
      Marshal.FreeHGlobal(this.UserEventsPtr);
      Marshal.FreeHGlobal(this.ImageEventsPtr);
      Marshal.FreeHGlobal(this.ActivityEventsPtr);
      Marshal.FreeHGlobal(this.RelationshipEventsPtr);
      Marshal.FreeHGlobal(this.LobbyEventsPtr);
      Marshal.FreeHGlobal(this.NetworkEventsPtr);
      Marshal.FreeHGlobal(this.OverlayEventsPtr);
      Marshal.FreeHGlobal(this.StorageEventsPtr);
      Marshal.FreeHGlobal(this.StoreEventsPtr);
      Marshal.FreeHGlobal(this.VoiceEventsPtr);
      Marshal.FreeHGlobal(this.AchievementEventsPtr);
    }

    public void RunCallbacks()
    {
      Result result = this.Methods.RunCallbacks(this.MethodsPtr);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void SetLogHook(LogLevel minLevel, Discord.Discord.SetLogHookHandler callback)
    {
      this.setLogHook = (Discord.Discord.FFIMethods.SetLogHookCallback) ((ptr, level, message) => callback(level, message));
      this.Methods.SetLogHook(this.MethodsPtr, minLevel, IntPtr.Zero, this.setLogHook);
    }

    public ApplicationManager GetApplicationManager()
    {
      if (this.ApplicationManagerInstance == null)
        this.ApplicationManagerInstance = new ApplicationManager(this.Methods.GetApplicationManager(this.MethodsPtr), this.ApplicationEventsPtr, ref this.ApplicationEvents);
      return this.ApplicationManagerInstance;
    }

    public UserManager GetUserManager()
    {
      if (this.UserManagerInstance == null)
        this.UserManagerInstance = new UserManager(this.Methods.GetUserManager(this.MethodsPtr), this.UserEventsPtr, ref this.UserEvents);
      return this.UserManagerInstance;
    }

    public ImageManager GetImageManager()
    {
      if (this.ImageManagerInstance == null)
        this.ImageManagerInstance = new ImageManager(this.Methods.GetImageManager(this.MethodsPtr), this.ImageEventsPtr, ref this.ImageEvents);
      return this.ImageManagerInstance;
    }

    public ActivityManager GetActivityManager()
    {
      if (this.ActivityManagerInstance == null)
        this.ActivityManagerInstance = new ActivityManager(this.Methods.GetActivityManager(this.MethodsPtr), this.ActivityEventsPtr, ref this.ActivityEvents);
      return this.ActivityManagerInstance;
    }

    public RelationshipManager GetRelationshipManager()
    {
      if (this.RelationshipManagerInstance == null)
        this.RelationshipManagerInstance = new RelationshipManager(this.Methods.GetRelationshipManager(this.MethodsPtr), this.RelationshipEventsPtr, ref this.RelationshipEvents);
      return this.RelationshipManagerInstance;
    }

    public LobbyManager GetLobbyManager()
    {
      if (this.LobbyManagerInstance == null)
        this.LobbyManagerInstance = new LobbyManager(this.Methods.GetLobbyManager(this.MethodsPtr), this.LobbyEventsPtr, ref this.LobbyEvents);
      return this.LobbyManagerInstance;
    }

    public NetworkManager GetNetworkManager()
    {
      if (this.NetworkManagerInstance == null)
        this.NetworkManagerInstance = new NetworkManager(this.Methods.GetNetworkManager(this.MethodsPtr), this.NetworkEventsPtr, ref this.NetworkEvents);
      return this.NetworkManagerInstance;
    }

    public OverlayManager GetOverlayManager()
    {
      if (this.OverlayManagerInstance == null)
        this.OverlayManagerInstance = new OverlayManager(this.Methods.GetOverlayManager(this.MethodsPtr), this.OverlayEventsPtr, ref this.OverlayEvents);
      return this.OverlayManagerInstance;
    }

    public StorageManager GetStorageManager()
    {
      if (this.StorageManagerInstance == null)
        this.StorageManagerInstance = new StorageManager(this.Methods.GetStorageManager(this.MethodsPtr), this.StorageEventsPtr, ref this.StorageEvents);
      return this.StorageManagerInstance;
    }

    public StoreManager GetStoreManager()
    {
      if (this.StoreManagerInstance == null)
        this.StoreManagerInstance = new StoreManager(this.Methods.GetStoreManager(this.MethodsPtr), this.StoreEventsPtr, ref this.StoreEvents);
      return this.StoreManagerInstance;
    }

    public VoiceManager GetVoiceManager()
    {
      if (this.VoiceManagerInstance == null)
        this.VoiceManagerInstance = new VoiceManager(this.Methods.GetVoiceManager(this.MethodsPtr), this.VoiceEventsPtr, ref this.VoiceEvents);
      return this.VoiceManagerInstance;
    }

    public AchievementManager GetAchievementManager()
    {
      if (this.AchievementManagerInstance == null)
        this.AchievementManagerInstance = new AchievementManager(this.Methods.GetAchievementManager(this.MethodsPtr), this.AchievementEventsPtr, ref this.AchievementEvents);
      return this.AchievementManagerInstance;
    }

    internal struct FFIEvents
    {
    }

    internal struct FFIMethods
    {
      internal Discord.Discord.FFIMethods.DestroyHandler Destroy;
      internal Discord.Discord.FFIMethods.RunCallbacksMethod RunCallbacks;
      internal Discord.Discord.FFIMethods.SetLogHookMethod SetLogHook;
      internal Discord.Discord.FFIMethods.GetApplicationManagerMethod GetApplicationManager;
      internal Discord.Discord.FFIMethods.GetUserManagerMethod GetUserManager;
      internal Discord.Discord.FFIMethods.GetImageManagerMethod GetImageManager;
      internal Discord.Discord.FFIMethods.GetActivityManagerMethod GetActivityManager;
      internal Discord.Discord.FFIMethods.GetRelationshipManagerMethod GetRelationshipManager;
      internal Discord.Discord.FFIMethods.GetLobbyManagerMethod GetLobbyManager;
      internal Discord.Discord.FFIMethods.GetNetworkManagerMethod GetNetworkManager;
      internal Discord.Discord.FFIMethods.GetOverlayManagerMethod GetOverlayManager;
      internal Discord.Discord.FFIMethods.GetStorageManagerMethod GetStorageManager;
      internal Discord.Discord.FFIMethods.GetStoreManagerMethod GetStoreManager;
      internal Discord.Discord.FFIMethods.GetVoiceManagerMethod GetVoiceManager;
      internal Discord.Discord.FFIMethods.GetAchievementManagerMethod GetAchievementManager;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void DestroyHandler(IntPtr MethodsPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result RunCallbacksMethod(IntPtr methodsPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SetLogHookCallback(IntPtr ptr, LogLevel level, [MarshalAs(UnmanagedType.LPStr)] string message);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SetLogHookMethod(
        IntPtr methodsPtr,
        LogLevel minLevel,
        IntPtr callbackData,
        Discord.Discord.FFIMethods.SetLogHookCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate IntPtr GetApplicationManagerMethod(IntPtr discordPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate IntPtr GetUserManagerMethod(IntPtr discordPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate IntPtr GetImageManagerMethod(IntPtr discordPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate IntPtr GetActivityManagerMethod(IntPtr discordPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate IntPtr GetRelationshipManagerMethod(IntPtr discordPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate IntPtr GetLobbyManagerMethod(IntPtr discordPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate IntPtr GetNetworkManagerMethod(IntPtr discordPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate IntPtr GetOverlayManagerMethod(IntPtr discordPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate IntPtr GetStorageManagerMethod(IntPtr discordPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate IntPtr GetStoreManagerMethod(IntPtr discordPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate IntPtr GetVoiceManagerMethod(IntPtr discordPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate IntPtr GetAchievementManagerMethod(IntPtr discordPtr);
    }

    internal struct FFICreateParams
    {
      internal long ClientId;
      internal ulong Flags;
      internal IntPtr Events;
      internal IntPtr EventData;
      internal IntPtr ApplicationEvents;
      internal uint ApplicationVersion;
      internal IntPtr UserEvents;
      internal uint UserVersion;
      internal IntPtr ImageEvents;
      internal uint ImageVersion;
      internal IntPtr ActivityEvents;
      internal uint ActivityVersion;
      internal IntPtr RelationshipEvents;
      internal uint RelationshipVersion;
      internal IntPtr LobbyEvents;
      internal uint LobbyVersion;
      internal IntPtr NetworkEvents;
      internal uint NetworkVersion;
      internal IntPtr OverlayEvents;
      internal uint OverlayVersion;
      internal IntPtr StorageEvents;
      internal uint StorageVersion;
      internal IntPtr StoreEvents;
      internal uint StoreVersion;
      internal IntPtr VoiceEvents;
      internal uint VoiceVersion;
      internal IntPtr AchievementEvents;
      internal uint AchievementVersion;
    }

    public delegate void SetLogHookHandler(LogLevel level, string message);
  }
}
*/