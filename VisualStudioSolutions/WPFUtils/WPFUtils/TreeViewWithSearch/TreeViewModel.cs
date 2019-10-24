using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WPFUtils.TreeViewWithSearch {

  public class TreeViewModel : ModelBase {

    #region Data
    readonly ReadOnlyCollection<TreeObjectViewModel> _topLevel;
    readonly TreeObjectViewModel _rootObject;
    readonly ICommand _searchCommand;
    IEnumerator<TreeObjectViewModel> _matchingTreeObjectEnumerator;
    string _searchText = String.Empty;
    private bool _showTree;
    private string _btn_search = "_Suchen";
    #endregion Data

    #region Properties
    public ReadOnlyCollection<TreeObjectViewModel> TopLevel {
      get { return _topLevel; }
    }
    public ICommand SearchCommand {
      get { return _searchCommand; }
    }
    private class SearchTreeCommand : ICommand {
      readonly TreeViewModel _objectTree;
      public SearchTreeCommand(TreeViewModel objectTree) {
        _objectTree = objectTree;
      }
      public bool CanExecute(object parameter) {
        return true;
      }
      event EventHandler ICommand.CanExecuteChanged {
        // I intentionally left these empty because
        // this command never raises the event, and
        // not using the WeakEvent pattern here can
        // cause memory leaks.  WeakEvent pattern is
        // not simple to implement, so why bother.
        add { }
        remove { }
      }
      public void Execute(object parameter) {
        _objectTree.PerformSearch();
      }
    }
    public string SearchText {
      get { return _searchText; }
      set {
        if (value == _searchText) {
          return;
        }
        _searchText = value;
        _matchingTreeObjectEnumerator = null;
        OnPropertyChanged();
      }
    }
    public bool ShowTree { get { return _showTree; } set { _showTree = value; OnPropertyChanged(); } }
    public string Btn_Search { get { return _btn_search; } set { _btn_search = value; OnPropertyChanged(); } }
    #endregion Properties

    #region Constructor
    public TreeViewModel(TreeObject rootTreeObject) {
      _rootObject = new TreeObjectViewModel(rootTreeObject);
      _topLevel = new ReadOnlyCollection<TreeObjectViewModel>(
          new TreeObjectViewModel[] { _rootObject });
      _searchCommand = new SearchTreeCommand(this);
      ShowTree = true;
    }
    #endregion Constructor

    #region Search_Logic
    public void PerformSearch() {
      if (_matchingTreeObjectEnumerator == null || !_matchingTreeObjectEnumerator.MoveNext()) {
        this.VerifyMatchingTreeObjectEnumerator();
      }
      var treeObject = _matchingTreeObjectEnumerator.Current;
      if (treeObject == null) {
        return;
      }
      // Ensure that this treeObject is in view.
      if (treeObject.Parent != null) {
        treeObject.Parent.IsExpanded = true;
      }
      treeObject.IsSelected = true;
    }
    void VerifyMatchingTreeObjectEnumerator() {
      var matches = this.FindMatches(_searchText, _rootObject);
      _matchingTreeObjectEnumerator = matches.GetEnumerator();
      if (!_matchingTreeObjectEnumerator.MoveNext()) {
        MessageBox.Show(
            "No matching names were found.",
            "Try Again",
            MessageBoxButton.OK,
            MessageBoxImage.Information
            );
      }
    }
    IEnumerable<TreeObjectViewModel> FindMatches(string searchText, TreeObjectViewModel treeObject) {
      if (treeObject.NameContainsText(searchText))
        yield return treeObject;
      foreach (TreeObjectViewModel child in treeObject.Children)
        foreach (TreeObjectViewModel match in this.FindMatches(searchText, child))
          yield return match;
    }
    #endregion Search_Logic

  }

}
