using VContainer;
using VContainer.Unity;
using Modules.SoundSystems;
using UnityEngine;

public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private SoundSystem soundSystem;
    [SerializeField] private GameConfigData gameConfigData;
    protected override void Configure(IContainerBuilder builder)
    {
        SoundSystem soundSystemInstance = Instantiate(soundSystem, transform);
        builder.RegisterComponent(soundSystemInstance).AsSelf();

        builder.RegisterInstance(gameConfigData);

        builder.RegisterEntryPoint<ProjectAudioService>(Lifetime.Singleton).AsSelf();

        builder.RegisterEntryPoint<PlayerInputSystem>(Lifetime.Singleton).AsSelf();
        // builder.RegisterEntryPoint<InputSystemService>(Lifetime.Singleton).AsSelf();
    }
}
