using HamstarHelpers.ItemHelpers;
using HamstarHelpers.TileHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace LosingIsFun {
	public class MyGlobalNPC : GlobalNPC {
		public override void SetupShop( int type, Chest shop, ref int nextSlot ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			NPC my_npc = null;
			IList<NPC> town_npcs = new List<NPC>();
			string too_close = "";
			bool too_high = false;

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc != null && npc.active && npc.type != 453 && npc.type != 368 ) {	// Skeleton Merchant & Travelling Merchant
					if( npc.type == type ) {
						my_npc = npc;
					} else if( npc.townNPC && !npc.homeless ) {
						town_npcs.Add( npc );
					}
				}
			}

			if( my_npc != null ) {
				// Check distances between NPCs
				foreach( NPC npc in town_npcs ) {
					int x_dist = Math.Abs( my_npc.homeTileX - npc.homeTileX );
					int y_dist = Math.Abs( my_npc.homeTileY - npc.homeTileY );
					double dist = Math.Sqrt( (x_dist * x_dist) + (y_dist * y_dist) );

					if( dist <= mymod.Config.Data.MinimumTownNpcTileSpacing ) {
						too_close = npc.TypeName;
						break;
					}
				}

				// Check space beneath NPC's house
				if( !my_npc.homeless ) {
					int solids = 0;
					int walls = 0;
					int min_x = 16;
					int min_y = 40;
					for( int i = my_npc.homeTileX - (min_x / 2); i < my_npc.homeTileX + (min_x / 2); i++ ) {
						for( int j = my_npc.homeTileY; j < my_npc.homeTileY + min_y; j++ ) {
							Tile tile = Main.tile[i, j];
							if( tile == null || TileHelpers.IsAir( tile ) ) { continue; }

							else if( tile.wall > 0 ) {
								walls++;
							} else if( TileHelpers.IsSolid( tile, true, true) ) {
								solids++;
							}
						}
					}

					// +1/2 solid needed
//DebugHelper.Display["tiles"] = "walls "+walls+", solids "+solids+" = "+(walls+solids)+" < "+((float)((min_x + 1) * min_y) * mymod.Config.Data.MinimumRatioTownNPCSolidBlocks);
					if( (solids+walls) < (float)((min_x + 1) * min_y) * mymod.Config.Data.MinimumRatioTownNPCSolidBlocks ) {
						too_high = true;
					}
				}
			}

			if( too_close != "" || too_high ) {
				if( too_close != "" ) {
					Main.NewText( my_npc.GivenName + " the " + my_npc.TypeName + " is housed too close to the " + too_close + " to setup shop!", Main.errorColor );
				} else if( too_high ) {
					Main.NewText( my_npc.GivenName + " the " + my_npc.TypeName + " is housed too high to setup shop!", Main.errorColor );
				}

				for( int i = nextSlot - 1; i >= 0; i-- ) {
					if( shop.item[i] != null && !shop.item[i].IsAir ) {
						shop.item[i].active = false;
						shop.item[i].type = 0;
						//shop.item[i].name = "";
						shop.item[i].stack = 0;
					}
				}
			}
		}


		public override void SetDefaults( NPC npc ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			switch( npc.type ) {
			case 548:   // Eternia Crystal
				if( mymod.Config.Data.EterniaCrystalAntiHoik ) {
					npc.noGravity = true;
				}
				break;
			}
		}


		public override void NPCLoot( NPC npc ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			switch( npc.type ) {
			case NPCID.Crimera:
			case NPCID.BloodCrawler:
			case NPCID.FaceMonster:
				float chance = mymod.Config.Data.CrimsonMobsWormToothDropChance;

				if( chance >= 0 && Main.rand.NextFloat() <= chance ) {
					var item = new Item();
					item.SetDefaults( ItemID.WormTooth );
					ItemHelpers.CreateItem( npc.position, item.type, 1, item.width, item.height );
				}
				break;
			}
		}
	}
}
