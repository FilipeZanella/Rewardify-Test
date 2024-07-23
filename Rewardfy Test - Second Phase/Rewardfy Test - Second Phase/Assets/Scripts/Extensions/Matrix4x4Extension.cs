using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Matrix4x4Extension 
{
    public static Matrix4x4 SetPosition(this Matrix4x4 originalMatrix, Vector3 newPosition)
    {
        Matrix4x4 newMatrix = originalMatrix;
        newMatrix.m03 = newPosition.x;
        newMatrix.m13 = newPosition.y;
        newMatrix.m23 = newPosition.z;
        return newMatrix;
    }
}
