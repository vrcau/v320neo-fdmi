using System;
using UdonSharp;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRChatAerospaceUniversity.V320.Avionics.Instruments.DisplayUnit
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class DisplayUnit : UdonSharpBehaviour {
        [SerializeField] private GameObject _selfTestPage;
        [SerializeField] private GameObject _invalidDisplayUnitPage;
        [SerializeField] private GameObject _invalidDataPage;

        [SerializeField] private GameObject _flashPage;

        [SerializeField] private GameObject _displayUnitContentContainer;
        [SerializeField] private GameObject _displayUnitRootCanvas;

        [SerializeField] [UdonSynced] private bool _isSelfTestCompleted;

        private float _lastLostPowerTime = -1;

        private void Start() {
            ResetUI();
        }

        public void _OnPower() {
            if (_isSelfTestCompleted && Time.time - _lastLostPowerTime < 5f) {
                ResetUI();

                _displayUnitContentContainer.SetActive(true);
                return;
            }

            BeginSelfTest();
        }

        public void _OnLostPower() {
            _lastLostPowerTime = Time.time;

            ResetUI(false);
        }

        private void ResetUI(bool enableCanvasRoot = true) {
            _selfTestPage.SetActive(false);
            _invalidDisplayUnitPage.SetActive(false);
            _invalidDataPage.SetActive(false);
            _flashPage.SetActive(false);
            _displayUnitContentContainer.SetActive(false);

            _displayUnitRootCanvas.SetActive(enableCanvasRoot);
        }

        public void SetContent(RectTransform contentTransform) {
            foreach (Transform childTransform in _displayUnitContentContainer.transform) {
                childTransform.gameObject.SetActive(false);
            }

            contentTransform.SetParent(_displayUnitContentContainer.transform, false);

            contentTransform.pivot = new Vector2(0f, 0f);
            contentTransform.gameObject.SetActive(true);
        }

    #region SelfTestSequence

        private void BeginSelfTest() {
            ResetUI();

            _isSelfTestCompleted = false;

            var delay = Random.Range(1f, 2f);
            SendCustomEventDelayedSeconds(nameof(_BeginFlash), delay);
        }

        public void _BeginFlash() {
            _flashPage.SetActive(true);
            SendCustomEventDelayedSeconds(nameof(_FinishFlash), 0.2f);
        }

        public void _FinishFlash() {
            _flashPage.SetActive(false);

            var delay = Random.Range(0.2f, 2f);
            SendCustomEventDelayedSeconds(nameof(_ShowSelfTestPage), delay);
        }

        public void _ShowSelfTestPage() {
            _selfTestPage.SetActive(true);

            var delay = Random.Range(10f, 40f);
            SendCustomEventDelayedSeconds(nameof(_FinishSelfTest), delay);
        }

        public void _FinishSelfTest() {
            _selfTestPage.SetActive(false);
            _displayUnitContentContainer.SetActive(true);

            _isSelfTestCompleted = true;
        }

    #endregion
    }
}
