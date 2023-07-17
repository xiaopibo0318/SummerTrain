using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;
using System;

public class StoryTreeEditor : EditorWindow
{
    StoryTreeView treeView;
    InspectorView inspectorView;

    [MenuItem("StoryTreeEditor/Editor")]
    public static void OpenWindow()
    {
        StoryTreeEditor wnd = GetWindow<StoryTreeEditor>();
        wnd.titleContent = new GUIContent("StoryTreeEditor");
        Debug.Log("Trigger Button Success");
    }

    //To make your assest can open by click the tree scriptable object.
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is StoryTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }


    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;


        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/StorySystem/Editor/StoryTreeEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/StorySystem/Editor/StoryTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<StoryTreeView>();
        inspectorView = root.Q<InspectorView>();
        treeView.OnNodeSelected = OnNodeSelectionChanged;
        OnSelectionChange();
    }


    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
        }
    }
    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }


    private void OnSelectionChange()
    {
        StoryTree tree = Selection.activeObject as StoryTree;

        if (!tree)
        {
            if (Selection.activeGameObject)
            {
                StoryTreeRunner runner = Selection.activeGameObject.GetComponent<StoryTreeRunner>();
                if (runner)
                {
                    tree = runner.tree;
                }
            }
        }

        if (Application.isPlaying)
        {
            if (tree)
            {
                treeView?.PopulateView(tree);
            }
        }

        //Need to wait the Asset ready
        if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
        {
            treeView?.PopulateView(tree);
        }
    }

    void OnNodeSelectionChanged(NodeView node)
    {
        inspectorView.UpdateSelection(node);
    }

    private void OnInspectorUpdate()
    {
        treeView?.UpdateNodeStates();
    }
}
