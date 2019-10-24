using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtils.LeveledTree {


  public class TreeViewItemViewModel : INotifyPropertyChanged {

    static readonly TreeViewItemViewModel DummyChild = new TreeViewItemViewModel();
    readonly ObservableCollection<TreeViewItemViewModel> _children;
    readonly TreeViewItemViewModel _parent;
    bool _isExpanded;
    bool _isSelected;
    protected TreeViewItemViewModel(TreeViewItemViewModel parent, bool lazyLoadChildren) {
      _parent = parent;

      _children = new ObservableCollection<TreeViewItemViewModel>();

      if (lazyLoadChildren)
        _children.Add(DummyChild);
    }
    // This is used to create the DummyChild instance.
    private TreeViewItemViewModel() {
    }
    /// <summary>
    /// Returns the logical child items of this object.
    /// </summary>
    public ObservableCollection<TreeViewItemViewModel> Children {
      get { Console.WriteLine("Children of <" + this.ToString() + ">"); return _children; }
    }
    /// <summary>
    /// Returns true if this object's Children have not yet been populated.
    /// </summary>
    public bool HasDummyChild {
      get { Console.WriteLine("HasDummyChild? at <" + this.ToString() + ">"); return this.Children != null && this.Children.Count == 1 && this.Children[0] == DummyChild; }
    }
    /// <summary>
    /// Gets/sets whether the TreeViewItem 
    /// associated with this object is expanded.
    /// </summary>
    public bool IsExpanded {
      get { return _isExpanded; }
      set {
        if (value != _isExpanded) {
          _isExpanded = value;
          this.OnPropertyChanged("IsExpanded");
          Console.WriteLine("IsExpanded - changed to <" + _isExpanded + "> at <" + this.ToString() + ">");
        }

        // Expand all the way up to the root.
        if (_isExpanded && _parent != null)
          _parent.IsExpanded = true;

        // Lazy load the child items, if necessary.
        if (this.HasDummyChild) {
          Console.WriteLine("Remove(DummyChild) at <" + this.ToString() + ">");
          this.Children.Remove(DummyChild);
          Console.WriteLine("LoadChildren at <" + this.ToString() + ">");
          this.LoadChildren();
        }
      }
    }
    /// <summary>
    /// Gets/sets whether the TreeViewItem 
    /// associated with this object is selected.
    /// </summary>
    public bool IsSelected {
      get { return _isSelected; }
      set {
        if (value != _isSelected) {
          _isSelected = value;
          this.OnPropertyChanged("IsSelected");
          Console.WriteLine("IsSelected - changed to <" + _isSelected + "> at <" + this.ToString() + ">");
        }
      }
    }
    /// <summary>
    /// Invoked when the child items need to be loaded on demand.
    /// Subclasses can override this to populate the Children collection.
    /// </summary>
    protected virtual void LoadChildren() {
    }
    public TreeViewItemViewModel Parent {
      get { Console.WriteLine("Parent of <" + this.ToString() + ">"); return _parent; }
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName) {
      if (this.PropertyChanged != null)
        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

  }

}
