﻿using Mirror;

namespace Core.Networking.Messages
{
	public struct InitialClientJoinMessage : NetworkMessage
	{
		//TODO: We should move this to an authenticator
		public ServerConfig ServerConfig;
	}
}