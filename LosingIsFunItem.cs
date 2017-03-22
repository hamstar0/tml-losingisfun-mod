using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun {
	public class LosingIsFunItem : GlobalItem {
		/*public override bool UseItem( Item item, Player player ) {
			switch( item.type ) {
			//case 2351:  // Teleportation Potion
			case 2350:  // Recall Potion
			case 3199:  // Ice Mirror
			case 50:    // Magic Mirror
			case 3124:  // Cell Phone
Main.NewText("using "+ item.type );
				var modplayer = player.GetModPlayer<LosingIsFunPlayer>( this.mod );
				modplayer.RunEvac( item );
				break;
			//case 2997:  // Unity/Wormhole Potion
			//	var info = item.GetModInfo<LosingIsFunItemInfo>( this.mod );
			//	info.RunUnity( player );
			//	return false;
			}
			return true;
		}*/


		public override void SetDefaults( Item item ) {
			switch( item.type ) {
			case 3029:  // Daedalus Stormbow
				item.useAnimation = 30; // was 19
				item.useTime = 30;	// was 19
				break;
			case 3013:  // Fetid Baghnakhs
				item.damage = 62;	// was 70
				break;
			}
		}
	}
}
