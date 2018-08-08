using HamstarHelpers.Helpers.DebugHelpers;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun.NetProtocol {
	static class ClientPacketHandlers {
		public static void HandlePacket( LosingIsFunMod mymod, BinaryReader reader ) {
			LosingIsFunNetProtocolTypes protocol = (LosingIsFunNetProtocolTypes)reader.ReadByte();
			//bool is_debug = (DebugHelper.DEBUGMODE & 1) > 0;

			switch( protocol ) {
			case LosingIsFunNetProtocolTypes.ModSettings:
				//if( is_debug ) { DebugHelper.Log( "SendModSettings" ); }
				ClientPacketHandlers.ReceiveModSettingsOnClient( mymod, reader );
				break;
			default:
				LogHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////
		// Client Senders
		////////////////

		public static void SendModSettingsRequestFromClient( Mod mod ) {
			// Clients only
			if( Main.netMode != 1 ) { return; }

			ModPacket packet = mod.GetPacket();

			packet.Write( (byte)LosingIsFunNetProtocolTypes.RequestModSettings );

			packet.Send();
		}



		////////////////
		// Client Receivers
		////////////////

		private static void ReceiveModSettingsOnClient( LosingIsFunMod mymod, BinaryReader reader ) {
			// Clients only
			if( Main.netMode != 1 ) { return; }

			bool success;

			mymod.ConfigJson.DeserializeMe( reader.ReadString(), out success );
		}
	}
}
