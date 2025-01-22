using Zenject;

public class Dependencier : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<InputHandler>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Map>().FromComponentInHierarchy().AsSingle();
        Container.Bind<DotsMover>().FromComponentInHierarchy().AsSingle();
        Container.Bind<DotsFinder>().FromComponentInHierarchy().AsSingle();
        Container.Bind<DotsShiner>().FromComponentInHierarchy().AsSingle();
        Container.Bind<RoadBuilder>().FromComponentInHierarchy().AsSingle();
        Container.Bind<RoadChanger>().FromComponentInHierarchy().AsSingle();
        Container.Bind<CameraConverter>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AudioSourcePool>().FromComponentInHierarchy().AsSingle();
    }
}
