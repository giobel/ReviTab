
using System;
using System.Windows.Forms;

namespace ReviTab
{
    public partial class FormHowl : Form
    {


        public string TextString { get; set; }
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