using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class StoryTreeRunner : Singleton<StoryTreeRunner>
{
    public StoryTree tree;
    private Node nowNode;

    private void Start()
    {
        tree = tree.Clone();
    }

    private void Update()
    {
        tree.Update();
    }


    public void UpdateNowNode()
    {
        foreach (var node in tree.nodes)
        {
            //Debug.Log(node.description + " status is " + node.state );
            if (node.state == Node.State.Running)
            {
                nowNode = node;
                Debug.Log($"nowNodeIs:{nowNode.description}");
            }
        }
    }

    public StoryNode GetNowStoryNode()
    {
        if (nowNode is StoryNode)
        {
            return (StoryNode)nowNode;
        }
        else return null;
    }

    public EndNode GetEndNode()
    {
        if (nowNode is StoryNode)
        {
            StoryNode tempNode = (StoryNode)nowNode;
            if (tempNode.children.Count > 0)
            {
                foreach (var child in tempNode.children)
                {
                    if (child is EndNode)
                    {
                        return (EndNode)child;
                    }
                }
                return null;
            }
            else return null;
        }
        else return null;
    }

    public void RefreshTree()
    {
        foreach (var child in tree.nodes)
        {
            child.started = false;
            child.state = Node.State.Failure;
        }
        tree.rootNode.state = Node.State.Running;
    }


}
