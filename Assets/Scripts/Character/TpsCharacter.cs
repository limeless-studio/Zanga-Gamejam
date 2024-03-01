using System;

using Scriptables;
using UnityEngine;
using Utilities;

namespace Character
{
    public class TpsCharacter : MonoBehaviour
    {
        [SerializeField] SkinnedMeshRenderer meshRenderer;
        [SerializeField] [ReorderableList] private TpMaterial[] mats;
        [SerializeField, OnValueChanged(nameof(ShowChanged))] bool showTps;

        private CharacterMaterial currentMaterial;
        [SerializeField, InLineEditor]private Material invisibleMat;

        [Button("Setup Mat List", "Set up the material list to match the mesh renderer", ButtonActivityType.Everything)]
        public void SetUpList()
        {
            mats = new TpMaterial[meshRenderer.sharedMaterials.Length];
        } 
        
        
        private void Start()
        {
            currentMaterial = GlobalSettings.Instance.GetDefaultMaterial();
            invisibleMat = GlobalSettings.Instance.GetInvisibleMat();
            
            ShowChanged();
            ApplyMaterial();
        }

        private void ApplyMaterial()
        {
            Material[] materials = new Material[mats.Length];
            for (var i = 0; i < materials.Length; i++) materials[i] = mats[i].material;

            meshRenderer.sharedMaterials = materials;
        }

        public void ShowChanged()
        {
            Debug.Log("Show Changed");
            foreach (var mat in mats)
            {
                mat.material = mat.materialType == MaterialType.AlwaysVisible ? currentMaterial.bodyMaterial : showTps ? currentMaterial.bodyMaterial : invisibleMat;
            }
            ApplyMaterial();
        }
    }
}