using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class MainMenuUIController : MonoSingleton<MainMenuUIController>
{
    //Menu Items
    public MenuItem[] MainMenuItems;
    public MenuItem[] SettingsMenuItems;
    public MenuItem[] PopUpSaveMenuItems;
    public MenuItem[] PopUpQuitMenuItems;
    public MenuItem[] CreditsMenuItems;

    //Item Settings
    public float itemRevealDuration = 0.5f; //The total duration of the reveal animation for the items in the main menu
    public float itemHideDuration = 0.5f; //The total duration of the hide animation for the items in the main menu
    public Ease itemRevealEase;
    public float itemVerticalSpacing = 25f;
    public float itemHorizontalSpacing = 25f;
    public float itemHoverDuration = 0.5f;
    public Ease itemHoverEase;
    public float itemHoverScale = 1.1f;

    //Fade Settings
    public CanvasGroup fadeImageCanvasGroup;
    public float fadeDuration = 0.5f;
    public Ease fadeEase;
    public Transform backgroundSprite;
    public Vector2 backgroundMovementRange = new Vector2(0.5f, 0.5f);
    public TextMeshProUGUI activeControllerText;

    //Prompt Settings
    public RectTransform[] gamepadPrompts;
    public RectTransform[] keyboardPrompts;

    //Private Variables
    private Sequence mainMenuSequence; //Reveals the items in the main menu
    private Sequence settingsMenuSequence; //Reveals the items in the settings menu
    private Sequence popUpSaveSequence; //Reveals the items in the pop up menu
    private Sequence popUpQuitSequence; //Reveals the items in the pop up menu
    private Sequence creditsMenuSequence; //Reveals the items in the credits menu
    private int currentItemIndex = 0;
    private MenuType currentMenu = MenuType.MainMenu;
    private MenuItem[][] menuItems;
    private Vector2 screenSize;
    void Start()
    {
        screenSize = new Vector2(Screen.width, Screen.height);

        #region Initialize Main Menu Items
        menuItems = new MenuItem[5][];
        menuItems[0] = MainMenuItems;
        menuItems[1] = SettingsMenuItems;
        menuItems[2] = PopUpSaveMenuItems;
        menuItems[3] = PopUpQuitMenuItems;
        menuItems[4] = CreditsMenuItems;
        mainMenuSequence = DOTween.Sequence();
        mainMenuSequence.SetAutoKill(false);
        foreach (MenuItem item in MainMenuItems)
        {
            if (item.itemType != MenuItemType.NonInteractable)
                AddHoverEvents(item);

            item.AssignEvents();
            SequenceSetupRightToLeft(mainMenuSequence, item.rectTransform, true);
        }
        mainMenuSequence.Pause();
        #endregion

        #region Initialize Settings Menu Items
        settingsMenuSequence = DOTween.Sequence();
        settingsMenuSequence.SetAutoKill(false);
        foreach (MenuItem item in SettingsMenuItems)
        {
            if (item.itemType != MenuItemType.NonInteractable)
                AddHoverEvents(item);

            item.AssignEvents();
            if (item.name.Contains("Quality"))
                item.itemText.text = GameManager.Instance.GetQualityName();

            if (item.name.Contains("Audio"))
                item.itemText.text = Mathf.CeilToInt(AudioManager.Instance.GetMasterVolume() * 10).ToString();

            SequenceSetupRightToLeft(settingsMenuSequence, item.rectTransform, false);
        }
        settingsMenuSequence.Pause();
        #endregion

        #region Initialize Pop Up Save Menu Items
        popUpSaveSequence = DOTween.Sequence();
        popUpSaveSequence.SetAutoKill(false);
        SequenceSetupPopUp(popUpSaveSequence, PopUpSaveMenuItems[0].rectTransform.parent.GetComponent<RectTransform>());
        foreach (MenuItem item in PopUpSaveMenuItems)
        {
            if (item.itemType != MenuItemType.NonInteractable)
                AddHoverEvents(item);

            item.AssignEvents();

            SequenceSetupPopUp(popUpSaveSequence, item.rectTransform);
        }
        popUpSaveSequence.Pause();
        #endregion

        #region Initialize Pop Up Quit Menu Items
        popUpQuitSequence = DOTween.Sequence();
        popUpQuitSequence.SetAutoKill(false);
        SequenceSetupPopUp(popUpQuitSequence, PopUpQuitMenuItems[0].rectTransform.parent.GetComponent<RectTransform>());
        foreach (MenuItem item in PopUpQuitMenuItems)
        {
            if (item.itemType != MenuItemType.NonInteractable)
                AddHoverEvents(item);

            item.AssignEvents();

            SequenceSetupPopUp(popUpQuitSequence, item.rectTransform);
        }
        popUpQuitSequence.Pause();
        #endregion

        #region Initialize Credits Menu Items
        creditsMenuSequence = DOTween.Sequence();
        creditsMenuSequence.SetAutoKill(false);
        foreach (MenuItem item in CreditsMenuItems)
        {
            if (item.itemType != MenuItemType.NonInteractable)
                AddHoverEvents(item);

            item.AssignEvents();
            SequenceSetupPopUp(creditsMenuSequence, item.rectTransform);
        }
        creditsMenuSequence.Pause();
        #endregion

        #region Fade In
        fadeImageCanvasGroup.gameObject.SetActive(true);
        fadeImageCanvasGroup.alpha = 1;
        fadeImageCanvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEase).OnComplete(() => fadeImageCanvasGroup.gameObject.SetActive(false)).OnComplete(() => ShowHideMainMenu(true));
        #endregion
    }

    void LateUpdate()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mousePositionFromCenter = new Vector2(mousePosition.x - screenSize.x / 2f, mousePosition.y - screenSize.y / 2f);
        Vector2 mousePositionFromCenterNormalized = new Vector2(mousePositionFromCenter.x / screenSize.x, mousePositionFromCenter.y / screenSize.y);
        Vector2 backgroundMovement = new Vector2(mousePositionFromCenterNormalized.x * backgroundMovementRange.x, mousePositionFromCenterNormalized.y * backgroundMovementRange.y);
        backgroundSprite.localPosition = backgroundMovement;
    }

    #region Menu Navigation
    void AddHoverEvents(MenuItem item)
    {
        item.eventTrigger.triggers.Add(new EventTrigger.Entry() { eventID = EventTriggerType.PointerEnter, callback = new EventTrigger.TriggerEvent() });
        item.eventTrigger.triggers[0].callback.AddListener((data) => { OnPointerEnter(item.rectTransform); });
        item.eventTrigger.triggers.Add(new EventTrigger.Entry() { eventID = EventTriggerType.PointerExit, callback = new EventTrigger.TriggerEvent() });
        item.eventTrigger.triggers[1].callback.AddListener((data) => { OnPointerExit(item.rectTransform); });
    }

    void OnPointerEnter(RectTransform button)
    {
        if (button != menuItems[(int)currentMenu][currentItemIndex].rectTransform)
            OnPointerExit(menuItems[(int)currentMenu][currentItemIndex].rectTransform);
        button.DOScale(itemHoverScale, itemHoverDuration).SetEase(itemHoverEase);
    }

    void OnPointerExit(RectTransform button)
    {
        button.DOScale(1f, itemHoverDuration).SetEase(itemHoverEase);
    }

    public void MoveSelection(Vector2 direction)
    {
        int lastIndex = currentItemIndex;
        MenuItem[] menu = menuItems[(int)currentMenu];
        if (direction.y > 0)
        {
            currentItemIndex--;
            if (currentItemIndex < 0)
                currentItemIndex = menu.Length - 1;
            while (menu[currentItemIndex].itemType == MenuItemType.NonInteractable)
            {
                currentItemIndex--;
                if (currentItemIndex < 0)
                    currentItemIndex = menu.Length - 1;
            }
        }
        else if (direction.y < 0)
        {
            currentItemIndex++;
            if (currentItemIndex > menu.Length - 1)
                currentItemIndex = 0;
            while (menu[currentItemIndex].itemType == MenuItemType.NonInteractable)
            {
                currentItemIndex++;
                if (currentItemIndex > menu.Length - 1)
                    currentItemIndex = 0;
            }
        }
        else if (menu[currentItemIndex].itemType == MenuItemType.Selector)
        {
            if (direction.x < 0)
            {
                menu[currentItemIndex].events[0].Invoke();
            }
            else if (direction.x > 0)
            {
                menu[currentItemIndex].events[1].Invoke();
            }
            return;
        }
        else
        {
            return;
        }
        MoveSelectionToItem(lastIndex, currentItemIndex);
    }

    public void MoveSelectionToItem(int lastIndex, int newIndex)
    {
        int menuIndex = (int)currentMenu;
        if (menuItems[menuIndex][lastIndex].itemType != MenuItemType.NonInteractable)
            OnPointerExit(menuItems[menuIndex][lastIndex].rectTransform);

        if (menuItems[menuIndex][newIndex].itemType == MenuItemType.NonInteractable)
            return;
        OnPointerEnter(menuItems[menuIndex][newIndex].rectTransform);
    }

    public void Submit()
    {
        MenuItem item = menuItems[(int)currentMenu][currentItemIndex];
        if (item.itemType == MenuItemType.Button)
        {
            item.events[0].Invoke();
        }
    }

    public void Cancel()
    {
        MenuItem item = menuItems[(int)currentMenu][currentItemIndex];
        if (currentMenu == MenuType.SettingsMenu)
        {
            OnSettingsBackClick();
        }
    }

    public void SetDevicePrompt(bool isGamepad)
    {
        if (isGamepad)
        {
            foreach (RectTransform rect in gamepadPrompts)
            {
                rect.DOScale(1f, itemHideDuration).SetEase(itemHoverEase);
            }
            foreach (RectTransform rect in keyboardPrompts)
            {
                rect.DOScale(0f, itemHideDuration).SetEase(itemHoverEase);
            }
        }
        else
        {
            foreach (RectTransform rect in gamepadPrompts)
            {
                rect.DOScale(0f, itemHideDuration).SetEase(itemHoverEase);
            }
            foreach (RectTransform rect in keyboardPrompts)
            {
                rect.DOScale(1f, itemHideDuration).SetEase(itemHoverEase);
            }
        }
    }
    #endregion

    #region Menu Events
    public void OnNextQualityClick(TextMeshProUGUI text)
    {
        int index = GameManager.Instance.GetQualityIndex();
        index++;
        index = Mathf.Clamp(index, 0, GameManager.Instance.QualitySettingsNames.Length - 1);
        GameManager.Instance.CurrentQualityLevel = index;
        GameManager.Instance.SetQuality(SaveManager.Instance.qualityIndex = index);
        text.text = GameManager.Instance.GetQualityName();
    }

    public void OnPreviousQualityClick(TextMeshProUGUI text)
    {
        int index = GameManager.Instance.GetQualityIndex();
        index--;
        index = Mathf.Clamp(index, 0, GameManager.Instance.QualitySettingsNames.Length - 1);
        GameManager.Instance.CurrentQualityLevel = index;
        GameManager.Instance.SetQuality(SaveManager.Instance.qualityIndex = index);
        text.text = GameManager.Instance.GetQualityName();
    }

    public void OnSettingsClick()
    {
        ShowHideMainMenu(false);
        ShowHideSettingsMenu(true);
        //currentMenu = MenuType.SettingsMenu;
    }

    public void OnSettingsBackClick()
    {
        ShowHideSettingsMenu(false);
        //ShowHideMainMenu(true);
        //currentMenu = MenuType.MainMenu;
    }

    public void OnPopUpSaveYesClick()
    {
        SaveManager.Instance.SaveSettings();
        ShowHidePopUpSaveMenu(false);
        ShowHideMainMenu(true);
    }

    public void OnPopUpSaveNoClick()
    {
        SaveManager.Instance.LoadSettings();
        ShowHidePopUpSaveMenu(false);
        ShowHideMainMenu(true);
    }

    public void OnPopUpQuitYesClick()
    {
        Application.Quit();
    }

    public void OnPopUpQuitNoClick()
    {
        ShowHidePopUpQuitMenu(false);
        ShowHideMainMenu(true);
    }

    public void OnCreditsClick()
    {
        ShowHideMainMenu(false);
        ShowHideCreditsMenu(true);
    }

    public void OnCreditsBackClick()
    {
        ShowHideCreditsMenu(false);
        ShowHideMainMenu(true);
    }

    public void OnMasterVolumeIncreaseClick(TextMeshProUGUI text)
    {
        AudioManager.Instance.SetMasterVolume(Mathf.Clamp01(AudioManager.Instance.GetMasterVolume() + 0.1f));
        text.text = Mathf.CeilToInt((SaveManager.Instance.masterVolume = AudioManager.Instance.GetMasterVolume()) * 10).ToString();
    }

    public void OnMasterVolumeDecreaseClick(TextMeshProUGUI text)
    {
        AudioManager.Instance.SetMasterVolume(Mathf.Clamp01(AudioManager.Instance.GetMasterVolume() - 0.1f));
        text.text = Mathf.CeilToInt((SaveManager.Instance.masterVolume = AudioManager.Instance.GetMasterVolume()) * 10).ToString();
    }

    public void OnQuitClick()
    {
        ShowHideMainMenu(false);
        ShowHidePopUpQuitMenu(true);
    }
    #endregion

    #region Menu Sequence
    void SequenceSetupRightToLeft(Sequence s, RectTransform r, bool RightToLeft)
    {
        float widthBackup = r.rect.width;
        Vector2 offsetMinBackup = r.offsetMin;
        Vector2 offsetMaxBackup = r.offsetMax;
        if (RightToLeft)
        {
            r.offsetMin = new Vector2(r.offsetMin.x * -1, r.offsetMin.y);
            r.offsetMax = new Vector2(r.offsetMin.x + widthBackup, r.offsetMax.y);
            r.gameObject.SetActive(false);
            s.AppendCallback(() => r.gameObject.SetActive(!r.gameObject.activeSelf));
            s.Append(r.DOScale(Vector3.one, 0f));
            s.Append(DOTween.To(x1 => r.offsetMin = new Vector2(x1, r.offsetMin.y), r.offsetMin.x, offsetMinBackup.x, itemRevealDuration / MainMenuItems.Length * 0.5f).SetEase(itemRevealEase));
            s.Append(DOTween.To(x2 => r.offsetMax = new Vector2(x2, r.offsetMax.y), r.offsetMax.x, offsetMaxBackup.x, itemRevealDuration / MainMenuItems.Length * 0.5f).SetEase(itemRevealEase));
        }
        else
        {

            r.offsetMax = new Vector2(r.offsetMin.x * -1, r.offsetMax.y);
            r.offsetMin = new Vector2(r.offsetMin.x - widthBackup, r.offsetMin.y);
            r.gameObject.SetActive(false);
            s.AppendCallback(() => r.gameObject.SetActive(!r.gameObject.activeSelf));
            s.Append(r.DOScale(Vector3.one, 0f));
            s.Append(DOTween.To(x2 => r.offsetMax = new Vector2(x2, r.offsetMax.y), r.offsetMax.x, offsetMaxBackup.x, itemRevealDuration / MainMenuItems.Length * 0.5f).SetEase(itemRevealEase));
            s.Append(DOTween.To(x1 => r.offsetMin = new Vector2(x1, r.offsetMin.y), r.offsetMin.x, offsetMinBackup.x, itemRevealDuration / MainMenuItems.Length * 0.5f).SetEase(itemRevealEase));
        }
    }

    void SequenceSetupPopUp(Sequence s, RectTransform r)
    {
        r.gameObject.SetActive(false);
        r.localScale = Vector3.zero;
        s.AppendCallback(() => r.gameObject.SetActive(!r.gameObject.activeSelf));
        s.Append(DOTween.To(() => r.localScale, x => r.localScale = x, Vector3.one, itemRevealDuration / PopUpSaveMenuItems.Length * 0.5f).SetEase(itemRevealEase));
    }

    void ShowHideMainMenu(bool show)
    {
        if (show)
        {
            mainMenuSequence.timeScale = 1f;
            mainMenuSequence.PlayForward();
            currentItemIndex = 0;
            currentMenu = MenuType.MainMenu;
        }
        else
        {
            mainMenuSequence.timeScale *= (itemRevealDuration / itemHideDuration);
            mainMenuSequence.PlayBackwards();
        }
    }

    void ShowHideSettingsMenu(bool show)
    {
        if (show)
        {
            foreach (MenuItem item in SettingsMenuItems)
            {
                if (item.name.Contains("Quality"))
                    item.itemText.text = GameManager.Instance.GetQualityName();

                if (item.name.Contains("Audio"))
                    item.itemText.text = Mathf.CeilToInt(AudioManager.Instance.GetMasterVolume() * 10).ToString();
            }
            settingsMenuSequence.timeScale = 1f;
            settingsMenuSequence.PlayForward();
            currentItemIndex = 0;
            currentMenu = MenuType.SettingsMenu;
        }
        else
        {
            settingsMenuSequence.timeScale *= (itemRevealDuration / itemHideDuration);
            settingsMenuSequence.PlayBackwards();
            if (!SaveManager.Instance.isSettingsSaved())
            {
                ShowHidePopUpSaveMenu(true);
            }
            else
            {
                ShowHideMainMenu(true);
            }
        }
    }

    void ShowHidePopUpSaveMenu(bool show)
    {
        if (show)
        {
            popUpSaveSequence.timeScale = 1f;
            popUpSaveSequence.PlayForward();
            currentItemIndex = 0;
            currentMenu = MenuType.PopUpSaveMenu;
        }
        else
        {
            popUpSaveSequence.timeScale *= (itemRevealDuration / itemHideDuration);
            popUpSaveSequence.PlayBackwards();
        }
    }

    void ShowHidePopUpQuitMenu(bool show)
    {
        if (show)
        {
            popUpQuitSequence.timeScale = 1f;
            popUpQuitSequence.PlayForward();
            currentItemIndex = 0;
            currentMenu = MenuType.PopUpQuitMenu;
        }
        else
        {
            popUpQuitSequence.timeScale *= (itemRevealDuration / itemHideDuration);
            popUpQuitSequence.PlayBackwards();
        }
    }

    void ShowHideCreditsMenu(bool show)
    {
        if (show)
        {
            creditsMenuSequence.timeScale = 1f;
            creditsMenuSequence.PlayForward();
            currentItemIndex = 0;
            currentMenu = MenuType.CreditsMenu;
        }
        else
        {
            creditsMenuSequence.timeScale *= (itemRevealDuration / itemHideDuration);
            creditsMenuSequence.PlayBackwards();
        }
    }
    #endregion
}