using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndNode : Node
{
    public Sprite finalImage;
    private bool isStarted = false;
    [HideInInspector] public bool isFinished = false;

    protected override void OnStart()
    {
        isStarted = true;
        MainStoryManager.Instance.GoStoryEnd();
    }

    protected override void OnStop()
    {
        TimeManager.Instance.Delay(.05f, StoryTreeRunner.Instance.RefreshTree);

    }

    protected override State OnUpdate()
    {
        if (!isStarted) { return State.Failure; }

        if (isFinished)
        {
            return State.Success;
        }
        else { return State.Running; }
    }
}






