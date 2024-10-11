using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;

namespace VRChatAerospaceUniversity.V320
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DisableWhenInStation : UdonSharpBehaviour {
        [SerializeField] private FDMiStation[] _stations;

        [SerializeField] private GameObject _disableInStation;

        private void Update() {
            foreach (var station in _stations) {
                if (station.seatedPlayer != null) {
                    _disableInStation.SetActive(false);
                    return;
                }
            }

            _disableInStation.SetActive(true);
        }
    }
}
