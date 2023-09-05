using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class StoryTreeRunner : Singleton<StoryTreeRunner>
{
    public StoryTree tree;
    private Node nowNode;

    //private StoryTree cloneOriginTree;

    private void Start()
    {
        tree = tree.Clone();
        //TimeManager.Instance.Delay(.5f, CloneOriginTree);
    }

    //private void CloneOriginTree()
    //{
    //    cloneOriginTree = tree;
    //}

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
            if (child is StoryNode)
            {
                var storyChild = (StoryNode)child;
                storyChild.started = false;
                storyChild.isFinished = false;
            }
            else if (child is EndNode)
            {
                var endNode = (EndNode)child;
                endNode.started = false;
                endNode.isFinished = false;
            }
            child.state = Node.State.Failure;
        }
        tree.rootNode.state = Node.State.Running;
        MainStoryManager.Instance.ResetStory();
        //tree = cloneOriginTree;
        //tree = tree.Clone();
    }


}
