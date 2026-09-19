using VContainer;
using VContainer.Unity;

public class GameplayLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<InputSystemService>(Lifetime.Scoped).AsSelf();
        builder.RegisterEntryPoint<PlayerInteractionService>(Lifetime.Scoped).AsSelf();
        builder.RegisterEntryPoint<InputSystemManager>(Lifetime.Scoped).AsSelf();
    }
}
