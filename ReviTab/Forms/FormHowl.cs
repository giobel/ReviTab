
using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace ReviTab
{
    public partial class FormHowl : Form
    {


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TextString { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TextAddress { get; set; }



        public FormHowl(string labeltext)
        {
            InitializeComponent();
            label1.Text = labeltext;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TextString = textBoxMessage.Text;
            TextAddress = textBoxAddress.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}