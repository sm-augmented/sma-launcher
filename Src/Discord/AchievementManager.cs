// Decompiled with JetBrains decompiler
// Type: Discord.AchievementManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Runtime.InteropServices;

namespace Discord
{
  public class AchievementManager
  {
    private IntPtr MethodsPtr;
    private object MethodsStructure;

    private AchievementManager.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (AchievementManager.FFIMethods));
        return (AchievementManager.FFIMethods) this.MethodsStructure;
      }
    }

    public event AchievementManager.UserAchievementUpdateHandler OnUserAchievementUpdate;

    internal AchievementManager(
      IntPtr ptr,
      IntPtr eventsPtr,
      ref AchievementManager.FFIEvents events)
    {
      if (eventsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
      this.InitEvents(eventsPtr, ref events);
      this.MethodsPtr = ptr;
      if (this.MethodsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
    }

    private void InitEvents(IntPtr eventsPtr, ref AchievementManager.FFIEvents events)
    {
      events.OnUserAchievementUpdate = (AchievementManager.FFIEvents.UserAchievementUpdateHandler) ((IntPtr ptr, ref UserAchievement userAchievement) =>
      {
        if (this.OnUserAchievementUpdate == null)
          return;
        this.OnUserAchievementUpdate(ref userAchievement);
      });
      Marshal.StructureToPtr<AchievementManager.FFIEvents>(events, eventsPtr, false);
    }

    public void SetUserAchievement(
      long achievementId,
      long percentComplete,
      AchievementManager.SetUserAchievementHandler callback)
    {
      AchievementManager.FFIMethods.SetUserAchievementCallback callback1 = (AchievementManager.FFIMethods.SetUserAchievementCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.SetUserAchievement(this.MethodsPtr, achievementId, percentComplete, Utility.Retain<AchievementManager.FFIMethods.SetUserAchievementCallback>(callback1), callback1);
    }

    public void FetchUserAchievements(
      AchievementManager.FetchUserAchievementsHandler callback)
    {
      AchievementManager.FFIMethods.FetchUserAchievementsCallback callback1 = (AchievementManager.FFIMethods.FetchUserAchievementsCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.FetchUserAchievements(this.MethodsPtr, Utility.Retain<AchievementManager.FFIMethods.FetchUserAchievementsCallback>(callback1), callback1);
    }

    public int CountUserAchievements()
    {
      int count = 0;
      this.Methods.CountUserAchievements(this.MethodsPtr, ref count);
      return count;
    }

    public UserAchievement GetUserAchievement(long userAchievementId)
    {
      UserAchievement userAchievement = new UserAchievement();
      Result result = this.Methods.GetUserAchievement(this.MethodsPtr, userAchievementId, ref userAchievement);
      if (result != Result.Ok)
        throw new ResultException(result);
      return userAchievement;
    }

    public UserAchievement GetUserAchievementAt(int index)
    {
      UserAchievement userAchievement = new UserAchievement();
      Result result = this.Methods.GetUserAchievementAt(this.MethodsPtr, index, ref userAchievement);
      if (result != Result.Ok)
        throw new ResultException(result);
      return userAchievement;
    }

    internal struct FFIEvents
    {
      internal AchievementManager.FFIEvents.UserAchievementUpdateHandler OnUserAchievementUpdate;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void UserAchievementUpdateHandler(
        IntPtr ptr,
        ref UserAchievement userAchievement);
    }

    internal struct FFIMethods
    {
      internal AchievementManager.FFIMethods.SetUserAchievementMethod SetUserAchievement;
      internal AchievementManager.FFIMethods.FetchUserAchievementsMethod FetchUserAchievements;
      internal AchievementManager.FFIMethods.CountUserAchievementsMethod CountUserAchievements;
      internal AchievementManager.FFIMethods.GetUserAchievementMethod GetUserAchievement;
      internal AchievementManager.FFIMethods.GetUserAchievementAtMethod GetUserAchievementAt;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SetUserAchievementCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void SetUserAchievementMethod(
        IntPtr methodsPtr,
        long achievementId,
        long percentComplete,
        IntPtr callbackData,
        AchievementManager.FFIMethods.SetUserAchievementCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void FetchUserAchievementsCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void FetchUserAchievementsMethod(
        IntPtr methodsPtr,
        IntPtr callbackData,
        AchievementManager.FFIMethods.FetchUserAchievementsCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void CountUserAchievementsMethod(IntPtr methodsPtr, ref int count);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetUserAchievementMethod(
        IntPtr methodsPtr,
        long userAchievementId,
        ref UserAchievement userAchievement);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetUserAchievementAtMethod(
        IntPtr methodsPtr,
        int index,
        ref UserAchievement userAchievement);
    }

    public delegate void SetUserAchievementHandler(Result result);

    public delegate void FetchUserAchievementsHandler(Result result);

    public delegate void UserAchievementUpdateHandler(ref UserAchievement userAchievement);
  }
}
