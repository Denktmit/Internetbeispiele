using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WPFUtils.TreeOnDemand {
  /// <summary>
  /// Interaktionslogik für TreeOnDemand.xaml
  /// </summary>
  public partial class TreeOnDemand : UserControl {
    public TreeOnDemand() {
      InitializeComponent();
    }
  }
  public class NodeTemplateSelector : DataTemplateSelector {
    public override DataTemplate SelectTemplate(object item, DependencyObject container) {
      var xe = item as XElement;
      if (xe == null)
        return null;
      return (container as FrameworkElement).FindResource(xe.Name.LocalName) as DataTemplate;
    }
  }
}
