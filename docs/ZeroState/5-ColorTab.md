---
layout: default
title: Color Tab
parent: Zero State
nav_order: 5
---

## ![image](https://raw.githubusercontent.com/giobel/ReviTab/master/ReviTab/Resources/rainbow.png) Color Tab
[/Buttons Zero State/ColorTab.cs](https://github.com/giobel/ReviTab/blob/master/ReviTab/Buttons%20Zero%20State/ColorTab.cs)

All credits to [pyrevit](https://github.com/eirannejad/pyRevit/blob/4afd56ccb4d77e4e0228b8e64d80d1f541bc791e/pyrevitlib/pyrevit/runtime/EventHandling.cs)

Color the tab based on Project and view type

![image](https://user-images.githubusercontent.com/27025848/170897776-65bb0ce7-3aa1-4575-be8d-f38a343dd4b4.png)

Only 4 view types are colored:

```csharp
if (tab.ToolTip.ToString().Contains("Plan:"))
    tab.Background = planBrush;
else if (tab.ToolTip.ToString().Contains("Section:"))
    tab.Background = sectBrush;
else if (tab.ToolTip.ToString().Contains("3D View:"))
    tab.Background = threeDBrush;
else if (tab.ToolTip.ToString().Contains("Sheet:"))
    tab.Background = sheetBrush;
```