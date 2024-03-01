namespace Core.StateMachine.Generic
{
    public abstract class Decorator : StateSingleOutput
    {
        public StateNode Child => GetChild<StateNode>();
    }
}