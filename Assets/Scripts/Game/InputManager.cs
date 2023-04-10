using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoSingleton<InputManager>
{
    //Yeni InputSytem'ı kullanıyoruz. Bir Input Action oluşturdum zaten. Burada tanımlamaları ve fonskiyon atamalarını yapıyoruz. Bu da singleton yapıda o yüzden sahnede bir tane olacak.
    private GameActions input;
    private PlayerController playerController;
    [NonSerialized] public Vector2 movement;
    [NonSerialized] public Vector2 aim;
    [NonSerialized] public GameUIController UIController;

    void Start()
    {
        #region Input Setup
        playerController = PlayerController.Instance;
        UIController = GameUIController.Instance;
        input = new GameActions();
        input.Enable();
        input.Character.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();
        input.Character.Movement.canceled += ctx => movement = Vector2.zero;
        input.Character.Aim.performed += ctx => aim = ctx.ReadValue<Vector2>();
        input.Character.Aim.canceled += ctx => aim = Vector2.zero;
        input.Character.Dash.performed += ctx => playerController.Dash(movement);
        input.Character.Special.performed += ctx => playerController.SpecialAttack();
        input.Character.Pause.performed += ctx => GameManager.Instance.Pause();
        input.MenuMap.Navigation.performed += ctx => Navigation(ctx.ReadValue<Vector2>().y, ctx.ReadValue<Vector2>().x, ctx.action.activeControl.device);
        input.MenuMap.Submit.performed += ctx => Submit(ctx.action.activeControl.device);
        input.MenuMap.Cancel.performed += ctx => Cancel(ctx.action.activeControl.device);
        #endregion
    }

    #region UI Input Events
    private void Navigation(float UpDown, float LeftRight, InputDevice device = null)
    {
        Debug.Log("Up Down: " + UpDown + " Left Right: " + LeftRight);
        if (!GameManager.Instance.isPaused)
            return;
        UIController.MoveSelection(new Vector2(LeftRight, UpDown));
        //UIController.SetDevicePrompt(device.name.Contains("Controller"));
    }

    private void Submit(InputDevice device = null)
    {
        if (!GameManager.Instance.isPaused)
            return;
        UIController.Submit();
        //UIController.SetDevicePrompt(device.name.Contains("Controller"));
    }

    private void Cancel(InputDevice device = null)
    {
        if (!GameManager.Instance.isPaused)
            return;
        UIController.Cancel();
        //UIController.SetDevicePrompt(device.name.Contains("Controller"));
    }
    #endregion
}