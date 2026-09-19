using VContainer;
using VContainer.Unity;
using Modules.SoundSystems;
using UnityEngine;

public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private SoundSystem soundSystem;
    protected override void Configure(IContainerBuilder builder)
    {
        SoundSystem soundSystemInstance = Instantiate(soundSystem, transform);
        builder.RegisterComponent(soundSystemInstance).AsSelf();

    }
}
