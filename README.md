# ReviTab
WIP toolbar for Revit. 

![image](https://user-images.githubusercontent.com/27025848/160990246-d376e8cd-b8a3-4b4b-b5c7-de72a09704a3.png)

## DOCUMENTATION
![image](https://user-images.githubusercontent.com/27025848/161169121-58d85f10-16ed-46c7-9ecc-9d82976c1f98.png)

## TOOLS

![image](https://user-images.githubusercontent.com/27025848/161169145-a438a256-4c28-42c9-86a9-df168cd945e3.png)

# STRUCTURAL FRAMINGS



# WALLS

## Split Wall by Levels
Copy a wall in place and set its Top and Base constraints to the level it intersects. 
Note: 
1. The wall should not have a top/bottom offset applied; 
2. The original wall will be deleted. 

[![Watch the video](https://img.youtube.com/vi/gcMeTedRd2o/maxresdefault.jpg)](https://youtu.be/gcMeTedRd2o)

# GEOMETRY

## Element to DirectShape
![alt text](images/Flatten.gif)

## Project Lines to Surface

## Draw Axis

# COMMAND LINE

Call methods directly:
keywords: 
* select
+ create
- delete

\> larger

< shorter

= equal

! not equal

examples:

\*Structural Framing -> select all structural framings in the active view.

\*Structural Framing+Length>10000 -> select all structural framings in the active view longer than 10m. 

\*Walls+Mark=aa -> select all walls with a Mark equal to 'aa'

sheets: all -> select all Sheets

sheets: A101 A103 A201 -> select Sheets by Sheet Number

tblocks: all -> select all Title Blocks

[![Watch the video](https://img.youtube.com/vi/axukGCgBRys/maxresdefault.jpg)](https://youtu.be/axukGCgBRys)

[![Watch the video](https://img.youtube.com/vi/56_nDryHPzA/maxresdefault.jpg)](https://youtu.be/56_nDryHPzA)

# ZERO STATE

## Push to DB
Export selected parameters to a [db](https://remotemysql.com)

![IMG](https://i.imgur.com/DLuhkuM.png)

Data can be then visualized with online dashboards like [grafana](https://giobel.grafana.net/d/TS8vWBriz/project-2?orgId=1) 

![IMG](https://i.imgur.com/QzCrmAW.png)

or [desktop](https://github.com/giobel/rvtDashboard) apps

![IMG](https://i.imgur.com/NFat1uW.png)

## Background Print
Open a model in background and print its drawings. The printer setting should be already defined in the Revit model. The default printer should be Bluebeam.
[![Watch the video](https://img.youtube.com/vi/dBtYdCITQhw/maxresdefault.jpg)](https://youtu.be/dBtYdCITQhw)

## Purge Families
Purge families and leave only a type called Default. Requires the Purge Unused that can be found in the Revit Purge Unused branch. Credits: Matt Taylor https://gitlab.com/MattTaylor/RevitPurgeUnused/blob/master/PurgeTool.vb

## Installation
Extract the content of the release zip file in %AppData%\Autodesk\Revit\AddIns\20xx
