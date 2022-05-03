// Decompiled with JetBrains decompiler
// Type: Discord.ImageHandle
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

namespace Discord
{
  public struct ImageHandle
  {
    public ImageType Type;
    public long Id;
    public uint Size;

    public static ImageHandle User(long id) => ImageHandle.User(id, 128U);

    public static ImageHandle User(long id, uint size) => new ImageHandle()
    {
      Type = ImageType.User,
      Id = id,
      Size = size
    };
  }
}
