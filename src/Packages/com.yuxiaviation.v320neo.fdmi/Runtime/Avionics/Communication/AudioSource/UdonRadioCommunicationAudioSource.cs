using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using URC;

namespace VRChatAerospaceUniversity.V320.Avionics.Communication.AudioSource {
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    [AircraftLifecycleReceiver]
    public class UdonRadioCommunicationAudioSource : UdonSharpBehaviour {
        public float defaultFrequency = 118.0f;

        [PublicAPI]
        public float Frequency {
            get => _transceiver.Frequency;
            set => _transceiver._SetFrequency(value);
        }

        [PublicAPI]
        public bool Receive {
            get => _transceiver._GetReceive();
            set => _transceiver._SetReceive(value);
        }

        [PublicAPI]
        public bool Transmit {
            get => _transceiver._GetTransmit();
            set => _transceiver._SetTransmit(value);
        }

        [SerializeField] private Transceiver _transceiver;

        [PublicAPI]
        public void _OnInit() {
            // Delay 4s due to
            // https://github.com/esnya/UdonRadioCommunications/blob/v5.0.0/Packages/com.nekometer.esnya.udon-radio-communications/Scripts/UdonRadioCommunication.cs#L63
            SendCustomEventDelayedSeconds(nameof(_SetDefaultFrequency), 4);
        }

        public void _SetDefaultFrequency() {
            _transceiver.Frequency = defaultFrequency;
        }
    }
}
