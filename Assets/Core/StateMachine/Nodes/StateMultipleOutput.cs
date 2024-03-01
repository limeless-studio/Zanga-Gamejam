namespace Core.StateMachine
{
    public abstract class StateMultipleOutput : StateNode
    {
        [Input] public Empty enter;
        [Output(connectionType = ConnectionType.Multiple)] public Empty exit;
    }
}