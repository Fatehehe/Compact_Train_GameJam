using VContainer;
using VContainer.Unity;
using Modules.SoundSystems;
using UnityEngine;
using System.Collections.Generic;

public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private SoundSystem soundSystem;
    [SerializeField] private GameConfigData gameConfigData;
    [SerializeField] LevelDatabase levelDatabase;
    protected override void Configure(IContainerBuilder builder)
    {
        SoundSystem soundSystemInstance = Instantiate(soundSystem, transform);
        builder.RegisterComponent(soundSystemInstance).AsSelf();

        builder.RegisterInstance(gameConfigData);
        builder.RegisterInstance(levelDatabase);

        builder.RegisterEntryPoint<ProjectAudioService>(Lifetime.Singleton).AsSelf();

        builder.RegisterEntryPoint<PlayerInputSystem>(Lifetime.Singleton).AsSelf();
        // builder.RegisterEntryPoint<InputSystemService>(Lifetime.Singleton).AsSelf();
    }
}
