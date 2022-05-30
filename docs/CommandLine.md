---
layout: default
title: CommandLine
nav_order: 8
---

# Command Line

Text Input in the Revit toolbar.
[/ReviTab/ApplicationRibbon.cs](https://github.com/giobel/ReviTab/blob/master/ReviTab/ApplicationRibbon.cs#L847)

![image](https://user-images.githubusercontent.com/27025848/170902996-7963c64c-ae62-4eca-ae08-80cbd7196d7b.png)


```csharp
/// <summary>
/// Text Box Event handler
/// </summary>
/// <param name="sender"></param>
/// <param name="args"></param>
public void MyTextBoxEnter(object sender, TextBoxEnterPressedEventArgs args)
{
    UIDocument uiDoc = args.Application.ActiveUIDocument;
    Document doc = uiDoc.Document;

    Autodesk.Revit.UI.TextBox textBox = sender as Autodesk.Revit.UI.TextBox;

    string message = textBox.Value.ToString();

    if (message.StartsWith("open"))
    caseSwitch = "openView";

    switch (caseSwitch){
        case "default":
        MessageBox.Show(textBox.Value.ToString(), "Command Line");
        break;
        case "openView":
        Helpers.OpenView(uiDoc, message);
        break;
    ...}
}
```


Examples:

open S-101 -> open the sheet S-101

*Structural Framing -> select all structural framings in the active view.

*Structural Framing+Length>10000 -> select all structural framings in the active view longer than 10m.

*Walls+Mark=aa -> select all walls with a Mark equal to 'aa'

sheets: all -> select all Sheets

sheets: A101 A103 A201 -> select Sheets by Sheet Number

tblocks: all -> select all Title Blocks