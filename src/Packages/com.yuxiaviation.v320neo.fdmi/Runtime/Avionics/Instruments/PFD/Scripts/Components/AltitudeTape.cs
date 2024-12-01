using System;
using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;

namespace VRChatAerospaceUniversity.V320.Avionics.Instruments.PFD.Scripts.Components
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class AltitudeTape : UdonSharpBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private FDMiFloat _altitude;

        private readonly int FlightLevelAltitude1stAnimationHash = Animator
            .StringToHash("FlightLevelAltitude1st");
        private readonly int FlightLevelAltitude2ndAnimationHash = Animator
            .StringToHash("FlightLevelAltitude2nd");
        private readonly int FlightLevelAltitude3rdAnimationHash = Animator
            .StringToHash("FlightLevelAltitude3rd");
        private readonly int SecondAltitudeAnimationHash = Animator.StringToHash("SecondAltitude");

        private readonly int AltitudeFailFlagHash = Animator.StringToHash("AltitudeFailFlag");

        private void Update() {
            if (float.IsNaN(_altitude.Data)) {
                _animator.SetBool(AltitudeFailFlagHash, true);
                return;
            }

            var altitude = _altitude.Data * 3.2808399f;

            _animator.SetBool(AltitudeFailFlagHash, false);
            _animator.SetFloat(FlightLevelAltitude1stAnimationHash, altitude / 10000f % 10f / 10f);
            _animator.SetFloat(FlightLevelAltitude2ndAnimationHash, altitude / 1000f % 10f / 10f);
            _animator.SetFloat(FlightLevelAltitude3rdAnimationHash, altitude / 100f % 10f / 10f);
            _animator.SetFloat(SecondAltitudeAnimationHash, altitude % 100f / 100f);
        }
    }
}
