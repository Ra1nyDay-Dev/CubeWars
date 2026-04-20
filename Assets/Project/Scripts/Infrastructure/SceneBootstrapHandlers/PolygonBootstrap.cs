using Project.Scripts.Gameplay.Services.Factories.LevelFactory;
using Zenject;

namespace Project.Scripts.Infrastructure.SceneBootstrapHandlers
{
    public class PolygonBootstrap
    {
        private readonly ILevelFactory _levelFactory;

        [Inject]
        public PolygonBootstrap(ILevelFactory levelFactory) => 
            _levelFactory = levelFactory;

        public void PrepareGame() => 
            _levelFactory.Create(playersCount: 7);
    }
}