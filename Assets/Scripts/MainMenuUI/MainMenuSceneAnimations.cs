using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuSceneAnimations : MonoSingleton<MainMenuSceneAnimations>
{
    public Transform character;
    public float characterHoverHeight = 0.5f;
    public float characterHoverDuration = 0.5f;
    public Ease characterHoverEase;

    void Start()
    {
        character.DOMoveY(characterHoverHeight, characterHoverDuration).SetEase(characterHoverEase).SetLoops(-1, LoopType.Yoyo);
    }
}
