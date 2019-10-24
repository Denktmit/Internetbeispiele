using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtils.NavigationAccordion {

  public class NavigationAccordionViewModel :ModelBase {

    private ObservableCollection<INavigationItem> _navItems;
    private INavigationItem _selectedNavItem;

    public ObservableCollection<INavigationItem> NavigationItems {
      get {
        return _navItems;
      }
      set {
        _navItems = value;
        foreach (NavigationItem ni in NavigationItems) {
          ni.NAVM = this;
        }
        OnPropertyChanged();
      }
    }
    public INavigationItem SelectedNavItem {
      get {
        return _selectedNavItem;
      }
      set {
        _selectedNavItem = value;
        OnPropertyChanged();
      }
    }

    public NavigationAccordionViewModel() {
      NavigationItems = new ObservableCollection<INavigationItem>();
      SelectedNavItem = null;
    }
    public NavigationAccordionViewModel(ObservableCollection<INavigationItem> inNavItems) {
      NavigationItems = inNavItems;
      foreach(NavigationItem ni in NavigationItems) {
        ni.NAVM = this;
      }
      SelectedNavItem = null;
    }

  }

}
