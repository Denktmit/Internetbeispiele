using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtils.LeveledTree {

  public class TreeContentViewModel {

    private ReadOnlyCollection<TreeLevel1ViewModel> _roots;

    public ReadOnlyCollection<TreeLevel1ViewModel> RootElements { get { return _roots; } }

    public TreeContentViewModel(List<WPFUtils.LeveledTree.TreeObject> rootElements) {
      /*_roots = new ReadOnlyCollection<TreeObjectViewModel>(
        (from rootElement in rootElements
         select new TreeObjectViewModel(rootElement))
         .ToList());
      */
      List<TreeLevel1ViewModel> temp = new List<TreeLevel1ViewModel>();
      foreach (TreeObject to in rootElements) {
        temp.Add(new TreeLevel1ViewModel(to));
      }
      _roots = new ReadOnlyCollection<TreeLevel1ViewModel>(temp);

    }

  }

}
