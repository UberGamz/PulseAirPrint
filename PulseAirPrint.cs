using System;
using System.Collections.Generic;
using Mastercam.Database;
using Mastercam.Math;
using Mastercam.IO;
using Mastercam.Database.Types;
using Mastercam.GeometryUtility.Types;
using Mastercam.App.Types;
using Mastercam.GeometryUtility;
using Mastercam.Support;
using Mastercam.Curves;
using Mastercam.BasicGeometry;
using Mastercam.Database.Interop;
using Mastercam.IO.Types;
using PulseAirPrint;
using System.Windows.Forms;


namespace _PulseAirPrint
{
    public class PulseAirPrint : Mastercam.App.NetHook3App
    {
        public Mastercam.App.Types.MCamReturn PulseAirPrintRun(Mastercam.App.Types.MCamReturn notused)
        {

            ///paSide
            ///drillSizeBox
            ///thruBox
            ///rotaryPDBox
            ///nGearToInsideDim
            ///nGearToOutsideDim
            ///insideDim
            ///gearToInsideDim
            ///gearToOutsideDim

            var form = new PulseAirPrintForm();
            form.ShowDialog();

            var paSide = form.paSide.Text; // Gearside / Tailstock
            var drillSizeBox = double.Parse(form.drillSizeBox.Text);
            var thruBox = form.thruBox.Text; // No / Yes
            var rotaryPDBox = double.Parse(form.rotaryPDBox.Text);
            var nGearToInsideDim = double.Parse(form.nGearToInsideDim.Text);
            var nGearToOutsideDim = double.Parse(form.nGearToOutsideDim.Text);
            var insideDim = double.Parse(form.insideDim.Text);
            var gearToInsideDim = double.Parse(form.gearToInsideDim.Text);
            var gearToOutsideDim = double.Parse(form.gearToOutsideDim.Text);








            void StepOne(){

                ////////// Params
                var PD = rotaryPDBox;
                var drillSize = drillSizeBox;
                var origin = new Point3D(0, 0, 0);


                ////////// Creates level 300 points, moves to Z0 and X0 from TOP, deletes duplicates
                var points11 = SearchManager.GetGeometry(11);
                foreach (var point in points11)
                {
                    if (point is PointGeometry currentPoint)
                    {
                        var newPoint = new PointGeometry();
                        newPoint.Data.x = 0;
                        newPoint.Data.y = currentPoint.Data.y;
                        newPoint.Data.z = 0;
                        newPoint.Level = 300;
                        newPoint.Selected = false;
                        newPoint.Commit();
                        point.Selected = false;
                        point.Commit();
                    }
                }
                SelectionManager.UnselectAllGeometry();
                LevelsManager.RefreshLevelsManager();
                LevelsManager.SetMainLevel(300);
                var shown = LevelsManager.GetVisibleLevelNumbers();
                foreach (var level in shown)
                {
                    LevelsManager.SetLevelVisible(level, false);
                }
                LevelsManager.SetLevelVisible(300, true);
                LevelsManager.RefreshLevelsManager();
                GraphicsManager.Repaint(true);

                ////////// Rolls Geo
                ViewManager.GraphicsView = SearchManager.GetSystemView(SystemPlaneType.Right);
                ViewManager.WorkCoordinateSystem = SearchManager.GetSystemView(SystemPlaneType.Right);
                ViewManager.TPlane = SearchManager.GetSystemView(SystemPlaneType.Right);
                ViewManager.CPlane = SearchManager.GetSystemView(SystemPlaneType.Right);
                ViewManager.RefreshPlanesManager();
                GraphicsManager.Repaint(true);
                GraphicsManager.FitScreen();
                GeometryInteractionManager.DeleteDuplicates(true);
                GraphicsManager.Repaint(true);

                var points300 = SearchManager.GetGeometry(300);
                foreach (var point in points300)
                {
                    if (point is PointGeometry pointy) {
                        var pointLocation = new Point3D(pointy.Data.y, 0, 0);
                        var distance = VectorManager.Distance(pointLocation, origin);
                        var percent = distance / (PD * Math.PI);
                        pointy.Data.y = 0;
                        pointy.Data.z = PD / 2;
                        pointy.Selected = true;
                        pointy.Commit();
                        pointy.Retrieve();
                        var newPointLocation = new Point3D(pointy.Data.x, pointy.Data.y, pointy.Data.z);
                        var newLine = new LineGeometry(newPointLocation, origin);
                        newLine.Selected = true;
                        newLine.Commit();
                        GeometryManipulationManager.RotateGeometry(origin, (percent * 360), SearchManager.GetSystemView(SystemPlaneType.Right), false);
                        pointy.Retrieve();
                        pointy.Selected = false;
                        pointy.Commit();
                        newLine.Retrieve();
                        newLine.Selected = false;
                        newLine.Commit();
                        var drillPoint = pointy.ScaleAndCopy(origin, (PD - 1.2) / PD);
                        drillPoint.Commit();
                        drillPoint.Retrieve();
                        if (drillPoint is PointGeometry thisDrillPoint)
                        {
                            var drillPointLocation = new Point3D(thisDrillPoint.Data.y, thisDrillPoint.Data.z, thisDrillPoint.Data.x);
                            var drilledHole = new ArcGeometry();
                            drilledHole.Data.CenterPoint = drillPointLocation;
                            drilledHole.Data.Radius = (drillSize / 2);
                            drilledHole.Retrieve();
                            drilledHole.Commit();
                        }
                    }
                }
                GraphicsManager.ClearColors(new GroupSelectionMask(true));
                GraphicsManager.FitScreen();
                GraphicsManager.Repaint(true);

                var pdCircle = new ArcGeometry();
                pdCircle.Data.CenterPoint = origin;
                pdCircle.Data.Radius = (PD / 2);
                var paCircle = new ArcGeometry();
                paCircle.Data.CenterPoint = origin;
                paCircle.Data.Radius = ((PD - 1.2) / 2);
                pdCircle.Commit();
                paCircle.Commit();
                GraphicsManager.Repaint(true);


                ViewManager.GraphicsView = SearchManager.GetSystemView(SystemPlaneType.Front);
                ViewManager.WorkCoordinateSystem = SearchManager.GetSystemView(SystemPlaneType.Front);
                ViewManager.TPlane = SearchManager.GetSystemView(SystemPlaneType.Front);
                ViewManager.CPlane = SearchManager.GetSystemView(SystemPlaneType.Front);
                ViewManager.RefreshPlanesManager();
                GraphicsManager.Repaint(true);
                GraphicsManager.FitScreen();

                var geo300 = SearchManager.GetGeometry(300);
                foreach (var entity in geo300)
                {
                    entity.Selected = true;
                    entity.Commit();
                    var geo301 = entity.CopyAndRotate(origin, -90, SearchManager.GetSystemView(SystemPlaneType.Front));
                    entity.Selected = false;
                    entity.Commit();
                    geo301.Retrieve();
                    geo301.Rotate(origin, 90, SearchManager.GetSystemView(SystemPlaneType.Top));
                    geo301.Retrieve();
                    var translatePoint = new Point3D(30, 0, 0);
                    geo301.Translate(origin, translatePoint, SearchManager.GetSystemView(SystemPlaneType.Top), SearchManager.GetSystemView(SystemPlaneType.Top));
                    geo301.Retrieve();
                    geo301.Level = 301;
                    geo301.Color = 15;
                    geo301.Commit();

                }
                GraphicsManager.ClearColors(new GroupSelectionMask(true));
                GraphicsManager.Repaint(true);
                ViewManager.GraphicsView = SearchManager.GetSystemView(SystemPlaneType.Top);
                ViewManager.WorkCoordinateSystem = SearchManager.GetSystemView(SystemPlaneType.Top);
                ViewManager.TPlane = SearchManager.GetSystemView(SystemPlaneType.Top);
                ViewManager.CPlane = SearchManager.GetSystemView(SystemPlaneType.Top);
                ViewManager.RefreshPlanesManager();
                LevelsManager.SetLevelVisible(301, true);
                LevelsManager.SetMainLevel(301);
                LevelsManager.SetLevelVisible(300, false);
                LevelsManager.RefreshLevelsManager();
                GraphicsManager.Repaint(true);
                GraphicsManager.FitScreen();

                var geoLevel301 = SearchManager.GetGeometry(301);
                foreach (var entity in geoLevel301)
                {
                    if (entity is ArcGeometry) { }
                    else { entity.Delete(); };
                }
            }
            void StepTwo() {
                var textPoint = new Point3D(30, (-rotaryPDBox / 2), 0);
                var TextDataFormat = new LetterCreationData{ 
                    LetterText = "GearSide",
                    StartingPoint = textPoint,
                    FontHeight = 0.16,
                    FontSpacing = 0.08,
                    FontAlignment = FontAlignmentType.Horizontal,
                    FontMode = FontModeType.MastercamBoxFont
                };
                Mastercam.GeometryUtility.LetterCreationManager.CreateLetters(TextDataFormat);
                
            }


            StepOne();
            StepTwo();

            return MCamReturn.NoErrors;
        }
    }
}

        
    


            

                



            

        


    


