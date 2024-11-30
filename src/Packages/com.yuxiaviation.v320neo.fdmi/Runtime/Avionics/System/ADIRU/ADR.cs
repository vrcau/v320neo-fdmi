using JetBrains.Annotations;
using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRChatAerospaceUniversity.V320.Avionics.System.ADIRU {
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    [AircraftLifecycleReceiver]
    public class ADR : UdonSharpBehaviour {
        [Header("Other")]
        [SerializeField] private FDMiInt Mode;

        private ADIRUMode AdiruMode => (ADIRUMode)Mode.Data;

        [Header("Raw Data From Simulation")]
        [SerializeField] private FDMiFloat SimPressureAltitude;
        [SerializeField] private FDMiFloat SimSAT;
        [SerializeField] private FDMiFloat SimAirSpeed;
        [SerializeField] private FDMiFloat SimTAS;
        [SerializeField] private FDMiFloat SimAoA;
        [SerializeField] private FDMiFloat SimMach;

        [Header("Output")]
        [SerializeField] private FDMiFloat PressureAltitude;
        [SerializeField] private FDMiFloat SAT;
        [SerializeField] private FDMiFloat AirSpeed;
        [SerializeField] private FDMiFloat TAS;
        [SerializeField] private FDMiFloat AoA;
        [SerializeField] private FDMiFloat Mach;

        [SerializeField] [UdonSynced] private int _alignTime = -1;

        [PublicAPI] public bool IsAligned => _alignTime != -1 && Networking.GetServerTimeInSeconds() > _alignTime;

        [PublicAPI]
        public void _Init() {
            if (Networking.IsOwner(gameObject)) {
                _alignTime = -1;

                RequestSerialization();
            }

            ResetData();
        }

        private void LateUpdate() {
            UpdateLocal();

            if (Networking.IsOwner(gameObject))
                UpdateOwner();
        }

        private void UpdateLocal() {
            if (AdiruMode == ADIRUMode.OFF || IsAligned) {
                ResetData();
                return;
            }

            UpdateData();
        }

        private void UpdateOwner() {
            if (AdiruMode == ADIRUMode.OFF) {
                if (!IsAligned && _alignTime == -1) return;

                _alignTime = -1;
                RequestSerialization();

                return;
            }

            if (_alignTime != -1) return;

            _alignTime = (int)Networking.GetServerTimeInSeconds() + 10;

            RequestSerialization();
        }

        private void UpdateData() {
            PressureAltitude.Data = SimPressureAltitude.Data;
            SAT.Data = SimSAT.Data;
            AirSpeed.Data = SimAirSpeed.Data;
            TAS.Data = SimTAS.Data;
            AoA.Data = SimAoA.Data;
            Mach.Data = SimMach.Data;
        }

        private void ResetData() {
            PressureAltitude.Data = float.NaN;
            SAT.Data = float.NaN;
            AirSpeed.Data = float.NaN;
            TAS.Data = float.NaN;
            AoA.Data = float.NaN;
            Mach.Data = float.NaN;
        }
    }
}
