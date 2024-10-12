using System;
using JetBrains.Annotations;
using tech.gyoku.FDMi.core;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRChatAerospaceUniversity.V320.Avionics.Instruments.Clock {
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    [AircraftLifecycleReceiver]
    public class Clock : UdonSharpBehaviour {
        [SerializeField] private TextMeshProUGUI _dateTimeText;
        [SerializeField] private TextMeshProUGUI _secondYearText;

        [SerializeField] private TextMeshProUGUI _chronoText;

        [HideInInspector] [UdonSynced] [SerializeField] private float _chronoStartTime = float.NaN;
        [HideInInspector] [UdonSynced] [SerializeField] private float _chronoPauseTime = float.NaN;
        [HideInInspector] [UdonSynced] [SerializeField] private float _chronoOffsetTime;
        [HideInInspector] [UdonSynced] [SerializeField] private bool _isChronoRunning;

        [PublicAPI]
        public void _OnInit() {
            if (Networking.IsOwner(gameObject)) _ResetChrono();
        }

        [PublicAPI]
        public void _OnLostPower() {
            if (Networking.IsOwner(gameObject)) _ResetChrono();
        }

        private void LateUpdate() {
            var currentDateTime = DateTime.UtcNow;

            _dateTimeText.text = currentDateTime.ToString("HH:MM");
            _secondYearText.text = currentDateTime.ToString("ss");

            if (float.IsNaN(_chronoStartTime)) {
                _chronoText.text = "";
                return;
            }

            var currentTime = _isChronoRunning ? Networking.GetServerTimeInSeconds() : _chronoPauseTime;
            var chronoTime = currentTime - _chronoStartTime + _chronoOffsetTime;

            // When running: MM:SS, when stopped: MM SS
            _chronoText.text =
                $"{(int)chronoTime / 60:D2}{(_isChronoRunning ? ":" : " ")}{(int)chronoTime % 60:D2}";
        }

        [PublicAPI]
        public void _ToggleChrono() {
            _TakeOwnership();

            if (float.IsNaN(_chronoStartTime)) {
                _chronoStartTime = (float)Networking.GetServerTimeInSeconds();
                _isChronoRunning = true;

                RequestSerialization();
                return;
            }

            if (_isChronoRunning) {
                _chronoPauseTime = (float)Networking.GetServerTimeInSeconds();
                _isChronoRunning = false;

                RequestSerialization();
                return;
            }

            _chronoOffsetTime += _chronoPauseTime - (float)Networking.GetServerTimeInSeconds();
            _isChronoRunning = true;

            RequestSerialization();
        }

        [PublicAPI]
        public void _ResetChrono() {
            _TakeOwnership();

            _chronoStartTime = float.NaN;
            _chronoPauseTime = float.NaN;
            _chronoOffsetTime = 0;
            _isChronoRunning = false;

            RequestSerialization();
        }

        private void _TakeOwnership() {
            if (Networking.IsOwner(gameObject)) return;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
    }
}
