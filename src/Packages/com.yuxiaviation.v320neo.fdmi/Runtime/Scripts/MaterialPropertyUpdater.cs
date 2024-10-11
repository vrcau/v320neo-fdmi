using tech.gyoku.FDMi.core;
using UdonSharp;
using UnityEngine;

namespace VRChatAerospaceUniversity.V320
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class MaterialPropertyUpdater : UdonSharpBehaviour
    {
        public FDMiFloat propertyValue;

        public string propertyName;

        public Material material;
    }
}
