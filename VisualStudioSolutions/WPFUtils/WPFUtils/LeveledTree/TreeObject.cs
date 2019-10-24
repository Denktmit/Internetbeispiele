using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtils.LeveledTree {

  public interface TreeObject {
    string Name { get; }
    object Object { get; }
    List<object> SubObjects { get; }
  }

}
