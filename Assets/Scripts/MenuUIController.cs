using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{

    [SerializeField] private Button startGameButton;
    [SerializeField] private Button personIntroButton;
    [SerializeField] private Button leaveGameButton;

    [SerializeField] private Button personIntroGoBackButton;

    private void Start()
    {
        ButtonInit();
    }

    private void ButtonInit()
    {
        startGameButton.onClick.AddListener(delegate
        {
            ViewSwitchManager.Instance.OpenViewByName("MainGame");

            if (AudioStoryManager.Instance != null)
            {
                AudioStoryManager.Instance.PlayFirstBGM();
            }
            if (AudioStoryManager.Instance != null) { AudioStoryManager.Instance.PlayButtonClickAudio(); }

            MainStoryManager.Instance.StartFirstDialogue();
        });
        personIntroButton.onClick.AddListener(delegate
        {
            ViewSwitchManager.Instance.OpenViewByName("PersonIntro");
            if (AudioStoryManager.Instance != null) { AudioStoryManager.Instance.PlayButtonClickAudio(); }
        });
        personIntroGoBackButton.onClick.AddListener(delegate
        {
            ViewSwitchManager.Instance.OpenViewByName("Menu");
            if (AudioStoryManager.Instance != null) { AudioStoryManager.Instance.PlayButtonClickAudio(); }
        });
        leaveGameButton.onClick.AddListener(delegate
        {
            Application.Quit();
            if (AudioStoryManager.Instance != null) { AudioStoryManager.Instance.PlayButtonClickAudio(); }
        });
    }

}
