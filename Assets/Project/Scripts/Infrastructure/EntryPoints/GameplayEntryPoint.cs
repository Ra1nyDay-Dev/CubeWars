using Project.Scripts.UI;

namespace Project.Scripts.Infrastructure.EntryPoints
{
    public class GameplayEntryPoint : SceneEntryPoint
    {
        public override void Run(GameUI gameUI)
        {
            base.Run(gameUI);
            GetProjectDependencies();
            RegisterSceneServices();
        }

        private void GetProjectDependencies()
        {

        }

        private void RegisterSceneServices()
        {
            
        }
    }
}
