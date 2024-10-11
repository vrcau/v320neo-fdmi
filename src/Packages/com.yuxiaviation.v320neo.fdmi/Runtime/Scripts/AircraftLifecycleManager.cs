using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDKBase;

#if UNITY_EDITOR
using UdonSharpEditor;
using UnityEditor;
#endif

namespace VRChatAerospaceUniversity.V320 {
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class AircraftLifecycleManager : UdonSharpBehaviour
    #if !COMPILER_UDONSHARP && UNITY_EDITOR
        , IPreprocessCallbackBehaviour
#endif
    {
        [SerializeField] private UdonSharpBehaviour[] _receivers;

        private bool _isInitInProcessOrCompleted;
        private bool _isFirstInit = true;

        private void Start() => _StartInitProcess();

        [PublicAPI]
        public void _StartInitProcess() {
            if (_isInitInProcessOrCompleted) {
                Debug.Log("Init process already started");
                return;
            }

            SendCustomEventDelayedFrames(nameof(_ContinueInitProcess), 1);
        }

        public void _ContinueInitProcess() {
            Debug.Log("Starting init process");
            if (_isFirstInit)
                SendEventToReceivers("_OnFirstPreInit");

            SendEventToReceivers("_OnPreInit");
            Debug.Log("PreInit done");

            if (_isFirstInit)
                SendEventToReceivers("_OnFirstInit");
            SendEventToReceivers("_OnInit");
            Debug.Log("Init done");

            if (_isFirstInit)
                SendEventToReceivers("_OnFirstPostInit");
            SendEventToReceivers("_OnPostInit");
            Debug.Log("PostInit done");

            _isFirstInit = false;
        }

        [PublicAPI]
        public void _StartReInit() {
            _isInitInProcessOrCompleted = false;

            _StartInitProcess();
        }

        private void SendEventToReceivers(string eventName) {
            foreach (var receiver in _receivers) {
                receiver.SendCustomEvent(eventName);
            }
        }

    #if !COMPILER_UDONSHARP && UNITY_EDITOR
        [InitializeOnEnterPlayMode]
        public static void OnEnterPlayMode() {
            var aircraftLifecycleManagers = SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(gameObject =>
                    gameObject.GetComponentsInChildren<AircraftLifecycleManager>(true))
                .ToArray();

            foreach (var manager in aircraftLifecycleManagers) {
                manager.Init();
            }
        }

        private void Init() {
            var receiversTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type =>
                    type.GetCustomAttribute<AircraftLifecycleReceiverAttribute>() != null)
                .ToArray();

            Debug.Log($"Found {receiversTypes.Length} receiver types");

            var receivers = new List<UdonSharpBehaviour>();
            foreach (var type in receiversTypes) {
                var children = GetComponentsInChildren(type, true);

                receivers.AddRange(children.Select(component => component as UdonSharpBehaviour));
            }

            _receivers = receivers.ToArray();

            EditorUtility.SetDirty(this);
        }

        public bool OnPreprocess() {
            Init();

            return true;
        }

        public int PreprocessOrder => 0;

        [CustomEditor(typeof(AircraftLifecycleManager))]
        public class AircraftLifecycleManagerEditor : Editor {
            public override void OnInspectorGUI() {
                if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;

                base.OnInspectorGUI();

                if (GUILayout.Button("Setup")) {
                    var manager = target as AircraftLifecycleManager;

                    if (!manager)
                        return;

                    manager.Init();
                }
            }
        }
    #endif
    }
}
