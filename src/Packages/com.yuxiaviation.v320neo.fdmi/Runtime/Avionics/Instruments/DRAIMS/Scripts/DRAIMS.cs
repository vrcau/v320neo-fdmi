using JetBrains.Annotations;
using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;
using VRChatAerospaceUniversity.V320.Avionics.Instruments.DRAIMS.Scripts.Pages;
using VRChatAerospaceUniversity.V320.Scripts;

namespace VRChatAerospaceUniversity.V320.Avionics.Instruments.DRAIMS.Scripts {
    [AircraftLifecycleReceiver]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DRAIMS : UdonSharpBehaviour {
        public FDMiBool _isVHF1Receive;
        public FDMiBool _isVHF1Transmit;

        public FDMiBool _isVHF2Receive;
        public FDMiBool _isVHF2Transmit;

        public FDMiBool _isVHF3Receive;
        public FDMiBool _isVHF3Transmit;

        [SerializeField] private DraimsPageBase[] _pages = { };
        [SerializeField] private PageRouter _pageRouter;

        [PublicAPI]
        public void _OnFirstInit() {
            foreach (var page in _pages) {
                page._FirstInitPage(this);
            }
        }

        [PublicAPI]
        public void _OnFInit() {
            foreach (var page in _pages) {
                page._InitPage(this);
            }
        }

    #region Input

    #region SelectionKeys

        [PublicAPI] public void _OnLeft1Pressed() => SendInputToPage(DRAIMSKeyType.SelectionLeft1);
        [PublicAPI] public void _OnLeft2Pressed() => SendInputToPage(DRAIMSKeyType.SelectionLeft2);
        [PublicAPI] public void _OnLeft3Pressed() => SendInputToPage(DRAIMSKeyType.SelectionLeft3);
        [PublicAPI] public void _OnLeft4Pressed() => SendInputToPage(DRAIMSKeyType.SelectionLeft4);
        [PublicAPI] public void _OnRight1Pressed() => SendInputToPage(DRAIMSKeyType.SelectionRight1);
        [PublicAPI] public void _OnRight2Pressed() => SendInputToPage(DRAIMSKeyType.SelectionRight2);
        [PublicAPI] public void _OnRight3Pressed() => SendInputToPage(DRAIMSKeyType.SelectionRight3);
        [PublicAPI] public void _OnRight4Pressed() => SendInputToPage(DRAIMSKeyType.SelectionRight4);

    #endregion

    #region Keyboard

        [PublicAPI] public void _OnNumber1Pressed() => SendInputToPage(DRAIMSKeyType.Number1);
        [PublicAPI] public void _OnNumber2Pressed() => SendInputToPage(DRAIMSKeyType.Number2);
        [PublicAPI] public void _OnNumber3Pressed() => SendInputToPage(DRAIMSKeyType.Number3);
        [PublicAPI] public void _OnNumber4Pressed() => SendInputToPage(DRAIMSKeyType.Number4);
        [PublicAPI] public void _OnNumber5Pressed() => SendInputToPage(DRAIMSKeyType.Number5);
        [PublicAPI] public void _OnNumber6Pressed() => SendInputToPage(DRAIMSKeyType.Number6);
        [PublicAPI] public void _OnNumber7Pressed() => SendInputToPage(DRAIMSKeyType.Number7);
        [PublicAPI] public void _OnNumber8Pressed() => SendInputToPage(DRAIMSKeyType.Number8);
        [PublicAPI] public void _OnNumber9Pressed() => SendInputToPage(DRAIMSKeyType.Number9);
        [PublicAPI] public void _OnNumber0Pressed() => SendInputToPage(DRAIMSKeyType.Number0);

        [PublicAPI] public void _OnDotPressed() => SendInputToPage(DRAIMSKeyType.Dot);
        [PublicAPI] public void _OnClearPressed() => SendInputToPage(DRAIMSKeyType.Clear);

    #endregion

        [PublicAPI] public void _OnLeftGenericKeyPressed() => SendInputToPage(DRAIMSKeyType.GenericLeft);
        [PublicAPI] public void _OnRightGenericKeyPressed() => SendInputToPage(DRAIMSKeyType.GenericRight);
        [PublicAPI] public void _OnUpKeyPressed() => SendInputToPage(DRAIMSKeyType.Up);
        [PublicAPI] public void _OnDownKeyPressed() => SendInputToPage(DRAIMSKeyType.Down);

        private void SendInputToPage(DRAIMSKeyType keyType) {
            var pageObject = _pageRouter._GetObjectByPath(_pageRouter.CurrentPath);
            if (!pageObject)
                return;

            var page = pageObject.GetComponentInChildren<DraimsPageBase>(true);
            if (!page)
                return;

            page._OnKeyPressed(keyType);
        }

    #endregion

    #region Navigation Keys

        [PublicAPI] public void _VhfPageKeyPressed() {
            _pageRouter._ChangePath("/draims/vhf");
        }

        [PublicAPI] public void _HfPageKeyPressed() {
            _pageRouter._ChangePath("/draims/hf");
        }

        [PublicAPI] public void _TelPageKeyPressed() {
            _pageRouter._ChangePath("/draims/tel");
        }

        [PublicAPI] public void _AtcPageKeyPressed() {
            _pageRouter._ChangePath("/draims/atc");
        }

        [PublicAPI] public void _MenuPageKeyPressed() {
            _pageRouter._ChangePath("/draims/menu");
        }

        [PublicAPI] public void _NavPageKeyPressed() {
            _pageRouter._ChangePath("/draims/nav");
        }

    #endregion
    }

    [PublicAPI]
    public enum DRAIMSKeyType {
        SelectionLeft1,
        SelectionLeft2,
        SelectionLeft3,
        SelectionLeft4,
        SelectionRight1,
        SelectionRight2,
        SelectionRight3,
        SelectionRight4,
        GenericLeft,
        GenericRight,
        Up,
        Down,
        Number1,
        Number2,
        Number3,
        Number4,
        Number5,
        Number6,
        Number7,
        Number8,
        Number9,
        Number0,
        Dot,
        Clear
    }
}
