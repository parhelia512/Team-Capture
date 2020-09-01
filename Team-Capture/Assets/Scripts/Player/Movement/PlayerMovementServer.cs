using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Movement
{
	//This code is built on unity-fastpacedmultiplayer
	//https://github.com/JoaoBorks/unity-fastpacedmultiplayer
	//
	//MIT License
	//Copyright (c) 2015 ultimatematchthree, 2017 Joao Borks [joao.borks@gmail.com]

	/// <summary>
	/// Handles processing inputs and performing movements on the server
	/// </summary>
	public class PlayerMovementServer : MonoBehaviour
	{
		private Queue<PlayerInput> inputBuffer;
		private PlayerMovementManager character;
		private int serverTick;

		private PlayerInput lastInput;

		private void Awake()
		{
			inputBuffer = new Queue<PlayerInput>();
			character = GetComponent<PlayerMovementManager>();
			character.state = PlayerState.Zero;
		}

		private void FixedUpdate()
		{
			serverTick++;

			PlayerState state = character.state;

			if (inputBuffer.Count != 0)
				lastInput = inputBuffer.Dequeue();

			state = character.Move(state, lastInput, serverTick);
			character.SyncState(state);

			state.Position = transform.position;
			character.state = state;
			character.OnServerStateChange(state, state);
		}

		private void OnDisable()
		{
			inputBuffer.Clear();
		}

		/// <summary>
		/// Adds all the inputs and moves
		/// </summary>
		/// <param name="inputs"></param>
		public void AddInputs(PlayerInput[] inputs)
		{
			foreach (PlayerInput input in inputs)
				inputBuffer.Enqueue(input);
		}
	}
}