// Decompiled with JetBrains decompiler
// Type: Discord.RelationshipManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Runtime.InteropServices;

namespace Discord
{
  public class RelationshipManager
  {
    private IntPtr MethodsPtr;
    private object MethodsStructure;

    private RelationshipManager.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (RelationshipManager.FFIMethods));
        return (RelationshipManager.FFIMethods) this.MethodsStructure;
      }
    }

    public event RelationshipManager.RefreshHandler OnRefresh;

    public event RelationshipManager.RelationshipUpdateHandler OnRelationshipUpdate;

    internal RelationshipManager(
      IntPtr ptr,
      IntPtr eventsPtr,
      ref RelationshipManager.FFIEvents events)
    {
      if (eventsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
      this.InitEvents(eventsPtr, ref events);
      this.MethodsPtr = ptr;
      if (this.MethodsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
    }

    private void InitEvents(IntPtr eventsPtr, ref RelationshipManager.FFIEvents events)
    {
      events.OnRefresh = (RelationshipManager.FFIEvents.RefreshHandler) (ptr =>
      {
        if (this.OnRefresh == null)
          return;
        this.OnRefresh();
      });
      events.OnRelationshipUpdate = (RelationshipManager.FFIEvents.RelationshipUpdateHandler) ((IntPtr ptr, ref Relationship relationship) =>
      {
        if (this.OnRelationshipUpdate == null)
          return;
        this.OnRelationshipUpdate(ref relationship);
      });
      Marshal.StructureToPtr<RelationshipManager.FFIEvents>(events, eventsPtr, false);
    }

    public void Filter(RelationshipManager.FilterHandler callback)
    {
      RelationshipManager.FFIMethods.FilterCallback callback1 = (RelationshipManager.FFIMethods.FilterCallback) ((IntPtr ptr, ref Relationship relationship) => callback(ref relationship));
      this.Methods.Filter(this.MethodsPtr, IntPtr.Zero, callback1);
    }

    public int Count()
    {
      int count = 0;
      Result result = this.Methods.Count(this.MethodsPtr, ref count);
      if (result != Result.Ok)
        throw new ResultException(result);
      return count;
    }

    public Relationship Get(long userId)
    {
      Relationship relationship = new Relationship();
      Result result = this.Methods.Get(this.MethodsPtr, userId, ref relationship);
      if (result != Result.Ok)
        throw new ResultException(result);
      return relationship;
    }

    public Relationship GetAt(uint index)
    {
      Relationship relationship = new Relationship();
      Result result = this.Methods.GetAt(this.MethodsPtr, index, ref relationship);
      if (result != Result.Ok)
        throw new ResultException(result);
      return relationship;
    }

    internal struct FFIEvents
    {
      internal RelationshipManager.FFIEvents.RefreshHandler OnRefresh;
      internal RelationshipManager.FFIEvents.RelationshipUpdateHandler OnRelationshipUpdate;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void RefreshHandler(IntPtr ptr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void RelationshipUpdateHandler(IntPtr ptr, ref Relationship relationship);
    }

    internal struct FFIMethods
    {
      internal RelationshipManager.FFIMethods.FilterMethod Filter;
      internal RelationshipManager.FFIMethods.CountMethod Count;
      internal RelationshipManager.FFIMethods.GetMethod Get;
      internal RelationshipManager.FFIMethods.GetAtMethod GetAt;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate bool FilterCallback(IntPtr ptr, ref Relationship relationship);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void FilterMethod(
        IntPtr methodsPtr,
        IntPtr callbackData,
        RelationshipManager.FFIMethods.FilterCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result CountMethod(IntPtr methodsPtr, ref int count);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetMethod(
        IntPtr methodsPtr,
        long userId,
        ref Relationship relationship);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetAtMethod(
        IntPtr methodsPtr,
        uint index,
        ref Relationship relationship);
    }

    public delegate bool FilterHandler(ref Relationship relationship);

    public delegate void RefreshHandler();

    public delegate void RelationshipUpdateHandler(ref Relationship relationship);
  }
}
