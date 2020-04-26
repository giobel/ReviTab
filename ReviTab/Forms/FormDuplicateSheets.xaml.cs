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

        public ObservableCollection<Element> SheetsList { get; set; }

        public FormDuplicateSheets()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void CheckBoxZone_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chkZone = (CheckBox)sender;
            //ZoneText.Text = "Selected Zone Name= " + chkZone.Content.ToString();
            //ZoneValue.Text = "Selected Zone Value= " + chkZone.Tag.ToString();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(SheetsList.Count.ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}
