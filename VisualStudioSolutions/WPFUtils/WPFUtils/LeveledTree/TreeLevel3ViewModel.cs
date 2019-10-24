using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtils.LeveledTree {

  public class TreeLevel3ViewModel : TreeViewItemViewModel {

    private readonly TreeObject _object;

    public string Name { get { return _object.Name; } }

    // constructor Branch-Object
    public TreeLevel3ViewModel(TreeObject inObject, TreeLevel2ViewModel inParent) : base(inParent, true) {
      _object = inObject;
    }

    protected override void LoadChildren() {
      Console.WriteLine("LoadChildren() at #" + this.ToString() + "#");
      if (_object.SubObjects.Count != 0) {
        foreach (TreeObject obj in _object.SubObjects) {
          if (obj.SubObjects.Count == 0) {
            // child-object has no subObjects -> IsLeaf
            Console.WriteLine("   " + obj.Name + " -> IsLeaf");
            base.Children.Add(new TreeLevel4ViewModel(obj, this));
          }
          else {
            // child-object has no subObjects -> IsBranch
            Console.WriteLine("   " + obj.Name + " -> IsBranch");
            base.Children.Add(new TreeLevel4ViewModel(obj, this));
          }
        }
      }
      else {
        Console.WriteLine("TODO (delete): no subobjects for " + this.Name);
      }
    }

    public override string ToString() {
      return "_TreeObjectViewModel:Name=" + Name + "_";
    }

  }

}
