﻿#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Linq;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.ExternalService;
using Application = Autodesk.Revit.ApplicationServices.Application;
using System.Windows.Interop;
using System.IO;

#endregion

namespace ReviTab
{
    public class Availability : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(
          UIApplication a,
          CategorySet b)
        {
            return true;
        }
    }



  [Regeneration(RegenerationOption.Manual)]
    class ApplicationRibbon : IExternalApplication
    {

        public Result OnStartup(UIControlledApplication a)
        {

            #if SAM
            string tabName = "SAM";
            #else
            string tabName = "SuperTab";
            #endif

            try
            {
                              
                AddRibbonPanel(a,tabName);

                RibbonPanel docsPanel = GetSetRibbonPanel(a, tabName, "Documentation");

                RibbonPanel toolsPanel = GetSetRibbonPanel(a, tabName, "Tools");

                RibbonPanel beams = GetSetRibbonPanel(a, tabName, "Structural Framings");

                RibbonPanel walls = GetSetRibbonPanel(a, tabName, "Walls");
#if DEBUG
                RibbonPanel geometry = GetSetRibbonPanel(a, tabName, "Geometry");
#endif
                RibbonPanel commandPanel = GetSetRibbonPanel(a, tabName, "Command Line");

                RibbonPanel zeroState = GetSetRibbonPanel(a, tabName, "Zero State");


#region Documentation

                IList<PushButtonData> splitButtonsViews = new List<PushButtonData>();

                splitButtonsViews.Add(CreatePushButton("btnSheetAddCurrentView", "Add View\nto Sheet","","pack://application:,,,/ReviTab;component/Resources/addView.png", "ReviTab.AddActiveViewToSheet",  "Add the active view to a sheet"));
                
                splitButtonsViews.Add(CreatePushButton("btnAddSheetByNumber", "Add Sheet\nby Number", null, Resource1.addSheetByNumber, "ReviTab.CreateSheetByNumber", "Create a Sheet by providing its number and package."));

                splitButtonsViews.Add(CreatePushButton("btnAddMultipleViews", "Add Multiple\nViews","", "pack://application:,,,/ReviTab;component/Resources/addMultipleViews.png", "ReviTab.AddMultipleViewsToSheet", "Add multiple views to a sheet. Select the views in the project browser."));

                splitButtonsViews.Add(CreatePushButton("btnAddLegends", "Add Legend\nto Sheets", "","pack://application:,,,/ReviTab;component/Resources/legend.png", "ReviTab.AddLegendToSheets",  "Place a legend onto multiple sheets in the same place."));

                splitButtonsViews.Add(CreatePushButton("btnCreateViewset", "Create\nViewset", "", "pack://application:,,,/ReviTab;component/Resources/createViewSet.png", "ReviTab.CreateViewSet", "Create a Viewset from a list of Sheet Numbers"));

                splitButtonsViews.Add(CreatePushButton("btnAlignViews", "Align Viewports", null, Resource1.alignViewports, "ReviTab.AlignViews", "Select a list of views in the project browser then click on this button to pick a point on a sheet. The point will be used as a center of all the viewports of the selected views."));

                splitButtonsViews.Add(CreatePushButton("btnAlignSectionCropBox", "Match Section CropView", "", "pack://application:,,,/ReviTab;component/Resources/movement-arrows.png", "ReviTab.MatchSectionViewCrop", "Select a list of sections views in the project browser then click on this button to assign the same cropbox view to all. Section views must all be parallel between themselves.")); 

                splitButtonsViews.Add(CreatePushButton("btnTagInView", "Tag Elements", "", "pack://application:,,,/ReviTab;component/Resources/tag.png", "ReviTab.TagElementsInViewport", "Tag all the columns within the selected Viewports."));
                
                splitButtonsViews.Add(CreatePushButton("btnDuplicateViews", "Duplicate Views", null, Resource1.duplicateView, "ReviTab.DuplicateSheets", "Duplicate selected sheets with viewports, schedules and legends."));

                splitButtonsViews.Add(CreatePushButton("btnExtractDetail", "Extract Detail", null, Resource1.extractDetail, "ReviTab.ExtractDetail", "Cut and Paste lines to a new detail"));

                splitButtonsViews.Add(CreatePushButton("btnMoveViewport", "Move Viewports", null, Resource1.moveViewports, "ReviTab.MoveViewportToSheet", "Move selected viewports to another Sheet"));
                
                splitButtonsViews.Add(CreatePushButton("btnImportRhino", "Rhino Import", null, Resource1.rhinoImport, "ReviTab.RhinoImport", "Import details from Rhino to Revit drafting view."));

                splitButtonsViews.Add(CreatePushButton("btnSetTitleblock", "Set Titleblock\nScale", "", "pack://application:,,,/ReviTab;component/Resources/rulerSmall.png", "ReviTab.SetTitleblockScale", "Set the current sheet titleblock scale to the most used."));

                AddSplitButton(docsPanel, splitButtonsViews, "DocumentationButton", "Documentation");

                //Titleblock revisions

                IList<PushButtonData> stackedButtonsSheets = new List<PushButtonData>
                {
                    CreatePushButton("btnUpRevSheet", "Uprev Sheet", "pack://application:,,,/ReviTab;component/Resources/addRev.png", "", "ReviTab.UpRevSheet", "Up rev the current sheet. It copies the content from the previous revision excluding the date"),

                    CreatePushButton("btnTextFonts", "List TextNotes", "pack://application:,,,/ReviTab;component/Resources/addRev.png", "", "ReviTab.TextFonts", "List all the TextNotes in the project to the active view. Set View Scale to 1:1 to display the list correctly."),

                    CreatePushButton("btnLineStyles", "List Line Styles", "pack://application:,,,/ReviTab;component/Resources/addRev.png", "", "ReviTab.LineStyles", "List all the Line Styles in the project to the active view. Set View Scale to 1:1 to display the list correctly.")
                };

                AddStackedButton(docsPanel, stackedButtonsSheets, "SheetsButton", "Sheets");

#if DEBUG
                //TextFonts and LineStyles
                IList<PushButtonData> stackedButtonsTextAndLines = new List<PushButtonData>
                {

                    CreatePushButton("btnRemoveRev", "Remove first\nRevision", "pack://application:,,,/ReviTab;component/Resources/deleteRev.png", "", "ReviTab.RemoveFirstRevision", "Remove the first revision of a titleblock (i.e. shift revisions down)."),

                    CreatePushButton("btnDeleteTextNotes", "Delete TextNotes Types", "pack://application:,,,/ReviTab;component/Resources/deleteRev.png", "", "ReviTab.DeleteTextFont", "Delete the selcted TextNotes types."),

                    CreatePushButton("btnDeleteLineStyles", "Delete Line Styles", "pack://application:,,,/ReviTab;component/Resources/deleteRev.png", "", "ReviTab.DeleteLineStyles", "Delete the selcted Line Style. Lines with that line style will be changed to Thin Lines.")
                    
                };
                
                AddStackedButton(docsPanel, stackedButtonsTextAndLines, "TextAndLines", "TextLines");            
#endif
                

#if DEBUG
                //DB interop
                IList<PushButtonData> splitButtonsInterop = new List<PushButtonData>();
                
                splitButtonsInterop.Add(CreatePushButton("btnManage", "Update Status", "", "pack://application:,,,/ReviTab;component/Resources/manage.png", "ReviTab.UpdateModelStatus", "Update model status dashboard."));
                
               /* splitButtonsInterop.Add(CreatePushButton("btnAirtable", "Push to Airtable", "", "pack://application:,,,/ReviTab;component/Resources/airtable.png", "ReviTab.PushToAirtable", "Push the Model Status content to Airtable table"));


                splitButtonsInterop.Add(CreatePushButton("btnPush", "Push to DB", "", "pack://application:,,,/ReviTab;component/Resources/arrowUp.png", "ReviTab.PushToDB", "Push date, user, rvtFileSize, elementsCount, typesCount, sheetsCount, viewsCount, viewportsCount, warningsCount to 127.0.0.1"));

                */
                AddSplitButton(docsPanel, splitButtonsInterop, "InteropDBButton", "InteropDB");
#endif
                #endregion

#region ExcelLink
                // Excel interop
                IList<PushButtonData> splitButtonsDataToExcel = new List<PushButtonData>();

                splitButtonsDataToExcel.Add(CreatePushButton("btnDataToExcel", "Data to\nExcel", "", "pack://application:,,,/ReviTab;component/Resources/excel.png", "ReviTab.SelectedDataToExcel", "Export parameters content to Excel for easy manipulation."));

                splitButtonsDataToExcel.Add(CreatePushButton("btnDataFromExcel", "Data from\nExcel", "", "pack://application:,,,/ReviTab;component/Resources/excel.png", "ReviTab.UpdateDataFromExcel", "Update parameter values with data from Excel."));

                splitButtonsDataToExcel.Add(CreatePushButton("btnVportToExcel", "Viewport to\nExcel", "", "pack://application:,,,/ReviTab;component/Resources/excel.png", "ReviTab.ViewportsToExcel", "Export viewports info to Excel."));

                splitButtonsDataToExcel.Add(CreatePushButton("btnVportFromExcel", "Viewport from\nExcel", "", "pack://application:,,,/ReviTab;component/Resources/excel.png", "ReviTab.UpdateViewportsFromExcel", "Update viewports info from Excel."));

                splitButtonsDataToExcel.Add(CreatePushButton("btnSelectFromExcel", "Select from\nExcel", "", "pack://application:,,,/ReviTab;component/Resources/excel.png", "ReviTab.SelectFromExcel", "Select elements in the project from a list of IDs saved in the clipboard from Excel (Ctrl+C the Ids in Excel, then run the command)."));

                splitButtonsDataToExcel.Add(CreatePushButton("btnParametersExcel", "Parameter to\nExcel", "", "pack://application:,,,/ReviTab;component/Resources/excel.png", "ReviTab.SelectedDataParametersToExcel", "Export selected element parameters names and values to Excel for easy manipulation."));

                //Revision Clouds. Parameters are project specific. Removed from toolbar

                splitButtonsDataToExcel.Add(CreatePushButton("btnSetRevCloud", "Rev Cloud\nSummary", "", "pack://application:,,,/ReviTab;component/Resources/excel.png", "ReviTab.RevisionCloudsSummary", "Export revision cloud summary."));

                //View Templates
                splitButtonsDataToExcel.Add(CreatePushButton("btnExportVT", "Export View\nTemplates", "", "pack://application:,,,/ReviTab;component/Resources/excel.png", "ReviTab.ExportViewTemplates", "Export all view templates."));

                AddSplitButton(docsPanel, splitButtonsDataToExcel, "ExcelButton", "ExcelLink");
                #endregion

#region Tools

                IList<PushButtonData> splitButtonsSections = new List<PushButtonData>
                {
                    CreatePushButton("btnCreateSectionsColumns", "Column Sections", null, Resource1.columnSection, "ReviTab.CreateSectionColumns", "Create multiple sections for selected columns."),

                    CreatePushButton("btnCreateSections", "Line based" + Environment.NewLine + "Sections", null, Resource1.lineSection, "ReviTab.CreateSections", "Create multiple sections for line based elements (walls, beams, lines)."),
                    
                    CreatePushButton("btnFlipSections", "Flip Sections", null, Resource1.flipSection, "ReviTab.FlipSections", "Flip multiple sections.")
                };

                AddSplitButton(toolsPanel, splitButtonsSections, "SectionsButton", "MultipleSections");
                
                if (AddPushButton(toolsPanel, "btnRevCloudSelected", "Cloud Selection", null, Resource1.cloudSelection, "ReviTab.RevCloudsSelected", "Cloud the selected elements on sheet") == false)
                {
                    MessageBox.Show("Failed to add button Cloud Selection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //GRIDS

                IList<PushButtonData> gridTools = new List<PushButtonData>
                {
                    CreatePushButton("btnSwapGrid", "Swap Grid" + Environment.NewLine + "Head", "", "pack://application:,,,/ReviTab;component/Resources/swapGrids.png", "ReviTab.SwapGridBubbles", "Swap the head of the selected grids"),

                    CreatePushButton("btnCopyGrid", "Copy Grid Extents", null, Resource1.copyGridExtents, "ReviTab.PropagateGridExtents", "Copy the grid extents from a view to the active one."),
                    
                    CreatePushButton("btnSwapLevels", "Swap Level Bubble", null, Resource1.swapLevels, "ReviTab.SwapLevelsBubbles", "Swap the head of the selected levels")
                };

                AddSplitButton(toolsPanel, gridTools, "gridTools", "Grid Tools");

                //OVERRIDES
                IList<PushButtonData> overrideTools = new List<PushButtonData>
                {
                    CreatePushButton("btnOverrideColoor", "Override Colours", null, Resource1.overrideColors, "ReviTab.OverrideColors", "Override the colours of the structural elements in the active view."),

                    CreatePushButton("btnOverrideDimensions", "Override \nDimension", "", "pack://application:,,,/ReviTab;component/Resources/dimensionOverride.png", "ReviTab.OverrideDimensions", "Override the text of a dimension")
                };
                
                AddSplitButton(toolsPanel, overrideTools, "overrideTools", "Override Tools");

                //LINK FILES
                IList<PushButtonData> linkFiles = new List<PushButtonData>
                {
                    CreatePushButton("btnCopyLinkedElements", "Copy Linked \nElements", "", "pack://application:,,,/ReviTab;component/Resources/copyLinked.png", "ReviTab.CopyLinkedElements", "Copy elements from linked models"),
                    
                   CreatePushButton("btnCopyTemplateFilters", "Copy View Template\nFilters", null, Resource1.copyViewFilter, "ReviTab.CopyViewFilters", "Copy View Template Filters"),

                    CreatePushButton("btnAlignColumns", "Align Columns", "", "pack://application:,,,/ReviTab;component/Resources/alignColumns.png", "ReviTab.AlignColumns", "Align the columns in the model to those selected in a linked model. There is an hardcoded tolerance of 3feet as maximum distance between the linked column and the one to be moved.")
                };

                AddSplitButton(toolsPanel, linkFiles, "linkFiles", "Link Files");

                //FILTER SELECTION
                IList<PushButtonData> filterSelection = new List<PushButtonData>
                {
                    CreatePushButton("selBeams", "Select Beams", Resource1.selectBeams, Resource1.selectBeams, "ReviTab.FilterSelectionBeams", "Select Beams Only"),

                    CreatePushButton("selColumns", "Select Columns", Resource1.selectColumn, Resource1.selectColumn, "ReviTab.FilterSelectionColumns", "Select Columns Only"),

                    CreatePushButton("selDim", "Select Dimensions", Resource1.selectDimensions, Resource1.selectDimensions, "ReviTab.FilterSelectionDimensions", "Select Dimensions Only"),

                    CreatePushButton("selGrids", "Select Grids", Resource1.selectGrid, Resource1.selectGrid, "ReviTab.FilterSelectionGrids", "Select Grids Only"),

                    CreatePushButton("selLines", "Select Lines", Resource1.selectLines, Resource1.selectLines, "ReviTab.FilterSelectionLines", "Select Lines Only"),

                    CreatePushButton("selTags", "Select Tags", Resource1.selectTags, Resource1.selectTags, "ReviTab.FilterSelectionTags", "Select Tags Only"),

                    CreatePushButton("selText", "Select Text", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "ReviTab.FilterSelectionText", "Select Text Only"),

                    CreatePushButton("selWalls", "Select Walls", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "pack://application:,,,/ReviTab;component/Resources/selectFilter.png", "ReviTab.FilterSelectionWalls", "Select Walls Only"),

                    CreatePushButton("btnSelectText", "Select All Text", "pack://application:,,,/ReviTab;component/Resources/selectText.png", "pack://application:,,,/ReviTab;component/Resources/selectText.png", "ReviTab.SelectAllText", "Select all text notes in the project.Useful if you want to run the check the spelling."),

                    CreatePushButton("btnSelectTblocks", "Select Titleblocks", null, Resource1.tblock, "ReviTab.SelectTitleblocks", "Get the Titleblocks of the selected Sheets"),
                    
                    CreatePushButton("btnIsolateCategories", "Isolate Categories", null, Resource1.isoCategory, "ReviTab.IsolateCategories", "Isolate the selected elements categories in the active view"),

                    //<a href="https://www.flaticon.com/free-icons/eye" title="eye icons">Eye icons created by Freepik - Flaticon</a>
                    CreatePushButton("btnShowSelected", "Show Selected", null, Resource1.showSelected, "ReviTab.ShowSelected", "Show the selected elements in their own view"),
                    
                    CreatePushButton("btnShowLastCreatedView", "Show Latest View", null, Resource1.showLatest, "ReviTab.OpenLastCreatedView", "Open the latest created view")

                    };


                

                AddSplitButton(toolsPanel, filterSelection, "filterSelection", "Filter Selection");

                if (AddPushButton(toolsPanel, "btnPrintSelected", "Print Selected", "", "pack://application:,,,/ReviTab;component/Resources/backgroundPrint.png", "ReviTab.PrintSelected", "Print the selected Sheets in the Project Browsser") == false)
                {
                    MessageBox.Show("Failed to add button Print Selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

#endregion

#region Structural Framing
#if DEBUG
                IList<PushButtonData> stackedButtonsCQT = new List<PushButtonData>();

                stackedButtonsCQT.Add(CreatePushButton("btnPlaceVoidByFace", "Place Void" + Environment.NewLine + "By Face", "","pack://application:,,,/ReviTab;component/Resources/addBeamOpening.png", "ReviTab.VoidByFace", "Place a void on a beam face"));

                stackedButtonsCQT.Add(CreatePushButton("btnPlaceVoidByLine", "Void By Line", "", "pack://application:,,,/ReviTab;component/Resources/line.png", "ReviTab.VoidByLine", "Place a void at line beam intersection. Contact: Ethan Gear."));
                
                stackedButtonsCQT.Add(CreatePushButton("btnPlaceVoidByRefPlane", "Void By Reference Plane", "", "pack://application:,,,/ReviTab;component/Resources/line.png", "ReviTab.VoidByRefPlane", "Place a void at line beam intersection. Contact: Ethan Gear."));

                stackedButtonsCQT.Add(CreatePushButton("btnPlaceTags", "Place Tags","", "pack://application:,,,/ReviTab;component/Resources/tag.png", "ReviTab.AddTagsApplyUndo", "Place a tag on multiple beams"));

                stackedButtonsCQT.Add(CreatePushButton("btnPlaceDimensions", "Lock Openings", "", "pack://application:,,,/ReviTab;component/Resources/lock.png", "ReviTab.LockOpenings", "Place a dimension between an opening and a reference plane and lock it."));

                AddSplitButton(beams, stackedButtonsCQT, "CQTButton", "CQT");
#endif
                IList<PushButtonData> beamsEdit = new List<PushButtonData>();
                
                beamsEdit.Add(CreatePushButton("btnBeamByTypeMark", "Beam by \nType Name", null, Resource1.cloudSelectionCopy, "ReviTab.BeamByTypeName", "Provide an Type Name to change the selected beam type."));

                beamsEdit.Add(CreatePushButton("btnMoveBeamEnd", "Move Beam End", "", "pack://application:,,,/ReviTab;component/Resources/movement-arrows.png", "ReviTab.MoveBeamEnd", "Move a beam endpoint to match a selected beam closest point"));

                beamsEdit.Add(CreatePushButton("btnChangeBeamLocation", "Change Beam" + Environment.NewLine + "Location", "", "pack://application:,,,/ReviTab;component/Resources/changebeamlocation.png", "ReviTab.ChangeBeamLocation", "Move a beam to new location."));

                beamsEdit.Add(CreatePushButton("btnEditJoin", "Edit Beam" + Environment.NewLine + "End Join", "", "pack://application:,,,/ReviTab;component/Resources/joinEnd.png", "ReviTab.EditBeamJoin", "Allow/Disallow beam end join"));


                AddSplitButton(beams, beamsEdit, "BeamsEdit", "Beams Edit");

                //if (AddPushButton(beams, "btnMoveBeamEnd", "Move Beam End", "", "pack://application:,,,/ReviTab;component/Resources/movement-arrows.png", "ReviTab.MoveBeamEnd", "Move a beam endpoint to match a selected beam closest point") == false)
                //{
                //    MessageBox.Show("Failed to add button Move Beam End", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}

                //if (AddPushButton(beams, "btnChangeBeamLocation", "Change Beam" + Environment.NewLine + "Location", "", "pack://application:,,,/ReviTab;component/Resources/changebeamlocation.png", "ReviTab.ChangeBeamLocation", "Move a beam to new location.") == false)
                //{
                //    MessageBox.Show("Failed to add button change beam location", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}

                //if (AddPushButton(beams, "btnEditJoin", "Edit Beam" + Environment.NewLine + "End Join", "", "pack://application:,,,/ReviTab;component/Resources/joinEnd.png", "ReviTab.EditBeamJoin", "Allow/Disallow beam end join") == false)
                //{
                //    MessageBox.Show("Failed to add button Edit Beam", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}


#endregion

#region Splitter

                if (AddPushButton(walls, "btnWallSplitter", "Split Wall" + Environment.NewLine + "By Levels", "", "pack://application:,,,/ReviTab;component/Resources/splitWalls.png", "ReviTab.WallSplitter", "Split a wall by levels. NOTE: The original wall will be deleted.") == false)
                {
                    MessageBox.Show("Failed to add button Split Wall", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (AddPushButton(walls, "btnColumnSplitter", "Split Column" + Environment.NewLine + "By Levels", "", "pack://application:,,,/ReviTab;component/Resources/columnSplit.png", "ReviTab.ColumnSplitter", "Split a wall by levels. NOTE: The original wall will be deleted.") == false)
                {
                    MessageBox.Show("Failed to add button Split Column", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                #endregion

                #region Geometry

#if DEBUG
                IList<PushButtonData> stackedButtonsGroupGeometry = new List<PushButtonData>
                {
                    
                    CreatePushButton("intMesh", "Intersect Mesh", null, Resource1.topoIntersect, "ReviTab.WallTopographyIntersections", "Intersect line with mesh"),

                    CreatePushButton("btnSATtoDS", "Element to DirectShape", null, Resource1.directShape, "ReviTab.SATtoDirectShape", "Convert an element into a DirectShape. Deletes the original element."),

                    CreatePushButton("btnProjectLines", "Project Lines to Surface", null, Resource1.projectLines, "ReviTab.ProjectLines", "Project some lines onto a surface."),

                    CreatePushButton("btnDrawAxis", "Draw Axis", null, Resource1.drawAxis, "ReviTab.DrawObjectAxis", "Draw local and global axis on a point on a surface.")       

                };

                AddSplitButton(geometry, stackedButtonsGroupGeometry, "GeometryButton", "Geometry");
#endif

#endregion

                #region Zero State

#if DEBUG
                IList<PushButtonData> stackedButtonsZeroState = new List<PushButtonData>();

                stackedButtonsZeroState.Add(CreatePushButton("btnClaritySetup", "Clarity Setup", "", "pack://application:,,,/ReviTab;component/Resources/claSetup.png", "ReviTab.ClaritySetup", "Open a model in background and create a 3d view for Clarity IFC export.", "ReviTab.Availability"));


                //stackedButtonsZeroState.Add(CreatePushButton("btnPush", "Push to DB", "", "pack://application:,,,/ReviTab;component/Resources/arrowUp.png", "ReviTab.PushToDB", "Push date, user, rvtFileSize, elementsCount, typesCount, sheetsCount, viewsCount, viewportsCount, warningsCount to 127.0.0.1"));

                stackedButtonsZeroState.Add(CreatePushButton("btnPurgeFamilies", "Families", "", "pack://application:,,,/ReviTab;component/Resources/wiping.png", "ReviTab.PurgeFamily", "Purge families and leave only a type called Default", "ReviTab.Availability"));

                stackedButtonsZeroState.Add(CreatePushButton("btnPrintBackground", "Back Print", "", "pack://application:,,,/ReviTab;component/Resources/backgroundPrint.png", "ReviTab.PrintInBackground", "Open a model in background and print the selcted drawings", "ReviTab.Availability"));

                
                AddSplitButton(zeroState, stackedButtonsZeroState, "PurgeCommands", "Purge");

                /*
                if (AddZeroStatePushButton(zeroState, "btnDiasbleWarning", "Open No Warnings", "", "pack://application:,,,/ReviTab;component/Resources/addMultiViews.png", "ReviTab.SuppressWarnings", "Suppress warnings when opening files", "ReviTab.Availability") == false)
                {
                    MessageBox.Show("Failed to add button Purge Families", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                */

                IList<PushButtonData> stackedButtonsGroupMetadata = new List<PushButtonData>();

                stackedButtonsGroupMetadata.Add(CreatePushButton("btnInfo", "Info", "", "pack://application:,,,/ReviTab;component/Resources/info.png", "ReviTab.VersionInfo", "Display Version Info Task Dialog.", "ReviTab.Availability"));

                //Icon made Freepik from www.flaticon.com
                stackedButtonsGroupMetadata.Add(CreatePushButton("btnColor", "ColorTab", null, Resource1.rainbow, "ReviTab.ColorTab", "Color Revit Tabs based on view type.", "ReviTab.Availability"));

                stackedButtonsGroupMetadata.Add(CreatePushButton("btnHowl", "Howl",  null, Resource1.ghowlicon, "ReviTab.Howl", "Howl"));

                stackedButtonsGroupMetadata.Add(CreatePushButton("btnAddMetadata", "Metadata", null, Resource1.metadata, "ReviTab.AddPDFcustomProperties", "Add custom properties to a list of pdfs"));

                stackedButtonsGroupMetadata.Add(CreatePushButton("btnPanic", "Panic", Resource1.panButton_small, Resource1.panButton, "ReviTab.PanicButton", "Add custom properties to a list of pdfs"));
                //stackedButtonsGroupMetadata.Add(CreatePushButton("btnPanic", "Panic", "", "pack://application:,,,/ReviTab;component/Resources/panButton_small.png", "ReviTab.PanicButton", "Add custom properties to a list of pdfs"));

                AddSplitButton(zeroState, stackedButtonsGroupMetadata, "MetadataCommands", "Metadata");
#else
                if (AddZeroStatePushButton(zeroState, "btnInfo", "Info", "", "pack://application:,,,/ReviTab;component/Resources/info.png", "ReviTab.VersionInfo", "Display Version Info Task Dialog.", "ReviTab.Availability")==false)
                {
                    MessageBox.Show("Failed to add button Info", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
#endif
#endregion

                if (AddTextBox(commandPanel, "btnCommandLine", "*Structural Framing+Length>10000 \n *Walls+Mark!aa \n sheets: all \n sheets: A101 A103 A201\n tblocks: all") == false)
                {
                    MessageBox.Show("Failed to add Text Box", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

            }
            catch
            {
                // In Case of Failure
                return Result.Failed;
            }

            // Return Success
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            try
            {
                return Result.Succeeded;
            }
            catch
            {
                // In Case of Failure
                return Result.Failed;
            }
        }






        /// <summary>
        /// Create a Ribbon Tab and a Ribbon Panel
        /// </summary>
        /// <param name="application"></param>
        /// <param name="tabName"></param>
        /// <param name="ribbonName"></param>
        static void AddRibbonPanel(UIControlledApplication application, string tabName)

        {

            // Create a custom ribbon tab

            application.CreateRibbonTab(tabName);



            // Add a new ribbon panel

            //RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, ribbonName);

            /*

            // Get dll assembly path

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;



            // create push button for CurveTotalLength

            PushButtonData b1Data = new PushButtonData(

                "cmdCurveTotalLength",

                "Total" + System.Environment.NewLine + "  Length  ",

                thisAssemblyPath,

                "ReviTab.CommandHelloWorld");

            //typeof(CommandHelloWorld).FullName

            PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;

            pb1.ToolTip = "Select Multiple Lines to Obtain Total Length";

            //BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/GrimshawRibbon;component/Resources/totalLength.png"));

            //pb1.LargeImage = pb1Image;*/

        }

        /// <summary>
        /// Selects or Creates a Ribbon Panel in a specified Ribbon Tab
        /// </summary>
        /// <param name="application"></param>
        /// <param name="tabName"></param>
        /// <param name="panelName"></param>
        /// <returns></returns>
        private RibbonPanel GetSetRibbonPanel(UIControlledApplication application, string tabName, string panelName)
        {
            List<RibbonPanel> tabList = new List<RibbonPanel>();

            tabList = application.GetRibbonPanels(tabName);

            RibbonPanel tab = null;

            foreach (RibbonPanel r in tabList)
            {
                if (r.Name.ToUpper() == panelName.ToUpper())
                {
                    tab = r;
                }
            }

            if (tab is null)
                tab = application.CreateRibbonPanel(tabName, panelName);

            return tab;
        }


        /// <summary>
        /// Add min 2 or max 3 buttons to a stacked button.
        /// </summary>
        private bool AddStackedButton(RibbonPanel panel, IList<PushButtonData> stackedButtonsGroup, string stackedButtonName, string stackedButtonText)
        {
            try
            {
                List<RibbonItem> projectButtons = new List<RibbonItem>();
                projectButtons.AddRange(panel.AddStackedItems(stackedButtonsGroup[0], stackedButtonsGroup[1], stackedButtonsGroup[2]));
            
                return true;
            }
            catch
            {
                return false;
            }            
        }

        /// <summary>
        /// Add min 2 or max 3 buttons to a stacked button.
        /// </summary>
        private bool AddSplitButton(RibbonPanel panel, IList<PushButtonData> splitButtonsGroup, string splitButtonName, string splitButtonText)
        {
            try
            {
                SplitButtonData sb1 = new SplitButtonData(splitButtonName, splitButtonText);
                SplitButton sb = panel.AddItem(sb1) as SplitButton;

                foreach (PushButtonData item in splitButtonsGroup)
                {
                    sb.AddPushButton(item);
                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }


        ///<summary>
        ///Add a PushButton to the Ribbon Panel
        ///</summary>
        private PushButtonData CreatePushButton(string ButtonName, string ButtonText, string ImagePath16, string ImagePath32, string dllClass, string Tooltip, string availability="")
        {

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            try
            {
                PushButtonData m_pbData = new PushButtonData(ButtonName, ButtonText, thisAssemblyPath, dllClass);

                m_pbData.AvailabilityClassName = availability;

                if (ImagePath16 != "")
                {
                    try
                    {
                        m_pbData.Image = new BitmapImage(new Uri(ImagePath16));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }
                if (ImagePath32 != "")
                {
                    try
                    {
                        m_pbData.LargeImage = new BitmapImage(new Uri(ImagePath32));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }

                m_pbData.ToolTip = Tooltip;
                
                return m_pbData;
            }
            catch
            {
                return null;
            }
        }

        private PushButtonData CreatePushButton(string ButtonName, string ButtonText, System.Drawing.Bitmap Image16, System.Drawing.Bitmap Image32, string dllClass, string Tooltip, string availability = "")
        {

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            try
            {
                PushButtonData m_pbData = new PushButtonData(ButtonName, ButtonText, thisAssemblyPath, dllClass);

                m_pbData.AvailabilityClassName = availability;

                if (Image16 != null)
                {
                    try
                    {
                        m_pbData.Image = Convert(Image16);
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }
                if (Image32 != null)
                {
                    try
                    {
                        m_pbData.LargeImage = Convert(Image32);
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }

                m_pbData.ToolTip = Tooltip;

                return m_pbData;
            }
            catch
            {
                return null;
            }
        }


        ///<summary>
        ///Add a PushButton to the Ribbon Panel
        ///</summary>
        private Boolean AddPushButton(RibbonPanel Panel, string ButtonName, string ButtonText, string ImagePath16, string ImagePath32, string dllClass, string Tooltip)
        {

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            try
            {
                PushButtonData m_pbData = new PushButtonData(ButtonName, ButtonText, thisAssemblyPath, dllClass);
                
                if (ImagePath16 != "")
                {
                    try
                    {
                        m_pbData.Image = new BitmapImage(new Uri(ImagePath16));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }
                if (ImagePath32 != "")
                {
                    try
                    {
                        m_pbData.LargeImage = new BitmapImage(new Uri(ImagePath32));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }

                m_pbData.ToolTip = Tooltip;


                PushButton m_pb = Panel.AddItem(m_pbData) as PushButton;
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        private Boolean AddPushButton(RibbonPanel Panel, string ButtonName, string ButtonText, System.Drawing.Bitmap Image16, System.Drawing.Bitmap Image32, string dllClass, string Tooltip)
        {

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            try
            {
                PushButtonData m_pbData = new PushButtonData(ButtonName, ButtonText, thisAssemblyPath, dllClass);

                if (Image16 != null)
                {
                    try
                    {
                        m_pbData.Image = Convert(Image16);
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }
                if (Image32 != null)
                {
                    try
                    {
                        m_pbData.LargeImage = Convert(Image32);
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }

                m_pbData.ToolTip = Tooltip;


                PushButton m_pb = Panel.AddItem(m_pbData) as PushButton;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public BitmapImage Convert(System.Drawing.Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        /// <summary>
        /// Add a push button visible in Revit Zero State mode
        /// </summary>
        /// <param name="Panel"></param>
        /// <param name="ButtonName"></param>
        /// <param name="ButtonText"></param>
        /// <param name="ImagePath16"></param>
        /// <param name="ImagePath32"></param>
        /// <param name="dllClass"></param>
        /// <param name="Tooltip"></param>
        /// <returns></returns>
        private Boolean AddZeroStatePushButton(RibbonPanel Panel, string ButtonName, string ButtonText, string ImagePath16, string ImagePath32, string dllClass, string Tooltip, string Availability)
        {

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            try
            {
                PushButtonData m_pbData = new PushButtonData(ButtonName, ButtonText, thisAssemblyPath, dllClass);

                m_pbData.AvailabilityClassName = Availability;

                if (ImagePath16 != "")
                {
                    try
                    {
                        m_pbData.Image = new BitmapImage(new Uri(ImagePath16));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }
                if (ImagePath32 != "")
                {
                    try
                    {
                        m_pbData.LargeImage = new BitmapImage(new Uri(ImagePath32));
                    }
                    catch
                    {
                        //Could not find the image
                    }
                }

                m_pbData.ToolTip = Tooltip;


                PushButton m_pb = Panel.AddItem(m_pbData) as PushButton;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Add a Text Box to the Ribbon Panel
        /// </summary>
        /// <param name="Panel"></param>
        /// <param name="textboxName"></param>
        /// <param name="tooltip"></param>
        /// <returns></returns>
        private Boolean AddTextBox(RibbonPanel Panel, string textboxName, string tooltip)
        {
            try
            {
                TextBoxData tbData = new TextBoxData(textboxName);
                Autodesk.Revit.UI.TextBox textBox = Panel.AddItem(tbData) as Autodesk.Revit.UI.TextBox;

                textBox.PromptText = "Write something and hit Enter";
                textBox.ShowImageAsButton = true;

                textBox.ToolTip = tooltip;

                textBox.EnterPressed += new EventHandler<TextBoxEnterPressedEventArgs>(MyTextBoxEnter);
                     
                return true;
            }
            catch
            {
                return false;
            }
        }

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

            string caseSwitch = "default";

            if (message.Contains("babbo"))
                caseSwitch = "christmas";

            if (message.Contains("leann says"))
                caseSwitch = "leann";

            if (message.Contains("+"))
                caseSwitch = "sum";

            if (message.StartsWith("*"))
                caseSwitch = "select";

            if (message.StartsWith("/"))
                caseSwitch = "selectAll";

            if (message.StartsWith("sheets"))
                caseSwitch = "selectSheets";

            if (message.StartsWith("tblocks"))
                caseSwitch = "selectTBlocks";

            if (message.StartsWith("-"))
                caseSwitch = "delete";

            if (message.StartsWith("+viewset"))
                caseSwitch = "createViewSet";

            if (message.StartsWith("open"))
                caseSwitch = "openView";

            switch (caseSwitch)
            {
                case "default":
                    MessageBox.Show(textBox.Value.ToString(), "Command Line");
                    break;
                case "christmas":
                    Helpers.Christams();
                    break;
                case "leann":
                    Helpers.leannSays();
                    break;
                case "sum":
                    Helpers.AddTwoIntegers(message);
                    break;
                case "select":
                    Helpers.SelectAllElementsInView(uiDoc, message);
                    break;
                case "selectAll":
                    Helpers.SelectAllElements(uiDoc, message);
                    break;
                case "createViewSet":
                    Helpers.CreateViewset(doc, message);
                    break;
                case "delete":
                    break;
                case "selectSheets":
                    Helpers.HighlightSelectSheets(uiDoc, message);
                    break;
                case "selectTBlocks":
                    Helpers.HighlightSelectTitleBlocks(uiDoc, message);
                    break;
                case "openView":
                    Helpers.OpenView(uiDoc, message);
                    break;
            }

            
        }

    }


}
