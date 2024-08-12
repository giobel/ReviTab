/*
 * Created by SharpDevelop.
 * User: Joshua.Lumley
 * Date: 8/09/2017
 * Time: 11:45 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Autodesk.Windows;

namespace ReviTab
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class FormPlaceTags : System.Windows.Forms.Form
	{

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();


        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(
          IntPtr hWnd);

        public Document doc { get; set; }
			
			ButtonPlaceTags placeTagsParameter;
			ExternalEvent placeTagsEvent;
			
			ButtonUndoChanges undoChangesParameter;
			ExternalEvent undoChangesEvent;
			
			ButtonAcceptChanges acceptChangesParameter;
			ExternalEvent acceptChangesEvent;

            
	public FormPlaceTags()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			
            placeTagsParameter = new ButtonPlaceTags();
            placeTagsEvent = ExternalEvent.Create(placeTagsParameter);
            
            undoChangesParameter = new ButtonUndoChanges();
            undoChangesEvent = ExternalEvent.Create(undoChangesParameter);
            
            acceptChangesParameter = new ButtonAcceptChanges();
            acceptChangesEvent = ExternalEvent.Create(acceptChangesParameter);
            
		}

		void ButtonAcceptChangesClick(object sender, EventArgs e)
		{
			acceptChangesEvent.Raise();

            IntPtr hBefore = GetForegroundWindow();

            SetForegroundWindow(ComponentManager.ApplicationWindow);

            SetForegroundWindow(hBefore);
        }
		
		void ButtonUndoChangesClick(object sender, EventArgs e)
		{
			undoChangesEvent.Raise();

            IntPtr hBefore = GetForegroundWindow();

            SetForegroundWindow(ComponentManager.ApplicationWindow);

            SetForegroundWindow(hBefore);
        }
		
		void ButtonPlaceTagsClick(object sender, EventArgs e)
		{
			placeTagsEvent.Raise();

            IntPtr hBefore = GetForegroundWindow();

            SetForegroundWindow(ComponentManager.ApplicationWindow);

            SetForegroundWindow(hBefore);
        }

	}//close class

}//close namespace
