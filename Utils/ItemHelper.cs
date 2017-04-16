using Terraria;


namespace Utils {
	public static class ItemHelper {
		public static void ReduceStack( Item item, int amt ) {
			item.stack -= amt;

			if( item.stack <= 0 ) {
				item.TurnToAir();
				item.active = false;
			}

			if( Main.netMode != 0 && item.owner == Main.myPlayer && item.whoAmI > 0 ) {
				NetMessage.SendData( 21, -1, -1, "", item.whoAmI, 0f, 0f, 0f, 0, 0, 0 );
			}
		}
	}
}
