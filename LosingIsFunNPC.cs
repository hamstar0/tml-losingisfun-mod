using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Utils;

namespace LosingIsFun {
	class LosingIsFunNPC : GlobalNPC {
		public override void SetupShop( int type, Chest shop, ref int nextSlot ) {
			var mymod = (LosingIsFunMod)this.mod;
			NPC my_npc = null;
			IList<NPC> town_npcs = new List<NPC>();
			bool too_close = false;
			bool too_high = false;

			for( int i = 0; i < Main.npc.Length; i++ ) {
				if( Main.npc[i] != null && Main.npc[i].active ) {
					if( Main.npc[i].type == type ) {
						my_npc = Main.npc[i];
					} else if( Main.npc[i].townNPC && !Main.npc[i].homeless ) {
						town_npcs.Add( Main.npc[i] );
					}
				}
			}

			// Check distances between NPCs
			foreach( NPC npc in town_npcs ) {
				int x_dist = Math.Abs( my_npc.homeTileX - npc.homeTileX );
				int y_dist = Math.Abs( my_npc.homeTileY - npc.homeTileY );
				double dist = Math.Sqrt( (x_dist * x_dist) + (y_dist * y_dist) );

				if( dist <= mymod.Config.Data.MinimumTownNpcTileSpacing ) {
					too_close = true;
					break;
				}
			}

			// Check space beneath NPC house
			if( !too_close && !my_npc.homeless ) {
				int solids = 0;
				int min_x = 16;
				int min_y = 40;
				for( int i = my_npc.homeTileX - (min_x/2); i < my_npc.homeTileX + (min_x/2); i++ ) {
					for( int j = my_npc.homeTileY; j < my_npc.homeTileY + min_y; j++ ) {
						if( !TileHelper.IsEmpty( i, j ) ) {
							solids++;
						}
					}
				}

				if( solids < (float)((min_x+1) * min_y) * mymod.Config.Data.MinimumRatioTownNPCSolidBlocks ) {  // +1/2 solid needed
					too_high = true;
				}
			}

			if( too_close || too_high ) {
				if( too_close ) {
					Main.NewText( my_npc.displayName + " the " + my_npc.name + " is housed too close to others to setup shop!" );
				}
				if( too_high ) {
					Main.NewText( my_npc.displayName + " the " + my_npc.name + " is housed too high to setup shop!" );
				}

				for( int i = nextSlot - 1; i >= 0; i-- ) {
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
