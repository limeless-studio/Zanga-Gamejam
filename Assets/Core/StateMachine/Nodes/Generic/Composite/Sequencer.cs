namespace Core.StateMachine.Generic
{
    [CreateNodeMenu("Generic/Sequencer")]
    public class Sequencer : Composite
    {
        public override State Execute()
        {
            if (Children == null || Children.Length == 0) return State.Failure;

            if (lastChildExitState == State.Failure) return State.Failure;
            
            var nextChild = CurrentChild();
            
            if (nextChild == null) return State.Success;
            
            stateGraph.Traverse(nextChild);

            return State.Running;
        }
    }
}