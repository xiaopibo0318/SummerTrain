using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class StoryNode : CompositeNode
{
    public Sprite storyImage;
    [HideInInspector] public bool isFinished;
    [HideInInspector] public bool isStarted = false;
    public TextAsset storyFile;
    public List<string> dialogueText;
    public List<string> optionList;
    [HideInInspector] public int chooseEdge = 0;
    public AudioClip bgm;


    protected override void OnStart()
    {
        ChangeCsvToText();
        isStarted = true;
        if (bgm != null && AudioStoryManager.Instance != null)
        {
            Debug.Log("Update Success");
            AudioStoryManager.Instance.UpdateBGM(bgm);
        }

    }

    protected override void OnStop()
    {
        if (children.Count == 1 && children[0] is EndNode)
        {
            children[0].state = State.Running;
            children[0].Update();
        }
        Debug.Log("¿ï¾ÜªºIndex¬°" + chooseEdge);
        children[chooseEdge].state = State.Running;
        children[chooseEdge].Update();
    }

    protected override State OnUpdate()
    {
        if(AudioStoryManager.Instance != null)
        {
            if(bgm != null && AudioStoryManager.Instance.nowBGM == null)
            {
                Debug.Log("Update Success");
                AudioStoryManager.Instance.UpdateBGM(bgm);
            }
        }

        if (!isStarted) { return State.Failure; }

        if (isFinished)
        {
            return State.Success;
        }
        else { return State.Running; }
    }


    private void ChangeCsvToText()
    {
        if (!storyFile) { Debug.Log("Loading Story Data Failed."); return; }
        dialogueText.Clear();
        string[] textLine = storyFile.text.Split("\n");
        foreach (var text in textLine)
        {
            if (text == "") break;
            dialogueText.Add(text);
        }

    }



}
