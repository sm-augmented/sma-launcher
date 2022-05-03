// Decompiled with JetBrains decompiler
// Type: Discord.LobbyMemberTransaction
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Runtime.InteropServices;

namespace Discord
{
  public struct LobbyMemberTransaction
  {
    internal IntPtr MethodsPtr;
    internal object MethodsStructure;

    private LobbyMemberTransaction.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (LobbyMemberTransaction.FFIMethods));
        return (LobbyMemberTransaction.FFIMethods) this.MethodsStructure;
      }
    }

    public void SetMetadata(string key, string value)
    {
      if (!(this.MethodsPtr != IntPtr.Zero))
        return;
      Result result = this.Methods.SetMetadata(this.MethodsPtr, key, value);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void DeleteMetadata(string key)
    {
      if (!(this.MethodsPtr != IntPtr.Zero))
        return;
      Result result = this.Methods.DeleteMetadata(this.MethodsPtr, key);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    internal struct FFIMethods
    {
      internal LobbyMemberTransaction.FFIMethods.SetMetadataMethod SetMetadata;
      internal LobbyMemberTransaction.FFIMethods.DeleteMetadataMethod DeleteMetadata;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result SetMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result DeleteMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key);
    }
  }
}
