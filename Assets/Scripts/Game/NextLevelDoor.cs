using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelDoor : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(GameManager.Instance.isWin == false)
            {
                GameUIController.Instance.PushMessage("Defeat enemies to proceed to the next level!");
                return;
            }
            GameUIController.Instance.NextLevelSwitch();
        }
    }
}
