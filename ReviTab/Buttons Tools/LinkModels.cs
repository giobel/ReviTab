using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReviTab
{
    [Transaction(TransactionMode.Manual)]
    public class LinkModels : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            
                        string folderPath = @"C:\APAM Models";
            //string searchText = "B-TS1";

            string? searchText = Prompt.ShowDialog("Enter search text:", "Search RVT Files");

            if (string.IsNullOrWhiteSpace(searchText))
                return;

            string searchPattern = $"*{searchText}*.rvt";

    
            List<string> filePaths = Directory.EnumerateFiles(
                folderPath,
                searchPattern,
                SearchOption.TopDirectoryOnly)
                .ToList();

             //TaskDialog.Show("R",searchText);

            LinkModels.Links(doc, filePaths);

            return Result.Succeeded;

        }//close Execute


    }
public static class Prompt
{
    public static string? ShowDialog(string text, string caption)
    {
        Form form = new Form()
        {
            Width = 400,
            Height = 150,
            Text = caption,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            StartPosition = FormStartPosition.CenterScreen,
            MinimizeBox = false,
            MaximizeBox = false
        };

        Label label = new Label()
        {
            Left = 10,
            Top = 15,
            Text = text,
            AutoSize = true
        };

        TextBox textBox = new TextBox()
        {
            Left = 10,
            Top = 40,
            Width = 360
        };

        Button buttonOk = new Button()
        {
            Text = "OK",
            Left = 210,
            Width = 75,
            Top = 75,
            DialogResult = DialogResult.OK
        };

        Button buttonCancel = new Button()
        {
            Text = "Cancel",
            Left = 295,
            Width = 75,
            Top = 75,
            DialogResult = DialogResult.Cancel
        };

        form.Controls.Add(label);
        form.Controls.Add(textBox);
        form.Controls.Add(buttonOk);
        form.Controls.Add(buttonCancel);

        form.AcceptButton = buttonOk;
        form.CancelButton = buttonCancel;

        return form.ShowDialog() == DialogResult.OK
            ? textBox.Text
            : null;
    }
}
}