/*

void offsetCutchain();
{

    var selectedChain = ChainManager.ChainAll();
    int createdUpperLevel = 500;
    int createdLowerLevel = 501;
    LevelsManager.SetLevelName(500, "Upper Created Geo");
    LevelsManager.SetLevelName(501, "Lower Created Geo");

    foreach (var chain in selectedChain)
    {


        var lowerChainLarge = chain.OffsetChain2D(OffsetSideType.Left, .0225, OffsetRollCornerType.None, .5, false, .005, false);
        var lowerLargeGeometry = ChainManager.GetGeometryInChain(lowerChainLarge);

        var lowerChainSmall = chain.OffsetChain2D(OffsetSideType.Right, .0025, OffsetRollCornerType.None, .5, false, .005, false);
        var lowerSmallGeometry = ChainManager.GetGeometryInChain(lowerChainSmall);

        var resultGeometry = SearchManager.GetResultGeometry();
        foreach (var entity in resultGeometry)
        {
            entity.Color = 11;
            entity.Selected = true;
            entity.Commit();
        }
        GeometryManipulationManager.MoveSelectedGeometryToLevel(createdLowerLevel, true);
        GraphicsManager.ClearColors(new GroupSelectionMask(true));

        var upperChainLarge = chain.OffsetChain2D(OffsetSideType.Left, .0025, OffsetRollCornerType.None, .5, false, .005, false);
        var upperLargeGeometry = ChainManager.GetGeometryInChain(upperChainLarge);

        var upperChainSmall = chain.OffsetChain2D(OffsetSideType.Right, .0385, OffsetRollCornerType.None, .5, false, .005, false);
        var upperSmallGeometry = ChainManager.GetGeometryInChain(upperChainSmall);

        var resultGeometryNew = SearchManager.GetResultGeometry();
        foreach (var entity in resultGeometryNew)
        {
            entity.Color = 10;
            entity.Selected = true;
            entity.Commit();
        }
        GeometryManipulationManager.MoveSelectedGeometryToLevel(createdUpperLevel, true);
        GraphicsManager.ClearColors(new GroupSelectionMask(true));

    }


}





// Working Offset Chain
/*

var selectedChain = ChainManager.GetOneChain("Select a Chain");

var offsetChain = selectedChain.OffsetChain2D(OffsetSideType.Left,
                                              .245,
                                              OffsetRollCornerType.None,
                                              .5,
                                              false,
                                              .005,
                                              false);

var offsetGeometry = ChainManager.GetGeometryInChain(offsetChain);

foreach (var entity in offsetGeometry)
{
    entity.Commit();
}

return MCamReturn.NoErrors;
*/





