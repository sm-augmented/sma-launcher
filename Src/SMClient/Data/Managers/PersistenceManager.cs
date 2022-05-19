// Decompiled with JetBrains decompiler
// Type: SMClient.Data.Managers.PersistenceManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Api;
using SMClient.Data.Tasks;
using System;

namespace SMClient.Data.Managers
{
    public static class PersistenceManager
    {
        public static event PersistenceTask.AliveChecked OnlineChecked;

        public static void Initialize()
        {
            PersistenceTask.CheckAlive(new PersistenceTask.AliveChecked(PersistenceManager.AliveChecked));
        }

        private static void AliveChecked(bool isAlive, bool needUpdate)
        {
            PersistenceTask.AliveChecked onlineChecked = PersistenceManager.OnlineChecked;
            if (onlineChecked == null)
                return;
            onlineChecked(isAlive, needUpdate);
        }
    }
}
