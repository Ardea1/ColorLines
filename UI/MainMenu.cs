using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Класс, содержащий кнопки
/// и методы для отображения панелей Справка и Помощь
/// </summary>
public class MainMenu : MonoBehaviour
{
    public enum ButtonType
    {
        New,
        Save,
        Load,
        Info,
        Help,
        Exit,
        Ok,
        Ok1,
        Mute
    }

    public event System.Action<ButtonType> OnClick;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private MenuButton newGameButton;

    [SerializeField]
    private MenuButton saveGameButton;

    [SerializeField]
    private MenuButton loadGameButton;

    [SerializeField]
    private MenuButton infoGameButton;

    [SerializeField]
    private MenuButton helpGameButton;

    [SerializeField]
    private MenuButton exitGameButton;

    [SerializeField]
    private MenuButton okButton;

    [SerializeField]
    private CanvasGroup helpPanel;

    [SerializeField]
    private MenuButton ok1Button;

    [SerializeField]
    private CanvasGroup infoPanel;

    [SerializeField]
    private MenuButton muteButton;

    /// <summary>
    /// Панель с правилами игры
    /// </summary>
    public bool ActiveHelpPanel
    {
        get
        {
            return helpPanel.alpha == 1f;
        }
        set
        {
            helpPanel.alpha = value ? 1f : 0f;
            helpPanel.interactable = value;
            helpPanel.blocksRaycasts = value;
        }
    }

    /// <summary>
    /// Панель с информацией о создателях первой игры
    /// </summary>
    public bool ActiveInfoPanel
    {
        get
        {
            return infoPanel.alpha == 1f;
        }
        set
        {
            infoPanel.alpha = value ? 1f : 0f;
            infoPanel.interactable = value;
            infoPanel.blocksRaycasts = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        newGameButton.OnClick += NewGameButton_OnClick;
        saveGameButton.OnClick += SaveGameButton_OnClick;
        loadGameButton.OnClick += LoadGameButton_OnClick;
        infoGameButton.OnClick += InfoGameButton_OnClick;
        helpGameButton.OnClick += HelpGameButton_OnClick;
        exitGameButton.OnClick += ExitGameButton_OnClick;

        okButton.OnClick += OkButton_OnClick;
        ok1Button.OnClick += Ok1Button_OnClick;

        muteButton.OnClick += MuteButton_OnClick;
    }

    private void MuteButton_OnClick(MenuButton button)
    {
        OnClick?.Invoke(ButtonType.Mute);
    }

    private void Ok1Button_OnClick(MenuButton button)
    {
        OnClick?.Invoke(ButtonType.Ok1);
    }

    private void OkButton_OnClick(MenuButton button)
    {
        OnClick?.Invoke(ButtonType.Ok);
    }

    private void HelpGameButton_OnClick(MenuButton button)
    {
        OnClick?.Invoke(ButtonType.Help);
    }

    private void InfoGameButton_OnClick(MenuButton button)
    {
        OnClick?.Invoke(ButtonType.Info);
    }

    private void LoadGameButton_OnClick(MenuButton button)
    {
        OnClick?.Invoke(ButtonType.Load);
    }

    private void SaveGameButton_OnClick(MenuButton button)
    {
        OnClick?.Invoke(ButtonType.Save);
    }

    private void NewGameButton_OnClick(MenuButton button)
    {
        OnClick?.Invoke(ButtonType.New);
    }

    private void ExitGameButton_OnClick(MenuButton button)
    {
        OnClick?.Invoke(ButtonType.Exit);
    }
}
