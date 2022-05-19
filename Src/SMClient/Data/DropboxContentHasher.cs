// Decompiled with JetBrains decompiler
// Type: SMClient.Data.DropboxContentHasher
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SMClient.Data
{
  public class DropboxContentHasher : HashAlgorithm
  {
    private SHA256 overallHasher;
    private SHA256 blockHasher;
    private int blockPos;
    public const int BLOCK_SIZE = 4194304;
    private const string HEX_DIGITS = "0123456789abcdef";

    public DropboxContentHasher()
      : this(SHA256.Create(), SHA256.Create(), 0)
    {
    }

    public DropboxContentHasher(SHA256 overallHasher, SHA256 blockHasher, int blockPos)
    {
      this.overallHasher = overallHasher;
      this.blockHasher = blockHasher;
      this.blockPos = blockPos;
    }

    public override int HashSize => this.overallHasher.HashSize;

    protected override void HashCore(byte[] input, int offset, int len)
    {
      int inputCount;
      for (int val1 = offset + len; offset < val1; offset += inputCount)
      {
        if (this.blockPos == 4194304)
          this.FinishBlock();
        int num = 4194304 - this.blockPos;
        inputCount = Math.Min(val1, offset + num) - offset;
        this.blockHasher.TransformBlock(input, offset, inputCount, input, offset);
        this.blockPos += inputCount;
      }
    }

    protected override byte[] HashFinal()
    {
      if (this.blockPos > 0)
        this.FinishBlock();
      this.overallHasher.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
      return this.overallHasher.Hash;
    }

    public override void Initialize()
    {
      this.blockHasher.Initialize();
      this.overallHasher.Initialize();
      this.blockPos = 0;
    }

    private void FinishBlock()
    {
      this.blockHasher.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
      byte[] hash = this.blockHasher.Hash;
      this.blockHasher.Initialize();
      this.overallHasher.TransformBlock(hash, 0, hash.Length, hash, 0);
      this.blockPos = 0;
    }

    private static string ToHex(byte[] data)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in data)
      {
        stringBuilder.Append("0123456789abcdef"[(int) num >> 4]);
        stringBuilder.Append("0123456789abcdef"[(int) num & 15]);
      }
      return stringBuilder.ToString();
    }

    public string CalculateHash(string path)
    {
      byte[] numArray = new byte[1024];
      using (FileStream fileStream = File.OpenRead(path))
      {
        while (true)
        {
          int inputCount = fileStream.Read(numArray, 0, numArray.Length);
          if (inputCount > 0)
            this.TransformBlock(numArray, 0, inputCount, numArray, 0);
          else
            break;
        }
      }
      this.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
      return DropboxContentHasher.ToHex(this.Hash);
    }
  }
}
