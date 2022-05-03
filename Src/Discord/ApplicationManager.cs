// Decompiled with JetBrains decompiler
// Type: Discord.ApplicationManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord
{
  public class ApplicationManager
  {
    private IntPtr MethodsPtr;
    private object MethodsStructure;

    private ApplicationManager.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (ApplicationManager.FFIMethods));
        return (ApplicationManager.FFIMethods) this.MethodsStructure;
      }
    }

    internal ApplicationManager(
      IntPtr ptr,
      IntPtr eventsPtr,
      ref ApplicationManager.FFIEvents events)
    {
      if (eventsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
      this.InitEvents(eventsPtr, ref events);
      this.MethodsPtr = ptr;
      if (this.MethodsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
    }

    private void InitEvents(IntPtr eventsPtr, ref ApplicationManager.FFIEvents events) => Marshal.StructureToPtr<ApplicationManager.FFIEvents>(events, eventsPtr, false);

    public void ValidateOrExit(ApplicationManager.ValidateOrExitHandler callback)
    {
      ApplicationManager.FFIMethods.ValidateOrExitCallback callback1 = (ApplicationManager.FFIMethods.ValidateOrExitCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.ValidateOrExit(this.MethodsPtr, Utility.Retain<ApplicationManager.FFIMethods.ValidateOrExitCallback>(callback1), callback1);
    }

    public string GetCurrentLocale()
    {
      StringBuilder locale = new StringBuilder(128);
      this.Methods.GetCurrentLocale(this.MethodsPtr, locale);
      return locale.ToString();
    }

    public string GetCurrentBranch()
    {
      StringBuilder branch = new StringBuilder(4096);
      this.Methods.GetCurrentBranch(this.MethodsPtr, branch);
      return branch.ToString();
    }

    public void GetOAuth2Token(ApplicationManager.GetOAuth2TokenHandler callback)
    {
      ApplicationManager.FFIMethods.GetOAuth2TokenCallback callback1 = (ApplicationManager.FFIMethods.GetOAuth2TokenCallback) ((IntPtr ptr, Result result, ref OAuth2Token oauth2Token) =>
      {
        Utility.Release(ptr);
        callback(result, ref oauth2Token);
      });
      this.Methods.GetOAuth2Token(this.MethodsPtr, Utility.Retain<ApplicationManager.FFIMethods.GetOAuth2TokenCallback>(callback1), callback1);
    }

    public void GetTicket(ApplicationManager.GetTicketHandler callback)
    {
      ApplicationManager.FFIMethods.GetTicketCallback callback1 = (ApplicationManager.FFIMethods.GetTicketCallback) ((IntPtr ptr, Result result, ref string data) =>
      {
        Utility.Release(ptr);
        callback(result, ref data);
      });
      this.Methods.GetTicket(this.MethodsPtr, Utility.Retain<ApplicationManager.FFIMethods.GetTicketCallback>(callback1), callback1);
    }

    internal struct FFIEvents
    {
    }

    internal struct FFIMethods
    {
      internal ApplicationManager.FFIMethods.ValidateOrExitMethod ValidateOrExit;
      internal ApplicationManager.FFIMethods.GetCurrentLocaleMethod GetCurrentLocale;
      internal ApplicationManager.FFIMethods.GetCurrentBranchMethod GetCurrentBranch;
      internal ApplicationManager.FFIMethods.GetOAuth2TokenMethod GetOAuth2Token;
      internal ApplicationManager.FFIMethods.GetTicketMethod GetTicket;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ValidateOrExitCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ValidateOrExitMethod(
        IntPtr methodsPtr,
        IntPtr callbackData,
        ApplicationManager.FFIMethods.ValidateOrExitCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void GetCurrentLocaleMethod(IntPtr methodsPtr, StringBuilder locale);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void GetCurrentBranchMethod(IntPtr methodsPtr, StringBuilder branch);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void GetOAuth2TokenCallback(
        IntPtr ptr,
        Result result,
        ref OAuth2Token oauth2Token);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void GetOAuth2TokenMethod(
        IntPtr methodsPtr,
        IntPtr callbackData,
        ApplicationManager.FFIMethods.GetOAuth2TokenCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void GetTicketCallback(IntPtr ptr, Result result, [MarshalAs(UnmanagedType.LPStr)] ref string data);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void GetTicketMethod(
        IntPtr methodsPtr,
        IntPtr callbackData,
        ApplicationManager.FFIMethods.GetTicketCallback callback);
    }

    public delegate void ValidateOrExitHandler(Result result);

    public delegate void GetOAuth2TokenHandler(Result result, ref OAuth2Token oauth2Token);

    public delegate void GetTicketHandler(Result result, ref string data);
  }
}
