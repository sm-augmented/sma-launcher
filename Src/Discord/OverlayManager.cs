// Decompiled with JetBrains decompiler
// Type: Discord.OverlayManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Runtime.InteropServices;

namespace Discord
{
  public class OverlayManager
  {
    private IntPtr MethodsPtr;
    private object MethodsStructure;

    private OverlayManager.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (OverlayManager.FFIMethods));
        return (OverlayManager.FFIMethods) this.MethodsStructure;
      }
    }

    public event OverlayManager.ToggleHandler OnToggle;

    internal OverlayManager(IntPtr ptr, IntPtr eventsPtr, ref OverlayManager.FFIEvents events)
    {
      if (eventsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
      this.InitEvents(eventsPtr, ref events);
      this.MethodsPtr = ptr;
      if (this.MethodsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
    }

    private void InitEvents(IntPtr eventsPtr, ref OverlayManager.FFIEvents events)
    {
      events.OnToggle = (OverlayManager.FFIEvents.ToggleHandler) ((ptr, locked) =>
      {
        if (this.OnToggle == null)
          return;
        this.OnToggle(locked);
      });
      Marshal.StructureToPtr<OverlayManager.FFIEvents>(events, eventsPtr, false);
    }

    public bool IsEnabled()
    {
      bool enabled = false;
      this.Methods.IsEnabled(this.MethodsPtr, ref enabled);
      return enabled;
    }

    public bool IsLocked()
    {
      bool locked = false;
      this.Methods.IsLocked(this.MethodsPtr, ref locked);
      return locked;
    }

    public void SetLocked(bool locked, OverlayManager.SetLockedHandler callback)
    {
      OverlayManager.FFIMethods.SetLockedCallback callback1 = (OverlayManager.FFIMethods.SetLockedCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.SetLocked(this.MethodsPtr, locked, Utility.Retain<OverlayManager.FFIMethods.SetLockedCallback>(callback1), callback1);
    }

    public void OpenActivityInvite(
      ActivityActionType type,
      OverlayManager.OpenActivityInviteHandler callback)
    {
      OverlayManager.FFIMethods.OpenActivityInviteCallback callback1 = (OverlayManager.FFIMethods.OpenActivityInviteCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.OpenActivityInvite(this.MethodsPtr, type, Utility.Retain<OverlayManager.FFIMethods.OpenActivityInviteCallback>(callback1), callback1);
    }

    public void OpenGuildInvite(string code, OverlayManager.OpenGuildInviteHandler callback)
    {
      OverlayManager.FFIMethods.OpenGuildInviteCallback callback1 = (OverlayManager.FFIMethods.OpenGuildInviteCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.OpenGuildInvite(this.MethodsPtr, code, Utility.Retain<OverlayManager.FFIMethods.OpenGuildInviteCallback>(callback1), callback1);
    }

    public void OpenVoiceSettings(OverlayManager.OpenVoiceSettingsHandler callback)
    {
      OverlayManager.FFIMethods.OpenVoiceSettingsCallback callback1 = (OverlayManager.FFIMethods.OpenVoiceSettingsCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.OpenVoiceSettings(this.MethodsPtr, Utility.Retain<OverlayManager.FFIMethods.OpenVoiceSettingsCallback>(callback1), callback1);
    }

    internal struct FFIEvents
    {
      internal OverlayManager.FFIEvents.ToggleHandler OnToggle;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ToggleHandler(IntPtr ptr, bool locked);
    }

    internal struct FFIMethods
    {
      internal OverlayManager.FFIMethods.IsEnabledMethod IsEnabled;
      internal OverlayManager.FFIMethods.IsLockedMethod IsLocked;
      internal OverlayManager.FFIMethods.SetLockedMethod SetLocked;
      internal OverlayManager.FFIMethods.OpenActivityInviteMethod OpenActivityInvite;
      internal OverlayManager.FFIMethods.OpenGuildInviteMethod OpenGuildInvite;
      internal OverlayManager.FFIMethods.OpenVoiceSettingsMethod OpenVoiceSettings;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void IsEnabledMethod(IntPtr methodsPtr, ref bool enabled);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void IsLockedMethod(IntPtr methodsPtr, ref bool locked);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SetLockedCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SetLockedMethod(
        IntPtr methodsPtr,
        bool locked,
        IntPtr callbackData,
        OverlayManager.FFIMethods.SetLockedCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void OpenActivityInviteCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void OpenActivityInviteMethod(
        IntPtr methodsPtr,
        ActivityActionType type,
        IntPtr callbackData,
        OverlayManager.FFIMethods.OpenActivityInviteCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void OpenGuildInviteCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void OpenGuildInviteMethod(
        IntPtr methodsPtr,
        [MarshalAs(UnmanagedType.LPStr)] string code,
        IntPtr callbackData,
        OverlayManager.FFIMethods.OpenGuildInviteCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void OpenVoiceSettingsCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void OpenVoiceSettingsMethod(
        IntPtr methodsPtr,
        IntPtr callbackData,
        OverlayManager.FFIMethods.OpenVoiceSettingsCallback callback);
    }

    public delegate void SetLockedHandler(Result result);

    public delegate void OpenActivityInviteHandler(Result result);

    public delegate void OpenGuildInviteHandler(Result result);

    public delegate void OpenVoiceSettingsHandler(Result result);

    public delegate void ToggleHandler(bool locked);
  }
}
