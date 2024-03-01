using UnityEngine;

namespace Dungeon_Generator
{
    [CreateAssetMenu(fileName = "DungeonLayout", menuName = "Dungeon Generator/Dungeon Layout", order = 1)]
    public class DungeonLayoutScriptable : ScriptableObject
    {
        public GameObject startPrefab;
        public GameObject exitPrefab;
        public GameObject floorPrefab;
        public GameObject wallPrefab;
        public GameObject doorPrefab;
        public GameObject corridorFloorPrefab;
        public GameObject cornerPrefab;
    }
}