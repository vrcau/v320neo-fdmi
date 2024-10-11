using System;
using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRChatAerospaceUniversity.V320.Avionics.FlyByWire.ELAC {
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PitchLaw : UdonSharpBehaviour {
        [SerializeField] private FDMiFloat _verticalSpeed;
        [SerializeField] private FDMiFloat _mass;

        [SerializeField] private FDMiFloat _pitchInput;

        [SerializeField] private FDMiFloat _elevatorOutput;
        [SerializeField] private FDMiFloat _trimOutput;

        [SerializeField] private bool _isDirectLaw;

        private float _integral;
        private float _previousError;

        public float Kp = 1.5f;
        public float Ki = 0.00325f;
        public float Kd = 1.2f;

        public float TrimMinActive = 1f;

        public float TrimKi = 0.00325f;

        private float _lastVerticalSpeed;

        private float _lastTargetLoadFactor;

        private const float _maxLoadFactor = 2.5f;
        private const float _minLoadFactor = -1f;

        private void Update() {
            var mass = _mass.data[0];

            var verticalSpeed = _verticalSpeed.data[0];
            var verticalAcceleration = (verticalSpeed - _lastVerticalSpeed) / Time.deltaTime;

            _lastVerticalSpeed = verticalSpeed;

            var pilotPitchInput = _pitchInput.data[0];

            var inputLoadFactor = 1f + Mathf.Clamp(pilotPitchInput, -1f, 1f) * 1.5f;

            var targetLoadFactor = Mathf.Clamp(inputLoadFactor, _minLoadFactor, _maxLoadFactor);

            _lastTargetLoadFactor = targetLoadFactor;

            var targetLoadFactorChange = (targetLoadFactor - _lastTargetLoadFactor) / Time.deltaTime;

            if (_isDirectLaw) {
                _elevatorOutput.data[0] = pilotPitchInput;

                return;
            }

            var lift = mass * verticalAcceleration;
            var loadFactor = 1 + lift / (mass * 9.81f);

            var error = loadFactor - targetLoadFactor;
            _integral += error * Time.deltaTime;
            var derivative = (error - _previousError) / Time.deltaTime;

            var elevatorOutput = -(Kp * error + Ki * _integral + Kd * derivative);
            var elevatorFinalOutput = Mathf.Clamp(elevatorOutput, -1f, 1f);

            _elevatorOutput.data[0] = elevatorFinalOutput;

            var trimOutput = _trimOutput.data[0];

            if (targetLoadFactorChange < 0.5f && Mathf.Abs(_integral) > TrimMinActive) {
                trimOutput = Mathf.Clamp(_trimOutput.data[0] + _integral * TrimKi, -1f, 1f);
            }

            _trimOutput.data[0] = trimOutput;

            Debug.Log(
                $"pilot pitch input: {pilotPitchInput}, target load factor: {targetLoadFactor}, load factor: {loadFactor}, error: {error}, integral: {_integral} trim out: {trimOutput}, elevator output: {elevatorFinalOutput}");
        }
    }
}
