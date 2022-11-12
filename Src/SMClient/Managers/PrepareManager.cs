using Models.Exceptions;
using SMClient.Models;
using SMClient.Models.Exceptions;
using SMClient.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SMClient.Managers
{
    public class PrepareManager
    {
        private static Stack<Action> restoreActions = new Stack<Action>();

        public static void RemoveStaticData() => FileHelper.DeleteFile(Settings.Instance.ModPreviewPath);

        public static void ClearStaticData()
        {
            try
            {
                if (!Directory.Exists(Settings.Instance.ModPreviewPath))
                {
                    PackageManager.ReUnpackStaticArchives();
                }
                else
                {
                    foreach (string path in Directory.EnumerateDirectories(Settings.Instance.ModPreviewPath).Where<string>((Func<string, bool>)(x => Path.GetFileName(x) != "art" && Path.GetFileName(x) != "fx")))
                        FileHelper.DeleteFile(path);
                }
            }
            catch (Exception ex)
            {
                throw new SMException("Unable to clear static data", ex);
            }
        }

        private static void ExecuteActionStack(Stack<Action> actions)
        {
            while (actions.Count > 0)
            {
                Action action = actions.Pop();
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    actions.Push(action);
                    throw new SMException("Unable to execute action while restoring", ex);
                }
            }
        }

        public static void BackupData(Package profileBranch)
        {
            try
            {
                FileHelper.MoveDirectory(Settings.Instance.PreviewPath, Settings.Instance.PreviewBackupPath);
                restoreActions.Push(() => FileHelper.MoveDirectory(Settings.Instance.PreviewBackupPath, Settings.Instance.PreviewPath));

                FileHelper.MoveDirectory(Settings.Instance.ModPreviewPath, Settings.Instance.PreviewPath);
                restoreActions.Push(() => FileHelper.MoveDirectory(Settings.Instance.PreviewPath, Settings.Instance.ModPreviewPath));                

                FileHelper.MoveDirectory(Settings.Instance.ConfigPath, Settings.Instance.ConfigPath + "-backup");
                restoreActions.Push(() => FileHelper.MoveDirectory(Settings.Instance.ConfigPath + "-backup", Settings.Instance.ConfigPath));

                FileHelper.CopyDirectory(Settings.Instance.ModfigPath, Settings.Instance.ConfigPath);
                restoreActions.Push(() => FileHelper.MoveDirectory(Settings.Instance.ConfigPath, Settings.Instance.ModfigPath));

                FileHelper.MoveDirectory(Settings.Instance.LocalePath, Settings.Instance.LocalePath + "-backup");
                restoreActions.Push(() => FileHelper.MoveDirectory(Settings.Instance.LocalePath + "-backup", Settings.Instance.LocalePath));

                FileHelper.CopyDirectory(Settings.Instance.GetLocalePathByBranch(profileBranch), Settings.Instance.LocalePath);
                restoreActions.Push(() => FileHelper.MoveDirectory(Settings.Instance.LocalePath, Settings.Instance.GetLocalePathByBranch(profileBranch)));
            }
            catch (Exception ex)
            {
                try
                {
                    ExecuteActionStack(restoreActions);
                }
                catch (Exception ex2)
                {
                    Logger.LogError(new Exception("Unable to restore backupped data", ex2));
                }

                throw new UnableToUnpackException("Unable to backup data", ex);
            }
        }

        public static void PrepareData()
        {
            Logger.LogInfo("PrepareData: Data preparation");
            restoreActions.Clear();
            ClearStaticData();
        }

        public static void RestoreData()
        {
            ExecuteActionStack(restoreActions);
            ClearStaticData();
        }
    }
}
