---
layout: default
title: Sections
parent: Tools
nav_order: 1
---

## Column Sections

Columns are point based elements. An offset is applied to the location point to create the section box

```c#
//Start Point
XYZ p = lp.Point + xDir * 2;
//End Point
XYZ q = lp.Point - xDir * 2;
XYZ v = q - p;
//section alignment, section height, section depth on plan (far clip offset)
XYZ min = new XYZ(-v.GetLength() / 2 - 1, bottomZ, offsetFromAlignment);
XYZ max = new XYZ(v.GetLength() / 2 + 1, topZ, farClip);
```
![image](https://user-images.githubusercontent.com/27025848/165198015-0f0239b5-c2b5-4bed-a729-58836088c494.png)

## Line based sections

Works with walls, structural framings and lines.

## Flip Sections

Change the direction of a multiple sections. 

