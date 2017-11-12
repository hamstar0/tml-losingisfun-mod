using HamstarHelpers.DebugHelpers;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun.NetProtocol {
	static class ServerPacketHandlers {
		public static void HandlePacket( LosingIsFunMod mymod, BinaryReader reader, int player_who ) {
			LosingIsFunNetProtocolTypes protocol = (LosingIsFunNetProtocolTypes)reader.ReadByte();
			//bool is_debug = (DebugHelper.DEBUGMODE & 1) > 0;

			switch( protocol ) {
			case LosingIsFunNetProtocolTypes.RequestModSettings:
				//if( is_debug ) { DebugHelper.Log( "SendRequestModSettings" ); }
				ServerPacketHandlers.ReceiveModSettingsRequestOnServer( mymod, reader, player_who );
				break;
			default:
				DebugHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}


		
		////////////////
		// Server Senders
		////////////////

		public static void SendModSettingsFromServer( LosingIsFunMod mymod, Player player ) {
			// Server only
			if( Main.netMode != 2 ) { return; }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)LosingIsFunNetProtocolTypes.ModSettings );
			packet.Write( (string)mymod.Config.SerializeMe() );

			packet.Send( (int)player.whoAmI );
		}


		
		////////////////
		// Server Receivers
		////////////////

		private static void ReceiveModSettingsRequestOnServer( LosingIsFunMod mymod, BinaryReader reader, int player_who ) {
			// Server only
			if( Main.netMode != 2 ) { return; }

			ServerPacketHandlers.SendModSettingsFromServer( mymod, Main.player[player_who] );
		}
	}
}
