using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;

namespace VRChatAerospaceUniversity.V320.Avionics.Instruments.PFD.Scripts.Components
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class HeadingTape : UdonSharpBehaviour {
        [SerializeField] private FDMiFloat _heading;

        [SerializeField] private Animator _animator;

        private readonly int _headingAnimationHash = Animator.StringToHash("Heading");
        private readonly int _headingFailFlagHash = Animator.StringToHash("HeadingFailFlag");

        private void Update() {
            _animator.SetFloat(_headingAnimationHash, !float.IsNaN(_heading.Data) ? _heading.Data / 360f : 0f);
            _animator.SetBool(_headingFailFlagHash, float.IsNaN(_heading.Data));
        }
    }
}
