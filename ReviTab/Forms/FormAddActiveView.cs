
using System;
using System.Windows.Forms;

namespace ReviTab
{
    public partial class FormAddActiveView : Form
    {


        public string TextString { get; set; }



        public FormAddActiveView(string labeltext)
        {
            InitializeComponent();
            label1.Text = labeltext;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TextString = textBox1.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}