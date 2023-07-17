using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }

    public State state = State.Failure;
    [HideInInspector] public bool started = false;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    //[HideInInspector] public Aiagent agent;
    [TextArea] public string description;

    public State Update()
    {
        if (!started)
        {
            OnStart();
            started = true;
        }
        state = OnUpdate();

        if (state == State.Success)
        {
            OnStop();
            started = false;
        }

        return state;

    }

    public virtual Node Clone()
    {
        return Instantiate(this);
    }


    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();



}
