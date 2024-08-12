using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace ReviTab.Forms
{
    /// <summary>
    /// Interaction logic for FormPickFromDropDown.xaml
    /// </summary>
    public partial class FormPickFromDropDown : Window
    {
        public ObservableCollection<View> ViewSectionList { get; set; }
        public View SelectedViewSection { get; set; }

        public FormPickFromDropDown()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            SelectedViewSection = cboxViewSection.SelectedItem as View;
            DialogResult = true;
        }
    }
}
