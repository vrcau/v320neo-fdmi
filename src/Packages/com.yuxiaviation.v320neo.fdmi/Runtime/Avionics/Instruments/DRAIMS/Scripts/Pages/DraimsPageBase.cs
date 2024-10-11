using JetBrains.Annotations;
using UdonSharp;

namespace VRChatAerospaceUniversity.V320.Avionics.Instruments.DRAIMS.Scripts.Pages {
    public abstract class DraimsPageBase : UdonSharpBehaviour {
        protected DRAIMS _draims;

        [PublicAPI]
        public virtual void _FirstInitPage(DRAIMS draims) {
            _draims = draims;
        }


        [PublicAPI]
        public virtual void _InitPage(DRAIMS draims) { }

        [PublicAPI]
        public virtual void _OnKeyPressed(DRAIMSKeyType keyType) { }
    }
}
