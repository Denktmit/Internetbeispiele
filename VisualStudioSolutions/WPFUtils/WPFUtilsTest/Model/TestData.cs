using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtils;
using WPFUtils.NavigationAccordion;
using WPFUtils.TreeOnDemand;

namespace WPFUtilsTest.Model {

  public class TestData {

    public static WPFUtils.TreeViewWithSearch.TreeObject GetTreeStuffWithSearch() {
      Model.Action rootAction = new Model.Action("rootAction");
      for (int a = 5; a >= 0; a--) {
        Model.Action level1action = new Model.Action("0" + (6 - a) + " - level1action");
        for (int b = 4; b >= 0; b--) {
          Model.Action level2action = new Model.Action("0" + (6 - a) + " - level2action");
          for (int c = 3; c >= 0; c--) {
            Model.Action level3action = new Model.Action("0" + (6 - a) + " - level3action");
            for (int d = 2; d >= 0; d--) {
              Model.Action level4action = new Model.Action("0" + (6 - a) + " - level4action");
              level3action.Children.Add(level4action);
            }
            level2action.Children.Add(level3action);
          }
          level1action.Children.Add(level2action);
        }
        rootAction.Children.Add(level1action);
      }
      return rootAction;
    }

    public static List<WPFUtils.TreeOnDemand.TreeObject> GetTreeContentViewModelData() {
      List<WPFUtils.TreeOnDemand.TreeObject> ret = new List<WPFUtils.TreeOnDemand.TreeObject>();
      TreeObjectTestAction tota = new TreeObjectTestAction("tota - 001", "Dies ist die Beschreibung vom tota001.");
      TreeObjectTestAction temp = null;
      ret.Add(tota);
      for(int a=0; a<5; a++) {
        TreeObjectTestAction totaA = new TreeObjectTestAction("auto_tota A - 00"+a, "(auto) Dies ist die Beschreibung vom tota_A_00"+a+".");
        ret.Add(totaA);
        for (int b=0; b<3; b++) {
          TreeObjectTestAction totaB = new TreeObjectTestAction("auto_tota B - 00" + b, "(auto) Dies ist die Beschreibung vom tota_B_00" + b + ".");
          totaA.SubObjects.Add(totaB);
          for (int c = 0; c < 2; c++) {
            TreeObjectTestAction totaC = new TreeObjectTestAction("auto_tota C - 00" + c, "(auto) Dies ist die Beschreibung vom tota_C_00" + c + ".");
            totaB.SubObjects.Add(totaC);
            if (a == 2 && b == 1 && c == 0) {
              temp = totaC;
            }
            for (int d = 0; d < 5; d++) {
              TreeObjectTestAction totaD = new TreeObjectTestAction("auto_tota D - 00" + d, "(auto) Dies ist die Beschreibung vom tota_D_00" + d + ".");
              totaC.SubObjects.Add(totaD);
              for (int e = 0; e < 5; e++) {
                TreeObjectTestAction totaE = new TreeObjectTestAction("auto_tota E - 00" + e, "(auto) Dies ist die Beschreibung vom tota_E_00" + e + ".");
                totaD.SubObjects.Add(totaE);
                for (int f = 0; f < 50; f++) {
                  TreeObjectTestAction totaF = new TreeObjectTestAction("auto_tota F - 00" + f, "(auto) Dies ist die Beschreibung vom tota_F_00" + f + ".");
                  totaE.SubObjects.Add(totaF);
                }
              }
            }
          }
        }
      }
      for(int i = 0; i < 50; i++) {
        TreeObjectTestAction tt = new TreeObjectTestAction("a2-b1-c0 : " + i, "Dies ist ein tiefer Pfad. Du bist nun auf Level " + (i+4));
        temp.SubObjects.Add(tt);
        temp = tt;
      }
      return ret;
    }

    public static List<WPFUtils.LeveledTree.TreeObject> GetTreeContentViewModelDataLeveledTree() {
      List<WPFUtils.LeveledTree.TreeObject> ret = new List<WPFUtils.LeveledTree.TreeObject>();
      TreeObjectTestActionLT tota = new TreeObjectTestActionLT("tota - 001", "Dies ist die Beschreibung vom tota001.");
      ret.Add(tota);
      for (int a = 0; a < 5; a++) {
        TreeObjectTestActionLT totaA = new TreeObjectTestActionLT("auto_tota A - 00" + a, "(auto) Dies ist die Beschreibung vom tota_A_00" + a + ".");
        ret.Add(totaA);
        for (int b = 0; b < 3; b++) {
          TreeObjectTestActionLT totaB = new TreeObjectTestActionLT("auto_tota B - 00" + b, "(auto) Dies ist die Beschreibung vom tota_B_00" + b + ".");
          totaA.SubObjects.Add(totaB);
          for (int c = 0; c < 2; c++) {
            TreeObjectTestActionLT totaC = new TreeObjectTestActionLT("auto_tota C - 00" + c, "(auto) Dies ist die Beschreibung vom tota_C_00" + c + ".");
            totaB.SubObjects.Add(totaC);
            for (int d = 0; d < 2; d++) {
              TreeObjectTestActionLT totaD = new TreeObjectTestActionLT("auto_tota D - 00" + d, "(auto) Dies ist die Beschreibung vom tota_D_00" + d + ".");
              totaC.SubObjects.Add(totaD);
            }
          }
        }
      }
      return ret;
    }

