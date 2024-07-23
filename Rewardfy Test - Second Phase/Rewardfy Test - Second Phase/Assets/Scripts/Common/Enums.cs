using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellTemplateType 
{
    Default = 0,
    Obstacle = 1,
    Initial = 2,
    Path_Reached = 3,
    PathFinder_Around = 4,
    PathFinder_Included = 5,
    PathFinder_Excluded = 6,
    PathFinder_Current = 7,
}

public enum CellStatus 
{
    Empty,
    Obstacle
}