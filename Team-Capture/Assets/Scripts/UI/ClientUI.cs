﻿using Player;
using UnityEngine;

namespace UI
{
	public class ClientUI : MonoBehaviour
	{
		[HideInInspector] public PlayerManager player;

		public Hud hud;

		public GameObject pauseMenuGameObject;

		public static bool IsPauseMenuOpen;

		public ClientUI SetupUi(PlayerManager playerManager)
		{
			IsPauseMenuOpen = false;

			player = playerManager;

			hud.clientUi = this;

			pauseMenuGameObject.SetActive(false);

			return this;
		}

		public void TogglePauseMenu()
		{
			IsPauseMenuOpen = !IsPauseMenuOpen;

			Cursor.visible = IsPauseMenuOpen;
			Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;

			pauseMenuGameObject.SetActive(IsPauseMenuOpen);
			hud.gameObject.SetActive(!IsPauseMenuOpen);
		}
	}
}