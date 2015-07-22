using RAIN.Action;
using RAIN.Core;
using UnityEngine;

[RAINAction]
public class StopFiring : RAINAction
{
    public StopFiring()
    {
        actionName = "Stop Firing";
    }

    public override ActionResult Execute(AI ai)
    {
        ai.Body.SendMessage("StopFiring");
        return ActionResult.SUCCESS;
    }
}