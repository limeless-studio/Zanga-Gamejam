using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "Planet", menuName = "Snowy/Planet", order = 0)]
    public class Planet : ScriptableObject
    {
        public string planetName;
        public string description;
        public string sceneName;
        public float gravity;
        public Mesh hologramMesh;
        public GameObject prefab;
    }
}