using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace VRChatAerospaceUniversity.V320.Avionics.FlyByWire
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Monitor : UdonSharpBehaviour {
        [SerializeField] private FDMiFloat _verticalSpeed;
        [SerializeField] private FDMiFloat _mass;

        [SerializeField] private Text _velocityText;
        [SerializeField] private Text _loadFactorText;

        private float _lastVerticalSpeed;

        private void Update() {
            var mass = _mass.data[0];

            var verticalSpeed = _verticalSpeed.data[0];
            var verticalAcceleration = (verticalSpeed - _lastVerticalSpeed) / Time.deltaTime;

            var lift = mass * verticalAcceleration;
            var loadFactor = 1 + lift / (mass * 9.81f);

            _lastVerticalSpeed = verticalSpeed;

            _velocityText.text = verticalAcceleration.ToString("F");
            _loadFactorText.text = loadFactor.ToString("F");
        }
    }
}
