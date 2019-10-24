using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WPFUtils.TreeOnDemand {

  public class TreeContentViewModel : ModelBase{

    private ReadOnlyCollection<TreeObjectViewModel> _roots;
    private TreeObject _sel;

    public ReadOnlyCollection<TreeObjectViewModel> RootElements { get { return _roots; } }
    public TreeObject SelectedObject { get { return _sel; } set { _sel = value; OnPropertyChanged(); } }

    public TreeContentViewModel (List<TreeObject> rootElements){
      /*_roots = new ReadOnlyCollection<TreeObjectViewModel>(
        (from rootElement in rootElements
         select new TreeObjectViewModel(rootElement))
         .ToList());
      */
      List<TreeObjectViewModel> temp = new List<TreeObjectViewModel>();
      foreach(TreeObject to in rootElements) {
        temp.Add(new TreeObjectViewModel(to));
      }
      _roots = new ReadOnlyCollection<TreeObjectViewModel>(temp);

    }

  }

}
