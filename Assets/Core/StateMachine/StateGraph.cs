using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace Core.StateMachine
{
    public abstract class StateGraph : NodeGraph
    {
        public StateNode start;
        public StateNode previous;
        public StateNode current;

        public void Continue()
        {
            if (current == null) return;
            current.OnEnter();
            current.MoveNext();
        }
        
        public void SetCurrent(StateNode node)
        {
            previous = current;
            current = node;
        }

        public IEnumerable<U> GetNodes<U>()
        {
            return nodes.Where(node => node is U).Cast<U>();
        }

        public void ClearCache()
        {
            foreach (var node in nodes)
            {
                foreach (ICacheable cacheable in GetNodes<ICacheable>()) {
                    cacheable.ClearCache();
                }
            }
        }

        public void Init(AI ai)
        {
            if (start == null)
            {
                start = GetNodes<StateNode>().FirstOrDefault();
                if (start == null)
                {
                    Debug.LogError("Start node is not set");
                    return;
                }
            }
            
            current = start;
            
            foreach (IContextual context in GetNodes<IContextual>())
            {
                Debug.Log("Init");
                context.Context = ai;
            }
        }
        
        public void Traverse(StateNode node)
        {
            if (node == null) return;
            node.OnEnter();
            node.MoveNext();
        }
    }

    
    // T is an inherited class of AI
    public abstract class StateGraph<T> : StateGraph where T : AI 
    {
        public override Node AddNode(Type type) {
            Node node = base.AddNode(type);
            if (node is DataReaderNode dataReaderNode) {
                dataReaderNode.UpdateData<T>();
            }
            return node;
        }

        private void OnValidate() {
            foreach (DataReaderNode dataReaderNode in GetNodes<DataReaderNode>()) {
                dataReaderNode.UpdateData<T>();
            }
        }
        
        public T Context { get; private set; }
    }
}