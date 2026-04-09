using Cysharp.Threading.Tasks;
using Project.Scripts.Gameplay.Data;
using Project.Scripts.Infrastructure.Services.SceneLoader;
using Zenject;

namespace Project.Scripts.Infrastructure
{
    public class GameBootstrap
    {
        private const string FIRST_SCENE = Scenes.GAMEPLAY;
        
        private readonly ISceneLoader _sceneLoader;
        
        [Inject]
        public GameBootstrap(ISceneLoader sceneLoader) => 
            _sceneLoader = sceneLoader;
        
        public void ConfigureAndStartGame() =>
            _sceneLoader.Load(FIRST_SCENE).Forget();
    }
}