
using System;
using System.Windows.Forms;

namespace ReviTab
{
    public partial class FormTwoTextBoxes : Form
    {
        public string TextString { get; set; }
        public string TextString2 { get; set; }


        public FormTwoTextBoxes(string labeltext, string label2text)
        {
            InitializeComponent();
            label1.Text = labeltext;
            label2.Text = label2text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TextString = textBox1.Text;
            TextString2 = textBox2.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}