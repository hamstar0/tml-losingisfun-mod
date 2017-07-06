using System;


namespace LosingIsFun {
	public class ConfigurationData {
		public readonly static Version CurrentVersion = new Version( 1, 1, 0 );


		public string VersionSinceUpdate = "";

		public bool Enabled = true;

		public int MinimumTownNpcTileSpacing = 12;
		public float MinimumRatioTownNPCSolidBlocks = 0.5f;

		public int EvacWarpChargeDurationFrames = (int)(3f * 60f);

		public float LuckyHorseshoeFailChance = 0.35f;

		public float YoyoMoveSpeedClamp = 2f;

		public int ReaverSharkPower = 55;
		public int ReaverSharkSpeed = 17;
		public int FetidBaghnakhsDamage = 62;   // was 70
		public int DaedalusStormbowUseTime = 30;    // was 19
		public int SnowballDamage = 6;  // was 8
		public int CactusSwordDamage = 8;   // was 9
		public int AmberStaffUseTime = 24;  // was 28
		public int ChlorophyteBulletDamage = 2; // was 10
		public int MusketBallValue = 18; // was 7
		public int EmptyBulletValue = 15; // was 3
		public int LeafWingsTime = 20; // was 160

		public bool EterniaCrystalAntiHoik = true;

		public int SorenessDurationSeconds = 188;
		public float SorenessLamenessPercent = 0.1f;
		public float SorenessDefenselessnessPercent = 0.1f;

		public bool RangedCritWithAimOnly = true;
		public float RangedCritAddedAimPercentChance = 0.15f;

		public int GravPotionFlipDelay = 120;

		public int MountMaxHp = 5;
		public int MountHpRegenRate = 60 * 5;
		public int MountEjectDebuffTime = 60 * 12;

		public bool NoRespawnDuringBosses = false;	// Might be buggy!

		public float CrimsonMobsWormToothDropChance = 0.04f;



		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new ConfigurationData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= ConfigurationData.CurrentVersion ) {
				return false;
			}

			if( vers_since < new Version( 0, 1, 1 ) ) {
				if( ConfigurationData._0_1_0_MinimumTownNpcTileSpacing == this.MinimumTownNpcTileSpacing ) {
					this.MinimumTownNpcTileSpacing = new_config.MinimumTownNpcTileSpacing;
				}
			}
			if( vers_since < new Version( 0, 3, 1 ) ) {
				if( ConfigurationData._0_3_0_EvacWarpChargeDurationFrames == this.EvacWarpChargeDurationFrames ) {
					this.EvacWarpChargeDurationFrames = new_config.EvacWarpChargeDurationFrames;
				}
			}
			if( vers_since < new Version( 1, 0, 2 ) ) {
				if( ConfigurationData._1_0_1_LuckyHorseshoeFailChance == this.LuckyHorseshoeFailChance ) {
					this.LuckyHorseshoeFailChance = new_config.LuckyHorseshoeFailChance;
				}
			}

			this.VersionSinceUpdate = ConfigurationData.CurrentVersion.ToString();

			return true;
		}


		////////////////

		public readonly static float _1_0_1_LuckyHorseshoeFailChance = 0.35f;
		public readonly static int _0_3_0_EvacWarpChargeDurationFrames = (int)(3.5f * 60f);
		public readonly static int _0_1_0_MinimumTownNpcTileSpacing = 10;
	}
}
