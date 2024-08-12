using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ReviTab.Forms
{
    /// <summary>
    /// Interaction logic for FormPickFromDropDown.xaml
    /// </summary>
    public partial class FormInputCombobox : Window
    {
        public List<string> ViewSheetList { get; set; }
        public string SelectedViewSheet { get; set; }
        public List<View> ViewTemplateList {get; set;}
        public FormInputCombobox()
        {
            InitializeComponent();           
            this.DataContext = this;
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            if (cboxSourceTemplate.SelectedItem != null)
            {
                SelectedViewSheet = cboxSourceTemplate.SelectedItem.ToString();
            }

            DialogResult = true;
        }

        private void cboxSourceTemplate_LostFocus(object sender, RoutedEventArgs e)
        {
            SelectedViewSheet = cboxSourceTemplate.Text;
        }
    }
}
