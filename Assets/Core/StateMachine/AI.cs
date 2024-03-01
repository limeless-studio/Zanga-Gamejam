using System;
using UnityEngine;

namespace Core.StateMachine
{
    public class AI : MonoBehaviour
    {
        [SerializeField] private StateGraph stateGraph;
        [SerializeField] private StateGraph runtimeStateGraph;

        private void Start()
        {
            // Clone the state graph to avoid modifying the original asset
            runtimeStateGraph = stateGraph.Copy() as StateGraph;
            if (runtimeStateGraph == null) return;
            runtimeStateGraph.Init(this);
        }
        
        private void Update()
        {
            runtimeStateGraph.Continue();
        }
    }
}