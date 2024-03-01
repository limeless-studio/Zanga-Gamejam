using System;
using _3D_Menu;
using Game;
using Scriptable;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems._3D_Menu.Elements
{
    public class PlanetUI : UIElement
    {
        [SerializeField] TMP_Text planetName;
        WorldSwitchUI worldSwitchUI;
        SimulationWorld world;

        protected override void Start()
        {
            worldSwitchUI = GetComponentInParent<WorldSwitchUI>();
            base.Start();
        }

        public override void SetUIElement(object obj)
        {
            // Check if obj is a planet
            if (!(obj is SimulationWorld)) return;
            
            SimulationWorld planet = (SimulationWorld) obj;

            // Set the planet's visual
            var visual = Instantiate(planet.visual, transform);
            visual.transform.localPosition = Vector3.zero;
            visual.transform.localEulerAngles = Vector3.zero;
            
            // Set the planet's name
            world = planet;
            gameObject.name = planet.worldName;
            planetName.text = planet.worldName;
            base.SetUIElement(obj);
        }

        public override void OnHover()
        {
            base.OnHover();
        }

        public override void OnClick()
        {
            if (worldSwitchUI == null) return;
            if (GameManager.Instance) GameManager.Instance.SetCurrentWorld(world);
            worldSwitchUI.ShowConfirmationPanel(world.worldName, world.description);
            base.OnClick();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}