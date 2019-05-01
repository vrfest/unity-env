//========= Copyright 2016-2019, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.Pointer3D;
using HTC.UnityPlugin.VRModuleManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
#if VIU_STEAMVR_2_0_0_OR_NEWER
using Valve.VR;
#endif

namespace HTC.UnityPlugin.Vive {
    [AddComponentMenu("HTC/VIU/Teleportable", 3)]
    public class Menuable : MonoBehaviour, ReticlePoser.IMaterialChanger
        , IPointer3DPressExitHandler {
        public enum TeleportButton {
            Trigger,
            Pad,
            Grip,
        }

        public string username;
        public List<string> inventory;
        UserInterface UIScript;
        GameObject PlayerInteractionCanvas;

        public float fadeDuration = 0.3f;
        [SerializeField]
        private Material m_reticleMaterial;

        public TeleportButton teleportButton = TeleportButton.Pad;

        private Coroutine teleportCoroutine;

        public Material reticleMaterial { get { return m_reticleMaterial; } set { m_reticleMaterial = value; } }

        void Start() {
            inventory = new List<string>();
            UIScript = GameObject.Find("UIScript").GetComponent<UserInterface>();
            PlayerInteractionCanvas = GameObject.Find("PlayerInteractionCanvas");
        }

        public void OnPointer3DPressExit(Pointer3DEventData eventData) {
            // skip if it was teleporting
            if (teleportCoroutine != null) { return; }

            // skip if it was not releasing the button
            if (eventData.GetPress()) { return; }

            // check if is teleport button
            VivePointerEventData viveEventData;
            if (eventData.TryGetViveButtonEventData(out viveEventData)) {
                switch (teleportButton) {
                    case TeleportButton.Trigger: if (viveEventData.viveButton != ControllerButton.Trigger) { return; } break;
                    case TeleportButton.Pad: if (viveEventData.viveButton != ControllerButton.Pad) { return; } break;
                    case TeleportButton.Grip: if (viveEventData.viveButton != ControllerButton.Grip) { return; } break;
                }
            }
            else if (eventData.button != (PointerEventData.InputButton)teleportButton) {
                switch (teleportButton) {
                    case TeleportButton.Trigger: if (eventData.button != PointerEventData.InputButton.Left) { return; } break;
                    case TeleportButton.Pad: if (eventData.button != PointerEventData.InputButton.Right) { return; } break;
                    case TeleportButton.Grip: if (eventData.button != PointerEventData.InputButton.Middle) { return; } break;
                }
            }

            var hitResult = eventData.pointerCurrentRaycast;

            // check if hit something
            if (!hitResult.isValid) { return; }

            if (VRModule.activeModule != VRModuleActiveEnum.SteamVR && fadeDuration != 0f) {
                Debug.LogWarning("Install SteamVR plugin and enable SteamVRModule support to enable fading");
                fadeDuration = 0f;
            }
            openMenu();
        }

        private void openMenu() {
            UIScript.currentUsername = username;
            PlayerInteractionCanvas.transform.Find("UsernameText").gameObject.GetComponent<UnityEngine.UI.Text>().text = username;
            PlayerInteractionCanvas.GetComponent<Canvas>().enabled = true;
            // Move UI to view
        }
    }
}