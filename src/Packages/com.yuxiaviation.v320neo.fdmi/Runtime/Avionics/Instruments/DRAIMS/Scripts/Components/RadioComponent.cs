using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using tech.gyoku.FDMi.core;
using TMPro;
using UdonSharp;
using UnityEngine;

namespace VRChatAerospaceUniversity.V320.Avionics.Instruments.DRAIMS.Scripts.Components {
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
        [SerializeField] private TextMeshProUGUI _standByText;

        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _unselectedColor;
        [SerializeField] private Color _notValidColor;

        [PublicAPI]
        public void _InitComponent(FDMiFloat frequency, FDMiFloat standByFrequency, FDMiBool isReceive,
            FDMiBool isTransmit, DRAIMS draims) {
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

            _DeSelect();
        }

        public void _OnFrequencyChange() {
            _frequencyText.text = _frequency.Data.ToString("000.000");

            if (_editingTextString == "")
                _standByFrequencyText.text = _standByFrequency.Data.ToString("000.000");
        }

        public void _OnReceiveChange() {
            _receiveIcon.SetActive(_isReceive.Data);
        }

        public void _OnTransmitChange() {
            _transmitIcon.SetActive(_isTransmit.Data);
        }

        public void _SwipeFrequency() {
            if (_editingTextString.Length >= 3) {
                _standByFrequency.Data = float.Parse(_editingTextString);
            }

            if (_editingTextString != "") {
                _editingTextString = "";
                SetStandBy();
            }

            // ReSharper disable once SwapViaDeconstruction
            var newStandByFrequency = _frequency.Data;

            _frequency.Data = _standByFrequency.Data;
            _standByFrequency.Data = newStandByFrequency;
        }

        public void _Select() {
            _isSelectedIcon.SetActive(true);
            SetStandBy();
        }

        public void _DeSelect() {
            _isSelectedIcon.SetActive(false);
            SetStandBy();

            _editingTextString = "";
            _standByFrequencyText.color = _unselectedColor;
        }

        private string _editingTextString = "";
        private const string _editingPlaceholder = "___.___";
        private const string _EditingLast3DigitPlaceholder = "___.000";
        private const string _maxFrequency = "136.975";
        private const string _minFrequency = "108.000";

        public void _Input(DRAIMSKeyType keyType) {
            var charToInput = ' ';
            switch (keyType) {
                case DRAIMSKeyType.Number0:
                    charToInput = '0';
                    break;
                case DRAIMSKeyType.Number1:
                    charToInput = '1';
                    break;
                case DRAIMSKeyType.Number2:
                    charToInput = '2';
                    break;
                case DRAIMSKeyType.Number3:
                    charToInput = '3';
                    break;
                case DRAIMSKeyType.Number4:
                    charToInput = '4';
                    break;
                case DRAIMSKeyType.Number5:
                    charToInput = '5';
                    break;
                case DRAIMSKeyType.Number6:
                    charToInput = '6';
                    break;
                case DRAIMSKeyType.Number7:
                    charToInput = '7';
                    break;
                case DRAIMSKeyType.Number8:
                    charToInput = '8';
                    break;
                case DRAIMSKeyType.Number9:
                    charToInput = '9';
                    break;
                case DRAIMSKeyType.Clear:
                    _editingTextString = "";
                    UpdateEditing();
                    return;
            }

            if (charToInput == ' ') return;
            if (_editingTextString.Length == 3) {
                _editingTextString += ".";
            }

            _editingTextString += charToInput;

            UpdateEditing();
        }

        private void UpdateEditing() {
            var isInputNotValid = UpdateNotValid();

            if (_editingTextString.Length == 7) {
                if (!isInputNotValid) {
                    var newFrequency = float.Parse(_editingTextString);
                    _standByFrequency.Data = newFrequency;
                    _editingTextString = "";
                    SetStandBy();

                    return;
                }

                _editingTextString = "";
                SetStandBy();
            }
            else if (_editingTextString.Length >= 3) {
                // ReSharper disable once ReplaceSubstringWithRangeIndexer
                _standByFrequencyText.text = _editingTextString +
                                             _EditingLast3DigitPlaceholder.Substring(_editingTextString.Length);
                return;
            }

            // ReSharper disable once ReplaceSubstringWithRangeIndexer
            _standByFrequencyText.text = _editingTextString + _editingPlaceholder.Substring(_editingTextString.Length);
        }

        [SuppressMessage("ReSharper", "ReplaceSubstringWithRangeIndexer")]
        private bool UpdateNotValid() {
            if (_editingTextString == "") {
                SetStandBy();
                return false;
            }

            var minFrequencySub = float.Parse(_minFrequency.Substring(0, _editingTextString.Length));
            var maxFrequencySub = float.Parse(_maxFrequency.Substring(0, _editingTextString.Length));

            if (float.Parse(_editingTextString) < minFrequencySub ||
                float.Parse(_editingTextString) > maxFrequencySub) {
                SetNotValid();
                return true;
            }

            SetStandBy();
            return false;
        }

        private void SetStandBy() {
            _standByFrequencyText.text = _standByFrequency.Data.ToString("000.000");
            _standByFrequencyText.color = _selectedColor;
            _standByText.color = _unselectedColor;
            _standByText.text = "STBY";
        }

        private void SetNotValid() {
            _standByFrequencyText.text = _standByFrequency.Data.ToString("000.000");
            _standByFrequencyText.color = _notValidColor;
            _standByText.color = _notValidColor;
            _standByText.text = "NOT VALID";
        }
    }
}
