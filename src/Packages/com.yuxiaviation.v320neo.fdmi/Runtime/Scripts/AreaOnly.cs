using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRChatAerospaceUniversity.V320
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class AreaOnly : UdonSharpBehaviour
    {
        [SerializeField] private GameObject _area;

        public override void OnPlayerTriggerEnter(VRCPlayerApi player) {
            _area.SetActive(true);
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player) {
            _area.SetActive(false);
        }
    }
}
