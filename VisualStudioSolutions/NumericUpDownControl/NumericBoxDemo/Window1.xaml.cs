using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace NumericBoxDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        Person p;
        public Window1()
        {
            InitializeComponent();
            p=new Person { Age = 25 };
            this.DataContext = p;
         }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(p.Age.ToString());
        }

    }
  public class Person : INotifyPropertyChanged, IDataErrorInfo
    {
        Dictionary<string, string> Errors = new Dictionary<string, string>();
        private string name;
        private int age;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }
        public int Age { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region IDataErrorInfo Members

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                string msg = null;
                switch (columnName)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(Name))
                            msg = "Please enter Name";
                        break;
                    default:
                        throw new ArgumentException(
                            "Unrecognized property: " + columnName);
                }
                return msg;
            }
        }

        #endregion
    }
}


