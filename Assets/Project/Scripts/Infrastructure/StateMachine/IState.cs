namespace Project.Scripts.Infrastructure.StateMachine
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Tick(float deltaTime);
    }
}