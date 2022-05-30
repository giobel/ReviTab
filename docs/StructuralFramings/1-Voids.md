---
layout: default
title: Openings in beams
parent: Structural Framings
nav_order: 1
---

These commands have been developed for a specific project using these families: [open dropbox](https://www.dropbox.com/sh/n5z56p5hnmpxlm0/AADDpVXEmCPbObCBpxR8BUlWa?dl=0). They won't work otherwise. More info [here](https://giobel.notion.site/Beam-Penos-3c0ce8beb0be4e749070a447480e614c)



## ![image](https://raw.githubusercontent.com/giobel/ReviTab/master/ReviTab/Resources/addBeamOpening.png) Place Void by Face
[/Buttons Framings and Walls/VoidByFace.cs](https://github.com/giobel/ReviTab/blob/master/ReviTab/Buttons%20Framings%20and%20Walls/VoidByFace.cs)

Add one ore more voids to a beam face providing the distance(s) from the beam start, mid or end point and its size.
[Video](https://drive.google.com/file/d/1bFOfLDT6K9uV7vxEvi5O8D7sZDU91_z4/preview)

---

## ![image](https://raw.githubusercontent.com/giobel/ReviTab/master/ReviTab/Resources/line.png) Void By Line
[/Buttons Framings and Walls/VoidByLine.cs](https://github.com/giobel/ReviTab/blob/master/ReviTab/Buttons%20Framings%20and%20Walls/VoidByLine.cs)

Place a void by face at the intersection between a 2d line (on plan) and a beam. The opening will acquire distance and size from the line style name (*Peno Size - 705x305* or *Peno Size - 0x305* for circular penetration).

---

## ![image](https://raw.githubusercontent.com/giobel/ReviTab/master/ReviTab/Resources/line.png) Void By Reference Plane
[/Buttons Framings and Walls/VoidByRefPlane.cs](https://github.com/giobel/ReviTab/blob/master/ReviTab/Buttons%20Framings%20and%20Walls/VoidByRefPlane.cs)

Place a void by face at the intersection between a reference plane and a beam. The opening will acquire distance and size from the reference plane style name (*Peno Size - 705x305* or *Peno Size - 0x305* for circular penetration).

---

## ![image](https://raw.githubusercontent.com/giobel/ReviTab/master/ReviTab/Resources/tag.png) 
[/Buttons Framings and Walls/AddTagsApplyUndo.cs](https://github.com/giobel/ReviTab/blob/master/ReviTab/Buttons%20Framings%20and%20Walls/AddTagsApplyUndo.cs)

Modeless window that calculates the distance from the origin to the beam centerpoint and saves it as Mark. The user can accept or discard the changes.

---

## ![image](https://raw.githubusercontent.com/giobel/ReviTab/master/ReviTab/Resources/lock.png) Lock Openings
[/Buttons Framings and Walls/LockOpenings.cs](https://github.com/giobel/ReviTab/blob/master/ReviTab/Buttons%20Framings%20and%20Walls/LockOpenings.cs)

Lock the void families to their corresponding reference plane