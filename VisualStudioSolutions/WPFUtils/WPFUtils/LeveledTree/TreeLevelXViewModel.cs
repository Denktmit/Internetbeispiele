using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtils.LeveledTree {

  public class TreeLevelXViewModel : TreeViewItemViewModel {

    private readonly TreeObject _object;

    public string Name { get { return _object.Name; } }

    // constructor Branch-Object
    public TreeLevelXViewModel(TreeObject inObject, TreeLevel5ViewModel inParent) : base(inParent, false) {
      _object = inObject;
    }
    
    public override string ToString() {
      return "_TreeObjectViewModel:Name=" + Name + "_";
    }

  }

}
