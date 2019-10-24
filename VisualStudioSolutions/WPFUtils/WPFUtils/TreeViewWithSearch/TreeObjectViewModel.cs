using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtils.TreeViewWithSearch {

  public class TreeObjectViewModel : ModelBase {

    #region Data
    readonly ReadOnlyCollection<TreeObjectViewModel> _children;
    readonly TreeObjectViewModel _parent;
    readonly TreeObject _treeObject;
    bool _isExpanded;
    bool _isSelected;

    public ReadOnlyCollection<TreeObjectViewModel> Children { get { return _children; } }
    public string Name { get { return _treeObject.Name; } }
    #endregion Data

    #region Constructors
    public TreeObjectViewModel(TreeObject inTopObject)
        : this(inTopObject, null) {
    }
    private TreeObjectViewModel(TreeObject inTopObject, TreeObjectViewModel parent) {
      _treeObject = inTopObject;
      _parent = parent;
      _children = new ReadOnlyCollection<TreeObjectViewModel>(
              (from child in _treeObject.Children
               select new TreeObjectViewModel(child, this))
               .ToList<TreeObjectViewModel>());
    }
    #endregion // Constructors

    #region Presentation_Members
    #region IsExpanded
    public bool IsExpanded {
      get { return _isExpanded; }
      set {
        if (value != _isExpanded) {
          _isExpanded = value;
          this.OnPropertyChanged("IsExpanded");
        }
        // Expand all the way up to the root.
        if (_isExpanded && _parent != null)
          _parent.IsExpanded = true;
      }
    }
    #endregion IsExpanded

    #region IsSelected
    public bool IsSelected {
      get { return _isSelected; }
      set {
        if (value != _isSelected) {
          _isSelected = value;
          this.OnPropertyChanged("IsSelected");
        }
      }
    }
    #endregion IsSelected

    #region NameContainsText
    public bool NameContainsText(string text) {
      if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(this.Name)) {
        return false;
      }
      return this.Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;
    }
    #endregion NameContainsText

    #region Parent
    public TreeObjectViewModel Parent {
      get { return _parent; }
    }
    #endregion Parent
    #endregion Presentation_Members

  }

}
