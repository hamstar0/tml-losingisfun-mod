using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Tiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace LosingIsFun {
	class LosingIsFunNPC : GlobalNPC {
		public override void SetupShop( int type, Chest shop, ref int nextSlot ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

			NPC myNpc = null;
			IList<NPC> townNpcs = new List<NPC>();
			string tooClose = "";
			bool tooHigh = false;

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc != null && npc.active && npc.type != 453 && npc.type != 368 ) {	// Skeleton Merchant & Travelling Merchant
					if( npc.type == type ) {
						myNpc = npc;
					} else if( npc.townNPC && !npc.homeless ) {
						townNpcs.Add( npc );
					}
				}
			}

			if( myNpc != null ) {
				// Check distances between NPCs
				foreach( NPC npc in townNpcs ) {
					int xDist = Math.Abs( myNpc.homeTileX - npc.homeTileX );
					int yDist = Math.Abs( myNpc.homeTileY - npc.homeTileY );
					double dist = Math.Sqrt( (xDist * xDist) + (yDist * yDist) );

					if( dist <= mymod.Config.MinimumTownNpcTileSpacing ) {
						tooClose = npc.TypeName;
						break;
					}
				}

				// Check space beneath NPC's house
				if( !myNpc.homeless ) {
					int solids = 0;
					int walls = 0;
					int minX = 16;
					int minY = 40;
					for( int i = myNpc.homeTileX - (minX / 2); i < myNpc.homeTileX + (minX / 2); i++ ) {
						for( int j = myNpc.homeTileY; j < myNpc.homeTileY + minY; j++ ) {
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
					if( (solids+walls) < (float)((minX + 1) * minY) * mymod.Config.MinimumRatioTownNPCSolidBlocks ) {
						tooHigh = true;
					}
				}
			}

			if( tooClose != "" || tooHigh ) {
				if( tooClose != "" ) {
					Main.NewText( myNpc.GivenName + " the " + myNpc.TypeName + " is housed too close to the " + tooClose + " to setup shop!", Main.errorColor );
				} else if( tooHigh ) {
					Main.NewText( myNpc.GivenName + " the " + myNpc.TypeName + " is housed too high to setup shop!", Main.errorColor );
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
			if( !mymod.Config.Enabled ) { return; }

			switch( npc.type ) {
			case 548:   // Eternia Crystal
				if( mymod.Config.EterniaCrystalAntiHoik ) {
					npc.noGravity = true;
				}
				break;
			}
		}


		public override void NPCLoot( NPC npc ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

			switch( npc.type ) {
			case NPCID.Crimera:
			case NPCID.BloodCrawler:
			case NPCID.FaceMonster:
				float chance = mymod.Config.CrimsonMobsWormToothDropChance;

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
