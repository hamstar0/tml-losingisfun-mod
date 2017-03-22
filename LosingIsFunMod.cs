using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria.ModLoader;
using Utils;
using Utils.JsonConfig;


namespace LosingIsFun {
	public class ConfigurationData {
		public string VersionSinceUpdate = "";
		public int EvacWarpChargeDurationFrames = 5 * 60;
		public int MinimumTownNpcTileSpacing = 15;
	}


	public class LosingIsFunMod : Mod {
		public readonly static Version ConfigVersion = new Version( 1, 0, 0 );
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
			if( !this.Config.LoadFile() ) {
				this.Config.SaveFile();
			}

			Version vers_since = this.Config.Data.VersionSinceUpdate != "" ?
				new Version( this.Config.Data.VersionSinceUpdate ) :
				new Version();

			if( vers_since < LosingIsFunMod.ConfigVersion ) {
				ErrorLogger.Log( "Losing Is Fun config updated to " + LosingIsFunMod.ConfigVersion.ToString() );
				
				this.Config.Data.VersionSinceUpdate = LosingIsFunMod.ConfigVersion.ToString();
				this.Config.SaveFile();
			}
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int whoAmI ) {
			LosingIsFunNetProtocol.RouteReceivedPackets( this, reader );
		}

		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			Debug.PrintToBatch( sb );
		}
	}
}
