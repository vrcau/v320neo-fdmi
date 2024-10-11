using JetBrains.Annotations;
using tech.gyoku.FDMi.core;
using TMPro;
using UdonSharp;
using UnityEngine;

namespace VRChatAerospaceUniversity.V320.Avionics.Instruments.DRAIMS.Scripts.Components
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    [AircraftLifecycleReceiver]
    public class RadioComponent : UdonSharpBehaviour {
        private DRAIMS _draims;

        private FDMiFloat _frequency;
        private FDMiFloat _standByFrequency;

        private FDMiBool _isReceive;
        private FDMiBool _isTransmit;

        [SerializeField] private GameObject _receiveIcon;
        [SerializeField] private GameObject _transmitIcon;

        [SerializeField] private TextMeshProUGUI _frequencyText;
        [SerializeField] private TextMeshProUGUI _standByFrequencyText;

        [SerializeField] private GameObject _isSelectedIcon;


        [PublicAPI]
        public void _InitComponent(FDMiFloat frequency, FDMiFloat standByFrequency, FDMiBool isReceive, FDMiBool isTransmit, DRAIMS draims) {
            _frequency = frequency;
            _standByFrequency = standByFrequency;
            _isReceive = isReceive;
            _isTransmit = isTransmit;

            _frequency.subscribe(this, nameof(_OnFrequencyChange));
            _standByFrequency.subscribe(this, nameof(_OnFrequencyChange));
            _isReceive.subscribe(this, nameof(_OnReceiveChange));
            _isTransmit.subscribe(this, nameof(_OnTransmitChange));

            _OnFrequencyChange();
            _OnReceiveChange();
            _OnTransmitChange();
        }

        public void _OnFrequencyChange() {
            _frequencyText.text = _frequency.Data.ToString("000.000");
            _standByFrequencyText.text = _standByFrequency.Data.ToString("000.000");
        }

        public void _OnReceiveChange() {
            _receiveIcon.SetActive(_isReceive.Data);
        }

        public void _OnTransmitChange()
        {
            _transmitIcon.SetActive(_isTransmit.Data);
        }
    }
}
