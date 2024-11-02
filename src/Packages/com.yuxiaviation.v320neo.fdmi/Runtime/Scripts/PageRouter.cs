﻿using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRChatAerospaceUniversity.V320.Scripts
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    [AircraftLifecycleReceiver]
    public class PageRouter : UdonSharpBehaviour {
        [SerializeField] private string _indexPath;
        [SerializeField] private bool _isSync;

        [SerializeField] private GameObject _notFoundPage;

        [SerializeField] private string[] _paths;
        [SerializeField] private GameObject[] _pages;

        [HideInInspector] [SerializeField] [UdonSynced] [FieldChangeCallback(nameof(CurrentPath))] private string _currentPath;
        [PublicAPI] public string CurrentPath {
            get => _currentPath;
            set {
                ChangePathInternal(GetPathIndex(value));
                _currentPath = value;
            }
        }

        private VRCPlayerApi _localPlayer;

        [PublicAPI]
        public void _OnFirstInit() {
            _localPlayer = Networking.LocalPlayer;
        }

        [PublicAPI]
        public void _OnInit() {
            ChangePathInternal(GetPathIndex(_indexPath));
        }

        [PublicAPI]
        public void _ChangePath(string path) {
            var pageIndex = GetPathIndex(path);

            if (pageIndex == -1) {
                Debug.LogWarning($"Path {path} not found", this);
            }

            ChangePathInternal(pageIndex);

            TakeOwnership();
            CurrentPath = path;
            RequestSerialization();
        }

        private void ChangePathInternal(int index) {
            for (var i = 0; i < _pages.Length; i++) {
                _pages[i].SetActive(i == index);
            }

            _notFoundPage.SetActive(false);

            if (index != -1) return;

            if (_notFoundPage) {
                _notFoundPage.SetActive(true);
            }
        }

        private int GetPathIndex(string path) {
            for (var i = 0; i < _paths.Length; i++) {
                if (_paths[i] == path) {
                    return i;
                }
            }

            return -1;
        }

        private void TakeOwnership() {
            Networking.SetOwner(_localPlayer, gameObject);
        }
    }
}
