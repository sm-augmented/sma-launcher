// Decompiled with JetBrains decompiler
// Type: Discord.LobbyTransaction
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Runtime.InteropServices;

namespace Discord
{
  public struct LobbyTransaction
  {
    internal IntPtr MethodsPtr;
    internal object MethodsStructure;

    private LobbyTransaction.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (LobbyTransaction.FFIMethods));
        return (LobbyTransaction.FFIMethods) this.MethodsStructure;
      }
    }

    public void SetType(LobbyType type)
    {
      if (!(this.MethodsPtr != IntPtr.Zero))
        return;
      Result result = this.Methods.SetType(this.MethodsPtr, type);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void SetOwner(long ownerId)
    {
      if (!(this.MethodsPtr != IntPtr.Zero))
        return;
      Result result = this.Methods.SetOwner(this.MethodsPtr, ownerId);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void SetCapacity(uint capacity)
    {
      if (!(this.MethodsPtr != IntPtr.Zero))
        return;
      Result result = this.Methods.SetCapacity(this.MethodsPtr, capacity);
      if (result != Result.Ok)
        throw new ResultException(result);
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

    public void SetLocked(bool locked)
    {
      if (!(this.MethodsPtr != IntPtr.Zero))
        return;
      Result result = this.Methods.SetLocked(this.MethodsPtr, locked);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    internal struct FFIMethods
    {
      internal LobbyTransaction.FFIMethods.SetTypeMethod SetType;
      internal LobbyTransaction.FFIMethods.SetOwnerMethod SetOwner;
      internal LobbyTransaction.FFIMethods.SetCapacityMethod SetCapacity;
      internal LobbyTransaction.FFIMethods.SetMetadataMethod SetMetadata;
      internal LobbyTransaction.FFIMethods.DeleteMetadataMethod DeleteMetadata;
      internal LobbyTransaction.FFIMethods.SetLockedMethod SetLocked;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result SetTypeMethod(IntPtr methodsPtr, LobbyType type);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result SetOwnerMethod(IntPtr methodsPtr, long ownerId);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result SetCapacityMethod(IntPtr methodsPtr, uint capacity);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result SetMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result DeleteMetadataMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string key);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result SetLockedMethod(IntPtr methodsPtr, bool locked);
    }
  }
}
