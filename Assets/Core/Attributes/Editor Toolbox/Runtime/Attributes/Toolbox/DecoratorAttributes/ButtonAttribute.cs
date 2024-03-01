using System;
using System.Diagnostics;

namespace UnityEngine
{
    /// <summary>
    /// Creates Button control and allows to invoke particular methods within the target class.
    /// This extension supports static methods, standard methods, and <see cref="Coroutine"/>s.
    /// Requirements: method has to be parameterless. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class ButtonAttribute : ToolboxDecoratorAttribute
    {
        public ButtonAttribute(string extraLabel = null, string tooltip = null, ButtonActivityType activityType = ButtonActivityType.Everything)
        {
            ExtraLabel = extraLabel;
            Tooltip = tooltip;
            ActivityType = activityType;
        }
        
        public string ExtraLabel { get; private set; }
        public string Tooltip { get; set; }
        public ButtonActivityType ActivityType { get; private set; }
    }
}