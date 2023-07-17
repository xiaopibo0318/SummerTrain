using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MainStoryManager : Singleton<MainStoryManager>
{
    [SerializeField] Button nextButton;
    [SerializeField] float textSpeed;
    [SerializeField] Image storyImage;
    [SerializeField] Text signalText;
    [SerializeField] Transform optionParent;
    [SerializeField] Button optionPrefab;
    [SerializeField] GameObject optionBG;
    [SerializeField] Image endingImage;
    [SerializeField] Button endButton;
    private List<string> storyText;
    private StoryNode nowStoryNode;
    private EndNode nowEndNode;
    private bool textFinished;
    private Coroutine nowCoroutine = null;
    int index = 0;
    private bool isEndInStory = false;
    private bool isFin = false;

    private void Start()
    {
        if (endingImage != null)
        {
            endingImage.gameObject.SetActive(false);
        }
        if (endButton != null)
        {
            endButton.onClick.AddListener(delegate
            {
                ViewSwitchManager.Instance.OpenViewByName("Menu");
                AudioStoryManager.Instance.StartMainBgm();
                if (nowEndNode != null)
                {
                    nowEndNode.isFinished = true;
                    nowEndNode.state = Node.State.Success;
                    nowEndNode.Update();
                }
                if (AudioStoryManager.Instance != null) { AudioStoryManager.Instance.PlayButtonClickAudio(); }

            });
        }
        signalText.text = "";
        textFinished = true;
        nextButton.onClick.AddListener(() => GoToNextDialogue());
        StoryTreeRunner.Instance.UpdateNowNode();
        UpdateNowNode();
    }

    IEnumerator StartTypingText()
    {
        if (isFin) { yield break; }

        textFinished = false;
        signalText.text = "";
        //Debug.Log($"now node is:{nowStoryNode.description}");
        for (int i = 0; i < storyText[index].Length; i++)
        {
            signalText.text += storyText[index][i];
            yield return new WaitForSeconds(textSpeed);
        }
        textFinished = true;
        index++;
    }

    private void UpdateNowNode()
    {
        nowStoryNode = StoryTreeRunner.Instance.GetNowStoryNode();
        //Debug.Log(nowStoryNode.description);
        if (nowStoryNode != null)
        {
            storyImage.sprite = nowStoryNode.storyImage;
            storyText = nowStoryNode.dialogueText;
        }

        nowEndNode = StoryTreeRunner.Instance.GetEndNode();
        if (nowEndNode != null)
        {
            Debug.Log($"now end node is: {nowEndNode.description}");
            if (endingImage != null)
            {
                endingImage.sprite = nowEndNode.finalImage;
            }
        }
    }

    public void StartFirstDialogue() => InitStart();

    private void InitStart()
    {
        StoryTreeRunner.Instance.UpdateNowNode();
        UpdateNowNode();
        TimeManager.Instance.Delay(.1f, GoToNextDialogue);
    }

    private void GoToNextDialogue()
    {

        DetectGoToNextStory();


        if (!textFinished) // Double Click To See All Text
        {
            if (nowCoroutine != null) { StopCoroutine(nowCoroutine); }
            nowCoroutine = null;

            signalText.text = storyText[index];
            textFinished = true;
            index++;
            return;
        }

        if (isEndInStory) { return; }
        if (isFin) { return; }



        StoryTreeRunner.Instance.UpdateNowNode();
        UpdateNowNode();

        if (nowCoroutine != null) { StopCoroutine(nowCoroutine); }
        nowCoroutine = null;
        nowCoroutine = StartCoroutine(StartTypingText());
    }

    private void DetectGoToNextStory()
    {
        //if (nowStoryNode.children.Count != 0)
        //{
        //    foreach (var child in nowStoryNode.children)
        //    {
        //        if (child is EndNode)
        //        {
        //            StoryTreeRunner.Instance.UpdateNowNode();
        //            UpdateNowNode();
        //        }
        //    }
        //}

        if (nowEndNode != null && storyText != null && index == storyText.Count)
        {
            Debug.Log("Entered EndNode");
            nowStoryNode.isFinished = true;
            nowStoryNode.state = Node.State.Success;
            nowStoryNode.Update();
            isFin = true;
            return;
        }

        if (storyText != null && index == storyText.Count)
        {
            isEndInStory = true;
            index = 0;

            if (optionBG != null)
            {
                optionBG.SetActive(true);
            }

            for (int i = 0; i < nowStoryNode.children.Count; i++)
            {
                var temp = i;
                var option = Instantiate(optionPrefab, optionParent);
                option.GetComponentInChildren<Text>().text = nowStoryNode.optionList[temp];
                option.onClick.AddListener(delegate
                {
                    if (AudioStoryManager.Instance != null) { AudioStoryManager.Instance.PlayButtonClickAudio(); }
                    nowStoryNode.chooseEdge = temp;
                    nowStoryNode.isFinished = true;
                    nowStoryNode.state = Node.State.Success;
                    nowStoryNode.Update();
                    nextButton.interactable = true;
                    DestroyAllOption();
                    GoToNextDialogue();
                });
            }
            nextButton.interactable = false;
        }
        if (AudioStoryManager.Instance != null)
        {
            if (nowStoryNode.bgm != null)
            {
                AudioStoryManager.Instance.PlayMusic();
            }
        }
    }

    private void DestroyAllOption()
    {
        if (optionBG != null) optionBG.SetActive(false);

        for (int i = 0; i < optionParent.childCount; i++)
        {
            GameObject x = optionParent.GetChild(i).gameObject;
            Destroy(x);
        }
        isEndInStory = false;
    }

    public void GoStoryEnd()
    {
        if (endingImage != null)
        {
            endingImage.gameObject.SetActive(true);
        }
    }

}
