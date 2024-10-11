using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;

namespace VRChatAerospaceUniversity.V320.Avionics.Instruments.DisplayUnit {
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    [AircraftLifecycleReceiver]
    public class DisplayManageComputer : UdonSharpBehaviour {
        // Display Units
        [SerializeField] private DisplayUnit _leftPfdDisplayUnit;
        [SerializeField] private DisplayUnit _leftNdDisplayUnit;

        [SerializeField] private DisplayUnit _rightPfdDisplayUnit;
        [SerializeField] private DisplayUnit _rightNdDisplayUnit;

        [SerializeField] private DisplayUnit _ecamEwdDisplayUnit;
        [SerializeField] private DisplayUnit _ecamSdDisplayUnit;

        // Contents
        [SerializeField] private RectTransform _leftPfdContent;
        [SerializeField] private RectTransform _leftNdContent;

        [SerializeField] private RectTransform _rightPfdContent;
        [SerializeField] private RectTransform _rightNdContent;

        [SerializeField] private RectTransform _ecamEwdContent;
        [SerializeField] private RectTransform _ecamSdContent;

        [UdonSynced] [SerializeField] private bool _isLeftPfdNdSwitched;
        [UdonSynced] [SerializeField] private bool _isRightPfdNdSwitched;

        [PublicAPI]
        public void _OnInit() {
            SetPfdNdContent(_leftPfdDisplayUnit, _leftNdDisplayUnit, _leftPfdContent, _leftNdContent);
            SetPfdNdContent(_rightPfdDisplayUnit, _rightNdDisplayUnit, _rightPfdContent, _rightNdContent);
        }

        public void SwitchLeftPfdNd() => SwitchPfdNd(_leftPfdDisplayUnit, _leftNdDisplayUnit, _leftPfdContent,
            _leftNdContent, ref _isLeftPfdNdSwitched);

        public void SwitchRightPfdNd() => SwitchPfdNd(_rightPfdDisplayUnit, _rightNdDisplayUnit, _rightPfdContent,
            _rightNdContent, ref _isRightPfdNdSwitched);

        private void SwitchPfdNd(DisplayUnit pfdDisplayUnit, DisplayUnit ndDisplayUnit, RectTransform pfdContent,
            RectTransform ndContent, ref bool isSwitched) {
            if (isSwitched) {
                SetPfdNdContent(pfdDisplayUnit, ndDisplayUnit, pfdContent, ndContent);
            }
            else {
                SetPfdNdContent(ndDisplayUnit, pfdDisplayUnit, pfdContent, ndContent);
            }

            isSwitched = !isSwitched;

            RequestSerialization();
        }

        private static void SetPfdNdContent(DisplayUnit pfdDisplayUnit, DisplayUnit ndDisplayUnit, RectTransform pfdContent,
            RectTransform ndContent) {
            pfdDisplayUnit.SetContent(pfdContent);
            ndDisplayUnit.SetContent(ndContent);
        }
    }
}
