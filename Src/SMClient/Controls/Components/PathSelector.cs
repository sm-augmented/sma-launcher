﻿// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.PathSelector
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;

namespace SMClient.Controls
{
  public partial class PathSelector : System.Windows.Controls.UserControl, IComponentConnector
  {

    public string Title { get; set; }

    public string LabelText { get; set; }

    public string SelectedDirectory { get; set; }

    public event EventHandler OnPathSelected;

    public bool IsFile { get; set; }

    public PathSelector()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
    }

    private void OpenDirectoryBtn_Click(object sender, RoutedEventArgs e)
    {
      if (this.IsFile)
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.InitialDirectory = this.SelectedDirectory;
        openFileDialog.Title = this.Title;
        if (openFileDialog.ShowDialog() != DialogResult.OK)
          return;
        this.SelectedDirectory = this.DirTextBox.Text = Path.GetDirectoryName(openFileDialog.FileName);
        EventHandler onPathSelected = this.OnPathSelected;
        if (onPathSelected == null)
          return;
        onPathSelected((object) this.SelectedDirectory, (EventArgs) null);
      }
      else
      {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        folderBrowserDialog.SelectedPath = this.SelectedDirectory;
        folderBrowserDialog.Description = this.Title;
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
          return;
        this.SelectedDirectory = this.DirTextBox.Text = folderBrowserDialog.SelectedPath;
        EventHandler onPathSelected = this.OnPathSelected;
        if (onPathSelected == null)
          return;
        onPathSelected((object) this.SelectedDirectory, (EventArgs) null);
      }
    }
  }
}
