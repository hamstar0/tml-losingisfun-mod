using HamstarHelpers.Classes.UI.ModConfig;
using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace LosingIsFun {
	public class LosingIsFunConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		[DefaultValue( true )]
		public bool Enabled = true;


		[Range( 0, 10000 )]
		[DefaultValue( 12 )]
		public int MinimumTownNpcTileSpacing = 12;

		[Label( "Min. req. ratio of solid ground under town NPC" )]
		[Range( 0f, 1f )]
		[DefaultValue( 0.5f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float MinimumRatioTownNPCSolidBlocks = 0.5f;


		[Range( 0f, 60f * 60f )]
		[DefaultValue( (int)( 3f * 60f ) )]
		public int EvacWarpChargeDurationFrames = (int)(3f * 60f);


		[Range( 0f, 1f )]
		[DefaultValue( 0.35f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float LuckyHorseshoeFailChance = 0.35f;


		[Range( 0f, 20f )]
		[DefaultValue( 2f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float YoyoMoveSpeedClamp = 2f;


		[Range( 0, 1000 )]
		[DefaultValue( 55 )]
		public int ReaverSharkPower = 55;

		[Range( 0, 1000 )]
		[DefaultValue( 17 )]
		public int ReaverSharkSpeed = 17;

		[Range( 0, 1000 )]
		[DefaultValue( 62 )]
		public int FetidBaghnakhsDamage = 62;   // was 70

		[Range( 0, 1000 )]
		[DefaultValue( 30 )]
		public int DaedalusStormbowUseTime = 30;    // was 19

		[Range( 0, 1000 )]
		[DefaultValue( 6 )]
		public int SnowballDamage = 6;  // was 8

		[Range( 0, 1000 )]
		[DefaultValue( 8 )]
		public int CactusSwordDamage = 8;   // was 9

		[Range( 0, 1000 )]
		[DefaultValue( 24 )]
		public int AmberStaffUseTime = 24;  // was 28

		[Range( 0, 1000 )]
		[DefaultValue( 2 )]
		public int ChlorophyteBulletDamage = 2; // was 10

		[Range( 0, 1000 )]
		[DefaultValue( 18 )]
		public int MusketBallValue = 18; // was 7

		[Range( 0, 1000 )]
		[DefaultValue( 15 )]
		public int EmptyBulletValue = 15; // was 3

		[Range( 0, 1000 )]
		[DefaultValue( 20 )]
		public int LeafWingsTime = 20; // was 160

		[Range( 0, 1000 )]
		[DefaultValue( 46 )]
		public int BeesKneesUseTime = 46;   // was 23


		[DefaultValue( true )]
		public bool EterniaCrystalAntiHoik = true;


		[Range( 0, 10000 )]
		[DefaultValue( 188 )]
		public int SorenessDurationSeconds = 188;

		[Range( 0f, 1f )]
		[DefaultValue( 0.1f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float SorenessLamenessPercent = 0.1f;

		[Range( 0f, 1f )]
		[DefaultValue( 0.1f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float SorenessDefenselessnessPercent = 0.1f;


		[DefaultValue( true )]
		public bool RangedCritWithAimOnly = true;

		[Range( 0f, 1f )]
		[DefaultValue( 0.15f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float RangedCritAddedAimPercentChance = 0.15f;


		[Range( 0, 10000 )]
		[DefaultValue( 120 )]
		public int GravPotionFlipDelay = 120;


		[Range( 0, 100 )]
		[DefaultValue( 5 )]
		public int MountMaxHp = 5;

		[Range( 0, 1000 )]
		[DefaultValue( 60 * 5 )]
		public int MountHpRegenRate = 60 * 5;

		[Range( 0, 60 * 60 * 60 )]
		[DefaultValue( 60 * 12 )]
		public int MountEjectDebuffTime = 60 * 12;


		public bool NoRespawnDuringBosses = false;  // Might be buggy!


		[Range( 0f, 1f )]
		[DefaultValue( 0.04f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float CrimsonMobsWormToothDropChance = 0.04f;
	}
}
