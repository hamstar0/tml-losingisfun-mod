using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace LosingIsFun {
	public class LosingIsFunConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		[DefaultValue( true )]
		public bool Enabled = true;


		[DefaultValue( 12 )]
		public int MinimumTownNpcTileSpacing = 12;

		[DefaultValue( 0.5f )]
		public float MinimumRatioTownNPCSolidBlocks = 0.5f;


		[DefaultValue( (int)( 3f * 60f ) )]
		public int EvacWarpChargeDurationFrames = (int)(3f * 60f);


		[DefaultValue( 0.35f )]
		public float LuckyHorseshoeFailChance = 0.35f;


		[DefaultValue( 2f )]
		public float YoyoMoveSpeedClamp = 2f;


		[DefaultValue( 55 )]
		public int ReaverSharkPower = 55;

		[DefaultValue( 17 )]
		public int ReaverSharkSpeed = 17;

		[DefaultValue( 62 )]
		public int FetidBaghnakhsDamage = 62;   // was 70

		[DefaultValue( 30 )]
		public int DaedalusStormbowUseTime = 30;    // was 19

		[DefaultValue( 6 )]
		public int SnowballDamage = 6;  // was 8

		[DefaultValue( 8 )]
		public int CactusSwordDamage = 8;   // was 9

		[DefaultValue( 24 )]
		public int AmberStaffUseTime = 24;  // was 28

		[DefaultValue( 2 )]
		public int ChlorophyteBulletDamage = 2; // was 10

		[DefaultValue( 18 )]
		public int MusketBallValue = 18; // was 7

		[DefaultValue( 15 )]
		public int EmptyBulletValue = 15; // was 3

		[DefaultValue( 20 )]
		public int LeafWingsTime = 20; // was 160

		[DefaultValue( 46 )]
		public int BeesKneesUseTime = 46;   // was 23


		[DefaultValue( true )]
		public bool EterniaCrystalAntiHoik = true;


		[DefaultValue( 188 )]
		public int SorenessDurationSeconds = 188;

		[DefaultValue( 0.1f )]
		public float SorenessLamenessPercent = 0.1f;

		[DefaultValue( 0.1f )]
		public float SorenessDefenselessnessPercent = 0.1f;


		[DefaultValue( true )]
		public bool RangedCritWithAimOnly = true;

		[DefaultValue( 0.15f )]
		public float RangedCritAddedAimPercentChance = 0.15f;


		[DefaultValue( 120 )]
		public int GravPotionFlipDelay = 120;


		[DefaultValue( 5 )]
		public int MountMaxHp = 5;

		[DefaultValue( 60 * 5 )]
		public int MountHpRegenRate = 60 * 5;

		[DefaultValue( 60 * 12 )]
		public int MountEjectDebuffTime = 60 * 12;


		public bool NoRespawnDuringBosses = false;  // Might be buggy!


		[DefaultValue( 0.04f )]
		public float CrimsonMobsWormToothDropChance = 0.04f;
	}
}
