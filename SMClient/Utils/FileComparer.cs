// Decompiled with JetBrains decompiler
// Type: SMClient.Utils.FileComparer
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System.IO;

namespace SMClient.Utils
{
  public class FileComparer
  {
    public static bool FileCompare(string file1, string file2)
    {
      if (file1 == file2)
        return true;
      FileStream fileStream1 = new FileStream(file1, FileMode.Open, FileAccess.Read);
      FileStream fileStream2 = new FileStream(file2, FileMode.Open, FileAccess.Read);
      if (fileStream1.Length != fileStream2.Length)
      {
        fileStream1.Close();
        fileStream2.Close();
        return false;
      }
      int num1;
      int num2;
      do
      {
        num1 = fileStream1.ReadByte();
        num2 = fileStream2.ReadByte();
      }
      while (num1 == num2 && num1 != -1);
      fileStream1.Close();
      fileStream2.Close();
      return num1 - num2 == 0;
    }
  }
}
