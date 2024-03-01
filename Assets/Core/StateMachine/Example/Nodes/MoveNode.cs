using UnityEngine;

namespace Core.StateMachine.Example.Nodes
{
    
    [CreateNodeMenu("Example/Move")]
    public class MoveNode : StateSingleOutput
    {
        
        [Input] public float distance;
        
        public Vector3 startPosition;

        protected override void Init()
        {
            if (startPosition == Vector3.zero && Context != null)
            {
                startPosition = Context.transform.position;
            }
            base.Init();
        }

        public override State Execute()
        {
            // todo: implement
            
            // Move a certain distance
            if (Context == null) return State.Failure;
            float dist = GetInputValue<float>("distance", this.distance);
            if (Vector3.Distance(startPosition, Context.transform.position) < dist)
            {
                // Move
                Context.transform.position = Vector3.MoveTowards(Context.transform.position, Context.transform.position + Context.transform.forward, 1);
                return State.Running;
            }
            return State.Success;
        }
    }
}