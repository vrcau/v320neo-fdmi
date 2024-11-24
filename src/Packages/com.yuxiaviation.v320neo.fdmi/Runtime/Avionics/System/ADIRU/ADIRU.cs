using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;

namespace VRChatAerospaceUniversity.V320.Avionics.System.ADIRU
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ADIRU : UdonSharpBehaviour {
        [SerializeField] private ADR _adr;
        [SerializeField] private IR _ir;

        [PublicAPI]
        public bool IsAligned => _adr.IsAligned && _ir.IsAligned;
    }

    public enum ADIRUMode {
        OFF,
        NAV,
        ATT
    }
}
