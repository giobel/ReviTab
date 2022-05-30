---
layout: default
title: Excel Link
parent: Documentation
nav_order: 5
---

## ![image](https://raw.githubusercontent.com/giobel/ReviTab/master/ReviTab/Resources/excel.png) Data to/from Excel
[/Buttons Excel/SelectedDataToExcel.cs](https://github.com/giobel/ReviTab/tree/master/ReviTab/Buttons%20Excel/SelectedDataToExcel.cs)
[/Buttons Excel/UpdateDataFromExcel.cs](https://github.com/giobel/ReviTab/tree/master/ReviTab/Buttons%20Excel/UpdateDataFromExcel.cs)

Export/Import selected elements parameters values to/from Excel for easy manipulation. The parameters to be exported must be saved in a file called [RevitSettings.csv](https://www.dropbox.com/s/55vd4k3nx9agahk/RevitSettings.csv?dl=0) in *C:\Temp* folder:

```csharp
#Category, Parameter1, Parameter2, 
#Sheets, Sheet Number, Sheet Name, Appears In Sheet List, Sheet Issue Date
Sheets, Sheet Number, Package, Sheet Name 02 - DispSub, Sheet Name, Current Revision VT, Approved By,
Views, View Name, View Template, Scope Box, Title on Sheet, Sheet Number, Family and Type
Viewports, Sheet Number, Detail Number
Walls, Base Offset, Volume, Comments, Area, Family and Type, Model Name, Element Author, Mark
Title Blocks, Sheet Number, Sheet Name, Scalebar on off, North Point on off
...
```

---

## ![image](https://raw.githubusercontent.com/giobel/ReviTab/master/ReviTab/Resources/excel.png) Viewport to/from Excel
[/Buttons Excel/ViewportsToExcel.cs](https://github.com/giobel/ReviTab/tree/master/ReviTab/Buttons%20Excel/ViewportsToExcel.cs)
[/Buttons Excel/UpdateViewportsFromExcel.cs](https://github.com/giobel/ReviTab/tree/master/ReviTab/Buttons%20Excel/UpdateViewportsFromExcel.cs)

Export/Import selected viewports info to/from Excel. The parameters to be exported must be saved in a file called *RevitSettings.csv* in C:\Temp folder.

---

## ![image](https://raw.githubusercontent.com/giobel/ReviTab/master/ReviTab/Resources/excel.png) Select from/Excel
[/Buttons Excel/SelectFromExcel.cs](https://github.com/giobel/ReviTab/tree/master/ReviTab/Buttons%20Excel/SelectFromExcel.cs)

Select elements in the project from a list of IDs saved in the clipboard from Excel (Ctrl+C the Ids in Excel, then run the command).

---

## ![image](https://raw.githubusercontent.com/giobel/ReviTab/master/ReviTab/Resources/excel.png) Parameter to Excel
[/Buttons Excel/SelectedDataParametersToExcel.cs](https://github.com/giobel/ReviTab/tree/master/ReviTab/Buttons%20Excel/SelectedDataParametersToExcel.cs)

Export selected element parameters to Excel for easy manipulation.