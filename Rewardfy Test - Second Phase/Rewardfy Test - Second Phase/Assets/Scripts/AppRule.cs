using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AppRule : IAppInteractionHandler
{
    public IEnumerator Behaviour(IAppController controller, IMapInputHandler input)
    {
        yield return LoopUtility.LoopCoroutine(() => 
        {
            if (Input.GetMouseButtonDown(1)) 
            {
                var cell = input.GetSelectedCell();
                if (cell != null) 
                {
                    if (cell.Status == CellStatus.Obstacle) 
                    {
                        controller.FreeCell(cell.Coordenate);
                    }
                    else if (cell.Status == CellStatus.Empty)
                    {
                        controller.AddObstacle(cell.Coordenate);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0)) 
            {
                var cell = input.GetSelectedCell(); 
                if (cell != null) 
                {
                    controller.SelectCell(cell.Coordenate);
                }
            }
        });
    }
}