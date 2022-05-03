// Decompiled with JetBrains decompiler
// Type: Discord.NetworkManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Runtime.InteropServices;

namespace Discord
{
  public class NetworkManager
  {
    private IntPtr MethodsPtr;
    private object MethodsStructure;

    private NetworkManager.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (NetworkManager.FFIMethods));
        return (NetworkManager.FFIMethods) this.MethodsStructure;
      }
    }

    public event NetworkManager.MessageHandler OnMessage;

    public event NetworkManager.RouteUpdateHandler OnRouteUpdate;

    internal NetworkManager(IntPtr ptr, IntPtr eventsPtr, ref NetworkManager.FFIEvents events)
    {
      if (eventsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
      this.InitEvents(eventsPtr, ref events);
      this.MethodsPtr = ptr;
      if (this.MethodsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
    }

    private void InitEvents(IntPtr eventsPtr, ref NetworkManager.FFIEvents events)
    {
      events.OnMessage = (NetworkManager.FFIEvents.MessageHandler) ((ptr, peerId, channelId, dataPtr, dataLen) =>
      {
        if (this.OnMessage == null)
          return;
        byte[] numArray = new byte[dataLen];
        Marshal.Copy(dataPtr, numArray, 0, dataLen);
        this.OnMessage(peerId, channelId, numArray);
      });
      events.OnRouteUpdate = (NetworkManager.FFIEvents.RouteUpdateHandler) ((ptr, routeData) =>
      {
        if (this.OnRouteUpdate == null)
          return;
        this.OnRouteUpdate(routeData);
      });
      Marshal.StructureToPtr<NetworkManager.FFIEvents>(events, eventsPtr, false);
    }

    public ulong GetPeerId()
    {
      ulong peerId = 0;
      this.Methods.GetPeerId(this.MethodsPtr, ref peerId);
      return peerId;
    }

    public void Flush()
    {
      Result result = this.Methods.Flush(this.MethodsPtr);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void OpenPeer(ulong peerId, string routeData)
    {
      Result result = this.Methods.OpenPeer(this.MethodsPtr, peerId, routeData);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void UpdatePeer(ulong peerId, string routeData)
    {
      Result result = this.Methods.UpdatePeer(this.MethodsPtr, peerId, routeData);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void ClosePeer(ulong peerId)
    {
      Result result = this.Methods.ClosePeer(this.MethodsPtr, peerId);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void OpenChannel(ulong peerId, byte channelId, bool reliable)
    {
      Result result = this.Methods.OpenChannel(this.MethodsPtr, peerId, channelId, reliable);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void CloseChannel(ulong peerId, byte channelId)
    {
      Result result = this.Methods.CloseChannel(this.MethodsPtr, peerId, channelId);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    public void SendMessage(ulong peerId, byte channelId, byte[] data)
    {
      Result result = this.Methods.SendMessage(this.MethodsPtr, peerId, channelId, data, data.Length);
      if (result != Result.Ok)
        throw new ResultException(result);
    }

    internal struct FFIEvents
    {
      internal NetworkManager.FFIEvents.MessageHandler OnMessage;
      internal NetworkManager.FFIEvents.RouteUpdateHandler OnRouteUpdate;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void MessageHandler(
        IntPtr ptr,
        ulong peerId,
        byte channelId,
        IntPtr dataPtr,
        int dataLen);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void RouteUpdateHandler(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string routeData);
    }

    internal struct FFIMethods
    {
      internal NetworkManager.FFIMethods.GetPeerIdMethod GetPeerId;
      internal NetworkManager.FFIMethods.FlushMethod Flush;
      internal NetworkManager.FFIMethods.OpenPeerMethod OpenPeer;
      internal NetworkManager.FFIMethods.UpdatePeerMethod UpdatePeer;
      internal NetworkManager.FFIMethods.ClosePeerMethod ClosePeer;
      internal NetworkManager.FFIMethods.OpenChannelMethod OpenChannel;
      internal NetworkManager.FFIMethods.CloseChannelMethod CloseChannel;
      internal NetworkManager.FFIMethods.SendMessageMethod SendMessage;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void GetPeerIdMethod(IntPtr methodsPtr, ref ulong peerId);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result FlushMethod(IntPtr methodsPtr);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result OpenPeerMethod(
        IntPtr methodsPtr,
        ulong peerId,
        [MarshalAs(UnmanagedType.LPStr)] string routeData);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result UpdatePeerMethod(
        IntPtr methodsPtr,
        ulong peerId,
        [MarshalAs(UnmanagedType.LPStr)] string routeData);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result ClosePeerMethod(IntPtr methodsPtr, ulong peerId);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result OpenChannelMethod(
        IntPtr methodsPtr,
        ulong peerId,
        byte channelId,
        bool reliable);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result CloseChannelMethod(
        IntPtr methodsPtr,
        ulong peerId,
        byte channelId);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result SendMessageMethod(
        IntPtr methodsPtr,
        ulong peerId,
        byte channelId,
        byte[] data,
        int dataLen);
    }

    public delegate void MessageHandler(ulong peerId, byte channelId, byte[] data);

    public delegate void RouteUpdateHandler(string routeData);
  }
}
