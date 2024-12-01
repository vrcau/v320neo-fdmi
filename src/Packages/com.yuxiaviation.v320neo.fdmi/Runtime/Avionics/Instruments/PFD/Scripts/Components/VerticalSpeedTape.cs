using System;
using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace VRChatAerospaceUniversity.V320.Avionics.Instruments.PFD.Scripts.Components
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class VerticalSpeedTape : UdonSharpBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private Text _verticalSpeedText;

        [SerializeField] private FDMiFloat _verticalSpeed;

        private readonly int _verticalSpeedAnimationHash = Animator.StringToHash("VerticalSpeed");
        private readonly int _verticalSpeedFailFlagHash = Animator.StringToHash("VerticalSpeedFailFlag");

        private void Update() {
            var verticalSpeedFtPerMin = _verticalSpeed.Data * 60f * 3.2808399f;

            _animator.SetFloat(_verticalSpeedAnimationHash, !float.IsNaN(_verticalSpeed.Data) ? Mathf.Clamp01(_verticalSpeed.Data / 6000f + 0.5f) : 0f);
            _animator.SetBool(_verticalSpeedFailFlagHash, float.IsNaN(_verticalSpeed.Data));

            _verticalSpeedText.text = Mathf.Abs(verticalSpeedFtPerMin / 100f).ToString("00");
        }
    }
}
