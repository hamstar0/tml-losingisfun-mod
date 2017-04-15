using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun {
	public class LosingIsFunItem : GlobalItem {
		public override void SetDefaults( Item item ) {
			var mymod = (LosingIsFunMod)this.mod;

			switch( item.type ) {
			case 949:   // Snowball
				if( mymod.Config.Data.SnowballDamage >= 0 ) {
					item.damage = mymod.Config.Data.SnowballDamage;    // was 8
				}
				break;
			case 881:   // Cactus Sword
				if( mymod.Config.Data.CactusSwordDamage >= 0 ) {
					item.damage = mymod.Config.Data.CactusSwordDamage;    // was 9
				}
				break;
			case 3029:  // Daedalus Stormbow
				if( mymod.Config.Data.DaedalusStormbowUseTime >= 0 ) {
					item.useAnimation = mymod.Config.Data.DaedalusStormbowUseTime; // was 19
					item.useTime = mymod.Config.Data.DaedalusStormbowUseTime;  // was 19
				}
				break;
			case 3013:  // Fetid Baghnakhs
				if( mymod.Config.Data.FetidBaghnakhsDamage >= 0 ) {
					item.damage = mymod.Config.Data.FetidBaghnakhsDamage;   // was 70
				}
				break;
			case 2341:  // Reaver Shark
				if( mymod.Config.Data.ReaverSharkPower >= 0 ) {
					item.pick = mymod.Config.Data.ReaverSharkPower; // == Gold Pickaxe; otherwise too powerful
				}
				if( mymod.Config.Data.ReaverSharkSpeed >= 0 ) {
					item.useTime = mymod.Config.Data.ReaverSharkSpeed;
					item.useAnimation = item.useTime + 4;
				}
				break;
			case 3377:  // Amber Staff
				if( mymod.Config.Data.AmberStaffUseTime >= 0 ) {
					item.useTime = mymod.Config.Data.AmberStaffUseTime;	// was 28
					item.useAnimation = item.useTime;
				}
				break;
			}
		}
	}
}
