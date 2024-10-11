using UdonSharp;

namespace VRChatAerospaceUniversity.V320.Avionics.Electric.Components
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ElectricalConnector : ElectricalAppliance
    {
        public ElectricalAppliance consumer;

        public override void Power(float providePowerLevel) {
            base.Power(providePowerLevel);

            if (!enabled || !gameObject.activeInHierarchy)
                return;

            consumer.Power(providePowerLevel);
        }
    }
}
