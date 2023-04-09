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

    void Start()
    {
        #region Input Setup
        playerController = PlayerController.Instance;
        input = new GameActions();
        input.Enable();
        input.Character.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();
        input.Character.Movement.canceled += ctx => movement = Vector2.zero;
        input.Character.Aim.performed += ctx => aim = ctx.ReadValue<Vector2>();
        input.Character.Aim.canceled += ctx => aim = Vector2.zero;
        input.Character.Dash.performed += ctx => playerController.Dash(movement);
        #endregion
    }
}