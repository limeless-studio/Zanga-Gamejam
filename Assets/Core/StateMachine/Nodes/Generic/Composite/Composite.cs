namespace Core.StateMachine.Generic
{
    public abstract class Composite : StateSingleOutput
    {
        public StateNode[] Children => GetChildren<StateNode>();
        
        protected State lastChildExitState;
        
        public int CurrentChildIndex { get; private set; }

        public virtual StateNode CurrentChild()
        {
            if (CurrentChildIndex < Children.Length)
            {
                return Children[CurrentChildIndex];
            }
            
            return null;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            CurrentChildIndex = 0;
        }
        
        public virtual void OnChildExit(State state)
        {
            lastChildExitState = state;
            CurrentChildIndex++;
        }
    }
}