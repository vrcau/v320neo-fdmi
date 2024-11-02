using System;
using JetBrains.Annotations;
using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;

namespace VRChatAerospaceUniversity.V320.Avionics.Electric.Components {
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ElectricalAppliance : UdonSharpBehaviour {
        public float requiredPowerLevel;

        public UdonSharpBehaviour[] electricalEventReceivers;

        public FDMiBool fdmiIsPowered;

        private float powerLevel;
        private float _lastPowerLevel;
        private bool _isPowered;
        [PublicAPI]
        public bool IsPowered {
            get => _isPowered;
            private set {
                _isPowered = value;
                if (fdmiIsPowered)
                    fdmiIsPowered.Data = value;
            }
        }

        private bool _isPoweredLastFrame;
        private bool _isPowerCalledLastFrame;

        [PublicAPI]
        public virtual void Power(float providePowerLevel) {
            if (!enabled || !gameObject.activeInHierarchy) {
                AfterFrame();
                return;
            }

            _isPowerCalledLastFrame = true;

            powerLevel += providePowerLevel;

            IsPowered = powerLevel >= requiredPowerLevel;
        }

        private void LateUpdate() => AfterFrame();

        private void OnDisable() => AfterFrame();

        private void AfterFrame() {
            if (!_isPowerCalledLastFrame) {
                IsPowered = false;
            }

            _isPowerCalledLastFrame = false;

            if (IsPowered != _isPoweredLastFrame) {
                SendEventToReceivers("_OnPowerStateChanged");
                SendEventToReceivers(IsPowered ? "_OnPower" : "_OnLostPower");
            }

            if (!Mathf.Approximately(powerLevel, _lastPowerLevel)) {
                SendEventToReceivers("_OnPowerLevelChanged");

                if (IsPowered) {
                    SendEventToReceivers("_OnPowerLevelChangedWhilePowered");
                }
            }

            _lastPowerLevel = powerLevel;
            powerLevel = 0;
            _isPoweredLastFrame = IsPowered;
        }

        private void SendEventToReceivers(string eventName) {
            foreach (var receiver in electricalEventReceivers) {
                receiver.SendCustomEvent(eventName);
            }
        }
    }
}
