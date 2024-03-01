namespace UnityEngine
{
    public class RequiredAttribute : ToolboxSelfPropertyAttribute
    {
        public RequiredAttribute(string message = "")
        {
            Message = message;
        }
        
        public string Message { get; private set; }
    }
}