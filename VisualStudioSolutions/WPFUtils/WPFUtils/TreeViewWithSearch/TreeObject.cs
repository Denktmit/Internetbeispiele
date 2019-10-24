using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtils.TreeViewWithSearch {

  public interface TreeObject {
    List<TreeObject> Children { get; set; }
    string Name { get; set; }
  }
}
