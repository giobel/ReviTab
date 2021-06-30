using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace ReviTab.Forms
{
    /// <summary>
    /// Interaction logic for FormCopyViewFilter.xaml
    /// </summary>
    public partial class FormCopyViewFilter : Window
    {
        public ObservableCollection<FilterElement> ViewFiltersList { get; set; }
        public ObservableCollection<View> TargetTemplate{ get; set; }
        public FilterElement SelectedFilterElement { get; set; }
        public View SelectedTargetTemplate { get; set; }

        public FormCopyViewFilter()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            SelectedFilterElement = cboxFilterSection.SelectedItem as FilterElement;
            SelectedTargetTemplate = cboxViewSection.SelectedItem as View;
            DialogResult = true;
        }
    }
}
