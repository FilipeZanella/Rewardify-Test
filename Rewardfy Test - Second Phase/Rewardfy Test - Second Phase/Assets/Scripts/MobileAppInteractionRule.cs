using System.Collections; 
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MobileAppInteractionRule : IAppInteractionHandler
{
    public IEnumerator Behaviour(IAppController controller, IMapInputHandler input)
    {
        float time = 0, lastTime = 0, duration = 0.23f;
        ICell lastCellPressed = null;

        yield return LoopUtility.LoopCoroutine(() => 
        {
            if (lastCellPressed != null) 
            {
                time += Time.deltaTime;

                if (time > duration) 
                {
                    if (lastCellPressed.Status == CellStatus.Obstacle)
                    {
                        controller.FreeCell(lastCellPressed.Coordenate);
                    }
                    else if (lastCellPressed.Status == CellStatus.Empty)
                    {
                        controller.AddObstacle(lastCellPressed.Coordenate);
                    }

                    lastCellPressed = null;
                    time = 0;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (lastCellPressed != null && Time.time - lastTime <= duration) 
                {
                    var cell = input.GetSelectedCell();
                    if (cell == lastCellPressed)
                    {
                        controller.SelectCell(cell.Coordenate);
                    }
                    lastCellPressed = null;
                    time = 0;
                }
                else 
                {
                    lastCellPressed = input.GetSelectedCell();
                    lastTime = Time.time;
                }
            }
        });
    }
}
