// Decompiled with JetBrains decompiler
// Type: Discord.UserManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Runtime.InteropServices;

namespace Discord
{
  public class UserManager
  {
    private IntPtr MethodsPtr;
    private object MethodsStructure;

    private UserManager.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (UserManager.FFIMethods));
        return (UserManager.FFIMethods) this.MethodsStructure;
      }
    }

    public event UserManager.CurrentUserUpdateHandler OnCurrentUserUpdate;

    internal UserManager(IntPtr ptr, IntPtr eventsPtr, ref UserManager.FFIEvents events)
    {
      if (eventsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
      this.InitEvents(eventsPtr, ref events);
      this.MethodsPtr = ptr;
      if (this.MethodsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
    }

    private void InitEvents(IntPtr eventsPtr, ref UserManager.FFIEvents events)
    {
      events.OnCurrentUserUpdate = (UserManager.FFIEvents.CurrentUserUpdateHandler) (ptr =>
      {
        if (this.OnCurrentUserUpdate == null)
          return;
        this.OnCurrentUserUpdate();
      });
      Marshal.StructureToPtr<UserManager.FFIEvents>(events, eventsPtr, false);
    }

    public User GetCurrentUser()
    {
      User currentUser = new User();
      Result result = this.Methods.GetCurrentUser(this.MethodsPtr, ref currentUser);
      if (result != Result.Ok)
        throw new ResultException(result);
      return currentUser;
    }

    public void GetUser(long userId, UserManager.GetUserHandler callback)
    {
      UserManager.FFIMethods.GetUserCallback callback1 = (UserManager.FFIMethods.GetUserCallback) ((IntPtr ptr, Result result, ref User user) =>
      {
        Utility.Release(ptr);
        callback(result, ref user);
      });
      this.Methods.GetUser(this.MethodsPtr, userId, Utility.Retain<UserManager.FFIMethods.GetUserCallback>(callback1), callback1);
    }

    public PremiumType GetCurrentUserPremiumType()
    {
      PremiumType premiumType = PremiumType.None;
      Result result = this.Methods.GetCurrentUserPremiumType(this.MethodsPtr, ref premiumType);
      if (result != Result.Ok)
        throw new ResultException(result);
      return premiumType;
    }

    public bool CurrentUserHasFlag(UserFlag flag)
    {
      bool hasFlag = false;
      Result result = this.Methods.CurrentUserHasFlag(this.MethodsPtr, flag, ref hasFlag);
      if (result != Result.Ok)
        throw new ResultException(result);
      return hasFlag;
    }

    internal struct FFIEvents
    {
      internal UserManager.FFIEvents.CurrentUserUpdateHandler OnCurrentUserUpdate;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void CurrentUserUpdateHandler(IntPtr ptr);
    }

    internal struct FFIMethods
    {
      internal UserManager.FFIMethods.GetCurrentUserMethod GetCurrentUser;
      internal UserManager.FFIMethods.GetUserMethod GetUser;
      internal UserManager.FFIMethods.GetCurrentUserPremiumTypeMethod GetCurrentUserPremiumType;
      internal UserManager.FFIMethods.CurrentUserHasFlagMethod CurrentUserHasFlag;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetCurrentUserMethod(IntPtr methodsPtr, ref User currentUser);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void GetUserCallback(IntPtr ptr, Result result, ref User user);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void GetUserMethod(
        IntPtr methodsPtr,
        long userId,
        IntPtr callbackData,
        UserManager.FFIMethods.GetUserCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetCurrentUserPremiumTypeMethod(
        IntPtr methodsPtr,
        ref PremiumType premiumType);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result CurrentUserHasFlagMethod(
        IntPtr methodsPtr,
        UserFlag flag,
        ref bool hasFlag);
    }

    public delegate void GetUserHandler(Result result, ref User user);

    public delegate void CurrentUserUpdateHandler();
  }
}
