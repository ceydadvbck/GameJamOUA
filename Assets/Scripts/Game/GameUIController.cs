using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameUIController : MonoSingleton<GameUIController>
{
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
