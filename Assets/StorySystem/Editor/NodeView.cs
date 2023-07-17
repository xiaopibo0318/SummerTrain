using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<NodeView> OnNodeSelected;

    public Node node;
    public Port inputPort;
    public Port outputPort;

    /// <summary>
    /// Iniailize Node View
    /// </summary>
    /// <param name="node"></param>
    public NodeView(Node node) : base("Assets/StorySystem/Editor/NodeView.uxml")
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guid;

        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPorts();
        CreateOutputPorts();

        Label descrptionLabel = this.Q<Label>("description");
        descrptionLabel.bindingPath = "description";
        descrptionLabel.Bind(new SerializedObject(node));

        Button addChoiceButton = this.Q<Button>("AddChoiceButton");
        addChoiceButton.bindingPath = "AddChoicePort";
        addChoiceButton.Bind(new SerializedObject(node));
        addChoiceButton.clicked += AddChoisePort;

        
    }


    private void CreateInputPorts()
    {
        if (node is ActionNode)
        {
            AddToClassList("action"); // We Can change the style sheet in UI Builder.
            inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is CompositeNode)
        {
            AddToClassList("composite");
            inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is DecoratorNode)
        {
            AddToClassList("decorate");
            inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is RootNode)
        {
            AddToClassList("root");
        }else if (node is EndNode)
        {
            AddToClassList("end");
            inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));
        }

        if (inputPort != null)
        {
            inputPort.portName = "";
            inputPort.style.flexDirection = FlexDirection.Column;// handle the align problem.
            inputContainer.Add(inputPort);
        }
    }

    private void CreateOutputPorts()
    {
        if (node is ActionNode)
        {

        }
        else if (node is CompositeNode)
        {
            outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));

        }
        else if (node is DecoratorNode)
        {
            outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));

        }
        else if (node is RootNode)
        {
            outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }else if (node is EndNode)
        {

        }

        if (outputPort != null)
        {
            outputPort.portName = "";
            outputPort.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(outputPort);
        }

    }

    private void AddChoisePort()
    {
        Debug.Log("Button Click is triggered");
        if (node is CompositeNode)
        {
            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            if (outputPort != null)
            {
                var a = outputContainer.Query(name: "connector").ToList().Count;
                outputPort.portName = "";
                outputPort.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(outputPort);
            }
        }
        EditorUtility.SetDirty(node);
        AssetDatabase.SaveAssets();

    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Undo.RecordObject(node, "Story Tree {Set Position}");
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
        EditorUtility.SetDirty(node); // Make Sure the view can work
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
        {
            OnNodeSelected.Invoke(this);
        }
    }

    public void SortChildren()
    {
        CompositeNode composite = node as CompositeNode;
        if (composite != null)
        {
            composite.children.Sort(SortByHorizontalPosition);
        }
    }

    private int SortByHorizontalPosition(Node left, Node right)
    {
        return left.position.x > right.position.x ? 1 : -1;
    }


    public void UpdateState()
    {
        RemoveFromClassList("running");
        RemoveFromClassList("failure");
        RemoveFromClassList("success");

        if (Application.isPlaying)
        {
            switch (node.state)
            {
                case Node.State.Running:
                    if (node.started)
                    {
                        AddToClassList("running");
                    }
                    break;
                case Node.State.Failure:
                    AddToClassList("failure");
                    break;
                case Node.State.Success:
                    AddToClassList("success");
                    break;
            }
        }
    }



}