    public static ObservableCollection<INavigationItem> GetNavigationAccordionData(NavigationAccordionViewModel inNAVM) {
      ObservableCollection<INavigationItem> ret = new ObservableCollection<INavigationItem>();
      // startPage
      ret.Add(new NavigationItem("StartPage", inNAVM));
      // List
      NavigationItem temp = new NavigationItem("Personen", inNAVM);
      temp.AppendNewSubItem("Person anlegen");
      temp.AppendNewSubItem("Person ändern");
      temp.AppendNewSubItem("Person löschen");
      ret.Add(temp);
      temp = new NavigationItem("Konten", inNAVM);
      temp.AppendNewSubItem("Konto einsehen");
      temp.AppendNewSubItem("Konto anlegen");
      temp.AppendNewSubItem("Transaktion ausführen");
      temp.AppendNewSubItem("Bierliste eintragen");
      temp.AppendNewSubItem("Barliste eintragen");
      temp.AppendNewSubItem("Aktivenessen abrechnen");
      ret.Add(temp);
      temp = new NavigationItem("Semester", inNAVM);
      temp.AppendNewSubItem("Semester ändern");
      temp.AppendNewSubItem("Veranstaltung planen");
      temp.AppendNewSubItem("Schicht erstellen");
      temp.AppendNewSubItem("Schicht einteilen");
      temp.AppendNewSubItem("Entschuldigungen bearbeiten");
      temp.AppendNewSubItem("Einteilen");
      ret.Add(temp);
      temp = new NavigationItem("Lernmaterial", inNAVM);
      temp.AppendNewSubItem("Lernmaterial anschauen");
      temp.AppendNewSubItem("Lernmaterial erstellen");
      temp.AppendNewSubItem("Lehrer suchen");
      ret.Add(temp);
      temp = new NavigationItem("Verbindungen", inNAVM);
      temp.AppendNewSubItem("Verbindung ansehen");
      temp.AppendNewSubItem("Verbindung anlegen");
      temp.AppendNewSubItem("Dachverband ansehen");
      temp.AppendNewSubItem("Dachverband anlegen");
      temp.AppendNewSubItem("Zugehörigkeit erstellen");
      ret.Add(temp);
      temp = new NavigationItem("Hochschulen", inNAVM);
      temp.AppendNewSubItem("Hochschule erstellen");
      temp.AppendNewSubItem("Hochschule ändern");
      temp.AppendNewSubItem("Studiengang erstellen");
      temp.AppendNewSubItem("Studiengang ändern");
      temp.AppendNewSubItem("Studiengang wechseln");
      return ret;
    }

  }

  public class TreeObjectTestActionLT : ModelBase, WPFUtils.LeveledTree.TreeObject {
    private string _name;
    private string _desc;
    private List<object> _subObjects;

    public string Name { get { return _name; } set { _name = value; } }
    public string Description { get { return _desc; } set { _desc = value; OnPropertyChanged(); } }
    public List<object> SubObjects { get { return _subObjects; } }
    public object Object { get { return this; } }

    public TreeObjectTestActionLT(string inName, string inDesc) {
      Name = inName;
      Description = inDesc;
      _subObjects = new List<object>();
    }
  }

  public class TreeObjectTestAction : ModelBase, WPFUtils.TreeOnDemand.TreeObject {
    private string _name;
    private string _desc;
    private List<object> _subObjects;

    public string Name { get { return _name; } set { _name = value; } }
    public string Description { get { return _desc; } set { _desc = value; OnPropertyChanged(); } }
    public List<object> SubObjects { get { return _subObjects; } }
    public object Object { get { return this; } }

    public TreeObjectTestAction(string inName, string inDesc) {
      Name = inName;
      Description = inDesc;
      _subObjects = new List<object>();
    }
  }

  public class TestAction : ModelBase {

    private string _name;
    private string _desc;

    public string Name { get { return _name; } set { _name = value; OnPropertyChanged(); } }
    public string Description { get { return _desc; } set { _desc = value; OnPropertyChanged(); } }

    public TestAction(string inName, string inDescription) {
      Name = inName;
      Description = inDescription;
    }
  }

  public class Action : ModelBase, WPFUtils.TreeViewWithSearch.TreeObject {
    private List<WPFUtils.TreeViewWithSearch.TreeObject> _children;
    private string _actionName;
    public List<WPFUtils.TreeViewWithSearch.TreeObject> Children { get { return _children; } set { _children = value; OnPropertyChanged(); } }
    public string ActionName { get { return _actionName; } set { _actionName = value; OnPropertyChanged(); } }
    public string Name { get { return _actionName; } set { _actionName = value; OnPropertyChanged(); } }
    public Action() {
      ActionName = "New Action";
      Children = new List<WPFUtils.TreeViewWithSearch.TreeObject>();
    }
    public Action(string inActionName) {
      ActionName = inActionName;
      Children = new List<WPFUtils.TreeViewWithSearch.TreeObject>();
    }
    public Action(string inActionName, List<WPFUtils.TreeViewWithSearch.TreeObject> inChildren) {
      ActionName = inActionName;
      Children = inChildren;
    }
  }

}
