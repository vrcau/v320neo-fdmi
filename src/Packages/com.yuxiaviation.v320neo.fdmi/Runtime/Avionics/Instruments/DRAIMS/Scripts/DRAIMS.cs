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

        [PublicAPI] public void _OnLeft1Pressed() { }
        [PublicAPI] public void _OnLeft2Pressed() { }
        [PublicAPI] public void _OnLeft3Pressed() { }
        [PublicAPI] public void _OnLeft4Pressed() { }
        [PublicAPI] public void _OnRight1Pressed() { }
        [PublicAPI] public void _OnRight2Pressed() { }
        [PublicAPI] public void _OnRight3Pressed() { }
        [PublicAPI] public void _OnRight4Pressed() { }

    #endregion

    #region Keyboard

        [PublicAPI] public void _OnNumber1Pressed() { }
        [PublicAPI] public void _OnNumber2Pressed() { }
        [PublicAPI] public void _OnNumber3Pressed() { }
        [PublicAPI] public void _OnNumber4Pressed() { }
        [PublicAPI] public void _OnNumber5Pressed() { }
        [PublicAPI] public void _OnNumber6Pressed() { }
        [PublicAPI] public void _OnNumber7Pressed() { }
        [PublicAPI] public void _OnNumber8Pressed() { }
        [PublicAPI] public void _OnNumber9Pressed() { }
        [PublicAPI] public void _OnNumber0Pressed() { }

        [PublicAPI] public void _OnDotPressed() { }
        [PublicAPI] public void _OnClearPressed() { }

    #endregion

        [PublicAPI] public void _OnLeftGenericKeyPressed() { }
        [PublicAPI] public void _OnRightGenericKeyPressed() { }
        [PublicAPI] public void _OnUpKeyPressed() { }
        [PublicAPI] public void _OnDownKeyPressed() { }

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