//Working Translate
/*
bool MoveLine() {
    bool result = false;
    //Mastercam.IO.SelectionManager.SelectAllGeometry();
    Mastercam.Math.Point3D pt1 = new Mastercam.Math.Point3D(0.0, 0.0, 0.0);
    Mastercam.Math.Point3D pt2 = new Mastercam.Math.Point3D(100.0, 0.0, 0.0);
    MCView Top = new MCView();
    Mastercam.GeometryUtility.GeometryManipulationManager.TranslateGeometry(pt1, pt2, Top , Top, false);
    return result;
}
MoveLine();
*/


// working form
/*
var m = new Form1();
m.Show();
*/

// working line creation
/*
bool CreateLine()
{
    bool result = false;

    Mastercam.Math.Point3D pt1 = new Mastercam.Math.Point3D(0.0, 0.0, 0.0);
    Mastercam.Math.Point3D pt2 = new Mastercam.Math.Point3D(100.0, 0.0, 0.0);
    Mastercam.Curves.LineGeometry Line1 = new Mastercam.Curves.LineGeometry(pt1, pt2);
    result = Line1.Commit();
    result = Line1.Validate(); // Not really needed here, if Commit was successful - we're good!
                               //Mastercam.IO.GraphicsManager.Repaint(True)

    return result;
}
CreateLine();
*/

//working popup message
//  System.Windows.Forms.MessageBox.Show("Jeremy can make pop up messages!");
//return Mastercam.App.Types.MCamReturn.NoErrors;
