using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameUIController : MonoSingleton<GameUIController>
{
    //Menu Items
    public MenuItem[] PauseMenuItems;
    public MenuItem[] PopUpQuitMenuItems;
    public MenuItem[] PopUpMainMenuItems;
    public MenuItem[] UpgradeMenuItems;

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

    //Private Variables
    private Sequence pauseMenuSequence; //Reveals the items in the pause menuals the items in the settings menu
    private Sequence popUpMainMenuSequence; //Reveals the items in the pop up menu
    private Sequence popUpQuitSequence; //Reveals the items in the pop up menu
    private Sequence upgradeMenuSequence; //Reveals the items in the upgrade menu
    private int currentItemIndex = 0;
    private GameMenuType currentMenu = GameMenuType.None;
    private MenuItem[][] menuItems;

    public RectTransform healthBarParent;
    public RectTransform ArmorBarParent;
    public RectTransform DashBarParent;
    public RectTransform XPBarParent;
    public Image healthBar;
    public Image ArmorBar;
    public Image DashBar;
    public Image XPBarLeft, XPBarRight;
    public TextMeshProUGUI messageText;
    public RectTransform[] weapons; //0: Melee, 1: Ranged
    private Player player;

    public void Start()
    {
        player = Player.Instance;
        messageText.text = "";

        #region Initialize Menu Items
        menuItems = new MenuItem[4][];
        menuItems[0] = PauseMenuItems;
        menuItems[1] = PopUpQuitMenuItems;
        menuItems[2] = PopUpMainMenuItems;
        menuItems[3] = UpgradeMenuItems;
        #endregion
        #region Initialize Pause Menu
        pauseMenuSequence = DOTween.Sequence();
        pauseMenuSequence.SetAutoKill(false);
        SequenceSetupPopUp(pauseMenuSequence, PauseMenuItems[0].rectTransform.parent.GetComponent<RectTransform>(), itemRevealDuration / PauseMenuItems.Length);
        foreach (MenuItem item in PauseMenuItems)
        {
            if (item.itemType != MenuItemType.NonInteractable)
                AddHoverEvents(item);

            item.AssignEvents();
            SequenceSetupPopUp(pauseMenuSequence, item.rectTransform, itemRevealDuration / PauseMenuItems.Length);
        }
        pauseMenuSequence.Pause();
        #endregion

        #region Initialize Pop Up Quit Menu
        popUpQuitSequence = DOTween.Sequence();
        popUpQuitSequence.SetAutoKill(false);
        SequenceSetupPopUp(popUpQuitSequence, PopUpQuitMenuItems[0].rectTransform.parent.GetComponent<RectTransform>(), itemRevealDuration / PopUpQuitMenuItems.Length);
        foreach (MenuItem item in PopUpQuitMenuItems)
        {
            if (item.itemType != MenuItemType.NonInteractable)
                AddHoverEvents(item);

            item.AssignEvents();
            SequenceSetupPopUp(popUpQuitSequence, item.rectTransform, itemRevealDuration / PopUpQuitMenuItems.Length);
        }
        popUpQuitSequence.Pause();
        #endregion

        #region Initialize Pop Up Main Menu
        popUpMainMenuSequence = DOTween.Sequence();
        popUpMainMenuSequence.SetAutoKill(false);
        SequenceSetupPopUp(popUpMainMenuSequence, PopUpMainMenuItems[0].rectTransform.parent.GetComponent<RectTransform>(), itemRevealDuration / PopUpMainMenuItems.Length);
        foreach (MenuItem item in PopUpMainMenuItems)
        {
            if (item.itemType != MenuItemType.NonInteractable)
                AddHoverEvents(item);

            item.AssignEvents();
            SequenceSetupPopUp(popUpMainMenuSequence, item.rectTransform, itemRevealDuration / PopUpMainMenuItems.Length);
        }
        popUpMainMenuSequence.Pause();
        #endregion

        #region Initialize Upgrade Menu
        upgradeMenuSequence = DOTween.Sequence();
        upgradeMenuSequence.SetAutoKill(false);
        SequenceSetupPopUp(upgradeMenuSequence, UpgradeMenuItems[0].rectTransform.parent.GetComponent<RectTransform>(), itemRevealDuration / UpgradeMenuItems.Length);
        foreach (MenuItem item in UpgradeMenuItems)
        {
            if (item.itemType != MenuItemType.NonInteractable)
                AddHoverEvents(item);

            item.AssignEvents();
            SequenceSetupPopUp(upgradeMenuSequence, item.rectTransform, itemRevealDuration / UpgradeMenuItems.Length);
        }
        upgradeMenuSequence.Pause();
        #endregion

        #region Fade In
        fadeImageCanvasGroup.gameObject.SetActive(true);
        fadeImageCanvasGroup.alpha = 1;
        fadeImageCanvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEase).OnComplete(() => fadeImageCanvasGroup.gameObject.SetActive(false));
        #endregion
    }

    public void Update()
    {
        healthBar.fillAmount = (float)player.currentHealth / player.maxHealth;
        ArmorBar.fillAmount = (float)player.currentArmor / player.maxArmor;
        DashBar.fillAmount = (float)player.dashAmount / 100;
        XPBarLeft.fillAmount = XPBarRight.fillAmount = (float)player.xpAmount / player.maxXP;
        if (player.currentWeapon == WeaponType.Melee)
        {
            weapons[1].DOScale(0f, 0.1f).onComplete += () => weapons[0].DOScale(1.0f, 0.1f);
        }
        else
        {
            weapons[0].DOScale(0f, 0.1f).onComplete += () => weapons[1].DOScale(1.0f, 0.1f);
        }
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
        if (currentMenu == GameMenuType.PauseMenu)
        {
            OnResumeClick();
        }
        else if (currentMenu == GameMenuType.PopUpMainMenu)
        {
            OnPopUpMainMenuNoClick();
        }
        else if (currentMenu == GameMenuType.PopUpQuitMenu)
        {
            OnPopUpQuitNoClick();
        }
    }
    #endregion

    #region Menu Events
    public void OnResumeClick()
    {
        ShowHidePauseMenu(false);
        Time.timeScale = 1f;
    }

    public void OnMainMenuClick()
    {
        ShowHidePauseMenu(false);
        ShowHidePopUpMainMenu(true);
    }

    public void OnQuitClick()
    {
        ShowHidePauseMenu(false);
        ShowHidePopUpQuitMenu(true);
    }

    public void OnPopUpMainMenuYesClick()
    {
        Time.timeScale = 1f;
        GameManager.Instance.LoadScene(0);
    }

    public void OnPopUpMainMenuNoClick()
    {
        ShowHidePopUpMainMenu(false);
        ShowHidePauseMenu(true);
    }

    public void OnPopUpQuitYesClick()
    {
        Application.Quit();
    }

    public void OnPopUpQuitNoClick()
    {
        ShowHidePopUpQuitMenu(false);
        ShowHidePauseMenu(true);
    }
    #endregion

    #region Menu Sequence
    void SequenceSetupPopUp(Sequence s, RectTransform r, float duration)
    {
        r.gameObject.SetActive(false);
        r.localScale = Vector3.zero;
        s.AppendCallback(() => r.gameObject.SetActive(!r.gameObject.activeSelf));
        s.Append(DOTween.To(() => r.localScale, x => r.localScale = x, Vector3.one, duration).SetEase(itemRevealEase));
    }

    void ShowHidePauseMenu(bool show)
    {
        if (show)
        {
            pauseMenuSequence.timeScale = 1f;
            pauseMenuSequence.PlayForward();
            currentItemIndex = 0;
            currentMenu = GameMenuType.PauseMenu;
        }
        else
        {
            pauseMenuSequence.timeScale *= (itemRevealDuration / itemHideDuration);
            pauseMenuSequence.PlayBackwards();
        }
    }

    void ShowHidePopUpMainMenu(bool show)
    {
        if (show)
        {
            popUpMainMenuSequence.timeScale = 1f;
            popUpMainMenuSequence.PlayForward();
            currentItemIndex = 0;
            currentMenu = GameMenuType.PopUpMainMenu;
        }
        else
        {
            popUpMainMenuSequence.timeScale *= (itemRevealDuration / itemHideDuration);
            popUpMainMenuSequence.PlayBackwards();
        }
    }

    void ShowHidePopUpQuitMenu(bool show)
    {
        if (show)
        {
            popUpQuitSequence.timeScale = 1f;
            popUpQuitSequence.PlayForward();
            currentItemIndex = 0;
            currentMenu = GameMenuType.PopUpQuitMenu;
        }
        else
        {
            popUpQuitSequence.timeScale *= (itemRevealDuration / itemHideDuration);
            popUpQuitSequence.PlayBackwards();
        }
    }

    void ShowHideUpgradeMenu(bool show)
    {
        if (show)
        {
            upgradeMenuSequence.timeScale = 1f;
            upgradeMenuSequence.PlayForward();
            currentItemIndex = 0;
            currentMenu = GameMenuType.UpgradeMenu;
        }
        else
        {
            upgradeMenuSequence.timeScale *= (itemRevealDuration / itemHideDuration);
            upgradeMenuSequence.PlayBackwards();
        }
    }

    public void Pause()
    {
        ShowHidePauseMenu(true);
    }
    #endregion

    public void PushMessage(string message)
    {
        messageText.text = message;
        messageText.DOFade(1f, 0.1f).onComplete += () => messageText.DOFade(0f, 0.1f).SetDelay(0.5f);
    }

    public void healthMaxedOut()
    {
        healthBarParent.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 10, 1f);
    }

    public void armorMaxedOut()
    {
        ArmorBarParent.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 10, 1f);
    }

    public void dashMaxedOut()
    {
        DashBarParent.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 10, 1f);
    }

    public void xpMaxedOut()
    {
        XPBarParent.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 10, 1f);
    }
}
