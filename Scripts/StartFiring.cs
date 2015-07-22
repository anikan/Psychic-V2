using RAIN.Action;
using RAIN.Core;
using UnityEngine;

[RAINAction]
public class StartFiring : RAINAction
{
    public StartFiring()
    {
        actionName = "Start Firing";
    }

    public override ActionResult Execute(AI ai)
    {
        ai.Body.SendMessage("StartFiring");
        return ActionResult.SUCCESS;
    }
}