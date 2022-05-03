// Decompiled with JetBrains decompiler
// Type: Discord.ActivityManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Runtime.InteropServices;

namespace Discord
{
  public class ActivityManager
  {
    private IntPtr MethodsPtr;
    private object MethodsStructure;

    public void RegisterCommand() => this.RegisterCommand((string) null);

    private ActivityManager.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (ActivityManager.FFIMethods));
        return (ActivityManager.FFIMethods) this.MethodsStructure;
      }
    }

    public event ActivityManager.ActivityJoinHandler OnActivityJoin;

    public event ActivityManager.ActivitySpectateHandler OnActivitySpectate;

    public event ActivityManager.ActivityJoinRequestHandler OnActivityJoinRequest;

    public event ActivityManager.ActivityInviteHandler OnActivityInvite;

    internal ActivityManager(IntPtr ptr, IntPtr eventsPtr, ref ActivityManager.FFIEvents events)
    {
      if (eventsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
      this.InitEvents(eventsPtr, ref events);
      this.MethodsPtr = ptr;
      if (this.MethodsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
    }

    private void InitEvents(IntPtr eventsPtr, ref ActivityManager.FFIEvents events)
    {
      events.OnActivityJoin = (ActivityManager.FFIEvents.ActivityJoinHandler) ((ptr, secret) =>
      {
        if (this.OnActivityJoin == null)
          return;
        this.OnActivityJoin(secret);
      });
      events.OnActivitySpectate = (ActivityManager.FFIEvents.ActivitySpectateHandler) ((ptr, secret) =>
      {
        if (this.OnActivitySpectate == null)
          return;
        this.OnActivitySpectate(secret);
      });
      events.OnActivityJoinRequest = (ActivityManager.FFIEvents.ActivityJoinRequestHandler) ((IntPtr ptr, ref User user) =>
      {
        if (this.OnActivityJoinRequest == null)
          return;
        this.OnActivityJoinRequest(ref user);
      });
      events.OnActivityInvite = (ActivityManager.FFIEvents.ActivityInviteHandler) ((IntPtr ptr, ActivityActionType type, ref User user, ref Activity activity) =>
      {
        if (this.OnActivityInvite == null)
          return;
        this.OnActivityInvite(type, ref user, ref activity);
      });
      Marshal.StructureToPtr<ActivityManager.FFIEvents>(events, eventsPtr, false);
    }

    public void RegisterCommand(string command)
    {
      Result result = this.Methods.RegisterCommand(this.MethodsPtr, command);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void RegisterSteam(uint steamId)
    {
      Result result = this.Methods.RegisterSteam(this.MethodsPtr, steamId);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void UpdateActivity(Activity activity, ActivityManager.UpdateActivityHandler callback)
    {
      ActivityManager.FFIMethods.UpdateActivityCallback callback1 = (ActivityManager.FFIMethods.UpdateActivityCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.UpdateActivity(this.MethodsPtr, ref activity, Utility.Retain<ActivityManager.FFIMethods.UpdateActivityCallback>(callback1), callback1);
    }

    public void ClearActivity(ActivityManager.ClearActivityHandler callback)
    {
      ActivityManager.FFIMethods.ClearActivityCallback callback1 = (ActivityManager.FFIMethods.ClearActivityCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.ClearActivity(this.MethodsPtr, Utility.Retain<ActivityManager.FFIMethods.ClearActivityCallback>(callback1), callback1);
    }

    public void SendRequestReply(
      long userId,
      ActivityJoinRequestReply reply,
      ActivityManager.SendRequestReplyHandler callback)
    {
      ActivityManager.FFIMethods.SendRequestReplyCallback callback1 = (ActivityManager.FFIMethods.SendRequestReplyCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.SendRequestReply(this.MethodsPtr, userId, reply, Utility.Retain<ActivityManager.FFIMethods.SendRequestReplyCallback>(callback1), callback1);
    }

    public void SendInvite(
      long userId,
      ActivityActionType type,
      string content,
      ActivityManager.SendInviteHandler callback)
    {
      ActivityManager.FFIMethods.SendInviteCallback callback1 = (ActivityManager.FFIMethods.SendInviteCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.SendInvite(this.MethodsPtr, userId, type, content, Utility.Retain<ActivityManager.FFIMethods.SendInviteCallback>(callback1), callback1);
    }

    public void AcceptInvite(long userId, ActivityManager.AcceptInviteHandler callback)
    {
      ActivityManager.FFIMethods.AcceptInviteCallback callback1 = (ActivityManager.FFIMethods.AcceptInviteCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.AcceptInvite(this.MethodsPtr, userId, Utility.Retain<ActivityManager.FFIMethods.AcceptInviteCallback>(callback1), callback1);
    }

    internal struct FFIEvents
    {
      internal ActivityManager.FFIEvents.ActivityJoinHandler OnActivityJoin;
      internal ActivityManager.FFIEvents.ActivitySpectateHandler OnActivitySpectate;
      internal ActivityManager.FFIEvents.ActivityJoinRequestHandler OnActivityJoinRequest;
      internal ActivityManager.FFIEvents.ActivityInviteHandler OnActivityInvite;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ActivityJoinHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string secret);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ActivitySpectateHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string secret);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ActivityJoinRequestHandler(IntPtr ptr, ref User user);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ActivityInviteHandler(
        IntPtr ptr,
        ActivityActionType type,
        ref User user,
        ref Activity activity);
    }

    internal struct FFIMethods
    {
      internal ActivityManager.FFIMethods.RegisterCommandMethod RegisterCommand;
      internal ActivityManager.FFIMethods.RegisterSteamMethod RegisterSteam;
      internal ActivityManager.FFIMethods.UpdateActivityMethod UpdateActivity;
      internal ActivityManager.FFIMethods.ClearActivityMethod ClearActivity;
      internal ActivityManager.FFIMethods.SendRequestReplyMethod SendRequestReply;
      internal ActivityManager.FFIMethods.SendInviteMethod SendInvite;
      internal ActivityManager.FFIMethods.AcceptInviteMethod AcceptInvite;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result RegisterCommandMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string command);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result RegisterSteamMethod(IntPtr methodsPtr, uint steamId);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void UpdateActivityCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void UpdateActivityMethod(
        IntPtr methodsPtr,
        ref Activity activity,
        IntPtr callbackData,
        ActivityManager.FFIMethods.UpdateActivityCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ClearActivityCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ClearActivityMethod(
        IntPtr methodsPtr,
        IntPtr callbackData,
        ActivityManager.FFIMethods.ClearActivityCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SendRequestReplyCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SendRequestReplyMethod(
        IntPtr methodsPtr,
        long userId,
        ActivityJoinRequestReply reply,
        IntPtr callbackData,
        ActivityManager.FFIMethods.SendRequestReplyCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SendInviteCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SendInviteMethod(
        IntPtr methodsPtr,
        long userId,
        ActivityActionType type,
        [MarshalAs(UnmanagedType.LPStr)] string content,
        IntPtr callbackData,
        ActivityManager.FFIMethods.SendInviteCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void AcceptInviteCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void AcceptInviteMethod(
        IntPtr methodsPtr,
        long userId,
        IntPtr callbackData,
        ActivityManager.FFIMethods.AcceptInviteCallback callback);
    }

    public delegate void UpdateActivityHandler(Result result);

    public delegate void ClearActivityHandler(Result result);

    public delegate void SendRequestReplyHandler(Result result);

    public delegate void SendInviteHandler(Result result);

    public delegate void AcceptInviteHandler(Result result);

    public delegate void ActivityJoinHandler(string secret);

    public delegate void ActivitySpectateHandler(string secret);

    public delegate void ActivityJoinRequestHandler(ref User user);

    public delegate void ActivityInviteHandler(
      ActivityActionType type,
      ref User user,
      ref Activity activity);
  }
}
