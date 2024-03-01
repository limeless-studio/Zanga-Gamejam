using System;
using System.Linq;
using Core.StateMachine.Generic;
using UnityEngine;
using XNode;

namespace Core.StateMachine
{

    public enum State
    {
        Running,
        Success,
        Failure,
    }
    
    public abstract class StateNode : Node, IContextual {
        public AI Context { get; set; }
        
        public const int kInvalidOrder = -1;
        
        public int preOrderIndex => graph.nodes.IndexOf(this);
        
        public StateGraph stateGraph => graph as StateGraph;
        
        [SerializeField] public State state;

        public StateNode Parent
        {
            get
            {
                if (GetInputPort("enter").Connection != null)
                {
                    return GetInputPort("enter").Connection.node as StateNode;
                }

                return null;
            }
        }
        
        public virtual void MoveNext() {
            StateGraph fmGraph = graph as StateGraph;

            if (fmGraph.current != this) {
                Debug.LogWarning("Node isn't active");
                return;
            }
            
            state = Execute();
            if (state == State.Running) return;

            if (state == State.Failure)
            {
                if (Parent != null)
                {
                    fmGraph.SetCurrent(Parent);
                }
                else
                {
                    Debug.LogWarning("No parent found");
                }
                
                OnExit();
                return;
            }

            NodePort exitPort = GetOutputPort("exit");

            if (!exitPort.IsConnected) {
                Debug.LogWarning("Node isn't connected");
                return;
            }

            OnExit();
        }

        public virtual void OnEnter() {
            StateGraph fmGraph = graph as StateGraph;
            fmGraph.current = this;
        }
        
        public abstract State Execute();

        [Serializable]
        public class Empty { }
        
        // Get Child nodes
        public T[] GetChildren<T>() where T : StateNode
        {
            NodePort port = GetOutputPort("exit");
            if (!port.IsConnected) return null;

            T[] nodes = port.GetConnections().ToArray().Select(connection => connection.node as T).ToArray();
            
            return nodes;
        }

        // Handle OnExit event
        public virtual void OnExit()
        {
            if (Parent != null && Parent is Composite)
            {
                (Parent as Composite)?.OnChildExit(state);
            }
        }
        
        /// <summary>
        /// Called when a child fires an abort.
        /// </summary>
        /// <param name="aborter"></param>
        public virtual void OnAbort(int childIndex) { }

        /// <summary>
        /// Called when the iterator traverses the child.
        /// </summary>
        /// <param name="childIndex"></param>
        public virtual void OnChildEnter(int childIndex) { }

        /// <summary>
        /// Called when the iterator exits the the child.
        /// </summary>
        /// <param name="childIndex"></param>
        /// <param name="childStatus"></param>
        public virtual void OnChildExit(int childIndex, State childStatus) { }

        /// <summary>
        /// Called foreach child of the composite node when it exits.
        /// </summary>
        public virtual void OnCompositeParentExit() { }
        
        public virtual StateNode GetChildAt(int index)
        {
            return GetChildren<StateNode>()[index];
        }

        public bool IsComposite()
        {
            return this is Composite;
        }
        
        public bool IsDecorator()
        {
            return this is Decorator;
        }

        public int ChildCount() => GetChildren<StateNode>().Length;
    }
}