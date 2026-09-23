using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class UIMainController : BaseMenuController
{
    [SerializeField] private Button buttonStart;

    public event Action OnGameStart;


    protected override void Awake()
    {
        base.Awake();
        if (buttonStart != null)
        {
            buttonStart.onClick.AddListener(OnStartGame);
        }
    }

    private void OnStartGame()
    {
        OnGameStart.Invoke();
    }
}
