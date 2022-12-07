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

namespace _PulseAirPrint
{
    public class PulseAirPrint : Mastercam.App.NetHook3App
    {
        public Mastercam.App.Types.MCamReturn PulseAirPrintRun(Mastercam.App.Types.MCamReturn notused)
        {
            var PD = 0.0;
            var drillSize = 0.0;
            var points = SearchManager.GetGeometry(11);
            foreach (var point in points) {
                if (point is PointGeometry pointy){
                    MCView Top = new MCView();
                    Mastercam.Math.Point3D origin = new Mastercam.Math.Point3D(0.0, 0.0, 0.0);
                    Mastercam.Math.Point3D pt1 = new Mastercam.Math.Point3D(pointy.Data.x, 0.0, 0.0);
                    point.Translate(pt1, origin, Top, Top);
                }
            }
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
