using System;
using JetBrains.Annotations;
using tech.gyoku.FDMi.core;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.Serialization;
using VRC.SDKBase;

namespace VRChatAerospaceUniversity.V320.Avionics.Instruments.Clock
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    [AircraftLifecycleReceiver]
    public class Clock : UdonSharpBehaviour {
        [SerializeField] private TextMeshProUGUI _dateTimeText;
        [SerializeField] private TextMeshProUGUI _secondYearText;

        [SerializeField] private TextMeshProUGUI _chronoText;

        [SerializeField] private FDMiFloat _chronoStartTime;
        [SerializeField] private FDMiFloat _chronoPauseTime;
        [SerializeField] private FDMiFloat _chronoOffsetTime;
        [SerializeField] private FDMiBool _isChronoRunning;

        [PublicAPI]
        public void _OnInit() {
            _ResetChrono();
        }

        [PublicAPI]
        public void _OnLostPower() {
            _ResetChrono();
        }

        private void LateUpdate() {
            var currentDateTime = DateTime.UtcNow;

            _dateTimeText.text = currentDateTime.ToString("HH:MM");
            _secondYearText.text = currentDateTime.ToString("ss");

            if (float.IsNaN(_chronoStartTime.Data)) {
                _chronoText.text = "";
                return;
            }

            var currentTime = _isChronoRunning.Data ? Networking.GetServerTimeInSeconds() : _chronoPauseTime.Data;
            var chronoTime = currentTime - _chronoStartTime.Data + _chronoOffsetTime.Data;

            // When running: MM:SS, when stopped: MM SS
            _chronoText.text = $"{(int) chronoTime / 60:D2}{(_isChronoRunning.Data ? ":" : " ")}{(int) chronoTime % 60:D2}";
        }

        [PublicAPI]
        public void _ToggleChrono() {
            if (float.IsNaN(_chronoStartTime.Data)) {
                _chronoStartTime.Data = (float) Networking.GetServerTimeInSeconds();
                _isChronoRunning.Data = true;
                return;
            }

            if (_isChronoRunning.Data) {
                _chronoPauseTime.Data = (float) Networking.GetServerTimeInSeconds();
                _isChronoRunning.Data = false;
                return;
            }

            _chronoOffsetTime.Data += _chronoPauseTime.Data - (float) Networking.GetServerTimeInSeconds();
            _isChronoRunning.Data = true;
        }

        [PublicAPI]
        public void _ResetChrono() {
            _chronoStartTime.Data = float.NaN;
            _chronoPauseTime.Data = float.NaN;
            _chronoOffsetTime.Data = 0;
            _isChronoRunning.Data = false;
        }
    }
}
