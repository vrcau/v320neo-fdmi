using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;

namespace VRChatAerospaceUniversity.V320
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    [AircraftLifecycleReceiver]
    public class EnableObjectWhenPostInit : UdonSharpBehaviour {
        [SerializeField] private GameObject _targetGameObject;

        [PublicAPI]
        public void _OnPreInit() {
            _targetGameObject.SetActive(false);
        }

        [PublicAPI]
        public void _OnPostInit() {
            _targetGameObject.SetActive(true);
        }
    }
}
