using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun {
	public class LosingIsFunItem : GlobalItem {
		public override void SetDefaults( Item item ) {
			var mymod = (LosingIsFunMod)this.mod;
			switch( item.type ) {
			case 3029:  // Daedalus Stormbow
				if( mymod.Config.Data.DaedalusStormbowNerf ) {
					item.useAnimation = 30; // was 19
					item.useTime = 30;  // was 19
				}
				break;
			case 3013:  // Fetid Baghnakhs
				if( mymod.Config.Data.FetidBaghnakhsNerf ) {
					item.damage = 62;   // was 70
				}
				break;
			case 2341:  // Reaver Shark
				item.pick = mymod.Config.Data.ReaverSharkPower; // == Gold Pickaxe; otherwise too powerful
				item.useTime = mymod.Config.Data.ReaverSharkSpeed;
				item.useAnimation = item.useTime + 4;
				break;
			// TODO: Adjust Amber Staff
			}
		}
	}
}
