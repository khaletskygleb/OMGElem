using ElementGame.Core;
using ElementGame.Data;
using ElementGame.Pool;
using ElementGame.Save;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private LevelDatabase _levelDatabase;
    [SerializeField] private BalloonLibrarySO _balloonLibrary;
    [SerializeField] private BoardGenerator _boardGenerator;

    public override void InstallBindings()
    {
        Container.Bind<LevelDatabase>().FromInstance(_levelDatabase).AsSingle(); 
        Container.Bind<BalloonLibrarySO>().FromInstance(_balloonLibrary).AsSingle();

        Container.Bind<BoardGenerator>().FromInstance(_boardGenerator).AsSingle();

        Container.Bind<SaveSystem>().AsSingle();
        Container.Bind<DestroySystem>().AsSingle();
        Container.Bind<AnimationTracker>().AsSingle();
        Container.Bind<GravityAnimator>().AsSingle();

        Container.Bind<ObjectPoolService>().AsSingle().NonLazy();
        Container.Bind<GameController>().FromComponentInHierarchy().AsSingle();
        
    }
}

