using UdonSharp;

namespace VRChatAerospaceUniversity.V320.Avionics.Electric.Components {
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ElectricalBus : ElectricalAppliance {
        public ElectricalAppliance[] consumers;

        public override void Power(float providePowerLevel) {
            base.Power(providePowerLevel);

            if (!enabled || !gameObject.activeInHierarchy)
                return;

            foreach (var consumer in consumers) {
                consumer.Power(providePowerLevel);
            }
        }
    }
}
