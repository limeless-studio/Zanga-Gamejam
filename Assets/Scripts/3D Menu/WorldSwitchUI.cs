using System;
using Game;
using Systems._3D_Menu;
using TMPro;
using UnityEngine;

namespace _3D_Menu
{
    public class WorldSwitchUI : MonoBehaviour
    {
        [SerializeField] GameObject confirmationPanel;
        [SerializeField] TMP_Text planetName;
        [SerializeField] TMP_Text planetDescription;

        private SelectorMenu m_selectorMenu;

        private void Start()
        {
            m_selectorMenu = GetComponentInChildren<SelectorMenu>();
        }

        public void ShowConfirmationPanel(string name, string description)
        {
            planetName.text =  "Simulation: " + name;
            planetDescription.text = description;
            confirmationPanel.SetActive(true);
            m_selectorMenu.CanHover = false;
        }
        
        public void HideConfirmationPanel()
        {
            confirmationPanel.SetActive(false);
            m_selectorMenu.CanHover = true;
        }

        public void EnterSimulation()
        {
            HideConfirmationPanel();
            
            // Load the simulation
            if (GameManager.Instance) GameManager.Instance.EnterSimulationFrom(transform);
        }
    }
}