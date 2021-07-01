using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ReviTab.Forms
{
    /// <summary>
    /// Interaction logic for FormCopyViewFilter.xaml
    /// </summary>
    public partial class FormCopyViewFilter : Window
    {
        public List<View> ViewTemplateList { get; set; }
        public ObservableCollection<FilterElement> ViewFiltersList { get; set; }
        public List<View> TargetTemplate{ get; set; }
        public View SelectedViewSource { get; set; }
        public List<FilterElement> SelectedFilterElement { get; set; }
        public List<View> SelectedTargetTemplate { get; set; }
        private Document _doc = null;
        public FormCopyViewFilter(Document doc)
        {
            InitializeComponent();
            this.DataContext = this;
            _doc = doc;
            ViewFiltersList = new ObservableCollection<FilterElement>();
            gBoxViewFilters.Visibility = System.Windows.Visibility.Hidden;
            gBoxTargetTemplate.Visibility = System.Windows.Visibility.Hidden;
        }

        private void CboxSourceTemplate_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ViewFiltersList != null)
                ViewFiltersList.Clear();

            SelectedViewSource = cboxSourceTemplate.SelectedItem as View;

            if (SelectedViewSource != null && SelectedViewSource.HasDisplayStyle() == true )
            {
                ICollection<ElementId> filters = SelectedViewSource.GetFilters();
                List<FilterElement> filterElements = new List<FilterElement>();

                foreach (ElementId elementId in filters)
                {
                    filterElements.Add(_doc.GetElement(elementId) as FilterElement);
                    ViewFiltersList.Add(_doc.GetElement(elementId) as FilterElement);
                }                
            }                
        }


        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            SelectedViewSource = cboxSourceTemplate.SelectedItem as View;
            
            SelectedTargetTemplate = new List<View>();

            foreach (var item in targetTemplatesList.SelectedItems)
            {                
                SelectedTargetTemplate.Add(item as View);
            }

            SelectedFilterElement = new List<FilterElement>();

            foreach (var item in filterList.SelectedItems)
            {
                SelectedFilterElement.Add(item as FilterElement);
            }

            DialogResult = true;
        }

        private void FilterList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {            
            gBoxTargetTemplate.Visibility = System.Windows.Visibility.Visible;
        }

        private void CboxSourceTemplate_DropDownOpened(object sender, System.EventArgs e)
        {
            gBoxViewFilters.Visibility = System.Windows.Visibility.Visible;
        }


        private void Label_SelectAll_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            targetTemplatesList.SelectAll();
            
        }

        private void Label_ClearSelection_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            targetTemplatesList.UnselectAll();

        }
    }
}
