using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameplayLifetimeScope : LifetimeScope
{
    [SerializeField] private CharacterStateMachine characterStateMachine;
    [SerializeField] private PlayerAnimationFunctions playerAnimationFunctions;
    [SerializeField] private PlayerDetector playerDetector;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameplayUIManager gameplayUIManager;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(gameplayUIManager);

        builder.RegisterComponent(characterStateMachine);
        builder.RegisterComponent(playerAnimationFunctions);
        builder.RegisterComponent(playerDetector);
        builder.RegisterComponent(enemySpawner);

        builder.RegisterEntryPoint<PlayerEvents>(Lifetime.Scoped).AsSelf();

        builder.RegisterEntryPoint<InputSystemService>(Lifetime.Scoped).AsSelf();
        builder.RegisterEntryPoint<CharacterInteractionService>(Lifetime.Scoped).AsSelf();
        builder.RegisterEntryPoint<PlayerControlService>(Lifetime.Scoped).AsSelf();

        builder.RegisterEntryPoint<PlayerAnimationService>(Lifetime.Scoped).AsSelf();
        builder.RegisterEntryPoint<PlayerInteractionService>(Lifetime.Scoped).AsSelf();

        builder.RegisterEntryPoint<EnemyInteractionService>(Lifetime.Scoped).AsSelf();
        builder.RegisterEntryPoint<EnemyManager>(Lifetime.Scoped).AsSelf();
        builder.RegisterEntryPoint<PlayerManager>(Lifetime.Scoped).AsSelf();

        builder.RegisterEntryPoint<GameplayManager>(Lifetime.Scoped).AsSelf();
    }
}