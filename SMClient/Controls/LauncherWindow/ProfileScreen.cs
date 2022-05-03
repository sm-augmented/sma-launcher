// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.LauncherWindow.ProfileScreen
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMA.Core.Models.Dictionaries;
using SMA.Core.Models.Ingame;
using SMClient.Controls.Components;
using SMClient.Data.Managers;
using SMClient.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace SMClient.Controls.LauncherWindow
{
  public class ProfileScreen : UserControl, IComponentConnector
  {
    private LoadoutSlot currentSlot;
    private LoadoutSlot currentLoadout;
    private LoadoutSlot currentWargear;
    private Package versus;
    private Package exterminatus;
    private Package currentBranch;
    private bool PackagesDownloadedFlag;
    internal Label versusBr;
    internal Label exBr;
    internal ComboBox branches;
    internal TextBox loadoutName;
    internal LoadoutSlot loadoutClass;
    internal LoadoutSlot loadoutMelee;
    internal LoadoutSlot loadoutPistol;
    internal LoadoutSlot loadoutRanged1;
    internal LoadoutSlot loadoutRanged2;
    internal LoadoutSlot loadoutEquipment;
    internal LoadoutSlot loadoutPerk1;
    internal LoadoutSlot loadoutPerk2;
    internal StackPanel wargearGrid;
    internal TextBlock wargearDescription;
    internal StackPanel loadoutsPanel;
    private bool _contentLoaded;

    public event EventHandler BranchChanged;

    public ProfileScreen()
    {
      this.InitializeComponent();
      this.PackagesDownloadedFlag = false;
      PackageManager.PackagesDownloaded += new EventHandler(this.PackageManager_PackagesDownloaded);
      if (!PackageManager.IsPackagesDownloaded || this.PackagesDownloadedFlag)
        return;
      this.PackageManager_PackagesDownloaded((object) null, (object) null);
    }

    public void PackageManager_PackagesDownloaded(object p1, object p2)
    {
      this.PackagesDownloadedFlag = true;
      this.versus = this.currentBranch = PackageManager.Packages.FirstOrDefault<Package>((Func<Package, bool>) (x => x.Name == "Versus"));
      this.exterminatus = PackageManager.Packages.FirstOrDefault<Package>((Func<Package, bool>) (x => x.Name == "Exterminatus"));
      ProfileManager.Initialize().ContinueWith((Action<Task>) (t =>
      {
        IEnumerable<Package> versRelated = PackageManager.Packages.Where<Package>((Func<Package, bool>) (x => x.Dependencies.Contains(this.versus) && x.CountedAsBranch || x == this.versus));
        this.Dispatcher.Invoke((Action) (() =>
        {
          this.branches.ItemsSource = (IEnumerable) versRelated;
          this.branches.SelectedItem = (object) this.versus;
          this.versusBr.IsEnabled = true;
          this.exBr.IsEnabled = true;
        }));
      }));
    }

    private void LoadoutSlot_Click(object sender, EventArgs e)
    {
      if (this.currentLoadout != null)
      {
        this.currentLoadout.SetSelected(false);
        this.currentLoadout = (LoadoutSlot) null;
      }
      LoadoutSlot loadoutSlot = (LoadoutSlot) sender;
      this.currentLoadout = loadoutSlot;
      loadoutSlot.SetSelected(true);
      this.loadoutName.Text = ((Loadout) loadoutSlot.Value).Name;
      if (this.currentSlot != null)
      {
        this.currentSlot.SetSelected(false);
        this.currentSlot = (LoadoutSlot) null;
      }
      this.wargearGrid.Children.Clear();
    }

    public void OnLoggedIn()
    {
    }

    private void switchToVersus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.currentBranch = this.versus;
      this.versusBr.Foreground = (Brush) new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte) 225, (byte) 165, (byte) 26));
      this.exBr.Foreground = (Brush) new SolidColorBrush(System.Windows.Media.Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
      this.branches.ItemsSource = (IEnumerable) PackageManager.Packages.Where<Package>((Func<Package, bool>) (x => x.Dependencies.Contains(this.versus) && x.CountedAsBranch || x == this.versus));
      this.branches.SelectedItem = (object) this.versus;
      EventHandler branchChanged = this.BranchChanged;
      if (branchChanged != null)
        branchChanged((object) null, (EventArgs) null);
      e.Handled = true;
    }

    private void switchToExt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.currentBranch = this.exterminatus;
      this.exBr.Foreground = (Brush) new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte) 225, (byte) 165, (byte) 26));
      this.versusBr.Foreground = (Brush) new SolidColorBrush(System.Windows.Media.Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
      this.branches.ItemsSource = (IEnumerable) PackageManager.Packages.Where<Package>((Func<Package, bool>) (x => x.Dependencies.Contains(this.exterminatus) && x.CountedAsBranch || x == this.exterminatus));
      this.branches.SelectedItem = (object) this.exterminatus;
      EventHandler branchChanged = this.BranchChanged;
      if (branchChanged != null)
        branchChanged((object) null, (EventArgs) null);
      e.Handled = true;
    }

    private void exBr_MouseEnter(object sender, MouseEventArgs e) => this.exBr.Foreground = (Brush) new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte) 225, (byte) 165, (byte) 26));

    private void exBr_MouseLeave(object sender, MouseEventArgs e)
    {
      if (this.currentBranch != this.versus)
        return;
      this.exBr.Foreground = (Brush) new SolidColorBrush(System.Windows.Media.Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
    }

    private void versusBr_MouseEnter(object sender, MouseEventArgs e) => this.versusBr.Foreground = (Brush) new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte) 225, (byte) 165, (byte) 26));

    private void versusBr_MouseLeave(object sender, MouseEventArgs e)
    {
      if (this.currentBranch != this.exterminatus)
        return;
      this.versusBr.Foreground = (Brush) new SolidColorBrush(System.Windows.Media.Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
    }

    private void branches_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ComboBox ddown = (ComboBox) sender;
      if (ddown.SelectedItem == null)
        return;
      UserProfile branchProfile = ProfileManager.Profiles.FirstOrDefault<UserProfile>((Func<UserProfile, bool>) (x => x.ID.Branch == ((Package) ddown.SelectedItem).Name));
      if (branchProfile == null)
        return;
      this.Dispatcher.Invoke((Action) (() =>
      {
        this.loadoutsPanel.Children.Clear();
        foreach (Loadout loadout in branchProfile.Loadouts.Take<Loadout>(9))
        {
          LoadoutSlot element = new LoadoutSlot()
          {
            Value = (object) loadout
          };
          element.Click += new EventHandler(this.LoadoutSlot_Click);
          this.loadoutsPanel.Children.Add((UIElement) element);
          if (this.currentSlot != null)
          {
            this.currentSlot.SetSelected(false);
            this.currentSlot = (LoadoutSlot) null;
          }
          this.wargearGrid.Children.Clear();
        }
        this.currentLoadout = (LoadoutSlot) this.loadoutsPanel.Children[0];
        this.currentLoadout.SetSelected(true);
        this.loadoutName.Text = ((Loadout) this.currentLoadout.Value).Name;
      }));
    }

    private void Wargear_Click(object sender, EventArgs e)
    {
      if (this.currentWargear != null)
      {
        this.currentWargear.SetSelected(false);
        this.currentWargear = (LoadoutSlot) null;
      }
      LoadoutSlot loadoutSlot = (LoadoutSlot) sender;
      this.currentWargear = loadoutSlot;
      this.currentWargear.SetSelected(true);
      Loadout loadout = this.currentLoadout.Value as Loadout;
      if (this.currentSlot == this.loadoutClass)
        loadout.Class = loadoutSlot.Value as Class;
      else if (this.currentSlot == this.loadoutMelee)
        loadout.Melee = loadoutSlot.Value as Wargear;
      else if (this.currentSlot == this.loadoutPistol)
        loadout.Pistol = loadoutSlot.Value as Wargear;
      else if (this.currentSlot == this.loadoutRanged1)
        loadout.Ranged = loadoutSlot.Value as Wargear;
      else if (this.currentSlot == this.loadoutRanged2)
        loadout.Ranged2 = loadoutSlot.Value as Wargear;
      else if (this.currentSlot == this.loadoutEquipment)
        loadout.Equipment = loadoutSlot.Value as Wargear;
      else if (this.currentSlot == this.loadoutPerk1)
      {
        loadout.Perk1 = loadoutSlot.Value as Wargear;
      }
      else
      {
        if (this.currentSlot != this.loadoutPerk2)
          return;
        loadout.Perk2 = loadoutSlot.Value as Wargear;
      }
    }

    private void loadoutClass_Click(object sender, EventArgs e)
    {
      if (this.currentSlot != null)
      {
        this.currentSlot.SetSelected(false);
        this.currentSlot = (LoadoutSlot) null;
      }
      this.currentSlot = this.loadoutClass;
      this.currentSlot.SetSelected(true);
      this.wargearGrid.Children.Clear();
      foreach (Class @class in ProfileManager.Classes)
      {
        LoadoutSlot loadoutSlot = new LoadoutSlot();
        loadoutSlot.Value = (object) @class;
        loadoutSlot.HorizontalAlignment = HorizontalAlignment.Left;
        loadoutSlot.VerticalAlignment = VerticalAlignment.Top;
        loadoutSlot.Margin = new Thickness(0.0, 5.0, 0.0, 0.0);
        LoadoutSlot element = loadoutSlot;
        if (this.currentLoadout != null && (this.currentLoadout.Value as Loadout).Class == @class)
        {
          element.SetSelected(true);
          this.currentWargear = element;
        }
        element.Click += new EventHandler(this.Wargear_Click);
        this.wargearGrid.Children.Add((UIElement) element);
      }
    }

    private void createWargearEntries(IEnumerable<Wargear> wargears)
    {
      this.wargearGrid.Children.Clear();
      foreach (Wargear wargear in wargears)
      {
        LoadoutSlot loadoutSlot = new LoadoutSlot();
        loadoutSlot.Value = (object) wargear;
        loadoutSlot.HorizontalAlignment = HorizontalAlignment.Left;
        loadoutSlot.VerticalAlignment = VerticalAlignment.Top;
        loadoutSlot.Margin = new Thickness(0.0, 5.0, 0.0, 0.0);
        LoadoutSlot element = loadoutSlot;
        if (this.currentLoadout != null && (this.currentLoadout.Value as Loadout).Melee == wargear)
        {
          element.SetSelected(true);
          this.currentWargear = element;
        }
        element.Click += new EventHandler(this.Wargear_Click);
        this.wargearGrid.Children.Add((UIElement) element);
      }
    }

    private void loadoutMelee_Click(object sender, EventArgs e)
    {
      if (this.currentSlot != null)
      {
        this.currentSlot.SetSelected(false);
        this.currentSlot = (LoadoutSlot) null;
      }
      this.currentSlot = this.loadoutMelee;
      this.currentSlot.SetSelected(true);
      this.createWargearEntries(ProfileManager.Wargears.Where<Wargear>((Func<Wargear, bool>) (x => x.Type == WargearType.Perk)));
    }

    private void loadoutPistol_Click(object sender, EventArgs e)
    {
      if (this.currentSlot != null)
      {
        this.currentSlot.SetSelected(false);
        this.currentSlot = (LoadoutSlot) null;
      }
      this.currentSlot = this.loadoutPistol;
      this.currentSlot.SetSelected(true);
      this.createWargearEntries(ProfileManager.Wargears.Where<Wargear>((Func<Wargear, bool>) (x => x.Type == WargearType.Ranged)));
    }

    private void loadoutRanged1_Click(object sender, EventArgs e)
    {
      if (this.currentSlot != null)
      {
        this.currentSlot.SetSelected(false);
        this.currentSlot = (LoadoutSlot) null;
      }
      this.currentSlot = this.loadoutRanged1;
      this.currentSlot.SetSelected(true);
      this.createWargearEntries(ProfileManager.Wargears.Where<Wargear>((Func<Wargear, bool>) (x => x.Type == WargearType.Melee)));
    }

    private void loadoutRanged2_Click(object sender, EventArgs e)
    {
      if (this.currentSlot != null)
      {
        this.currentSlot.SetSelected(false);
        this.currentSlot = (LoadoutSlot) null;
      }
      this.currentSlot = this.loadoutRanged2;
      this.currentSlot.SetSelected(true);
      this.createWargearEntries(ProfileManager.Wargears.Where<Wargear>((Func<Wargear, bool>) (x => x.Type == WargearType.Melee)));
    }

    private void loadoutEquipment_Click(object sender, EventArgs e)
    {
      if (this.currentSlot != null)
      {
        this.currentSlot.SetSelected(false);
        this.currentSlot = (LoadoutSlot) null;
      }
      this.currentSlot = this.loadoutEquipment;
      this.currentSlot.SetSelected(true);
      this.createWargearEntries(ProfileManager.Wargears.Where<Wargear>((Func<Wargear, bool>) (x => x.Type == WargearType.CoreAbility)));
    }

    private void loadoutPerk1_Click(object sender, EventArgs e)
    {
      if (this.currentSlot != null)
      {
        this.currentSlot.SetSelected(false);
        this.currentSlot = (LoadoutSlot) null;
      }
      this.currentSlot = this.loadoutPerk1;
      this.currentSlot.SetSelected(true);
      this.createWargearEntries(ProfileManager.Wargears.Where<Wargear>((Func<Wargear, bool>) (x => x.Type == WargearType.Equipment)));
    }

    private void loadoutPerk2_Click(object sender, EventArgs e)
    {
      if (this.currentSlot != null)
      {
        this.currentSlot.SetSelected(false);
        this.currentSlot = (LoadoutSlot) null;
      }
      this.currentSlot = this.loadoutPerk2;
      this.currentSlot.SetSelected(true);
      this.createWargearEntries(ProfileManager.Wargears.Where<Wargear>((Func<Wargear, bool>) (x => x.Type == WargearType.Equipment)));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/SMClient;component/controls/windows/mainscreens/profilescreen.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.versusBr = (Label) target;
          this.versusBr.MouseEnter += new MouseEventHandler(this.versusBr_MouseEnter);
          this.versusBr.MouseLeave += new MouseEventHandler(this.versusBr_MouseLeave);
          this.versusBr.MouseLeftButtonDown += new MouseButtonEventHandler(this.switchToVersus_MouseLeftButtonDown);
          break;
        case 2:
          this.exBr = (Label) target;
          this.exBr.MouseEnter += new MouseEventHandler(this.exBr_MouseEnter);
          this.exBr.MouseLeave += new MouseEventHandler(this.exBr_MouseLeave);
          this.exBr.MouseLeftButtonDown += new MouseButtonEventHandler(this.switchToExt_MouseLeftButtonDown);
          break;
        case 3:
          this.branches = (ComboBox) target;
          this.branches.SelectionChanged += new SelectionChangedEventHandler(this.branches_SelectionChanged);
          break;
        case 4:
          this.loadoutName = (TextBox) target;
          break;
        case 5:
          this.loadoutClass = (LoadoutSlot) target;
          break;
        case 6:
          this.loadoutMelee = (LoadoutSlot) target;
          break;
        case 7:
          this.loadoutPistol = (LoadoutSlot) target;
          break;
        case 8:
          this.loadoutRanged1 = (LoadoutSlot) target;
          break;
        case 9:
          this.loadoutRanged2 = (LoadoutSlot) target;
          break;
        case 10:
          this.loadoutEquipment = (LoadoutSlot) target;
          break;
        case 11:
          this.loadoutPerk1 = (LoadoutSlot) target;
          break;
        case 12:
          this.loadoutPerk2 = (LoadoutSlot) target;
          break;
        case 13:
          this.wargearGrid = (StackPanel) target;
          break;
        case 14:
          this.wargearDescription = (TextBlock) target;
          break;
        case 15:
          this.loadoutsPanel = (StackPanel) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
