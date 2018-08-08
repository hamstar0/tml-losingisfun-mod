using System;


namespace LosingIsFun {
	public class LosingIsFunConfigData {
		public readonly static Version ConfigVersion = new Version( 1, 3, 0 );
		public readonly static string ConfigFileName = "Losing Is Fun Config.json";


		////////////////

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
			var new_config = new LosingIsFunConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= LosingIsFunConfigData.ConfigVersion ) {
				return false;
			}

			this.VersionSinceUpdate = LosingIsFunConfigData.ConfigVersion.ToString();

			return true;
		}
	}
}
