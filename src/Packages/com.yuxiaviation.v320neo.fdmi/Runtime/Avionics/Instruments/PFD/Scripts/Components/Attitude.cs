using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;

namespace VRChatAerospaceUniversity.V320.Avionics.Instruments.PFD.Scripts.Components {
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Attitude : UdonSharpBehaviour {
        [SerializeField] private Animator _animator;

        [SerializeField] private FDMiFloat _pitch, _bank;

        private readonly int _pitchAnimationHash = Animator.StringToHash("Pitch");
        private readonly int _bankAnimationHash = Animator.StringToHash("Bank");
        private readonly int _attitudeFailFlagHash = Animator.StringToHash("AttitudeFailFlag");

        private void Update() {
            _animator.SetFloat(_pitchAnimationHash, !float.IsNaN(_pitch.Data) ? _pitch.Data / 180f + 0.5f : 0f);
            _animator.SetFloat(_bankAnimationHash, !float.IsNaN(_bank.Data) ? _bank.Data / 180f + 0.5f : 0f);
            _animator.SetBool(_attitudeFailFlagHash, float.IsNaN(_bank.Data) || float.IsNaN(_pitch.Data));
        }
    }
}
