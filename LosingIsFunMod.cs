using HamstarHelpers.Components.Config;
using LosingIsFun.Buffs;
using LosingIsFun.NetProtocol;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun {
	partial class LosingIsFunMod : Mod {
		public static LosingIsFunMod Instance { get; private set; }
		


		////////////////

		public JsonConfig<LosingIsFunConfigData> ConfigJson { get; private set; }
		public LosingIsFunConfigData Config { get { return this.ConfigJson.Data; } }


		////////////////

		public LosingIsFunMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			this.ConfigJson = new JsonConfig<LosingIsFunConfigData>( LosingIsFunConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new LosingIsFunConfigData() );
		}

		////////////////

		public override void Load() {
			LosingIsFunMod.Instance = this;

			this.LoadConfig();

			if( !Main.dedServ ) {
				SorenessDebuff.LoadTextures( this );
			}
		}

		private void LoadConfig() {
			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
			}

			if( this.ConfigJson.Data.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Losing Is Fun updated to " + LosingIsFunConfigData.ConfigVersion.ToString() );
				this.ConfigJson.SaveFile();
			}
		}

		public override void Unload() {
			LosingIsFunMod.Instance = null;
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int player_who ) {
			if( Main.netMode == 1 ) {   // Client
				ClientPacketHandlers.HandlePacket( this, reader );
			} else if( Main.netMode == 2 ) {    // Server
				ServerPacketHandlers.HandlePacket( this, reader, player_who );
			}
		}
	}
}
