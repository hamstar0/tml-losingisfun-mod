using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace LosingIsFun {
	class MyItem : GlobalItem {
		public override void SetDefaults( Item item ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			switch( item.type ) {
			case ItemID.Snowball:
				if( mymod.Config.Data.SnowballDamage >= 0 ) {
					item.damage = mymod.Config.Data.SnowballDamage;    // was 8
				}
				break;
			case ItemID.CactusSword:
				if( mymod.Config.Data.CactusSwordDamage >= 0 ) {
					item.damage = mymod.Config.Data.CactusSwordDamage;    // was 9
				}
				break;
			case ItemID.DaedalusStormbow:
				if( mymod.Config.Data.DaedalusStormbowUseTime >= 0 ) {
					item.useAnimation = mymod.Config.Data.DaedalusStormbowUseTime; // was 19
					item.useTime = mymod.Config.Data.DaedalusStormbowUseTime;  // was 19
				}
				break;
			case ItemID.FetidBaghnakhs:
				if( mymod.Config.Data.FetidBaghnakhsDamage >= 0 ) {
					item.damage = mymod.Config.Data.FetidBaghnakhsDamage;   // was 70
				}
				break;
			case ItemID.ReaverShark:
				if( mymod.Config.Data.ReaverSharkPower >= 0 ) {
					item.pick = mymod.Config.Data.ReaverSharkPower; // == Gold Pickaxe; otherwise too powerful
				}
				if( mymod.Config.Data.ReaverSharkSpeed >= 0 ) {
					item.useTime = mymod.Config.Data.ReaverSharkSpeed;
					item.useAnimation = item.useTime + 4;
				}
				break;
			case ItemID.AmberStaff:
				if( mymod.Config.Data.AmberStaffUseTime >= 0 ) {
					item.useTime = mymod.Config.Data.AmberStaffUseTime;	// was 28
					item.useAnimation = item.useTime;
				}
				break;
			case ItemID.MusketBall:
				if( mymod.Config.Data.MusketBallValue >= 0 ) {
					item.value = mymod.Config.Data.MusketBallValue; // was 7
				}
				break;
			case ItemID.EmptyBullet:
				if( mymod.Config.Data.EmptyBulletValue >= 0 ) {
					item.value = mymod.Config.Data.EmptyBulletValue; // was 3
				}
				break;
			case ItemID.ChlorophyteBullet:
				if( mymod.Config.Data.ChlorophyteBulletDamage >= 0 ) {
					item.damage = mymod.Config.Data.ChlorophyteBulletDamage; // was 10
				}
				break;
			}
		}
	}
}
