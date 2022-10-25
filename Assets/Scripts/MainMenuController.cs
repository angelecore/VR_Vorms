using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private RectTransform mainMenu;

    [SerializeField]
    private RectTransform optionsMenu;
    
    private static void Show(Component component)
    {
        component.gameObject.SetActive(true);
    }
    private static void Hide(Component component)
    {
        component.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
    }
    public void StartGame()
    {
        Scenes.LoadNextScene();
    }
    public void ExitGame()
    {
        Scenes.ExitGame();
    }
    public void ShowMainMenu()
    {
        Show(mainMenu);
        Hide(optionsMenu);
    }
    public void ShowOptionsMenu()
    {
        Show(optionsMenu);
        Hide(mainMenu);
    }
}
