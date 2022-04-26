---
layout: default
title: Openings in beams
parent: StructuralFramings
nav_order: 1
---

These commands have been developed for a specific project using these families: [open dropbox](https://www.dropbox.com/sh/n5z56p5hnmpxlm0/AADDpVXEmCPbObCBpxR8BUlWa?dl=0). They won't work otherwise. More info [here](https://giobel.notion.site/Beam-Penos-3c0ce8beb0be4e749070a447480e614c)

## Place Void by Face

Add one ore more voids to a beam face providing the distance(s) from the beam start, mid or end point and its size.

[![](https://res.cloudinary.com/marcomontalbano/image/upload/v1650950655/video_to_markdown/images/google-drive--1bFOfLDT6K9uV7vxEvi5O8D7sZDU91_z4-c05b58ac6eb4c4700831b2b3070cd403.jpg)](https://drive.google.com/file/d/1bFOfLDT6K9uV7vxEvi5O8D7sZDU91_z4/preview "")


## Void By Line

Place a void by face at the intersection between a 2d line (on plan) and a beam. The opening will acquire distance and size from the line style name (*Peno Size - 705x305* or *Peno Size - 0x305* for circular penetration).
<img src="https://camo.githubusercontent.com/46afabc5e67c8277471a67620f9d401180f7027f6da766d1bb08b25975513d5d/68747470733a2f2f696d672e796f75747562652e636f6d2f76692f6a767371554a47337548412f6d617872657364656661756c742e6a7067" alt="Watch the video" data-canonical-src="https://img.youtube.com/vi/jvsqUJG3uHA/maxresdefault.jpg" style="max-width: 100%;">

## Void By Reference Plane

Place a void by face at the intersection between a reference plane and a beam. The opening will acquire distance and size from the reference plane style name (*Peno Size - 705x305* or *Peno Size - 0x305* for circular penetration).

## Place Tags

Modeless window that calculates the distance from the origin to the beam centerpoint and saves it as Mark. The user can accept or discard the changes.

## ![image](https://raw.githubusercontent.com/giobel/ReviTab/master/ReviTab/Resources/lock.png) Lock Openings

Lock the void families to their corresponding reference plane