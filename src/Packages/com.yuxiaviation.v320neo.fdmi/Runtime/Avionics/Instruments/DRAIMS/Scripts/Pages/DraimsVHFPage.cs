using JetBrains.Annotations;
using tech.gyoku.FDMi.core;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRChatAerospaceUniversity.V320.Avionics.Instruments.DRAIMS.Scripts.Components;

namespace VRChatAerospaceUniversity.V320.Avionics.Instruments.DRAIMS.Scripts.Pages {
    [AircraftLifecycleReceiver]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DraimsVHFPage : DraimsPageBase {
        private FDMiBool _isVHF1Receive;
        private FDMiBool _isVHF1Transmit;

        private FDMiBool _isVHF2Receive;
        private FDMiBool _isVHF2Transmit;

        private FDMiBool _isVHF3Receive;
        private FDMiBool _isVHF3Transmit;

        [SerializeField] private RadioComponent _vhf1Component;
        [SerializeField] private RadioComponent _vhf2Component;
        [SerializeField] private RadioComponent _vhf3Component;

        // Frequency

        [SerializeField] private FDMiFloat _vhf1Frequency;
        [SerializeField] private FDMiFloat _vhf2Frequency;
        [SerializeField] private FDMiFloat _vhf3Frequency;

        [SerializeField] private FDMiFloat _vhf1StandByFrequency;
        [SerializeField] private FDMiFloat _vhf2StandByFrequency;
        [SerializeField] private FDMiFloat _vhf3StandByFrequency;

        private RadioComponent _selectComponent;

        public override void _FirstInitPage(DRAIMS draims) {
            base._FirstInitPage(draims);

            _isVHF1Receive = _draims._isVHF1Receive;
            _isVHF1Transmit = _draims._isVHF1Transmit;

            _isVHF2Receive = _draims._isVHF2Receive;
            _isVHF2Transmit = _draims._isVHF2Transmit;

            _isVHF3Receive = _draims._isVHF3Receive;
            _isVHF3Transmit = _draims._isVHF3Transmit;

            _vhf1Component._InitComponent(_vhf1Frequency, _vhf1StandByFrequency, _isVHF1Receive, _isVHF1Transmit,
                draims);
            _vhf2Component._InitComponent(_vhf2Frequency, _vhf2StandByFrequency, _isVHF2Receive, _isVHF2Transmit,
                draims);
            _vhf3Component._InitComponent(_vhf3Frequency, _vhf3StandByFrequency, _isVHF3Receive, _isVHF3Transmit,
                draims);

            SelectComponent(_vhf1Component);
        }

        public override void _OnKeyPressed(DRAIMSKeyType keyType) {
            base._OnKeyPressed(keyType);

            switch (keyType) {
                case DRAIMSKeyType.SelectionLeft1:
                    _vhf1Component._SwipeFrequency();
                    break;
                case DRAIMSKeyType.SelectionLeft2:
                    _vhf2Component._SwipeFrequency();
                    break;
                case DRAIMSKeyType.SelectionLeft3:
                    _vhf3Component._SwipeFrequency();
                    break;
                case DRAIMSKeyType.SelectionRight1:
                    SelectComponent(_vhf1Component);
                    break;
                case DRAIMSKeyType.SelectionRight2:
                    SelectComponent(_vhf2Component);
                    break;
                case DRAIMSKeyType.SelectionRight3:
                    SelectComponent(_vhf3Component);
                    break;
                case DRAIMSKeyType.Number0:
                case DRAIMSKeyType.Number1:
                case DRAIMSKeyType.Number2:
                case DRAIMSKeyType.Number3:
                case DRAIMSKeyType.Number4:
                case DRAIMSKeyType.Number5:
                case DRAIMSKeyType.Number6:
                case DRAIMSKeyType.Number7:
                case DRAIMSKeyType.Number8:
                case DRAIMSKeyType.Number9:
                case DRAIMSKeyType.Clear:
                    _selectComponent._Input(keyType);
                    break;
            }
        }

        private void SelectComponent(RadioComponent component) {
            DeSelectAll();

            _selectComponent = component;
            _selectComponent._Select();
        }

        private void DeSelectAll() {
            _selectComponent = null;

            _vhf1Component._DeSelect();
            _vhf2Component._DeSelect();
            _vhf3Component._DeSelect();
        }
    }
}
