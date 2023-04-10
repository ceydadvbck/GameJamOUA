using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoSingleton<InputManager>
{
    private MainMenuControls input;
    private MainMenuUIController UIController;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        #region Input Setup
        UIController = MainMenuUIController.Instance;
        input = new MainMenuControls();
        input.Enable();
        input.MainMenuMap.Navigation.performed += ctx => Navigation(ctx.ReadValue<Vector2>().y, ctx.ReadValue<Vector2>().x, ctx.action.activeControl.device);
        input.MainMenuMap.Submit.performed += ctx => Submit(ctx.action.activeControl.device);
        input.MainMenuMap.Cancel.performed += ctx => Cancel(ctx.action.activeControl.device);
        #endregion
    }

    void OnDestroy()
    {
        input.Disable();
    }

    #region UI Input Events
    private void Navigation(float UpDown, float LeftRight, InputDevice device = null)
    {
        UIController.MoveSelection(new Vector2(LeftRight, UpDown));
        UIController.SetDevicePrompt(device.name.Contains("Controller"));
        audioSource.clip = UIController.itemSwitchClip;
        audioSource.Play();
    }

    private void Submit(InputDevice device = null)
    {
        UIController.Submit();
        UIController.SetDevicePrompt(device.name.Contains("Controller"));
        audioSource.clip = UIController.itemSelectClip;
        audioSource.Play();
    }

    private void Cancel(InputDevice device = null)
    {
        UIController.Cancel();
        UIController.SetDevicePrompt(device.name.Contains("Controller"));
        audioSource.clip = UIController.itemSelectClip;
        audioSource.Play();
    }
    #endregion
}