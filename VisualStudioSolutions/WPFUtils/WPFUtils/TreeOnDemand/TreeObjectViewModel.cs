using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtils.TreeOnDemand {

  public class TreeObjectViewModel : TreeViewItemViewModel {

    private readonly TreeObject _object;

    public string Name { get { return _object.Name; } }

    public TreeObject Object { get { return _object; } }

    // constructor Root-Object
    public TreeObjectViewModel(TreeObject inObject) : base(null, true) {
      _object = inObject;
    }
    // constructor Branch-Object
    public TreeObjectViewModel(TreeObject inObject, TreeObjectViewModel inParent) : base(inParent, true) {
      _object = inObject;
    }
    // constructor Branch-Or-Leaf-Object
    public TreeObjectViewModel(TreeObject inObject, TreeObjectViewModel inParent, bool isLeaf) : base(inParent, !isLeaf) {
      _object = inObject;
    }

    protected override void LoadChildren() {
      Console.WriteLine("LoadChildren() at #"+this.ToString()+"#");
      if (_object.SubObjects.Count != 0) {
        foreach (TreeObject obj in _object.SubObjects) {
          if (obj.SubObjects.Count == 0) {
            // child-object has no subObjects -> IsLeaf
            Console.WriteLine("   "+obj.Name+" -> IsLeaf");
            base.Children.Add(new TreeObjectViewModel(obj, this, true));
          }
          else {
            // child-object has no subObjects -> IsBranch
            Console.WriteLine("   "+obj.Name+" -> IsBranch");
            base.Children.Add(new TreeObjectViewModel(obj, this, false));
          }
        }
      }
      else {
        Console.WriteLine("TODO (delete): no subobjects for " + this.Name);
      }
    }

    public override string ToString() {
      return "_TreeObjectViewModel:Name="+Name+"_";
    }

  }

}
