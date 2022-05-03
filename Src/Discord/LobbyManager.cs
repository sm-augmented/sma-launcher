// Decompiled with JetBrains decompiler
// Type: Discord.LobbyManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord
{
  public class LobbyManager
  {
    private IntPtr MethodsPtr;
    private object MethodsStructure;

    private LobbyManager.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (LobbyManager.FFIMethods));
        return (LobbyManager.FFIMethods) this.MethodsStructure;
      }
    }

    public event LobbyManager.LobbyUpdateHandler OnLobbyUpdate;

    public event LobbyManager.LobbyDeleteHandler OnLobbyDelete;

    public event LobbyManager.MemberConnectHandler OnMemberConnect;

    public event LobbyManager.MemberUpdateHandler OnMemberUpdate;

    public event LobbyManager.MemberDisconnectHandler OnMemberDisconnect;

    public event LobbyManager.LobbyMessageHandler OnLobbyMessage;

    public event LobbyManager.SpeakingHandler OnSpeaking;

    public event LobbyManager.NetworkMessageHandler OnNetworkMessage;

    internal LobbyManager(IntPtr ptr, IntPtr eventsPtr, ref LobbyManager.FFIEvents events)
    {
      if (eventsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
      this.InitEvents(eventsPtr, ref events);
      this.MethodsPtr = ptr;
      if (this.MethodsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
    }

    private void InitEvents(IntPtr eventsPtr, ref LobbyManager.FFIEvents events)
    {
      events.OnLobbyUpdate = (LobbyManager.FFIEvents.LobbyUpdateHandler) ((ptr, lobbyId) =>
      {
        if (this.OnLobbyUpdate == null)
          return;
        this.OnLobbyUpdate(lobbyId);
      });
      events.OnLobbyDelete = (LobbyManager.FFIEvents.LobbyDeleteHandler) ((ptr, lobbyId, reason) =>
      {
        if (this.OnLobbyDelete == null)
          return;
        this.OnLobbyDelete(lobbyId, reason);
      });
      events.OnMemberConnect = (LobbyManager.FFIEvents.MemberConnectHandler) ((ptr, lobbyId, userId) =>
      {
        if (this.OnMemberConnect == null)
          return;
        this.OnMemberConnect(lobbyId, userId);
      });
      events.OnMemberUpdate = (LobbyManager.FFIEvents.MemberUpdateHandler) ((ptr, lobbyId, userId) =>
      {
        if (this.OnMemberUpdate == null)
          return;
        this.OnMemberUpdate(lobbyId, userId);
      });
      events.OnMemberDisconnect = (LobbyManager.FFIEvents.MemberDisconnectHandler) ((ptr, lobbyId, userId) =>
      {
        if (this.OnMemberDisconnect == null)
          return;
        this.OnMemberDisconnect(lobbyId, userId);
      });
      events.OnLobbyMessage = (LobbyManager.FFIEvents.LobbyMessageHandler) ((ptr, lobbyId, userId, dataPtr, dataLen) =>
      {
        if (this.OnLobbyMessage == null)
          return;
        byte[] numArray = new byte[dataLen];
        Marshal.Copy(dataPtr, numArray, 0, dataLen);
        this.OnLobbyMessage(lobbyId, userId, numArray);
      });
      events.OnSpeaking = (LobbyManager.FFIEvents.SpeakingHandler) ((ptr, lobbyId, userId, speaking) =>
      {
        if (this.OnSpeaking == null)
          return;
        this.OnSpeaking(lobbyId, userId, speaking);
      });
      events.OnNetworkMessage = (LobbyManager.FFIEvents.NetworkMessageHandler) ((ptr, lobbyId, userId, channelId, dataPtr, dataLen) =>
      {
        if (this.OnNetworkMessage == null)
          return;
        byte[] numArray = new byte[dataLen];
        Marshal.Copy(dataPtr, numArray, 0, dataLen);
        this.OnNetworkMessage(lobbyId, userId, channelId, numArray);
      });
      Marshal.StructureToPtr<LobbyManager.FFIEvents>(events, eventsPtr, false);
    }

    public LobbyTransaction GetLobbyCreateTransaction()
    {
      LobbyTransaction createTransaction = new LobbyTransaction();
      Result result = this.Methods.GetLobbyCreateTransaction(this.MethodsPtr, ref createTransaction.MethodsPtr);
      if (result != Result.Ok)
        throw new ResultException(result);
      return createTransaction;
    }

    public LobbyTransaction GetLobbyUpdateTransaction(long lobbyId)
    {
      LobbyTransaction updateTransaction = new LobbyTransaction();
      Result result = this.Methods.GetLobbyUpdateTransaction(this.MethodsPtr, lobbyId, ref updateTransaction.MethodsPtr);
      if (result != Result.Ok)
        throw new ResultException(result);
      return updateTransaction;
    }

    public LobbyMemberTransaction GetMemberUpdateTransaction(
      long lobbyId,
      long userId)
    {
      LobbyMemberTransaction updateTransaction = new LobbyMemberTransaction();
      Result result = this.Methods.GetMemberUpdateTransaction(this.MethodsPtr, lobbyId, userId, ref updateTransaction.MethodsPtr);
      if (result != Result.Ok)
        throw new ResultException(result);
      return updateTransaction;
    }

    public void CreateLobby(LobbyTransaction transaction, LobbyManager.CreateLobbyHandler callback)
    {
      LobbyManager.FFIMethods.CreateLobbyCallback callback1 = (LobbyManager.FFIMethods.CreateLobbyCallback) ((IntPtr ptr, Result result, ref Lobby lobby) =>
      {
        Utility.Release(ptr);
        callback(result, ref lobby);
      });
      this.Methods.CreateLobby(this.MethodsPtr, transaction.MethodsPtr, Utility.Retain<LobbyManager.FFIMethods.CreateLobbyCallback>(callback1), callback1);
      transaction.MethodsPtr = IntPtr.Zero;
    }

    public void UpdateLobby(
      long lobbyId,
      LobbyTransaction transaction,
      LobbyManager.UpdateLobbyHandler callback)
    {
      LobbyManager.FFIMethods.UpdateLobbyCallback callback1 = (LobbyManager.FFIMethods.UpdateLobbyCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.UpdateLobby(this.MethodsPtr, lobbyId, transaction.MethodsPtr, Utility.Retain<LobbyManager.FFIMethods.UpdateLobbyCallback>(callback1), callback1);
      transaction.MethodsPtr = IntPtr.Zero;
    }

    public void DeleteLobby(long lobbyId, LobbyManager.DeleteLobbyHandler callback)
    {
      LobbyManager.FFIMethods.DeleteLobbyCallback callback1 = (LobbyManager.FFIMethods.DeleteLobbyCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.DeleteLobby(this.MethodsPtr, lobbyId, Utility.Retain<LobbyManager.FFIMethods.DeleteLobbyCallback>(callback1), callback1);
    }

    public void ConnectLobby(
      long lobbyId,
      string secret,
      LobbyManager.ConnectLobbyHandler callback)
    {
      LobbyManager.FFIMethods.ConnectLobbyCallback callback1 = (LobbyManager.FFIMethods.ConnectLobbyCallback) ((IntPtr ptr, Result result, ref Lobby lobby) =>
      {
        Utility.Release(ptr);
        callback(result, ref lobby);
      });
      this.Methods.ConnectLobby(this.MethodsPtr, lobbyId, secret, Utility.Retain<LobbyManager.FFIMethods.ConnectLobbyCallback>(callback1), callback1);
    }

    public void ConnectLobbyWithActivitySecret(
      string activitySecret,
      LobbyManager.ConnectLobbyWithActivitySecretHandler callback)
    {
      LobbyManager.FFIMethods.ConnectLobbyWithActivitySecretCallback callback1 = (LobbyManager.FFIMethods.ConnectLobbyWithActivitySecretCallback) ((IntPtr ptr, Result result, ref Lobby lobby) =>
      {
        Utility.Release(ptr);
        callback(result, ref lobby);
      });
      this.Methods.ConnectLobbyWithActivitySecret(this.MethodsPtr, activitySecret, Utility.Retain<LobbyManager.FFIMethods.ConnectLobbyWithActivitySecretCallback>(callback1), callback1);
    }

    public void DisconnectLobby(long lobbyId, LobbyManager.DisconnectLobbyHandler callback)
    {
      LobbyManager.FFIMethods.DisconnectLobbyCallback callback1 = (LobbyManager.FFIMethods.DisconnectLobbyCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.DisconnectLobby(this.MethodsPtr, lobbyId, Utility.Retain<LobbyManager.FFIMethods.DisconnectLobbyCallback>(callback1), callback1);
    }

    public Lobby GetLobby(long lobbyId)
    {
      Lobby lobby = new Lobby();
      Result result = this.Methods.GetLobby(this.MethodsPtr, lobbyId, ref lobby);
      if (result != Result.Ok)
        throw new ResultException(result);
      return lobby;
    }

    public string GetLobbyActivitySecret(long lobbyId)
    {
      StringBuilder secret = new StringBuilder(128);
      Result result = this.Methods.GetLobbyActivitySecret(this.MethodsPtr, lobbyId, secret);
      if (result != Result.Ok)
        throw new ResultException(result);
      return secret.ToString();
    }

    public string GetLobbyMetadataValue(long lobbyId, string key)
    {
      StringBuilder stringBuilder = new StringBuilder(4096);
      Result result = this.Methods.GetLobbyMetadataValue(this.MethodsPtr, lobbyId, key, stringBuilder);
      if (result != Result.Ok)
        throw new ResultException(result);
      return stringBuilder.ToString();
    }

    public string GetLobbyMetadataKey(long lobbyId, int index)
    {
      StringBuilder key = new StringBuilder(256);
      Result result = this.Methods.GetLobbyMetadataKey(this.MethodsPtr, lobbyId, index, key);
      if (result != Result.Ok)
        throw new ResultException(result);
      return key.ToString();
    }

    public int LobbyMetadataCount(long lobbyId)
    {
      int count = 0;
      Result result = this.Methods.LobbyMetadataCount(this.MethodsPtr, lobbyId, ref count);
      if (result != Result.Ok)
        throw new ResultException(result);
      return count;
    }

    public int MemberCount(long lobbyId)
    {
      int count = 0;
      Result result = this.Methods.MemberCount(this.MethodsPtr, lobbyId, ref count);
      if (result != Result.Ok)
        throw new ResultException(result);
      return count;
    }

    public long GetMemberUserId(long lobbyId, int index)
    {
      long userId = 0;
      Result result = this.Methods.GetMemberUserId(this.MethodsPtr, lobbyId, index, ref userId);
      if (result != Result.Ok)
        throw new ResultException(result);
      return userId;
    }

    public User GetMemberUser(long lobbyId, long userId)
    {
      User user = new User();
      Result result = this.Methods.GetMemberUser(this.MethodsPtr, lobbyId, userId, ref user);
      if (result != Result.Ok)
        throw new ResultException(result);
      return user;
    }

    public string GetMemberMetadataValue(long lobbyId, long userId, string key)
    {
      StringBuilder stringBuilder = new StringBuilder(4096);
      Result result = this.Methods.GetMemberMetadataValue(this.MethodsPtr, lobbyId, userId, key, stringBuilder);
      if (result != Result.Ok)
        throw new ResultException(result);
      return stringBuilder.ToString();
    }

    public string GetMemberMetadataKey(long lobbyId, long userId, int index)
    {
      StringBuilder key = new StringBuilder(256);
      Result result = this.Methods.GetMemberMetadataKey(this.MethodsPtr, lobbyId, userId, index, key);
      if (result != Result.Ok)
        throw new ResultException(result);
      return key.ToString();
    }

    public int MemberMetadataCount(long lobbyId, long userId)
    {
      int count = 0;
      Result result = this.Methods.MemberMetadataCount(this.MethodsPtr, lobbyId, userId, ref count);
      if (result != Result.Ok)
        throw new ResultException(result);
      return count;
    }

    public void UpdateMember(
      long lobbyId,
      long userId,
      LobbyMemberTransaction transaction,
      LobbyManager.UpdateMemberHandler callback)
    {
      LobbyManager.FFIMethods.UpdateMemberCallback callback1 = (LobbyManager.FFIMethods.UpdateMemberCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.UpdateMember(this.MethodsPtr, lobbyId, userId, transaction.MethodsPtr, Utility.Retain<LobbyManager.FFIMethods.UpdateMemberCallback>(callback1), callback1);
      transaction.MethodsPtr = IntPtr.Zero;
    }

    public void SendLobbyMessage(
      long lobbyId,
      byte[] data,
      LobbyManager.SendLobbyMessageHandler callback)
    {
      LobbyManager.FFIMethods.SendLobbyMessageCallback callback1 = (LobbyManager.FFIMethods.SendLobbyMessageCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.SendLobbyMessage(this.MethodsPtr, lobbyId, data, data.Length, Utility.Retain<LobbyManager.FFIMethods.SendLobbyMessageCallback>(callback1), callback1);
    }

    public LobbySearchQuery GetSearchQuery()
    {
      LobbySearchQuery searchQuery = new LobbySearchQuery();
      Result result = this.Methods.GetSearchQuery(this.MethodsPtr, ref searchQuery.MethodsPtr);
      if (result != Result.Ok)
        throw new ResultException(result);
      return searchQuery;
    }

    public void Search(LobbySearchQuery query, LobbyManager.SearchHandler callback)
    {
      LobbyManager.FFIMethods.SearchCallback callback1 = (LobbyManager.FFIMethods.SearchCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.Search(this.MethodsPtr, query.MethodsPtr, Utility.Retain<LobbyManager.FFIMethods.SearchCallback>(callback1), callback1);
      query.MethodsPtr = IntPtr.Zero;
    }

    public int LobbyCount()
    {
      int count = 0;
      this.Methods.LobbyCount(this.MethodsPtr, ref count);
      return count;
    }

    public long GetLobbyId(int index)
    {
      long lobbyId = 0;
      Result result = this.Methods.GetLobbyId(this.MethodsPtr, index, ref lobbyId);
      if (result != Result.Ok)
        throw new ResultException(result);
      return lobbyId;
    }

    public void ConnectVoice(long lobbyId, LobbyManager.ConnectVoiceHandler callback)
    {
      LobbyManager.FFIMethods.ConnectVoiceCallback callback1 = (LobbyManager.FFIMethods.ConnectVoiceCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.ConnectVoice(this.MethodsPtr, lobbyId, Utility.Retain<LobbyManager.FFIMethods.ConnectVoiceCallback>(callback1), callback1);
    }

    public void DisconnectVoice(long lobbyId, LobbyManager.DisconnectVoiceHandler callback)
    {
      LobbyManager.FFIMethods.DisconnectVoiceCallback callback1 = (LobbyManager.FFIMethods.DisconnectVoiceCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.DisconnectVoice(this.MethodsPtr, lobbyId, Utility.Retain<LobbyManager.FFIMethods.DisconnectVoiceCallback>(callback1), callback1);
    }

    public void ConnectNetwork(long lobbyId)
    {
      Result result = this.Methods.ConnectNetwork(this.MethodsPtr, lobbyId);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void DisconnectNetwork(long lobbyId)
    {
      Result result = this.Methods.DisconnectNetwork(this.MethodsPtr, lobbyId);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void FlushNetwork()
    {
      Result result = this.Methods.FlushNetwork(this.MethodsPtr);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void OpenNetworkChannel(long lobbyId, byte channelId, bool reliable)
    {
      Result result = this.Methods.OpenNetworkChannel(this.MethodsPtr, lobbyId, channelId, reliable);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void SendNetworkMessage(long lobbyId, long userId, byte channelId, byte[] data)
    {
      Result result = this.Methods.SendNetworkMessage(this.MethodsPtr, lobbyId, userId, channelId, data, data.Length);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public IEnumerable<User> GetMemberUsers(long lobbyID)
    {
      int num = this.MemberCount(lobbyID);
      List<User> memberUsers = new List<User>();
      for (int index = 0; index < num; ++index)
        memberUsers.Add(this.GetMemberUser(lobbyID, this.GetMemberUserId(lobbyID, index)));
      return (IEnumerable<User>) memberUsers;
    }

    public void SendLobbyMessage(
      long lobbyID,
      string data,
      LobbyManager.SendLobbyMessageHandler handler)
    {
      this.SendLobbyMessage(lobbyID, Encoding.UTF8.GetBytes(data), handler);
    }

    internal struct FFIEvents
    {
      internal LobbyManager.FFIEvents.LobbyUpdateHandler OnLobbyUpdate;
      internal LobbyManager.FFIEvents.LobbyDeleteHandler OnLobbyDelete;
      internal LobbyManager.FFIEvents.MemberConnectHandler OnMemberConnect;
      internal LobbyManager.FFIEvents.MemberUpdateHandler OnMemberUpdate;
      internal LobbyManager.FFIEvents.MemberDisconnectHandler OnMemberDisconnect;
      internal LobbyManager.FFIEvents.LobbyMessageHandler OnLobbyMessage;
      internal LobbyManager.FFIEvents.SpeakingHandler OnSpeaking;
      internal LobbyManager.FFIEvents.NetworkMessageHandler OnNetworkMessage;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void LobbyUpdateHandler(IntPtr ptr, long lobbyId);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void LobbyDeleteHandler(IntPtr ptr, long lobbyId, uint reason);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void MemberConnectHandler(IntPtr ptr, long lobbyId, long userId);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void MemberUpdateHandler(IntPtr ptr, long lobbyId, long userId);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void MemberDisconnectHandler(IntPtr ptr, long lobbyId, long userId);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void LobbyMessageHandler(
        IntPtr ptr,
        long lobbyId,
        long userId,
        IntPtr dataPtr,
        int dataLen);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SpeakingHandler(IntPtr ptr, long lobbyId, long userId, bool speaking);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void NetworkMessageHandler(
        IntPtr ptr,
        long lobbyId,
        long userId,
        byte channelId,
        IntPtr dataPtr,
        int dataLen);
    }

    internal struct FFIMethods
    {
      internal LobbyManager.FFIMethods.GetLobbyCreateTransactionMethod GetLobbyCreateTransaction;
      internal LobbyManager.FFIMethods.GetLobbyUpdateTransactionMethod GetLobbyUpdateTransaction;
      internal LobbyManager.FFIMethods.GetMemberUpdateTransactionMethod GetMemberUpdateTransaction;
      internal LobbyManager.FFIMethods.CreateLobbyMethod CreateLobby;
      internal LobbyManager.FFIMethods.UpdateLobbyMethod UpdateLobby;
      internal LobbyManager.FFIMethods.DeleteLobbyMethod DeleteLobby;
      internal LobbyManager.FFIMethods.ConnectLobbyMethod ConnectLobby;
      internal LobbyManager.FFIMethods.ConnectLobbyWithActivitySecretMethod ConnectLobbyWithActivitySecret;
      internal LobbyManager.FFIMethods.DisconnectLobbyMethod DisconnectLobby;
      internal LobbyManager.FFIMethods.GetLobbyMethod GetLobby;
      internal LobbyManager.FFIMethods.GetLobbyActivitySecretMethod GetLobbyActivitySecret;
      internal LobbyManager.FFIMethods.GetLobbyMetadataValueMethod GetLobbyMetadataValue;
      internal LobbyManager.FFIMethods.GetLobbyMetadataKeyMethod GetLobbyMetadataKey;
      internal LobbyManager.FFIMethods.LobbyMetadataCountMethod LobbyMetadataCount;
      internal LobbyManager.FFIMethods.MemberCountMethod MemberCount;
      internal LobbyManager.FFIMethods.GetMemberUserIdMethod GetMemberUserId;
      internal LobbyManager.FFIMethods.GetMemberUserMethod GetMemberUser;
      internal LobbyManager.FFIMethods.GetMemberMetadataValueMethod GetMemberMetadataValue;
      internal LobbyManager.FFIMethods.GetMemberMetadataKeyMethod GetMemberMetadataKey;
      internal LobbyManager.FFIMethods.MemberMetadataCountMethod MemberMetadataCount;
      internal LobbyManager.FFIMethods.UpdateMemberMethod UpdateMember;
      internal LobbyManager.FFIMethods.SendLobbyMessageMethod SendLobbyMessage;
      internal LobbyManager.FFIMethods.GetSearchQueryMethod GetSearchQuery;
      internal LobbyManager.FFIMethods.SearchMethod Search;
      internal LobbyManager.FFIMethods.LobbyCountMethod LobbyCount;
      internal LobbyManager.FFIMethods.GetLobbyIdMethod GetLobbyId;
      internal LobbyManager.FFIMethods.ConnectVoiceMethod ConnectVoice;
      internal LobbyManager.FFIMethods.DisconnectVoiceMethod DisconnectVoice;
      internal LobbyManager.FFIMethods.ConnectNetworkMethod ConnectNetwork;
      internal LobbyManager.FFIMethods.DisconnectNetworkMethod DisconnectNetwork;
      internal LobbyManager.FFIMethods.FlushNetworkMethod FlushNetwork;
      internal LobbyManager.FFIMethods.OpenNetworkChannelMethod OpenNetworkChannel;
      internal LobbyManager.FFIMethods.SendNetworkMessageMethod SendNetworkMessage;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetLobbyCreateTransactionMethod(
        IntPtr methodsPtr,
        ref IntPtr transaction);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetLobbyUpdateTransactionMethod(
        IntPtr methodsPtr,
        long lobbyId,
        ref IntPtr transaction);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetMemberUpdateTransactionMethod(
        IntPtr methodsPtr,
        long lobbyId,
        long userId,
        ref IntPtr transaction);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void CreateLobbyCallback(IntPtr ptr, Result result, ref Lobby lobby);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void CreateLobbyMethod(
        IntPtr methodsPtr,
        IntPtr transaction,
        IntPtr callbackData,
        LobbyManager.FFIMethods.CreateLobbyCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void UpdateLobbyCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void UpdateLobbyMethod(
        IntPtr methodsPtr,
        long lobbyId,
        IntPtr transaction,
        IntPtr callbackData,
        LobbyManager.FFIMethods.UpdateLobbyCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void DeleteLobbyCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void DeleteLobbyMethod(
        IntPtr methodsPtr,
        long lobbyId,
        IntPtr callbackData,
        LobbyManager.FFIMethods.DeleteLobbyCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ConnectLobbyCallback(IntPtr ptr, Result result, ref Lobby lobby);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ConnectLobbyMethod(
        IntPtr methodsPtr,
        long lobbyId,
        [MarshalAs(UnmanagedType.LPStr)] string secret,
        IntPtr callbackData,
        LobbyManager.FFIMethods.ConnectLobbyCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ConnectLobbyWithActivitySecretCallback(
        IntPtr ptr,
        Result result,
        ref Lobby lobby);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ConnectLobbyWithActivitySecretMethod(
        IntPtr methodsPtr,
        [MarshalAs(UnmanagedType.LPStr)] string activitySecret,
        IntPtr callbackData,
        LobbyManager.FFIMethods.ConnectLobbyWithActivitySecretCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void DisconnectLobbyCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void DisconnectLobbyMethod(
        IntPtr methodsPtr,
        long lobbyId,
        IntPtr callbackData,
        LobbyManager.FFIMethods.DisconnectLobbyCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetLobbyMethod(
        IntPtr methodsPtr,
        long lobbyId,
        ref Lobby lobby);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetLobbyActivitySecretMethod(
        IntPtr methodsPtr,
        long lobbyId,
        StringBuilder secret);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetLobbyMetadataValueMethod(
        IntPtr methodsPtr,
        long lobbyId,
        [MarshalAs(UnmanagedType.LPStr)] string key,
        StringBuilder value);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetLobbyMetadataKeyMethod(
        IntPtr methodsPtr,
        long lobbyId,
        int index,
        StringBuilder key);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result LobbyMetadataCountMethod(
        IntPtr methodsPtr,
        long lobbyId,
        ref int count);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result MemberCountMethod(
        IntPtr methodsPtr,
        long lobbyId,
        ref int count);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetMemberUserIdMethod(
        IntPtr methodsPtr,
        long lobbyId,
        int index,
        ref long userId);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetMemberUserMethod(
        IntPtr methodsPtr,
        long lobbyId,
        long userId,
        ref User user);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetMemberMetadataValueMethod(
        IntPtr methodsPtr,
        long lobbyId,
        long userId,
        [MarshalAs(UnmanagedType.LPStr)] string key,
        StringBuilder value);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetMemberMetadataKeyMethod(
        IntPtr methodsPtr,
        long lobbyId,
        long userId,
        int index,
        StringBuilder key);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result MemberMetadataCountMethod(
        IntPtr methodsPtr,
        long lobbyId,
        long userId,
        ref int count);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void UpdateMemberCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void UpdateMemberMethod(
        IntPtr methodsPtr,
        long lobbyId,
        long userId,
        IntPtr transaction,
        IntPtr callbackData,
        LobbyManager.FFIMethods.UpdateMemberCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SendLobbyMessageCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SendLobbyMessageMethod(
        IntPtr methodsPtr,
        long lobbyId,
        byte[] data,
        int dataLen,
        IntPtr callbackData,
        LobbyManager.FFIMethods.SendLobbyMessageCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetSearchQueryMethod(IntPtr methodsPtr, ref IntPtr query);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SearchCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SearchMethod(
        IntPtr methodsPtr,
        IntPtr query,
        IntPtr callbackData,
        LobbyManager.FFIMethods.SearchCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void LobbyCountMethod(IntPtr methodsPtr, ref int count);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetLobbyIdMethod(
        IntPtr methodsPtr,
        int index,
        ref long lobbyId);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ConnectVoiceCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ConnectVoiceMethod(
        IntPtr methodsPtr,
        long lobbyId,
        IntPtr callbackData,
        LobbyManager.FFIMethods.ConnectVoiceCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void DisconnectVoiceCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void DisconnectVoiceMethod(
        IntPtr methodsPtr,
        long lobbyId,
        IntPtr callbackData,
        LobbyManager.FFIMethods.DisconnectVoiceCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result ConnectNetworkMethod(IntPtr methodsPtr, long lobbyId);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result DisconnectNetworkMethod(IntPtr methodsPtr, long lobbyId);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result FlushNetworkMethod(IntPtr methodsPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result OpenNetworkChannelMethod(
        IntPtr methodsPtr,
        long lobbyId,
        byte channelId,
        bool reliable);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result SendNetworkMessageMethod(
        IntPtr methodsPtr,
        long lobbyId,
        long userId,
        byte channelId,
        byte[] data,
        int dataLen);
    }

    public delegate void CreateLobbyHandler(Result result, ref Lobby lobby);

    public delegate void UpdateLobbyHandler(Result result);

    public delegate void DeleteLobbyHandler(Result result);

    public delegate void ConnectLobbyHandler(Result result, ref Lobby lobby);

    public delegate void ConnectLobbyWithActivitySecretHandler(Result result, ref Lobby lobby);

    public delegate void DisconnectLobbyHandler(Result result);

    public delegate void UpdateMemberHandler(Result result);

    public delegate void SendLobbyMessageHandler(Result result);

    public delegate void SearchHandler(Result result);

    public delegate void ConnectVoiceHandler(Result result);

    public delegate void DisconnectVoiceHandler(Result result);

    public delegate void LobbyUpdateHandler(long lobbyId);

    public delegate void LobbyDeleteHandler(long lobbyId, uint reason);

    public delegate void MemberConnectHandler(long lobbyId, long userId);

    public delegate void MemberUpdateHandler(long lobbyId, long userId);

    public delegate void MemberDisconnectHandler(long lobbyId, long userId);

    public delegate void LobbyMessageHandler(long lobbyId, long userId, byte[] data);

    public delegate void SpeakingHandler(long lobbyId, long userId, bool speaking);

    public delegate void NetworkMessageHandler(
      long lobbyId,
      long userId,
      byte channelId,
      byte[] data);
  }
}
