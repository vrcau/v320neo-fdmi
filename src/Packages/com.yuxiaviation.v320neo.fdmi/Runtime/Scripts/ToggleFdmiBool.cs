using JetBrains.Annotations;
using tech.gyoku.FDMi.core;
using UdonSharp;

namespace VRChatAerospaceUniversity.V320
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ToggleFdmiBool : UdonSharpBehaviour {
        public FDMiBool value;

        [PublicAPI]
        public void _Toggle() {
            value.Data = !value.Data;
        }
    }
}
