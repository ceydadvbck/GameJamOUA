using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;
using UnityEngine.Events;

[Serializable]
public enum MenuType
{
    MainMenu = 0,
    SettingsMenu = 1,
    PopUpSaveMenu = 2,
    PopUpQuitMenu = 3,
    CreditsMenu = 4,
    None = 5
}

[Serializable]
public enum MenuItemType
{
    Button,
    Selector,
    Toggle,
    NonInteractable
}

[RequireComponent(typeof(RectTransform))]
public class MenuItem : MonoBehaviour
{
    public MenuItemType itemType;
    public UnityEvent[] events;
    [NonSerialized] public RectTransform rectTransform;
    [NonSerialized] public EventTrigger eventTrigger;
    [NonSerialized] public RectTransform previousButton;
    [NonSerialized] public EventTrigger previousEventTrigger;
    [NonSerialized] public TextMeshProUGUI itemText;
    [NonSerialized] public RectTransform nextButton;
    [NonSerialized] public EventTrigger nextEventTrigger;
    [NonSerialized] public RectTransform toggleButton;
    [NonSerialized] public EventTrigger toggleEventTrigger;
    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (itemType == MenuItemType.Button)
        {
            eventTrigger = rectTransform.GetComponent<EventTrigger>();
            previousButton = null;
            previousEventTrigger = null;
            itemText = null;
            nextButton = null;
            nextEventTrigger = null;
            toggleButton = null;
            toggleEventTrigger = null;
        }
        else if (itemType == MenuItemType.Selector)
        {
            eventTrigger = rectTransform.GetComponent<EventTrigger>();
            previousButton = rectTransform.Find("PreviousButton").GetComponent<RectTransform>();
            previousEventTrigger = previousButton.GetComponent<EventTrigger>();
            itemText = rectTransform.Find("ItemText").GetChild(0).GetComponent<TextMeshProUGUI>();
            nextButton = rectTransform.Find("NextButton").GetComponent<RectTransform>();
            nextEventTrigger = nextButton.GetComponent<EventTrigger>();
            toggleButton = null;
            toggleEventTrigger = null;
        }
        else if (itemType == MenuItemType.Toggle)
        {
            eventTrigger = null;
            previousButton = null;
            previousEventTrigger = null;
            itemText = null;
            nextButton = null;
            nextEventTrigger = null;
            toggleButton = rectTransform.Find("ToggleButton").GetComponent<RectTransform>();
            toggleEventTrigger = toggleButton.GetComponent<EventTrigger>();
        }
        else
        {
            eventTrigger = null;
            previousButton = null;
            previousEventTrigger = null;
            itemText = null;
            nextButton = null;
            nextEventTrigger = null;
            toggleButton = null;
            toggleEventTrigger = null;
        }
    }

    public void AssignEvents()
    {
        if (itemType == MenuItemType.NonInteractable || events == null || events.Length == 0)
            return;

        //int lastIndex = eventTrigger.triggers.Count - 1;
        if (itemType == MenuItemType.Button)
        {
            UnityEvent tempEvent = events[0];
            eventTrigger.triggers.Add(new EventTrigger.Entry() { eventID = EventTriggerType.PointerClick, callback = new EventTrigger.TriggerEvent() });
            eventTrigger.triggers[eventTrigger.triggers.Count - 1].callback.AddListener((data) => { tempEvent.Invoke(); });
        }
        else if (itemType == MenuItemType.Selector)
        {
            if (events.Length != 2)
            {
                Debug.LogError("Selector type menu items must have exactly 2 events assigned to them.");
                return;
            }
            UnityEvent[] tempEvent = events;
            previousEventTrigger.triggers.Add(new EventTrigger.Entry() { eventID = EventTriggerType.PointerClick, callback = new EventTrigger.TriggerEvent() });
            previousEventTrigger.triggers[previousEventTrigger.triggers.Count - 1].callback.AddListener((data) => { tempEvent[0].Invoke(); });

            nextEventTrigger.triggers.Add(new EventTrigger.Entry() { eventID = EventTriggerType.PointerClick, callback = new EventTrigger.TriggerEvent() });
            nextEventTrigger.triggers[nextEventTrigger.triggers.Count - 1].callback.AddListener((data) => { tempEvent[1].Invoke(); });
        }
    }
}
