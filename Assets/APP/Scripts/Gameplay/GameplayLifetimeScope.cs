using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameplayLifetimeScope : LifetimeScope
{
    [SerializeField] private CharacterStateMachine characterStateMachine;
    [SerializeField] private PlayerAnimationFunctions playerAnimationFunctions;

    protected override void Configure(IContainerBuilder builder)
    {

        builder.RegisterComponent(characterStateMachine);
        builder.RegisterComponent(playerAnimationFunctions);

        builder.RegisterEntryPoint<PlayerEvents>(Lifetime.Scoped).AsSelf();

        builder.RegisterEntryPoint<InputSystemService>(Lifetime.Scoped).AsSelf();
        builder.RegisterEntryPoint<CharacterInteractionService>(Lifetime.Scoped).AsSelf();
        builder.RegisterEntryPoint<PlayerControlManager>(Lifetime.Scoped).AsSelf();

        builder.RegisterEntryPoint<PlayerAnimationManager>(Lifetime.Scoped).AsSelf();
    }
}