using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace LosingIsFun {
	class LosingIsFunNPC : GlobalNPC {
		public override void SetupShop( int type, Chest shop, ref int nextSlot ) {
			var mymod = (LosingIsFunMod)this.mod;
			NPC my_npc = null;
			IList<NPC> town_npcs = new List<NPC>();
			bool clear_shop = false;

			for( int i=0; i<Main.npc.Length; i++ ) {
				if( Main.npc[i] != null && Main.npc[i].active ) {
					if( Main.npc[i].type == type ) {
						my_npc = Main.npc[i];
					} else if( Main.npc[i].townNPC ) {
						town_npcs.Add( Main.npc[i] );
					}
				}
			}
			
			foreach( NPC npc in town_npcs ) {
				int x_dist = Math.Abs( my_npc.homeTileX - npc.homeTileX );
				int y_dist = Math.Abs( my_npc.homeTileY - npc.homeTileY );
				double dist = Math.Sqrt( (x_dist * x_dist) + (y_dist * y_dist) );

				if( dist <= mymod.Config.Data.MinimumTownNpcTileSpacing ) {
					clear_shop = true;
					break;
				}
			}

			if( clear_shop ) {
				Main.NewText( my_npc.displayName+" the "+my_npc.name+" is housed too close to others to setup shop!" );

				for( int i=nextSlot - 1; i>=0; i-- ) {
					if( shop.item[i] != null && !shop.item[i].IsAir ) {
						shop.item[i].active = false;
						shop.item[i].type = 0;
						shop.item[i].name = "";
						shop.item[i].stack = 0;
					}
				}
			}
		}
	}
}
