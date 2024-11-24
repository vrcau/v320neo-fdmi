using JetBrains.Annotations;
using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRChatAerospaceUniversity.V320.Avionics.System.ADIRU {
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    [AircraftLifecycleReceiver]
    public class IR : UdonSharpBehaviour {
        [Header("Other")]
        [SerializeField] private FDMiInt Mode;

        private ADIRUMode AdiruMode => (ADIRUMode)Mode.Data;

        [Header("Raw Data From Simulation")]
        [SerializeField] private FDMiFloat SimPitch;
        [SerializeField] private FDMiFloat SimRoll;
        [SerializeField] private FDMiFloat SimHDG;
        [SerializeField] private FDMiFloat SimGroundSpeed;
        [SerializeField] private FDMiVector3 SimPosition;

        [Header("Output")]
        [SerializeField] private FDMiFloat Pitch;
        [SerializeField] private FDMiFloat Roll;
        [SerializeField] private FDMiFloat HDG;
        [SerializeField] private FDMiFloat GroundSpeed;
        [SerializeField] private FDMiVector3 Position;

        [SerializeField] [UdonSynced] private int _alignTime = -1;

        private int _lastMode = (int)ADIRUMode.OFF;

        private ADIRUMode _lastAdiruMode {
            get => _lastMode == -1 ? ADIRUMode.OFF : (ADIRUMode)_lastMode;
            set => _lastMode = (int)value;
        }

        [PublicAPI] public bool IsAligned => _alignTime != -1 && Networking.GetServerTimeInSeconds() > _alignTime;

        [PublicAPI]
        public void _Init() {
            if (Networking.IsOwner(gameObject)) {
                _alignTime = -1;
                _lastAdiruMode = ADIRUMode.OFF;

                RequestSerialization();
            }

            ResetData();
        }

        private void LateUpdate() {
            UpdateLocal();

            if (Networking.IsOwner(gameObject))
                UpdateOwner();

            _lastAdiruMode = AdiruMode;
        }

        private void UpdateLocal() {
            if (AdiruMode == ADIRUMode.OFF || _lastAdiruMode != AdiruMode) {
                ResetData();
                return;
            }

            UpdateData();
        }

        private void UpdateOwner() {
            if (AdiruMode == ADIRUMode.OFF || _lastAdiruMode != AdiruMode) {
                if (_alignTime == -1) return;

                _alignTime = -1;

                RequestSerialization();
                return;
            }

            if (_alignTime != -1) return;

            _alignTime = (int)Networking.GetServerTimeInSeconds() + 30;
            RequestSerialization();
        }

        private void UpdateData() {
            Pitch.Data = SimPitch.Data;
            Roll.Data = SimRoll.Data;
            HDG.Data = SimHDG.Data;

            if (AdiruMode != ADIRUMode.NAV) {
                GroundSpeed.Data = float.NaN;
                Position.Data = Vector3.zero;
                return;
            }

            GroundSpeed.Data = SimGroundSpeed.Data;
            Position.Data = SimPosition.Data;
        }

        private void ResetData() {
            Pitch.Data = float.NaN;
            Roll.Data = float.NaN;
            HDG.Data = float.NaN;
            GroundSpeed.Data = float.NaN;
            Position.Data = Vector3.zero;
        }
    }
}
