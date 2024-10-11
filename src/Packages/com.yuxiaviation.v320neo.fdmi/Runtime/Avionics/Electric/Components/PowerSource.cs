using UdonSharp;

namespace VRChatAerospaceUniversity.V320.Avionics.Electric.Components {
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PowerSource : UdonSharpBehaviour {
        public ElectricalAppliance consumer;

        public float powerLevel;

        private void Update() {
            consumer.Power(powerLevel);
        }
    }
}
