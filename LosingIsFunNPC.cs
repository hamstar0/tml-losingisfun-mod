using Microsoft.Xna.Framework;
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
			string too_close = "";
			bool too_high = false;

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc != null && npc.active ) {
					if( npc.type == type ) {
						my_npc = npc;
					} else if( npc.townNPC && !npc.homeless ) {
						if( npc.type != 453 && npc.type != 368 ) {	// Skeleton Merchant & Travelling Merchant
							town_npcs.Add( npc );
						}
					}
				}
			}

			// Check distances between NPCs
			foreach( NPC npc in town_npcs ) {
				int x_dist = Math.Abs( my_npc.homeTileX - npc.homeTileX );
				int y_dist = Math.Abs( my_npc.homeTileY - npc.homeTileY );
				double dist = Math.Sqrt( (x_dist * x_dist) + (y_dist * y_dist) );

				if( dist <= mymod.Config.Data.MinimumTownNpcTileSpacing ) {
					too_close = npc.name;
					break;
				}
			}

			// Check space beneath NPC's house
			if( too_close == "" && !my_npc.homeless ) {
				int solids = 0;
				int min_x = 16;
				int min_y = 40;
				for( int i = my_npc.homeTileX - (min_x / 2); i < my_npc.homeTileX + (min_x / 2); i++ ) {
					for( int j = my_npc.homeTileY; j < my_npc.homeTileY + min_y; j++ ) {
						if( !TileHelper.IsEmpty( i, j, true, true, true ) ) {
							solids++;
						}
					}
				}

				if( solids < (float)((min_x + 1) * min_y) * mymod.Config.Data.MinimumRatioTownNPCSolidBlocks ) {  // +1/2 solid needed
					too_high = true;
				}
			}

			if( too_close != "" || too_high ) {
				if( too_close != "" ) {
					Main.NewText( my_npc.displayName + " the " + my_npc.name + " is housed too close to the "+too_close+" to setup shop!" );
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


		public override void SetDefaults( NPC npc ) {
			var mymod = (LosingIsFunMod)this.mod;

			switch( npc.type ) {
			case 548:   // Eternia Crystal
				if( mymod.Config.Data.EterniaCrystalAntiHoik ) {
					npc.noGravity = true;
				}
				break;
			}
		}
	}
}
