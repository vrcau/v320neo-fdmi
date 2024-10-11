using UdonSharp;
using UnityEngine;

namespace VRChatAerospaceUniversity.V320
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SendEventWhenObjectDisable : UdonSharpBehaviour
    {
        [SerializeField] private UdonSharpBehaviour _receiver;
        [SerializeField] private string _eventName;

        [SerializeField] private bool _isDelay;
        [SerializeField] private int _delayFrames;

        private void OnDisable()
        {
            if (_isDelay)
            {
                _receiver.SendCustomEventDelayedFrames(_eventName, _delayFrames);
                return;
            }

            _receiver.SendCustomEvent(_eventName);
        }
    }
}
