---
layout: default
title: Info
parent: ZeroState
nav_order: 4
---

[/Buttons Zero State/VersionInfo.cs](https://github.com/giobel/ReviTab/blob/master/ReviTab/Buttons%20Zero%20State/VersionInfo.cs)

Save links to useful project folders:

![versionInfo](https://user-images.githubusercontent.com/27025848/170897355-d0252bdf-90c8-4acf-b958-29b2c43cd520.png)

File path can be hardcoded or taken from text notes of a dedicated type:

```csharp
			IEnumerable<Element> listOfElements = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_TextNotes)
				.WhereElementIsNotElementType()
				.ToElements().Where(e => e.Name == "FolderLink");
```

![image](https://user-images.githubusercontent.com/27025848/170897665-220e4965-6db6-43cc-8e4b-2f74706f88d3.png)