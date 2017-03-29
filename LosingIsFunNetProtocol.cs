using System.IO;
using Terraria;
using Terraria.ModLoader;
using Utils;

namespace LosingIsFun {
	public enum TheLunaticNetProtocolTypes : byte {
		SendRequestModSettings,
		SendModSettings
	}


	public class LosingIsFunNetProtocol {
		public static void RouteReceivedPackets( LosingIsFunMod mymod, BinaryReader reader ) {
			TheLunaticNetProtocolTypes protocol = (TheLunaticNetProtocolTypes)reader.ReadByte();
			bool is_debug = (Debug.DEBUGMODE & 1) > 0;

			switch( protocol ) {
			case TheLunaticNetProtocolTypes.SendRequestModSettings:
				if( is_debug ) { Debug.Log( "SendRequestModSettings" ); }
				LosingIsFunNetProtocol.ReceiveModSettingsRequestOnServer( mymod, reader );
				break;
			case TheLunaticNetProtocolTypes.SendModSettings:
				if( is_debug ) { Debug.Log( "SendModSettings" ); }
				LosingIsFunNetProtocol.ReceiveModSettingsOnClient( mymod, reader );
				break;
			default:
				Debug.Log( "Invalid packet protocol: " + protocol );
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

			packet.Write( (byte)TheLunaticNetProtocolTypes.SendRequestModSettings );
			packet.Write( (int)Main.myPlayer );

			packet.Send();
		}


		////////////////
		// Server Senders
		////////////////

		public static void SendModSettingsFromServer( LosingIsFunMod mymod, Player player ) {
			// Server only
			if( Main.netMode != 2 ) { return; }
			
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TheLunaticNetProtocolTypes.SendModSettings );
			packet.Write( (string)mymod.Config.SerializeMe() );
			
			packet.Send( (int)player.whoAmI );
		}



		////////////////
		// Client Receivers
		////////////////

		private static void ReceiveModSettingsOnClient( LosingIsFunMod mymod, BinaryReader reader ) {
			// Clients only
			if( Main.netMode != 1 ) { return; }
			
			mymod.Config.DeserializeMe( reader.ReadString() );
		}
		


		////////////////
		// Server Receivers
		////////////////

		private static void ReceiveModSettingsRequestOnServer( LosingIsFunMod mymod, BinaryReader reader ) {
			// Server only
			if( Main.netMode != 2 ) { return; }

			int player_who = reader.ReadInt32();

			if( player_who < 0 || player_who >= Main.player.Length || Main.player[player_who] == null ) {
				Debug.Log( "LosingIsFunNetProtocol.ReceiveRequestModSettingsOnServer - Invalid player id " + player_who );
				return;
			}

			LosingIsFunNetProtocol.SendModSettingsFromServer( mymod, Main.player[player_who] );
		}
	}
}
