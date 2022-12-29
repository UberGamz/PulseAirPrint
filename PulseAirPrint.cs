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
            var repeatHeight = rotaryPDBox * Math.PI;
            var GearSideEndX = (insideDim / 2) + gearToInsideDim;
            var nGearSideEndX = (insideDim / 2) + nGearToInsideDim;
            var nGearToInsidePointX = nGearSideEndX - nGearToInsideDim;
            var nGearToOutsidePointX = nGearSideEndX - nGearToOutsideDim;
            var GearToInsidePointX = GearSideEndX - gearToInsideDim;
            var GearToOutsidePointX = GearSideEndX - gearToOutsideDim;
            var originPointList = new List<PointGeometry>();
            var origin = new Point3D(0, 0, 0);
            var totalLength = gearToInsideDim + insideDim + nGearToInsideDim;
            var translatePoint = new Point3D((totalLength / 2) + (rotaryPDBox / 2) + 5, 0, 0);
            var textPoint1 = new Point3D((translatePoint.x - 1.21), (-rotaryPDBox), 0);
            var textPoint2 = new Point3D((translatePoint.x - 2.75), (-rotaryPDBox) - .2, 0);
            var textPoint3 = new Point3D((translatePoint.x - 1.95), (-rotaryPDBox) - .4, 0);






            void StepOne(){

                ////////// Params
                var PD = rotaryPDBox;
                var drillSize = drillSizeBox;


                ////////// Creates level 300 points, moves to Z0 and X0 from TOP, deletes duplicates
                var points11 = SearchManager.GetGeometry(11);
                foreach (var point in points11)
                {
                    if (point is PointGeometry currentPoint)
                    {
                        originPointList.Add(currentPoint);
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
            void GearSide()
            {
                var TextDataFormat = new LetterCreationData
                {
                    LetterText = "Tailstock View",
                    StartingPoint = textPoint1,
                    FontHeight = 0.16,
                    FontSpacing = 0.08,
                    FontAlignment = FontAlignmentType.Horizontal,
                    FontMode = FontModeType.MastercamBoxFont
                };
                var nGearToInsidePoint = new Point3D(-nGearToInsidePointX, 0, 0);
                var nGearToOutsidePoint = new Point3D(-nGearToOutsidePointX, 0, 0);
                var GearToInsidePoint = new Point3D(GearToInsidePointX, 0, 0);
                var GearToOutsidePoint = new Point3D(GearToOutsidePointX, 0, 0);
                var GearSideEnd = new Point3D(GearSideEndX, 0, 0);
                var nGearSideEnd = new Point3D(-nGearSideEndX, 0, 0);
                var tempPoint1 = new Point3D(nGearToInsidePoint.x, repeatHeight, 0);
                var tempPoint2 = new Point3D(nGearToOutsidePoint.x, repeatHeight, 0);
                var tempPoint3 = new Point3D(GearToInsidePoint.x, repeatHeight, 0);
                var tempPoint4 = new Point3D(GearToOutsidePoint.x, repeatHeight, 0);
                var tempPoint5 = new Point3D(GearSideEnd.x, repeatHeight, 0);
                var tempPoint6 = new Point3D(nGearSideEnd.x, repeatHeight, 0);
                var nGearToInsideLine = new LineGeometry(nGearToInsidePoint, tempPoint1);
                var nGearToOutsideLine = new LineGeometry(nGearToOutsidePoint, tempPoint2);
                var GearToInsideLine = new LineGeometry(GearToInsidePoint, tempPoint3);
                var GearToOutsideLine = new LineGeometry(GearToOutsidePoint, tempPoint4);
                var gearEndLine = new LineGeometry(GearSideEnd, tempPoint5);
                var nGearEndLine = new LineGeometry(nGearSideEnd, tempPoint6);
                nGearToInsideLine.Commit();
                nGearToOutsideLine.Commit();
                GearToInsideLine.Commit();
                GearToOutsideLine.Commit();
                gearEndLine.Commit();
                nGearEndLine.Commit();
                Mastercam.GeometryUtility.LetterCreationManager.CreateLetters(TextDataFormat);
                var tempList = new List<double>();
                foreach (var point in originPointList)
                {
                    tempList.Add(point.Data.x);
                }
                tempList.Sort();
                var leftest = tempList[0];
                var leftestPoint = new Point3D(leftest, 0, 0);
                var bearerToHole = Math.Round(VectorManager.Distance(leftestPoint, GearToOutsidePoint) + .25, 4);
                var endToHole = Math.Round(VectorManager.Distance(leftestPoint, GearSideEnd) + .25, 4);
                var endToThru = (totalLength - nGearToOutsideDim) + .25;
                var bearerToThru = (totalLength - (gearToOutsideDim + nGearToOutsideDim)) + .25;
                if (thruBox == "No")
                {
                    var bearerTextDataFormat = new LetterCreationData
                    {
                        LetterText = bearerToHole.ToString() + "From Outside Bearer/Spacer",
                        StartingPoint = textPoint2,
                        FontHeight = 0.16,
                        FontSpacing = 0.08,
                        FontAlignment = FontAlignmentType.Horizontal,
                        FontMode = FontModeType.MastercamBoxFont
                    };
                    Mastercam.GeometryUtility.LetterCreationManager.CreateLetters(bearerTextDataFormat);
                    var endTextDataFormat = new LetterCreationData
                    {
                        LetterText = endToHole.ToString() + "From Outside Edge",
                        StartingPoint = textPoint3,
                        FontHeight = 0.16,
                        FontSpacing = 0.08,
                        FontAlignment = FontAlignmentType.Horizontal,
                        FontMode = FontModeType.MastercamBoxFont
                    };
                    Mastercam.GeometryUtility.LetterCreationManager.CreateLetters(endTextDataFormat);
                }
                if (thruBox == "Yes")
                {
                    var bearerTextDataFormat = new LetterCreationData
                    {
                        LetterText = bearerToThru.ToString() + "From Outside Bearer/Spacer & THRU",
                        StartingPoint = textPoint2,
                        FontHeight = 0.16,
                        FontSpacing = 0.08,
                        FontAlignment = FontAlignmentType.Horizontal,
                        FontMode = FontModeType.MastercamBoxFont
                    };
                    Mastercam.GeometryUtility.LetterCreationManager.CreateLetters(bearerTextDataFormat);
                    var endTextDataFormat = new LetterCreationData
                    {
                        LetterText = endToThru.ToString() + "From Outside Edge",
                        StartingPoint = textPoint3,
                        FontHeight = 0.16,
                        FontSpacing = 0.08,
                        FontAlignment = FontAlignmentType.Horizontal,
                        FontMode = FontModeType.MastercamBoxFont
                    };
                    Mastercam.GeometryUtility.LetterCreationManager.CreateLetters(endTextDataFormat);
                }



                /////////
                var level300Geo = SearchManager.GetGeometry(300);
                foreach (var entity in level300Geo)
                {
                    var newGeo = entity.CopyAndTranslate(origin, GearSideEnd, new MCView(), new MCView());
                    newGeo.Level = 302;
                    newGeo.Commit();
                }
                var level302Geo = SearchManager.GetGeometry(302);
                foreach (var entity in level302Geo)
                {
                    if (entity is ArcGeometry) { }
                    else { entity.Delete(); }
                }
                GraphicsManager.ClearColors(new GroupSelectionMask(true));
                GraphicsManager.Repaint(true);

                //////////
                SelectionManager.UnselectAllGeometry();
                LevelsManager.RefreshLevelsManager();
                LevelsManager.SetMainLevel(302);
                var shown = LevelsManager.GetVisibleLevelNumbers();
                foreach (var level in shown)
                {
                    LevelsManager.SetLevelVisible(level, false);
                }
                LevelsManager.SetLevelVisible(302, true);
                LevelsManager.RefreshLevelsManager();
                GraphicsManager.Repaint(true);

                //////////
                ViewManager.GraphicsView = SearchManager.GetSystemView(SystemPlaneType.Right);
                ViewManager.WorkCoordinateSystem = SearchManager.GetSystemView(SystemPlaneType.Right);
                ViewManager.TPlane = SearchManager.GetSystemView(SystemPlaneType.Right);
                ViewManager.CPlane = SearchManager.GetSystemView(SystemPlaneType.Right);
                ViewManager.RefreshPlanesManager();
                GraphicsManager.Repaint(true);
                GraphicsManager.FitScreen();
                var geo302 = SearchManager.GetGeometry(302);
                foreach (var entity in geo302)
                {
                    if (entity is ArcGeometry arc)
                    {
                        if ((rotaryPDBox / 2) - arc.Data.Radius <= 0.001)
                        {
                            entity.Selected = true;
                            entity.Commit();
                        }
                        else
                        {
                            entity.Selected = false;
                            entity.Commit();
                        }
                    }
                }
                var selectedGeo1 = ChainManager.ChainAllSelected();
                foreach (var chain in selectedGeo1)
                {
                    Mastercam.IO.Interop.SelectionManager.DoSolidExtrudeCreate(chain, totalLength);
                }
                SelectionManager.UnselectAllGeometry();
                foreach (var entity in geo302)
                {
                    if (entity is ArcGeometry arc)
                    {
                        if (arc.Data.Radius - (drillSizeBox / 2) <= 0.001)
                        {
                            entity.Selected = true;
                            entity.Commit();
                        }
                        else
                        {
                            entity.Selected = false;
                            entity.Commit();
                        }
                    }
                }
                var solid = SearchManager.GetSolidGeometry()[0];
                var selectedGeo2 = ChainManager.ChainAllSelected();
                foreach (var chain in selectedGeo2)
                {
                    if (thruBox == "No")
                    {
                        Mastercam.IO.Interop.SelectionManager.DoSolidExtrudeCut(chain, endToHole, solid.GetEntityID());
                    }
                    else
                    {
                        Mastercam.IO.Interop.SelectionManager.DoSolidExtrudeCut(chain, endToThru, solid.GetEntityID());
                    }
                }
                SelectionManager.UnselectAllGeometry();
                GraphicsManager.Repaint(true);
            }
            void nGearSide()
            {
                var TextDataFormat = new LetterCreationData
                {
                    LetterText = "Tailstock View",
                    StartingPoint = textPoint1,
                    FontHeight = 0.16,
                    FontSpacing = 0.08,
                    FontAlignment = FontAlignmentType.Horizontal,
                    FontMode = FontModeType.MastercamBoxFont
                };
                var nGearToInsidePoint = new Point3D(nGearToInsidePointX, 0, 0);
                var nGearToOutsidePoint = new Point3D(nGearToOutsidePointX, 0, 0);
                var GearToInsidePoint = new Point3D(-GearToInsidePointX, 0, 0);
                var GearToOutsidePoint = new Point3D(-GearToOutsidePointX, 0, 0);
                var GearSideEnd = new Point3D(-GearSideEndX, 0, 0);
                var nGearSideEnd = new Point3D(nGearSideEndX, 0, 0);
                var tempPoint1 = new Point3D(nGearToInsidePoint.x, repeatHeight, 0);
                var tempPoint2 = new Point3D(nGearToOutsidePoint.x, repeatHeight, 0);
                var tempPoint3 = new Point3D(GearToInsidePoint.x, repeatHeight, 0);
                var tempPoint4 = new Point3D(GearToOutsidePoint.x, repeatHeight, 0);
                var tempPoint5 = new Point3D(GearSideEnd.x, repeatHeight, 0);
                var tempPoint6 = new Point3D(nGearSideEnd.x, repeatHeight, 0);
                var nGearToInsideLine = new LineGeometry(nGearToInsidePoint, tempPoint1);
                var nGearToOutsideLine = new LineGeometry(nGearToOutsidePoint, tempPoint2);
                var GearToInsideLine = new LineGeometry(GearToInsidePoint, tempPoint3);
                var GearToOutsideLine = new LineGeometry(GearToOutsidePoint, tempPoint4);
                var gearEndLine = new LineGeometry(GearSideEnd, tempPoint5);
                var nGearEndLine = new LineGeometry(nGearSideEnd, tempPoint6);
                nGearToInsideLine.Commit();
                nGearToOutsideLine.Commit();
                GearToInsideLine.Commit();
                GearToOutsideLine.Commit();
                gearEndLine.Commit();
                nGearEndLine.Commit();
                Mastercam.GeometryUtility.LetterCreationManager.CreateLetters(TextDataFormat);
                var tempList = new List<double>();
                foreach (var point in originPointList)
                {
                    tempList.Add(point.Data.x);
                }
                tempList.Sort();
                var leftest = tempList[0];
                var leftestPoint = new Point3D(leftest, 0, 0);
                var bearerToHole = Math.Round(VectorManager.Distance(leftestPoint, nGearToOutsidePoint) + .25, 4);
                var endToHole = Math.Round(VectorManager.Distance(leftestPoint, nGearSideEnd) + .25, 4);
                var endToThru = (totalLength - gearToOutsideDim) + .25;
                var bearerToThru = (totalLength - (gearToOutsideDim + nGearToOutsideDim)) + .25;
                if (thruBox == "No")
                {
                    var bearerTextDataFormat = new LetterCreationData
                    {
                        LetterText = bearerToHole.ToString() + "From Outside Bearer/Spacer",
                        StartingPoint = textPoint2,
                        FontHeight = 0.16,
                        FontSpacing = 0.08,
                        FontAlignment = FontAlignmentType.Horizontal,
                        FontMode = FontModeType.MastercamBoxFont
                    };
                    Mastercam.GeometryUtility.LetterCreationManager.CreateLetters(bearerTextDataFormat);
                    var endTextDataFormat = new LetterCreationData
                    {
                        LetterText = endToHole.ToString() + "From Outside Edge",
                        StartingPoint = textPoint3,
                        FontHeight = 0.16,
                        FontSpacing = 0.08,
                        FontAlignment = FontAlignmentType.Horizontal,
                        FontMode = FontModeType.MastercamBoxFont
                    };
                    Mastercam.GeometryUtility.LetterCreationManager.CreateLetters(endTextDataFormat);
                }
                if (thruBox == "Yes")
                {
                    var bearerTextDataFormat = new LetterCreationData
                    {
                        LetterText = bearerToThru.ToString() + "From Outside Bearer/Spacer & THRU",
                        StartingPoint = textPoint2,
                        FontHeight = 0.16,
                        FontSpacing = 0.08,
                        FontAlignment = FontAlignmentType.Horizontal,
                        FontMode = FontModeType.MastercamBoxFont
                    };
                    Mastercam.GeometryUtility.LetterCreationManager.CreateLetters(bearerTextDataFormat);
                    var endTextDataFormat = new LetterCreationData
                    {
                        LetterText = endToThru.ToString() + "From Outside Edge",
                        StartingPoint = textPoint3,
                        FontHeight = 0.16,
                        FontSpacing = 0.08,
                        FontAlignment = FontAlignmentType.Horizontal,
                        FontMode = FontModeType.MastercamBoxFont
                    };
                    Mastercam.GeometryUtility.LetterCreationManager.CreateLetters(endTextDataFormat);
                }



                /////////
                var level300Geo = SearchManager.GetGeometry(300);
                foreach (var entity in level300Geo)
                {
                    var newGeo = entity.CopyAndTranslate(origin, nGearSideEnd, new MCView(), new MCView());
                    newGeo.Level = 302;
                    newGeo.Commit();
                }
                var level302Geo = SearchManager.GetGeometry(302);
                foreach (var entity in level302Geo)
                {
                    if (entity is ArcGeometry) { }
                    else { entity.Delete(); }
                }
                GraphicsManager.ClearColors(new GroupSelectionMask(true));
                GraphicsManager.Repaint(true);

                //////////
                SelectionManager.UnselectAllGeometry();
                LevelsManager.RefreshLevelsManager();
                LevelsManager.SetMainLevel(302);
                var shown = LevelsManager.GetVisibleLevelNumbers();
                foreach (var level in shown)
                {
                    LevelsManager.SetLevelVisible(level, false);
                }
                LevelsManager.SetLevelVisible(302, true);
                LevelsManager.RefreshLevelsManager();
                GraphicsManager.Repaint(true);

                //////////
                ViewManager.GraphicsView = SearchManager.GetSystemView(SystemPlaneType.Right);
                ViewManager.WorkCoordinateSystem = SearchManager.GetSystemView(SystemPlaneType.Right);
                ViewManager.TPlane = SearchManager.GetSystemView(SystemPlaneType.Right);
                ViewManager.CPlane = SearchManager.GetSystemView(SystemPlaneType.Right);
                ViewManager.RefreshPlanesManager();
                GraphicsManager.Repaint(true);
                GraphicsManager.FitScreen();
                var geo302 = SearchManager.GetGeometry(302);
                foreach (var entity in geo302){
                    if (entity is ArcGeometry arc){
                        if ((rotaryPDBox / 2) - arc.Data.Radius <= 0.001){
                            entity.Selected = true;
                            entity.Commit();
                        }
                        else{
                            entity.Selected = false;
                            entity.Commit();
                        }
                    }
                }
                var selectedGeo1 = ChainManager.ChainAllSelected();
                foreach (var chain in selectedGeo1){
                    Mastercam.IO.Interop.SelectionManager.DoSolidExtrudeCreate(chain, totalLength);
                }
                SelectionManager.UnselectAllGeometry();
                foreach (var entity in geo302){
                    if (entity is ArcGeometry arc){
                        if (arc.Data.Radius - (drillSizeBox / 2) <= 0.001){
                            entity.Selected = true;
                            entity.Commit();
                        }
                        else{
                            entity.Selected = false;
                            entity.Commit();
                        }
                    }
                }
                var solid = SearchManager.GetSolidGeometry()[0];
                var selectedGeo2 = ChainManager.ChainAllSelected();
                foreach (var chain in selectedGeo2){
                    if (thruBox == "No"){
                        Mastercam.IO.Interop.SelectionManager.DoSolidExtrudeCut(chain, endToHole, solid.GetEntityID());
                    }
                    else{
                        Mastercam.IO.Interop.SelectionManager.DoSolidExtrudeCut(chain, endToThru, solid.GetEntityID());
                    }
                }
                SelectionManager.UnselectAllGeometry();
                GraphicsManager.Repaint(true);
            }


            StepOne();
            if (paSide == "GearSide")
            {
                GearSide();
            }
            if (paSide == "Tailstock")
            {
                nGearSide();
            }

            LevelsManager.SetLevelVisible(300, true);
            LevelsManager.SetLevelVisible(301, true);
            LevelsManager.SetLevelVisible(302, true);
            LevelsManager.SetLevelVisible(14, true);
            LevelsManager.SetLevelVisible(11, true);

            ViewManager.GraphicsView = SearchManager.GetSystemView(SystemPlaneType.Top);
            ViewManager.WorkCoordinateSystem = SearchManager.GetSystemView(SystemPlaneType.Top);
            ViewManager.TPlane = SearchManager.GetSystemView(SystemPlaneType.Top);
            ViewManager.CPlane = SearchManager.GetSystemView(SystemPlaneType.Top);
            ViewManager.RefreshPlanesManager();
            GraphicsManager.Repaint(true);
            GraphicsManager.FitScreen();

            return MCamReturn.NoErrors;
        }
    }
}
