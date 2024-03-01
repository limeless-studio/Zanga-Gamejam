namespace Core.StateMachine.Generic
{
    [CreateNodeMenu("Generic/Utils/Failure")]
    public class Failure : Decorator
    {
        public override State Execute()
        {
            return State.Failure;
        }
    }
}