// Decompiled with JetBrains decompiler
// Type: Discord.StoreManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Discord
{
  public class StoreManager
  {
    private IntPtr MethodsPtr;
    private object MethodsStructure;

    private StoreManager.FFIMethods Methods
    {
      get
      {
        if (this.MethodsStructure == null)
          this.MethodsStructure = Marshal.PtrToStructure(this.MethodsPtr, typeof (StoreManager.FFIMethods));
        return (StoreManager.FFIMethods) this.MethodsStructure;
      }
    }

    public event StoreManager.EntitlementCreateHandler OnEntitlementCreate;

    public event StoreManager.EntitlementDeleteHandler OnEntitlementDelete;

    internal StoreManager(IntPtr ptr, IntPtr eventsPtr, ref StoreManager.FFIEvents events)
    {
      if (eventsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
      this.InitEvents(eventsPtr, ref events);
      this.MethodsPtr = ptr;
      if (this.MethodsPtr == IntPtr.Zero)
        throw new ResultException(Result.InternalError);
    }

    private void InitEvents(IntPtr eventsPtr, ref StoreManager.FFIEvents events)
    {
      events.OnEntitlementCreate = (StoreManager.FFIEvents.EntitlementCreateHandler) ((IntPtr ptr, ref Entitlement entitlement) =>
      {
        if (this.OnEntitlementCreate == null)
          return;
        this.OnEntitlementCreate(ref entitlement);
      });
      events.OnEntitlementDelete = (StoreManager.FFIEvents.EntitlementDeleteHandler) ((IntPtr ptr, ref Entitlement entitlement) =>
      {
        if (this.OnEntitlementDelete == null)
          return;
        this.OnEntitlementDelete(ref entitlement);
      });
      Marshal.StructureToPtr<StoreManager.FFIEvents>(events, eventsPtr, false);
    }

    public void FetchSkus(StoreManager.FetchSkusHandler callback)
    {
      StoreManager.FFIMethods.FetchSkusCallback callback1 = (StoreManager.FFIMethods.FetchSkusCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.FetchSkus(this.MethodsPtr, Utility.Retain<StoreManager.FFIMethods.FetchSkusCallback>(callback1), callback1);
    }

    public int CountSkus()
    {
      int count = 0;
      this.Methods.CountSkus(this.MethodsPtr, ref count);
      return count;
    }

    public Sku GetSku(long skuId)
    {
      Sku sku = new Sku();
      Result result = this.Methods.GetSku(this.MethodsPtr, skuId, ref sku);
      if (result != Result.Ok)
        throw new ResultException(result);
      return sku;
    }

    public Sku GetSkuAt(int index)
    {
      Sku sku = new Sku();
      Result result = this.Methods.GetSkuAt(this.MethodsPtr, index, ref sku);
      if (result != Result.Ok)
        throw new ResultException(result);
      return sku;
    }

    public void FetchEntitlements(StoreManager.FetchEntitlementsHandler callback)
    {
      StoreManager.FFIMethods.FetchEntitlementsCallback callback1 = (StoreManager.FFIMethods.FetchEntitlementsCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.FetchEntitlements(this.MethodsPtr, Utility.Retain<StoreManager.FFIMethods.FetchEntitlementsCallback>(callback1), callback1);
    }

    public int CountEntitlements()
    {
      int count = 0;
      this.Methods.CountEntitlements(this.MethodsPtr, ref count);
      return count;
    }

    public Entitlement GetEntitlement(long entitlementId)
    {
      Entitlement entitlement = new Entitlement();
      Result result = this.Methods.GetEntitlement(this.MethodsPtr, entitlementId, ref entitlement);
      if (result != Result.Ok)
        throw new ResultException(result);
      return entitlement;
    }

    public Entitlement GetEntitlementAt(int index)
    {
      Entitlement entitlement = new Entitlement();
      Result result = this.Methods.GetEntitlementAt(this.MethodsPtr, index, ref entitlement);
      if (result != Result.Ok)
        throw new ResultException(result);
      return entitlement;
    }

    public bool HasSkuEntitlement(long skuId)
    {
      bool hasEntitlement = false;
      Result result = this.Methods.HasSkuEntitlement(this.MethodsPtr, skuId, ref hasEntitlement);
      if (result != Result.Ok)
        throw new ResultException(result);
      return hasEntitlement;
    }

    public void StartPurchase(long skuId, StoreManager.StartPurchaseHandler callback)
    {
      StoreManager.FFIMethods.StartPurchaseCallback callback1 = (StoreManager.FFIMethods.StartPurchaseCallback) ((ptr, result) =>
      {
        Utility.Release(ptr);
        callback(result);
      });
      this.Methods.StartPurchase(this.MethodsPtr, skuId, Utility.Retain<StoreManager.FFIMethods.StartPurchaseCallback>(callback1), callback1);
    }

    public IEnumerable<Entitlement> GetEntitlements()
    {
      int num = this.CountEntitlements();
      List<Entitlement> entitlements = new List<Entitlement>();
      for (int index = 0; index < num; ++index)
        entitlements.Add(this.GetEntitlementAt(index));
      return (IEnumerable<Entitlement>) entitlements;
    }

    public IEnumerable<Sku> GetSkus()
    {
      int num = this.CountSkus();
      List<Sku> skus = new List<Sku>();
      for (int index = 0; index < num; ++index)
        skus.Add(this.GetSkuAt(index));
      return (IEnumerable<Sku>) skus;
    }

    internal struct FFIEvents
    {
      internal StoreManager.FFIEvents.EntitlementCreateHandler OnEntitlementCreate;
      internal StoreManager.FFIEvents.EntitlementDeleteHandler OnEntitlementDelete;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void EntitlementCreateHandler(IntPtr ptr, ref Entitlement entitlement);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void EntitlementDeleteHandler(IntPtr ptr, ref Entitlement entitlement);
    }

    internal struct FFIMethods
    {
      internal StoreManager.FFIMethods.FetchSkusMethod FetchSkus;
      internal StoreManager.FFIMethods.CountSkusMethod CountSkus;
      internal StoreManager.FFIMethods.GetSkuMethod GetSku;
      internal StoreManager.FFIMethods.GetSkuAtMethod GetSkuAt;
      internal StoreManager.FFIMethods.FetchEntitlementsMethod FetchEntitlements;
      internal StoreManager.FFIMethods.CountEntitlementsMethod CountEntitlements;
      internal StoreManager.FFIMethods.GetEntitlementMethod GetEntitlement;
      internal StoreManager.FFIMethods.GetEntitlementAtMethod GetEntitlementAt;
      internal StoreManager.FFIMethods.HasSkuEntitlementMethod HasSkuEntitlement;
      internal StoreManager.FFIMethods.StartPurchaseMethod StartPurchase;

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void FetchSkusCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void FetchSkusMethod(
        IntPtr methodsPtr,
        IntPtr callbackData,
        StoreManager.FFIMethods.FetchSkusCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void CountSkusMethod(IntPtr methodsPtr, ref int count);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetSkuMethod(IntPtr methodsPtr, long skuId, ref Sku sku);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetSkuAtMethod(IntPtr methodsPtr, int index, ref Sku sku);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void FetchEntitlementsCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void FetchEntitlementsMethod(
        IntPtr methodsPtr,
        IntPtr callbackData,
        StoreManager.FFIMethods.FetchEntitlementsCallback callback);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void CountEntitlementsMethod(IntPtr methodsPtr, ref int count);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetEntitlementMethod(
        IntPtr methodsPtr,
        long entitlementId,
        ref Entitlement entitlement);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result GetEntitlementAtMethod(
        IntPtr methodsPtr,
        int index,
        ref Entitlement entitlement);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate Result HasSkuEntitlementMethod(
        IntPtr methodsPtr,
        long skuId,
        ref bool hasEntitlement);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void StartPurchaseCallback(IntPtr ptr, Result result);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate void StartPurchaseMethod(
        IntPtr methodsPtr,
        long skuId,
        IntPtr callbackData,
        StoreManager.FFIMethods.StartPurchaseCallback callback);
    }

    public delegate void FetchSkusHandler(Result result);

    public delegate void FetchEntitlementsHandler(Result result);

    public delegate void StartPurchaseHandler(Result result);

    public delegate void EntitlementCreateHandler(ref Entitlement entitlement);

    public delegate void EntitlementDeleteHandler(ref Entitlement entitlement);
  }
}
