using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class UIEndgameController : BaseMenuController
{
    [SerializeField] private Button buttonRestart;
    [SerializeField] private Button buttonNext;
    [SerializeField] private Button buttonHome;
    public event Action OnRestartButtonPressed;
    public event Action OnNextButtonPressed;
    public event Action OnHomeButtonPressed;

    protected override void Awake()
    {
        base.Awake();
        if (buttonRestart != null)
        {
            buttonRestart.onClick.AddListener(RestartButtonPressed);
        }
        if (buttonNext != null)
        {
            buttonNext.onClick.AddListener(NextButtonPressed);
        }
        if (buttonHome != null)
        {
            buttonHome.onClick.AddListener(HomeButtonPressed);
        }
    }

    private void NextButtonPressed()
    {
        OnNextButtonPressed.Invoke();
    }

    private void RestartButtonPressed()
    {
        OnRestartButtonPressed.Invoke();
    }

    private void HomeButtonPressed()
    {
        OnHomeButtonPressed.Invoke();
    }

}
