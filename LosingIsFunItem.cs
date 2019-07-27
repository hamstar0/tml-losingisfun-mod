using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace LosingIsFun {
	class LosingIsFunItem : GlobalItem {
		public override void SetDefaults( Item item ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

			switch( item.type ) {
			case ItemID.Snowball:
				if( mymod.Config.SnowballDamage >= 0 ) {
					item.damage = mymod.Config.SnowballDamage;    // was 8
				}
				break;
			case ItemID.CactusSword:
				if( mymod.Config.CactusSwordDamage >= 0 ) {
					item.damage = mymod.Config.CactusSwordDamage;    // was 9
				}
				break;
			case ItemID.DaedalusStormbow:
				if( mymod.Config.DaedalusStormbowUseTime >= 0 ) {
					item.useAnimation = mymod.Config.DaedalusStormbowUseTime; // was 19
					item.useTime = mymod.Config.DaedalusStormbowUseTime;  // was 19
				}
				break;
			case ItemID.FetidBaghnakhs:
				if( mymod.Config.FetidBaghnakhsDamage >= 0 ) {
					item.damage = mymod.Config.FetidBaghnakhsDamage;   // was 70
				}
				break;
			case ItemID.ReaverShark:
				if( mymod.Config.ReaverSharkPower >= 0 ) {
					item.pick = mymod.Config.ReaverSharkPower; // == Gold Pickaxe; otherwise too powerful
				}
				if( mymod.Config.ReaverSharkSpeed >= 0 ) {
					item.useTime = mymod.Config.ReaverSharkSpeed;
					item.useAnimation = item.useTime + 4;
				}
				break;
			case ItemID.AmberStaff:
				if( mymod.Config.AmberStaffUseTime >= 0 ) {
					item.useTime = mymod.Config.AmberStaffUseTime;	// was 28
					item.useAnimation = item.useTime;
				}
				break;
			case ItemID.MusketBall:
				if( mymod.Config.MusketBallValue >= 0 ) {
					item.value = mymod.Config.MusketBallValue; // was 7
				}
				break;
			case ItemID.EmptyBullet:
				if( mymod.Config.EmptyBulletValue >= 0 ) {
					item.value = mymod.Config.EmptyBulletValue; // was 3
				}
				break;
			case ItemID.ChlorophyteBullet:
				if( mymod.Config.ChlorophyteBulletDamage >= 0 ) {
					item.damage = mymod.Config.ChlorophyteBulletDamage; // was 10
				}
				break;
			}
		}
	}
}
