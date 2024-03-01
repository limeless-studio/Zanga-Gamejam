namespace Core.StateMachine
{
    public abstract class StateSingleOutput : StateNode
    {
        [Input] public Empty enter;
        [Output] public Empty exit;
        
        public T GetChild<T>() where T : StateNode
        {
            return GetChildren<T>()[0];
        }
    }
}