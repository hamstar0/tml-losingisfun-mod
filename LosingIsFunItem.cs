using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace LosingIsFun {
	class LosingIsFunItem : GlobalItem {
		public override void SetDefaults( Item item ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.ConfigJson.Data.Enabled ) { return; }

			switch( item.type ) {
			case ItemID.Snowball:
				if( mymod.ConfigJson.Data.SnowballDamage >= 0 ) {
					item.damage = mymod.ConfigJson.Data.SnowballDamage;    // was 8
				}
				break;
			case ItemID.CactusSword:
				if( mymod.ConfigJson.Data.CactusSwordDamage >= 0 ) {
					item.damage = mymod.ConfigJson.Data.CactusSwordDamage;    // was 9
				}
				break;
			case ItemID.DaedalusStormbow:
				if( mymod.ConfigJson.Data.DaedalusStormbowUseTime >= 0 ) {
					item.useAnimation = mymod.ConfigJson.Data.DaedalusStormbowUseTime; // was 19
					item.useTime = mymod.ConfigJson.Data.DaedalusStormbowUseTime;  // was 19
				}
				break;
			case ItemID.FetidBaghnakhs:
				if( mymod.ConfigJson.Data.FetidBaghnakhsDamage >= 0 ) {
					item.damage = mymod.ConfigJson.Data.FetidBaghnakhsDamage;   // was 70
				}
				break;
			case ItemID.ReaverShark:
				if( mymod.ConfigJson.Data.ReaverSharkPower >= 0 ) {
					item.pick = mymod.ConfigJson.Data.ReaverSharkPower; // == Gold Pickaxe; otherwise too powerful
				}
				if( mymod.ConfigJson.Data.ReaverSharkSpeed >= 0 ) {
					item.useTime = mymod.ConfigJson.Data.ReaverSharkSpeed;
					item.useAnimation = item.useTime + 4;
				}
				break;
			case ItemID.AmberStaff:
				if( mymod.ConfigJson.Data.AmberStaffUseTime >= 0 ) {
					item.useTime = mymod.ConfigJson.Data.AmberStaffUseTime;	// was 28
					item.useAnimation = item.useTime;
				}
				break;
			case ItemID.MusketBall:
				if( mymod.ConfigJson.Data.MusketBallValue >= 0 ) {
					item.value = mymod.ConfigJson.Data.MusketBallValue; // was 7
				}
				break;
			case ItemID.EmptyBullet:
				if( mymod.ConfigJson.Data.EmptyBulletValue >= 0 ) {
					item.value = mymod.ConfigJson.Data.EmptyBulletValue; // was 3
				}
				break;
			case ItemID.ChlorophyteBullet:
				if( mymod.ConfigJson.Data.ChlorophyteBulletDamage >= 0 ) {
					item.damage = mymod.ConfigJson.Data.ChlorophyteBulletDamage; // was 10
				}
				break;
			}
		}
	}
}
