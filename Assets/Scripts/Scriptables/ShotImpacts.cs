using System;
using System.Linq;

using UnityEngine;

namespace Scriptables
{
    [Serializable]
    public class Impact
    {
        [TagSelector] public string tag;
        [AssetPreview] public GameObject impact;
    }

    [CreateAssetMenu(fileName = "Shot Impacts", menuName = "Snowy/FPS/Shot Impacts")]
    public class ShotImpacts : ScriptableObject
    {
        [ReorderableList] public Impact[] impacts;
    }
}