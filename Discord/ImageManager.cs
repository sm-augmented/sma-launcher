// Decompiled with JetBrains decompiler
// Type: Discord.ImageManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Runtime.InteropServices;

namespace Discord
{
  public class ImageManager
  {
    private IntPtr MethodsPtr;
    private object MethodsStructure;

    private ImageManager.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (ImageManager.FFIMethods));
        return (ImageManager.FFIMethods) this.MethodsStructure;
      }
    }

    internal ImageManager(IntPtr ptr, IntPtr eventsPtr, ref ImageManager.FFIEvents events)
    {
      if (eventsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
      this.InitEvents(eventsPtr, ref events);
      this.MethodsPtr = ptr;
      if (this.MethodsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
    }

    private void InitEvents(IntPtr eventsPtr, ref ImageManager.FFIEvents events) => Marshal.StructureToPtr<ImageManager.FFIEvents>(events, eventsPtr, false);

    public void Fetch(ImageHandle handle, bool refresh, ImageManager.FetchHandler callback)
    {
      ImageManager.FFIMethods.FetchCallback callback1 = (ImageManager.FFIMethods.FetchCallback) ((ptr, result, handleResult) =>
      {
        Utility.Release(ptr);
        callback(result, handleResult);
      });
      this.Methods.Fetch(this.MethodsPtr, handle, refresh, Utility.Retain<ImageManager.FFIMethods.FetchCallback>(callback1), callback1);
    }

    public ImageDimensions GetDimensions(ImageHandle handle)
    {
      ImageDimensions dimensions = new ImageDimensions();
      Result result = this.Methods.GetDimensions(this.MethodsPtr, handle, ref dimensions);
      if (result != Result.Ok)
        throw new ResultException(result);
      return dimensions;
    }

    public void GetData(ImageHandle handle, byte[] data)
    {
      Result result = this.Methods.GetData(this.MethodsPtr, handle, data, data.Length);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void Fetch(ImageHandle handle, ImageManager.FetchHandler callback) => this.Fetch(handle, false, callback);

    public byte[] GetData(ImageHandle handle)
    {
      ImageDimensions dimensions = this.GetDimensions(handle);
      byte[] data = new byte[(int) dimensions.Width * (int) dimensions.Height * 4];
      this.GetData(handle, data);
      return data;
    }

    internal struct FFIEvents
    {
    }

    internal struct FFIMethods
    {
      internal ImageManager.FFIMethods.FetchMethod Fetch;
      internal ImageManager.FFIMethods.GetDimensionsMethod GetDimensions;
      internal ImageManager.FFIMethods.GetDataMethod GetData;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void FetchCallback(IntPtr ptr, Result result, ImageHandle handleResult);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void FetchMethod(
        IntPtr methodsPtr,
        ImageHandle handle,
        bool refresh,
        IntPtr callbackData,
        ImageManager.FFIMethods.FetchCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetDimensionsMethod(
        IntPtr methodsPtr,
        ImageHandle handle,
        ref ImageDimensions dimensions);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetDataMethod(
        IntPtr methodsPtr,
        ImageHandle handle,
        byte[] data,
        int dataLen);
    }

    public delegate void FetchHandler(Result result, ImageHandle handleResult);
  }
}
