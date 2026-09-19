using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameplayLifetimeScope : LifetimeScope
{
    [SerializeField] private CharacterStateMachine characterStateMachine;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<InputSystemService>(Lifetime.Scoped).AsSelf();
        builder.RegisterEntryPoint<CharacterInteractionService>(Lifetime.Scoped).AsSelf();
        builder.RegisterEntryPoint<PlayerControlManager>(Lifetime.Scoped).AsSelf();

        if (characterStateMachine != null)
        {
            builder.RegisterComponent(characterStateMachine);
        }
    }
}