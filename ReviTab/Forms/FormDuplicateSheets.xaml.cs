using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ReviTab.Forms
{
    /// <summary>
    /// Interaction logic for FormDuplicateSheets.xaml
    /// </summary>
    public partial class FormDuplicateSheets : Window
    {

        public ObservableCollection<ViewSheet> SheetsList { get; set; }
        public IList<ViewSheet> SelectedSheets { get; set; }
        public string textSuffix { get; internal set; }

        public FormDuplicateSheets()
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedSheets = new List<ViewSheet>();
        }

        private void CheckBoxZone_Checked(object sender, RoutedEventArgs e)
        {
            //CheckBox chkZone = (CheckBox)sender;
            //selectedSheets.Add(chkZone.Content as ViewSheet);
            //ZoneText.Text = "Selected Zone Name= " + chkZone.Content.ToString();
            //ZoneValue.Text = "Selected Zone Value= " + chkZone.Tag.ToString();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedSheets.Clear();
            foreach (ViewSheet element in listBoxZone.SelectedItems)
            {
                SelectedSheets.Add(element);
            }
            //MessageBox.Show(selectedSheets.Count.ToString());
            textSuffix = tboxSuffix.Text;
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}
