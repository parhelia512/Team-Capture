﻿using Core;
using Mirror;
using UI;
using UnityEngine;
using Logger = Core.Logger.Logger;

namespace Player
{
	public class PlayerSetup : NetworkBehaviour
	{
		[SerializeField] private AudioListener localAudioListener;
		[SerializeField] private Camera localCamera;

		[Header("Components to Destroy")] [SerializeField]
		private CapsuleCollider localCapsuleCollider;

		[Header("Components to Enable")] [SerializeField]
		private CharacterController localCharacterController;

		[SerializeField] private PlayerInput localPlayerInput;
		[SerializeField] private PlayerMovement localPlayerMovement;

		[Header("Player UI")] [SerializeField] private GameObject clientUiPrefab;

		public override void OnStartLocalPlayer()
		{
			Logger.Log("Setting up player!");

			base.OnStartLocalPlayer();

			Destroy(localCapsuleCollider);

			localCharacterController.enabled = true;
			localPlayerMovement.enabled = true;

			GameManager.GetActiveSceneCamera().SetActive(false);

			localCamera.enabled = true;
			localAudioListener.enabled = true;
			localPlayerInput.enabled = true;

			//Setup UI
			ClientUI clientUi = Instantiate(clientUiPrefab).GetComponent<ClientUI>();
			GetComponent<PlayerManager>().clientUi = clientUi;
			clientUi.SetupUi(GetComponent<PlayerManager>());

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			Logger.Log("I am now ready! :)");
		}

		public override void OnStartClient()
		{
			GameManager.AddPlayer(GetComponent<NetworkIdentity>().netId.ToString(), GetComponent<PlayerManager>());

			base.OnStartClient();
		}

		private void OnDisable()
		{
			GameManager.GetActiveSceneCamera().SetActive(true);
			GameManager.RemovePlayer(transform.name);

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		public Camera GetPlayerCamera()
		{
			return localCamera;
		}
	}
}