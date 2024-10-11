using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRChatAerospaceUniversity.V320
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ZoomCameraSystem : UdonSharpBehaviour {
        [SerializeField] private Camera _camera;

        private VRCPlayerApi _localPlayer;

        private bool followTrackingData;

        private void Start() {
            _localPlayer = Networking.LocalPlayer;

            if (_localPlayer.IsUserInVR()) {
                gameObject.SetActive(false);
                return;
            }

            SendCustomEventDelayedFrames(nameof(_EnableCamera), 1);
        }

        public void _EnableCamera() {
            _camera.enabled = true;
            _camera.gameObject.SetActive(true);
        }

        private void LateUpdate() {
            if (Input.GetKeyDown(KeyCode.P)) {
                followTrackingData = !followTrackingData;
            }

            var mouseWheelAxis = Input.GetAxis("Mouse ScrollWheel");

            if (mouseWheelAxis != 0) {
                _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - mouseWheelAxis * 10, 10, 100);
            }

            if (!followTrackingData) {
                return;
            }

            var headTrackingData = _localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
            _camera.transform.position = headTrackingData.position;
            _camera.transform.rotation = headTrackingData.rotation;
        }
    }
}
