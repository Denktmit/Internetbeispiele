using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFUtils.NavigationAccordion {

  public interface INavigationItem {
    string Description { get; set; }
    WPFCommand CmdGoTo { get; set; }
    void LoadCommands();
    void GoTo();
    bool CanGoTo();
  }

  public class NavigationItem : ModelBase, INavigationItem {

    private string _desc;
    private bool _isExpanded;
    private WPFCommand _cmdExpandCollapse;
    private WPFCommand _cmdGoTo;
    private ObservableCollection<NavigationSubItem> _subitems;
    private NavigationAccordionViewModel _navm;

    public string Description { get { return _desc; } set { _desc = value; OnPropertyChanged(); } }
    public bool IsExpanded { get { return _isExpanded; } set { _isExpanded = value; OnPropertyChanged(); } }
    public bool IsCollapsed { get { return !IsExpanded; } set { IsExpanded = !value; OnPropertyChanged(); } }
    public bool IsExpandable { get { return !CanExpandCollapse(); } }
    public WPFCommand CmdExpandCollapse { get { return _cmdExpandCollapse; } set { _cmdExpandCollapse = value; OnPropertyChanged(); } }
    public WPFCommand CmdGoTo { get { return _cmdGoTo; } set { _cmdGoTo = value; OnPropertyChanged(); } }
    public ObservableCollection<NavigationSubItem> SubItems { get { return _subitems; } set { _subitems = value; OnPropertyChanged(); } }
    public NavigationAccordionViewModel NAVM { get { return _navm; } set { _navm = value; OnPropertyChanged(); } }

    public NavigationItem(string inDescription, NavigationAccordionViewModel inNAVM) {
      NAVM = inNAVM;
      Description = inDescription;
      SubItems = new ObservableCollection<NavigationSubItem>();
      IsExpanded = false;
      LoadCommands();
    }

    public void LoadCommands() {
      CmdExpandCollapse = new WPFCommand(ExpandCollapse, CanExpandCollapse);
      CmdGoTo = new WPFCommand(GoTo, CanGoTo);
    }

    public void AppendSubItem(NavigationSubItem inSubItem) {
      SubItems.Add(inSubItem);
    }

    public void AppendNewSubItem(string inDescription) {
      SubItems.Add(new NavigationSubItem(inDescription, this));
    }

    private void ExpandCollapse() {
      //Console.WriteLine("ExpandCollapse()");
      IsExpanded = !IsExpanded;
      //MessageBox.Show("ExpandCollapse");
    }
    private bool CanExpandCollapse() {
      if (SubItems.Count > 0) {
        return true;
      }
      else {
        return false;
      }
    }

    public void GoTo() {
      MessageBox.Show("GoTo (by NavigationItem <" + Description + ">)");
    }
    public bool CanGoTo() {
      return true;
    }

  }

  public class NavigationSubItem : ModelBase, INavigationItem {

    private string _desc;
    private WPFCommand _cmdGoTo;
    private NavigationItem _parent;

    public string Description { get { return _desc; } set { _desc = value; OnPropertyChanged(); } }
    public WPFCommand CmdGoTo { get { return _cmdGoTo; } set { _cmdGoTo = value;OnPropertyChanged(); } }
    public NavigationItem Parent { get { return _parent; } set { _parent = value; OnPropertyChanged(); } }

    public NavigationSubItem(string inDescription, NavigationItem inParent) {
      Description = inDescription;
      Parent = inParent;
      LoadCommands();
    }

    public void LoadCommands() {
      CmdGoTo = new WPFCommand(GoTo, CanGoTo);
    }

    public void GoTo() {
      MessageBox.Show("GoTo (by NavigationSubItem <"+Description+">)");
    }
    public bool CanGoTo() {
      return true;
    }

  }

  public class SelNavItem : ModelBase{
    private INavigationItem _selItem;
    public INavigationItem SelectedItem { get { return _selItem; } set { _selItem = value; OnPropertyChanged(); } }
  }

}
