using JetBrains.Annotations;
using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;
using VRChatAerospaceUniversity.V320.Avionics.Communication.AudioSource;

namespace VRChatAerospaceUniversity.V320.Avionics.Communication.AudioController {
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    [AircraftLifecycleReceiver]
    public class AudioController : UdonSharpBehaviour {
        [SerializeField] private UdonRadioCommunicationAudioSource[] _audioSources = { };

        [SerializeField] private string[] _keys = { };
        [SerializeField] private FDMiBool[] _isReceive = { };
        [SerializeField] private FDMiBool[] _isTransmit = { };
        [SerializeField] private FDMiFloat[] _frequency = { };

        [PublicAPI]
        public void _OnFirstInit() {
            foreach (var isReceiveBool in _isReceive) {
                isReceiveBool.subscribe(this, nameof(_OnAudioSourceStatusChange));
            }

            foreach (var isTransmitBool in _isTransmit) {
                isTransmitBool.subscribe(this, nameof(_OnAudioSourceStatusChange));
            }

            foreach (var frequencyFloat in _frequency) {
                frequencyFloat.subscribe(this, nameof(_OnAudioSourceStatusChange));
            }
        }

        [PublicAPI]
        public void _OnInit() {
            _OnAudioSourceStatusChange();
        }

        private void OnEnable() {
            _OnAudioSourceStatusChange();
        }

        private void OnDisable() {
            Debug.Log("Disable all audio sources");

            foreach (var audioSource in _audioSources)
            {
                audioSource.Receive = false;
                audioSource.Transmit = false;
            }
        }

        public void _OnAudioSourceStatusChange() {
            if (!enabled || !gameObject.activeInHierarchy)
                return;

            Debug.Log("Update audio sources");

            for (var audioSourceIndex = 0; audioSourceIndex < _audioSources.Length; audioSourceIndex++) {
                var audioSource = _audioSources[audioSourceIndex];

                audioSource.Frequency = _frequency[audioSourceIndex].Data;
                audioSource.Receive = _isReceive[audioSourceIndex].Data;
                audioSource.Transmit = _isTransmit[audioSourceIndex].Data;
            }
        }

        [PublicAPI]
        [CanBeNull]
        public UdonRadioCommunicationAudioSource GetAudioSourceByKey(string key) {
            for (var keyIndex = 0; keyIndex < _keys.Length; keyIndex++) {
                if (_keys[keyIndex] == key) {
                    return _audioSources[keyIndex];
                }
            }

            return null;
        }

        [PublicAPI]
        [CanBeNull]
        public UdonRadioCommunicationAudioSource GetAudioSourceByIndex(int index) {
            if (index < 0 || index >= _audioSources.Length) {
                return null;
            }

            return _audioSources[index];
        }
    }
}
