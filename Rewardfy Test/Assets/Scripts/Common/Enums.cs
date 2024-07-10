using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VisualTemplateType 
{
    Default = 0,
    Obstacle = 1,
    Initial = 2,
    Final = 3,
    Path_Unreached = 4,
    Path_Reached = 5
}

public enum CellStatus 
{
    Empty,
    Obstacle
}