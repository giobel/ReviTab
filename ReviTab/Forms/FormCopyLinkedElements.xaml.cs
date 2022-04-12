using Autodesk.Revit.UI;
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

namespace ReviTab.Forms
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class FormCopyLinkedElements : Window
    {
        StackPanel innerStack;
        List<string> categories;
        public FormCopyLinkedElements(List<string> categoriesNames)
        {
            categories = categoriesNames;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                innerStack = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };

                //List<Course> courses = ldc.Courses.ToList();

                foreach (var catName in categories)
                {
                    CheckBox cb = new CheckBox();
//                    cb.Name = catName;
                    cb.Content = catName;
                    innerStack.Children.Add(cb);
                }
                Grid.SetColumn(innerStack, 0) /*Set the column of your stackPanel, default is 0*/;
                Grid.SetRow(innerStack, 0)  /*Set the row of your stackPanel, default is 0*/;
                //Grid.SetColumnSpan(innerStack,  /*Set the columnSpan of your stackPanel, default is 1*/);
                //Grid.SetRowSpan(innerStack,  /*Set the rowSpan of your stackPanel, default is 1*/);
                content.Children.Add(innerStack);

            }
            catch(Exception ex) { TaskDialog.Show("Error", ex.Message); };
        }
    }
}
