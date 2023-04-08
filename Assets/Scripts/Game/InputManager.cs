using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoSingleton<InputManager>
{
    //Yeni InputSytem'ı kullanıyoruz. Bir Input Action oluşturdum zaten. Burada tanımlamaları ve fonskiyon atamalarını yapıyoruz. Bu da singleton yapıda o yüzden sahnede bir tane olacak.
    private GameActions input;
    private PlayerController playerController;

    void Start()
    {
        #region Input Setup
        playerController = PlayerController.Instance;
        input = new GameActions();
        input.Enable();
        input.Character.Movement.performed += ctx => playerController.Move(ctx.ReadValue<Vector2>());
        input.Character.Movement.canceled += ctx => playerController.Move(Vector2.zero);
        #endregion
    }
}