using HamstarHelpers.Utilities.Config;
using LosingIsFun.Buffs;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun {
	public class LosingIsFunMod : Mod {
		public JsonConfig<ConfigurationData> Config { get; private set; }


		public LosingIsFunMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			string filename = "Losing Is Fun Config.json";
			this.Config = new JsonConfig<ConfigurationData>( filename, "Mod Configs", new ConfigurationData() );
		}

		public override void Load() {
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
				ErrorLogger.Log( "Losing Is Fun updated to " + ConfigurationData.CurrentVersion.ToString() );
				this.Config.SaveFile();
			}
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int whoAmI ) {
			LosingIsFunNetProtocol.RouteReceivedPackets( this, reader );
		}
	}
}
