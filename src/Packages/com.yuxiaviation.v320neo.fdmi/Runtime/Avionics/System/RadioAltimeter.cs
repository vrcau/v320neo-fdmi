using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;

namespace VRChatAerospaceUniversity.V320.Avionics.System
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class RadioAltimeter : UdonSharpBehaviour
    {
        public FDMiFloat radioAltitude;

        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _offset;
        [SerializeField] private float _maxDistance = 762f; // 2500ft

        private void FixedUpdate() {
            if (!Physics.Raycast(gameObject.transform.position, Vector3.down, out var hit, _maxDistance, _groundLayer)) {
                radioAltitude.Data = float.PositiveInfinity;
                return;
            }

            radioAltitude.Data = hit.distance + _offset;
        }

        private void OnDisable() {
            radioAltitude.Data = float.NaN;
        }
    }
}
