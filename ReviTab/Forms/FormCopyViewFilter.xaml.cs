using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ReviTab.Forms
{
    /// <summary>
    /// Interaction logic for FormCopyViewFilter.xaml
    /// </summary>
    public partial class FormCopyViewFilter : Window
    {
        public ObservableCollection<View> ViewTemplateList { get; set; }
        public ObservableCollection<FilterElement> ViewFiltersList { get; set; }
        public ObservableCollection<View> TargetTemplate{ get; set; }
        public View SelectedViewSource { get; set; }
        public FilterElement SelectedFilterElement { get; set; }
        public View SelectedTargetTemplate { get; set; }
        private Document _doc = null;
        public FormCopyViewFilter(Document doc)
        {
            InitializeComponent();
            this.DataContext = this;
            _doc = doc;
            ViewFiltersList = new ObservableCollection<FilterElement>();
        }

        private void CboxSourceTemplate_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ViewFiltersList != null)
                ViewFiltersList.Clear();

            SelectedViewSource = cboxSourceTemplate.SelectedItem as View;

            if (SelectedViewSource != null)
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
            SelectedFilterElement = cboxFilterSection.SelectedItem as FilterElement;
            SelectedTargetTemplate = cboxViewSection.SelectedItem as View;
            DialogResult = true;
        }

        
    }
}
