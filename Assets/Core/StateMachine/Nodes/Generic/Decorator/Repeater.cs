namespace Core.StateMachine.Generic
{
    [CreateNodeMenu("Generic/Repeater")]
    public class Repeater : Decorator
    {
        public override State Execute()
        {
            if (Child == null) return State.Failure;
            
            stateGraph.Traverse(Child);
            
            return State.Running;
        }
    }
}