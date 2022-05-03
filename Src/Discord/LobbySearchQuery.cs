// Decompiled with JetBrains decompiler
// Type: Discord.LobbySearchQuery
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Runtime.InteropServices;

namespace Discord
{
  public struct LobbySearchQuery
  {
    internal IntPtr MethodsPtr;
    internal object MethodsStructure;

    private LobbySearchQuery.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (LobbySearchQuery.FFIMethods));
        return (LobbySearchQuery.FFIMethods) this.MethodsStructure;
      }
    }

    public void Filter(
      string key,
      LobbySearchComparison comparison,
      LobbySearchCast cast,
      string value)
    {
      if (!(this.MethodsPtr != IntPtr.Zero))
        return;
      Result result = this.Methods.Filter(this.MethodsPtr, key, comparison, cast, value);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void Sort(string key, LobbySearchCast cast, string value)
    {
      if (!(this.MethodsPtr != IntPtr.Zero))
        return;
      Result result = this.Methods.Sort(this.MethodsPtr, key, cast, value);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void Limit(uint limit)
    {
      if (!(this.MethodsPtr != IntPtr.Zero))
        return;
      Result result = this.Methods.Limit(this.MethodsPtr, limit);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void Distance(LobbySearchDistance distance)
    {
      if (!(this.MethodsPtr != IntPtr.Zero))
        return;
      Result result = this.Methods.Distance(this.MethodsPtr, distance);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    internal struct FFIMethods
    {
      internal LobbySearchQuery.FFIMethods.FilterMethod Filter;
      internal LobbySearchQuery.FFIMethods.SortMethod Sort;
      internal LobbySearchQuery.FFIMethods.LimitMethod Limit;
      internal LobbySearchQuery.FFIMethods.DistanceMethod Distance;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result FilterMethod(
        IntPtr methodsPtr,
        [MarshalAs(UnmanagedType.LPStr)] string key,
        LobbySearchComparison comparison,
        LobbySearchCast cast,
        [MarshalAs(UnmanagedType.LPStr)] string value);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result SortMethod(
        IntPtr methodsPtr,
        [MarshalAs(UnmanagedType.LPStr)] string key,
        LobbySearchCast cast,
        [MarshalAs(UnmanagedType.LPStr)] string value);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result LimitMethod(IntPtr methodsPtr, uint limit);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result DistanceMethod(IntPtr methodsPtr, LobbySearchDistance distance);
    }
  }
}
