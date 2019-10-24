using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtils;
using WPFUtils.NavigationAccordion;
using WPFUtilsTest.Model;

namespace WPFUtilsTest.ViewModel {

  public class MainWindowViewModel {
    
    private WPFUtils.TreeOnDemand.TreeContentViewModel _tcvm;
    private WPFUtils.LeveledTree.TreeContentViewModel _tcvmlt;
    private WPFUtils.TreeViewWithSearch.TreeViewModel _tvm;
    private WPFUtils.NavigationAccordion.NavigationAccordionViewModel _navm;
    
    public WPFUtils.TreeOnDemand.TreeContentViewModel TCVM { get { return _tcvm; } set { _tcvm = value; } }
    public WPFUtils.LeveledTree.TreeContentViewModel TCVMLT { get { return _tcvmlt; } set { _tcvmlt = value; } }
    public WPFUtils.TreeViewWithSearch.TreeViewModel TVM { get { return _tvm; } set { _tvm = value; } }
    public WPFUtils.NavigationAccordion.NavigationAccordionViewModel NAVM { get { return _navm; } set { _navm = value; } }

    public MainWindowViewModel() {
      LoadTVM();
      LoadTCVMLT();
      LoadTCVM();
      LoadNAVM();
    }

    private void LoadTVM() {
      WPFUtils.TreeViewWithSearch.TreeObject loaded = TestData.GetTreeStuffWithSearch();
      TVM = new WPFUtils.TreeViewWithSearch.TreeViewModel(loaded);
    }

    private void LoadTCVM() {
      List<WPFUtils.TreeOnDemand.TreeObject> loaded = TestData.GetTreeContentViewModelData();
      TCVM = new WPFUtils.TreeOnDemand.TreeContentViewModel(loaded);
    }

    private void LoadTCVMLT() {
      List<WPFUtils.LeveledTree.TreeObject> loaded = TestData.GetTreeContentViewModelDataLeveledTree();
      TCVMLT = new WPFUtils.LeveledTree.TreeContentViewModel(loaded);
    }

    private void LoadNAVM() {
      NAVM = new WPFUtils.NavigationAccordion.NavigationAccordionViewModel();
      ObservableCollection<WPFUtils.NavigationAccordion.INavigationItem> loaded = TestData.GetNavigationAccordionData(NAVM);
      NAVM.NavigationItems = loaded;
    }

  }

}
