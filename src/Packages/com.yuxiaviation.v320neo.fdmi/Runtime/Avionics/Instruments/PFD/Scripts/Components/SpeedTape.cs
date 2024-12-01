using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;

namespace VRChatAerospaceUniversity.V320.Avionics.Instruments.PFD.Scripts.Components
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SpeedTape : UdonSharpBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private FDMiFloat _ias;
        [SerializeField] private FDMiFloat _mach;

        private readonly int _iasAnimationHash = Animator.StringToHash("IAS");
        private readonly int _iasFailFlagHash = Animator.StringToHash("IASFailFlag");

        private void Update()
        {
            _animator.SetFloat(_iasAnimationHash, !float.IsNaN(_ias.Data) ? _ias.Data / 660f : 0f);

            _animator.SetBool(_iasFailFlagHash, float.IsNaN(_ias.Data));
        }
    }
}
