
using Core;
using FPS;
using Game;
using Systems._3D_Menu.Elements;
using TMPro;
using UnityEngine;

namespace Systems._3D_Menu
{

    enum Element
    {
        Planet,
        Weapon,
        Resource,
    }
    
    public class SelectorMenu : MonoBehaviour
    {
        [SerializeField] Element menuElements;
        [SerializeField] Vector2Int gridSize = new Vector2Int(3, 3);
        [SerializeField] float cellSize = 1f;
        [SerializeField] Vector2 padding;
        
        [SerializeField] bool scaleOnHover = true;
        [SerializeField] float hoverScale = 1.2f;
        [SerializeField] UIElement prefab;
        
        UIElement[] elements;
        private GameObject hovered;
        private Vector3 lastHoveredScale;
        private bool isHovering;

        public bool CanHover = true;
        
        Vector3[,] grid;
        
        void Start()
        {
            // Initialize grid
            InitializeGrid();
            
            switch (menuElements)
            {
                case Element.Planet:
                    var planets = GameManager.Instance.GetSimulationWorlds();
                    elements = new UIElement[planets.Length];
                    
                    // spawn elements from left to right, top to bottom
                    
                    for (int i = 0; i < planets.Length; i++)
                    {
                        var element = Instantiate(prefab, transform);
                        
                        var pos = grid[i % gridSize.x, i / gridSize.x];
                        element.transform.localPosition = pos;
                        
                        element.SetUIElement(planets[i]);
                        elements[i] = element;
                        element.gameObject.name = planets[i].worldName;
                    }

                    break;
                case Element.Weapon:
                    break;
                case Element.Resource:
                    break;
            }

            InputManager.Instance.OnAttack += SelectMap;
        }
        
        void SelectMap(ButtonState state)
        {
            if (state != ButtonState.Pressed) return;
            
            if (GameManager.Instance == null || GameManager.Instance.GetPlayer() == null) return;
            if (GameManager.Instance.GetPlayer().FixedCamera == null) return;
            if (!GameManager.Instance.GetPlayer().FixedCamera.IsFixed()) return;
            
            if (hovered == null) return;
            if (hovered.TryGetComponent(out UIElement element))
            {
                element.OnClick();
            }
        }

        private void InitializeGrid()
        {
            grid = new Vector3[gridSize.x, gridSize.y];
            int x = - Mathf.FloorToInt(gridSize.x / 2f);
            int y = - Mathf.FloorToInt(gridSize.y / 2f);
            
            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    // Set the grid position
                    grid[i, j] = new Vector3(x, 0, y) * cellSize * padding;
                    
                    y++;
                }
                x++;
                y = - Mathf.FloorToInt(gridSize.y / 2f);
            }
        }

        void LateUpdate()
        {
            //if (GameManager.Instance != null && GameManager.Instance.GetPlayer() != null)
            //    transform.LookAt(GameManager.Instance.GetPlayer().FPSCamera.GetCameraParent());
        }

        // Check if the mouse is over a object;
        private void Update()
        {
            if (GameManager.Instance == null || GameManager.Instance.GetPlayer() == null) return;
            if (GameManager.Instance.GetPlayer().FixedCamera == null) return;
            if (!GameManager.Instance.GetPlayer().FixedCamera.IsFixed() || !CanHover)
            {
                if (hovered != null && scaleOnHover)
                {
                    hovered.transform.localScale = lastHoveredScale;
                    hovered = null;
                }
                
                return;
            }
            
            FixedCamera cam = GameManager.Instance.GetPlayer().FixedCamera;
            RaycastHit hit;
            
            
            Ray ray = cam.GetFixedCamera().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f, ~0, QueryTriggerInteraction.Collide))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("3DUI"))
                {
                    if (hovered == hit.transform.gameObject) return;

                    UIElement element = hit.transform.GetComponentInParent<UIElement>();
                    if (element != null)
                    {
                        element.OnHover();

                        if (scaleOnHover)
                        {
                            if (hovered != null)
                            {
                                hovered.transform.localScale = lastHoveredScale;
                            }

                            hovered = hit.transform.gameObject;
                            lastHoveredScale = hovered.transform.localScale;
                            hovered.transform.localScale = lastHoveredScale * hoverScale;
                            isHovering = true;
                        }

                        return;
                    }
                }

                if (isHovering)
                {
                    if (hovered != null && scaleOnHover)
                    {
                        hovered.transform.localScale = lastHoveredScale;
                    }

                    isHovering = false;
                    hovered = null;
                }
            } 
        }

        public void StaticCameraGoBack()
        {
            GameManager.Instance?.GetPlayer()?.FixedCamera?.GoBackToMainCamera();
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (grid != null)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        Gizmos.DrawWireCube(transform.position + grid[i, j], Vector3.one * cellSize);
                    }
                }
            }else
            {
                InitializeGrid();
            }
        }
        
        private void OnValidate()
        {
            InitializeGrid();
        }
#endif
        
    }
}