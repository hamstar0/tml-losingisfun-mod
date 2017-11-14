using HamstarHelpers.Utilities.Config;
using LosingIsFun.Buffs;
using LosingIsFun.NetProtocol;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun {
	class LosingIsFunMod : Mod {
		public static LosingIsFunMod Instance { get; private set; }

		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-losingisfun-mod"; } }

		public static string ConfigRelativeFilePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + LosingIsFunConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( LosingIsFunMod.Instance != null ) {
				LosingIsFunMod.Instance.Config.LoadFile();
			}
		}


		////////////////

		public JsonConfig<LosingIsFunConfigData> Config { get; private set; }


		////////////////

		public LosingIsFunMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			this.Config = new JsonConfig<LosingIsFunConfigData>( LosingIsFunConfigData.ConfigFileName,
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
			if( !this.Config.LoadFile() ) {
				this.Config.SaveFile();
			}

			if( this.Config.Data.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Losing Is Fun updated to " + LosingIsFunConfigData.CurrentVersion.ToString() );
				this.Config.SaveFile();
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
