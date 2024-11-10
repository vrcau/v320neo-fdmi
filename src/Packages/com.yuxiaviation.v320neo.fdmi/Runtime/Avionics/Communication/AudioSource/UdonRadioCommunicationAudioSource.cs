using JetBrains.Annotations;
using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;
using URC;
using VRC.SDKBase;

namespace VRChatAerospaceUniversity.V320.Avionics.Communication.AudioSource {
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    [AircraftLifecycleReceiver]
    public class UdonRadioCommunicationAudioSource : UdonSharpBehaviour {
        public float defaultFrequency = 118.0f;

        [PublicAPI]
        public float Frequency {
            get => _transceiver.Frequency;
            set => _transceiver._SetFrequency(value);
        }

        [UdonSynced] [SerializeField] [HideInInspector] private bool _receive;
        [PublicAPI]
        public bool Receive {
            get => _receive;
            set {
                TakeOwnership();

                _receive = value;
                _UpdateReceiveTransmit();

                RequestSerialization();
            }
        }

        [UdonSynced] [SerializeField] [HideInInspector] private bool _transmit;
        [PublicAPI]
        public bool Transmit {
            get => _transmit;
            set {
                TakeOwnership();

                _transmit = value;
                _UpdateReceiveTransmit();

                RequestSerialization();
            }
        }

        [SerializeField] private Transceiver _transceiver;
        [SerializeField] private FDMiBool _isPowered;

        public void _UpdateReceiveTransmit() {
            var isPowered = !_isPowered || _isPowered.Data;

            _transceiver._SetTransmit(isPowered && _transmit);
            _transceiver._SetReceive(isPowered && _receive);
        }

        [PublicAPI]
        private void _OnPowerStateChanged() => _UpdateReceiveTransmit();
        public override void OnDeserialization() => _UpdateReceiveTransmit();

        [PublicAPI]
        public void _OnInit() {
            if (_isPowered) {
                _isPowered.subscribe(this, nameof(_UpdateReceiveTransmit));
            }

            // Delay 4s due to
            // https://github.com/esnya/UdonRadioCommunications/blob/v5.0.0/Packages/com.nekometer.esnya.udon-radio-communications/Scripts/UdonRadioCommunication.cs#L63
            SendCustomEventDelayedSeconds(nameof(_SetDefaultFrequency), 4);
            _UpdateReceiveTransmit();
        }

        public void _SetDefaultFrequency() {
            _transceiver.Frequency = defaultFrequency;
        }

        private void TakeOwnership() {
            if (Networking.IsOwner(gameObject)) return;

            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
    }
}
