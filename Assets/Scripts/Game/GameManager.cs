using System;
using System.Collections;
using Dungeon_Generator;
using FPS;
using JetBrains.Annotations;
using Scriptable;
using UnityEngine;

namespace Game
{
    
    [Serializable] public class SimulationWorld
    {
        public string worldName;
        public string description;
        public DungeonLayoutScriptable dungeonLayout;
        public GameObject visual;
    }
    
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        [SerializeField] ShaderController shaderController;
        [SerializeField] DungeonGenerator genPrefab;
        [SerializeField] SimulationWorld[] simulationWorlds;
        [SerializeField] private float transitionTime = 1f;
        
        private DungeonGenerator _dungeonGenerator;
        private SimulationWorld _currentWorld;
        private FPSCharacter _player;
        [CanBeNull] public event Action<float> SceneLoadProgress;
        
        
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (!shaderController) shaderController = GetComponent<ShaderController>();
        }
        
        void Start()
        {
            if (_player) _player = FindFirstObjectByType<FPSCharacter>();
            if (_player) DontDestroyOnLoad(_player.gameObject);
        }   
        
        public SimulationWorld[] GetSimulationWorlds()
        {
            return simulationWorlds;
        }
        
        public void SetPlayer(FPSCharacter player)
        {
            _player = player;
            DontDestroyOnLoad(_player.gameObject);
        }
        
        public FPSCharacter GetPlayer() => _player;

        public void SetCurrentWorld(SimulationWorld world)
        {
            _currentWorld = world;
        }

        public void EnterSimulationFrom(Transform target)
        {
            if (!transform) return;
            
            shaderController.SetInvinsibleRadius(0);
            shaderController.GetVisualSphere().position = target.position;
            
            StartCoroutine(SimulationLoad());
        }

        IEnumerator SimulationLoad()
        {
            if (!_player) yield break;
            _player.FixedCamera.GoBackToMainCamera();
            _player.SetImmobilized(true);
            _player.SetCameraLocked(true);
            _player.SetGravityMultiplier(0);
            
            // Scale.
            float t = 0;
            float targetR = 40f;
            while (t < transitionTime)
            {
                t += Time.deltaTime;
                shaderController.SetInvinsibleRadius(Mathf.Lerp(0, targetR, t / transitionTime));
                yield return null;
            }
            
            
            // Load scene
            SceneLoadProgress?.Invoke(0);
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;
            while (!asyncLoad.isDone)
            {
                SceneLoadProgress?.Invoke(asyncLoad.progress);
                if (asyncLoad.progress >= 0.9f)
                {
                    SceneLoadProgress?.Invoke(1);
                    asyncLoad.allowSceneActivation = true;
                    
                    break;
                }
                yield return null;
            }
            
            _player.SetCameraLocked(false);
            _player.SetImmobilized(true);
            
            
            // Unload.
            // Change the main scene to the simulation scene
            yield return new WaitForSeconds(0.01f);

            UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(1));
            
            yield return new WaitForSeconds(0.01f);
            
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(0);
            
            Debug.Log("Simulation loaded"); 
            _player.SetImmobilized(true);
            
            // Load In.
            shaderController.SetInvinsibleRadius(300f);

            DungeonGenerator gen = Instantiate(genPrefab);
            gen.transform.position = _player.transform.position;
            gen.Generate(_currentWorld.dungeonLayout);
            shaderController.GetVisualSphere().position = gen.GetStartRoomPosition();
            _player.transform.position = gen.GetStartRoomPosition();
            
            t = 0;
            while (t < transitionTime)
            {
                t += Time.deltaTime;
                shaderController.SetInvinsibleRadius(Mathf.Lerp(300, 0, t / transitionTime));
                yield return null;
            }
            
            _player.SetImmobilized(false);
            _player.ResetGravityMultiplier();
        }
    }
}