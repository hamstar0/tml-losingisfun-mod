﻿using LosingIsFun.Buffs;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Utils;
using Utils.JsonConfig;


namespace LosingIsFun {
	public class ConfigurationData {
		public string VersionSinceUpdate = "";

		public bool Enabled = true;

		public int MinimumTownNpcTileSpacing = 12;
		public float MinimumRatioTownNPCSolidBlocks = 0.5f;

		public int EvacWarpChargeDurationFrames = (int)(3f * 60f);

		public float LuckyHorseshoeFailChance = 0.35f;

		public float YoyoMoveSpeedClamp = 2f;

		public int ReaverSharkPower = 55;
		public int ReaverSharkSpeed = 17;
		public int FetidBaghnakhsDamage = 62;	// was 70
		public int DaedalusStormbowUseTime = 30;	// was 19
		public int SnowballDamage = 6;  // was 8
		public int CactusSwordDamage = 8;   // was 9
		public int AmberStaffUseTime = 24;  // was 28

		public bool EterniaCrystalAntiHoik = true;

		public int SorenessDurationSeconds = 188;
		public float SorenessLamenessPercent = 0.1f;
		public float SorenessDefenselessnessPercent = 0.1f;

		public bool RangedCritWithAimOnly = true;
		public float RangedCritAddedAimPercentChance = 0.15f;

		public int GravPotionFlipDelay = 120;
	}


	public class LosingIsFunMod : Mod {
		public readonly static Version ConfigVersion = new Version( 1, 0, 3 );
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
			} else {
				Version vers_since = this.Config.Data.VersionSinceUpdate != "" ?
					new Version( this.Config.Data.VersionSinceUpdate ) :
					new Version();

				if( vers_since < LosingIsFunMod.ConfigVersion ) {
					var new_config = new ConfigurationData();
					ErrorLogger.Log( "Losing Is Fun config updated to " + LosingIsFunMod.ConfigVersion.ToString() );

					if( vers_since < new Version( 0, 1, 1 ) ) {
						this.Config.Data.MinimumTownNpcTileSpacing = new_config.MinimumTownNpcTileSpacing;
					}
					if( vers_since < new Version( 0, 3, 1 ) ) {
						this.Config.Data.EvacWarpChargeDurationFrames = new_config.EvacWarpChargeDurationFrames;
					}
					if( vers_since < new Version( 1, 0, 2 ) ) {
						this.Config.Data.LuckyHorseshoeFailChance = new_config.LuckyHorseshoeFailChance;
					}

					this.Config.Data.VersionSinceUpdate = LosingIsFunMod.ConfigVersion.ToString();
					this.Config.SaveFile();
				}
			}

			if( !Main.dedServ ) {
				SorenessDebuff.LoadTextures( this );
			}
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int whoAmI ) {
			LosingIsFunNetProtocol.RouteReceivedPackets( this, reader );
		}

		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			if( !this.Config.Data.Enabled ) { return; }

			DebugHelper.PrintToBatch( sb );
		}
	}
}
