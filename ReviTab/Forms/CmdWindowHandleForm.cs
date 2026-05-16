using System.Windows.Forms;
using System.ComponentModel;

namespace ReviTab
{
  public partial class CmdWindowHandleForm : Form
  {
    public CmdWindowHandleForm()
    {
      InitializeComponent();
    }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string LabelText
    {
      get
      {
        return label1.Text;
      }
      set
      {
        label1.Text = value;
      }
    }
  }
}
