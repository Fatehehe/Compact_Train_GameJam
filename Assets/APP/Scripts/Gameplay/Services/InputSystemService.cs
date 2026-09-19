using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class InputSystemService : IInitializable, IDisposable
{
    private readonly PlayerInputSystem inputSystem;
    private readonly GameConfigData config;

    public event Action OnPressStarted;
    public event Action OnPressPerformed;
    public event Action OnPressCanceled;

    public event Action<Vector2> OnPressPosPerformed;
    public event Action<Vector2> OnPressPosEnded;

    [Inject]
    public InputSystemService(PlayerInputSystem inputSystem, GameConfigData config)
    {
        this.inputSystem = inputSystem;
        this.config = config;
    }

    public void Initialize()
    {
        inputSystem.Input.Player.Press.started += HandlePressStarted;
        inputSystem.Input.Player.Press.performed += HandlePressPerformed;
        inputSystem.Input.Player.Press.canceled += HandlePressCanceled;

        inputSystem.Input.Player.ScreenPos.performed += HandlePressPosPerformed;
        inputSystem.Input.Player.ScreenPos.canceled += HandlePressPosCanceled;
    }

    public void Dispose()
    {
        inputSystem.Input.Player.Press.started -= HandlePressStarted;
        inputSystem.Input.Player.Press.performed -= HandlePressPerformed;
        inputSystem.Input.Player.Press.canceled -= HandlePressCanceled;

        inputSystem.Input.Player.ScreenPos.performed -= HandlePressPosPerformed;
        inputSystem.Input.Player.ScreenPos.canceled -= HandlePressPosCanceled;
    }

    public void ChangeInputState(InputStateType state) => inputSystem.ChangeInputState(state);

    private void HandlePressStarted(InputAction.CallbackContext context) => OnPressStarted?.Invoke();

    private void HandlePressPerformed(InputAction.CallbackContext context) => OnPressPerformed?.Invoke();

    private void HandlePressCanceled(InputAction.CallbackContext context) => OnPressCanceled?.Invoke();

    private void HandlePressPosPerformed(InputAction.CallbackContext context) => OnPressPosPerformed?.Invoke(context.ReadValue<Vector2>());

    private void HandlePressPosCanceled(InputAction.CallbackContext context) => OnPressPosEnded?.Invoke(context.ReadValue<Vector2>());

}
