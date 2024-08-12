using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;


namespace ReviTab
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class FormPickSheets : Form
	{

		public string pickedNumbers;
		public string prefix;
		public List<string> sheetNames = new List<string>();
		public List<string> sheetNumbers = new List<string>();
        public List<string> textValue = new List<string>();

        public Dictionary<string, string> dictSheetSetsNames = new Dictionary<string, string>();

        public List<string> printSettings;
        public int pickedPrintSet = 0;


        public FormPickSheets()
		{
			
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
        }

		void Form1Load(object sender, EventArgs e)
		{
			for (int i=0; i<sheetNames.Count; i++){
				sheetCheckedList.Items.Add(sheetNumbers[i] + "-" + sheetNames[i]);
			}

            foreach (string ps in printSettings)
            {
                cBoxPrintSettings.Items.Add(ps);
            }

            foreach (string s in dictSheetSetsNames.Keys)
            {
                comboBoxViewset.Items.Add(s);
            }

        }

		void OkBtnClick(object sender, EventArgs e)
		{
			
			pickedNumbers = tBoxSheetNumbers.Text;
			prefix = tBoxPrefix.Text;
			
			
		}

		void Btn_checkClick(object sender, System.EventArgs e)
		{
			CheckAll();
		}
		
		void Btn_UncheckClick(object sender, System.EventArgs e)
		{
			UncheckAll();
		}

        private void SheetCheckedList_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            	textValue.Add(sheetNumbers[e.Index]);
            else
            	textValue.Remove(sheetNumbers[e.Index]);

            string result = "";
            foreach (string item in textValue)
            {
                result += item + " ";
            }
            tBoxSheetNumbers.Text = result;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UncheckAll();
            string choosenSheetNumbers = dictSheetSetsNames.Values.ElementAt(comboBoxViewset.SelectedIndex);

            List<int> chosen = new List<int>();

            foreach (string s in choosenSheetNumbers.Split(' '))
            {
                chosen.Add(sheetNumbers.IndexOf(s));
            }
            try
            {
                checkOnlyInViewSet(chosen);
            }
            catch
            {
                //do nothing
            }

        }//close method

        #region ComboBox check/uncheck

        /// <summary>
        /// Check only the items at the provided indexes
        /// </summary>
        /// <param name="chosenViewSheetSet"></param>
        public void checkOnlyInViewSet(List<int> chosenViewSheetSet)
        {
            foreach (int i in chosenViewSheetSet)
            {

                sheetCheckedList.SetItemChecked(i, true);
            }
        }

        /// <summary>
        /// Check all items in a check box list
        /// </summary>
        public void CheckAll()
        {
            for (int i = 0; i <= sheetCheckedList.Items.Count - 1; i++)
            {
                //check item
                sheetCheckedList.SetItemChecked(i, true);

            }
        }

        /// <summary>
        /// Uncheck all items in a check box list
        /// </summary>
        public void UncheckAll()
        {
            for (int i = 0; i <= sheetCheckedList.Items.Count - 1; i++)
            {
                //check item
                sheetCheckedList.SetItemChecked(i, false);
            }
        }

        #endregion

        private void cBoxPrintSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            pickedPrintSet = cBoxPrintSettings.SelectedIndex;
        }
    }
}
