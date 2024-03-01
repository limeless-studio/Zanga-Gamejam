namespace Core.StateMachine.Generic
{
    [CreateNodeMenu("Generic/Utils/Success")]
    public class Success : Decorator
    {
        public override State Execute()
        {
            return State.Success;
        }
    }
}