// Decompiled with JetBrains decompiler
// Type: Discord.StorageManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord
{
  public class StorageManager
  {
    private IntPtr MethodsPtr;
    private object MethodsStructure;

    private StorageManager.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (StorageManager.FFIMethods));
        return (StorageManager.FFIMethods) this.MethodsStructure;
      }
    }

    internal StorageManager(IntPtr ptr, IntPtr eventsPtr, ref StorageManager.FFIEvents events)
    {
      if (eventsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
      this.InitEvents(eventsPtr, ref events);
      this.MethodsPtr = ptr;
      if (this.MethodsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
    }

    private void InitEvents(IntPtr eventsPtr, ref StorageManager.FFIEvents events) => Marshal.StructureToPtr<StorageManager.FFIEvents>(events, eventsPtr, false);

    public uint Read(string name, byte[] data)
    {
      uint read = 0;
      Result result = this.Methods.Read(this.MethodsPtr, name, data, data.Length, ref read);
      if (result != Result.Ok)
        throw new ResultException(result);
      return read;
    }

    public void ReadAsync(string name, StorageManager.ReadAsyncHandler callback)
    {
      StorageManager.FFIMethods.ReadAsyncCallback callback1 = (StorageManager.FFIMethods.ReadAsyncCallback) ((ptr, result, dataPtr, dataLen) =>
      {
        Utility.Release(ptr);
        byte[] numArray = new byte[dataLen];
        Marshal.Copy(dataPtr, numArray, 0, dataLen);
        callback(result, numArray);
      });
      this.Methods.ReadAsync(this.MethodsPtr, name, Utility.Retain<StorageManager.FFIMethods.ReadAsyncCallback>(callback1), callback1);
    }

    public void ReadAsyncPartial(
      string name,
      ulong offset,
      ulong length,
      StorageManager.ReadAsyncPartialHandler callback)
    {
      StorageManager.FFIMethods.ReadAsyncPartialCallback callback1 = (StorageManager.FFIMethods.ReadAsyncPartialCallback) ((ptr, result, dataPtr, dataLen) =>
      {
        Utility.Release(ptr);
        byte[] numArray = new byte[dataLen];
        Marshal.Copy(dataPtr, numArray, 0, dataLen);
        callback(result, numArray);
      });
      this.Methods.ReadAsyncPartial(this.MethodsPtr, name, offset, length, Utility.Retain<StorageManager.FFIMethods.ReadAsyncPartialCallback>(callback1), callback1);
    }

    public void Write(string name, byte[] data)
    {
      Result result = this.Methods.Write(this.MethodsPtr, name, data, data.Length);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void WriteAsync(string name, byte[] data, StorageManager.WriteAsyncHandler callback)
    {
      StorageManager.FFIMethods.WriteAsyncCallback callback1 = (StorageManager.FFIMethods.WriteAsyncCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.WriteAsync(this.MethodsPtr, name, data, data.Length, Utility.Retain<StorageManager.FFIMethods.WriteAsyncCallback>(callback1), callback1);
    }

    public void Delete(string name)
    {
      Result result = this.Methods.Delete(this.MethodsPtr, name);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public bool Exists(string name)
    {
      bool exists = false;
      Result result = this.Methods.Exists(this.MethodsPtr, name, ref exists);
      if (result != Result.Ok)
        throw new ResultException(result);
      return exists;
    }

    public int Count()
    {
      int count = 0;
      this.Methods.Count(this.MethodsPtr, ref count);
      return count;
    }

    public FileStat Stat(string name)
    {
      FileStat stat = new FileStat();
      Result result = this.Methods.Stat(this.MethodsPtr, name, ref stat);
      if (result != Result.Ok)
        throw new ResultException(result);
      return stat;
    }

    public FileStat StatAt(int index)
    {
      FileStat stat = new FileStat();
      Result result = this.Methods.StatAt(this.MethodsPtr, index, ref stat);
      if (result != Result.Ok)
        throw new ResultException(result);
      return stat;
    }

    public string GetPath()
    {
      StringBuilder path = new StringBuilder(4096);
      Result result = this.Methods.GetPath(this.MethodsPtr, path);
      if (result != Result.Ok)
        throw new ResultException(result);
      return path.ToString();
    }

    public IEnumerable<FileStat> Files()
    {
      int num = this.Count();
      List<FileStat> fileStatList = new List<FileStat>();
      for (int index = 0; index < num; ++index)
        fileStatList.Add(this.StatAt(index));
      return (IEnumerable<FileStat>) fileStatList;
    }

    internal struct FFIEvents
    {
    }

    internal struct FFIMethods
    {
      internal StorageManager.FFIMethods.ReadMethod Read;
      internal StorageManager.FFIMethods.ReadAsyncMethod ReadAsync;
      internal StorageManager.FFIMethods.ReadAsyncPartialMethod ReadAsyncPartial;
      internal StorageManager.FFIMethods.WriteMethod Write;
      internal StorageManager.FFIMethods.WriteAsyncMethod WriteAsync;
      internal StorageManager.FFIMethods.DeleteMethod Delete;
      internal StorageManager.FFIMethods.ExistsMethod Exists;
      internal StorageManager.FFIMethods.CountMethod Count;
      internal StorageManager.FFIMethods.StatMethod Stat;
      internal StorageManager.FFIMethods.StatAtMethod StatAt;
      internal StorageManager.FFIMethods.GetPathMethod GetPath;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result ReadMethod(
        IntPtr methodsPtr,
        [MarshalAs(UnmanagedType.LPStr)] string name,
        byte[] data,
        int dataLen,
        ref uint read);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ReadAsyncCallback(
        IntPtr ptr,
        Result result,
        IntPtr dataPtr,
        int dataLen);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ReadAsyncMethod(
        IntPtr methodsPtr,
        [MarshalAs(UnmanagedType.LPStr)] string name,
        IntPtr callbackData,
        StorageManager.FFIMethods.ReadAsyncCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ReadAsyncPartialCallback(
        IntPtr ptr,
        Result result,
        IntPtr dataPtr,
        int dataLen);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void ReadAsyncPartialMethod(
        IntPtr methodsPtr,
        [MarshalAs(UnmanagedType.LPStr)] string name,
        ulong offset,
        ulong length,
        IntPtr callbackData,
        StorageManager.FFIMethods.ReadAsyncPartialCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result WriteMethod(
        IntPtr methodsPtr,
        [MarshalAs(UnmanagedType.LPStr)] string name,
        byte[] data,
        int dataLen);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void WriteAsyncCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void WriteAsyncMethod(
        IntPtr methodsPtr,
        [MarshalAs(UnmanagedType.LPStr)] string name,
        byte[] data,
        int dataLen,
        IntPtr callbackData,
        StorageManager.FFIMethods.WriteAsyncCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result DeleteMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result ExistsMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, ref bool exists);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void CountMethod(IntPtr methodsPtr, ref int count);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result StatMethod(IntPtr methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string name, ref FileStat stat);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result StatAtMethod(IntPtr methodsPtr, int index, ref FileStat stat);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetPathMethod(IntPtr methodsPtr, StringBuilder path);
    }

    public delegate void ReadAsyncHandler(Result result, byte[] data);

    public delegate void ReadAsyncPartialHandler(Result result, byte[] data);

    public delegate void WriteAsyncHandler(Result result);
  }
}
