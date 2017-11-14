using HamstarHelpers.HudHelpers;
using HamstarHelpers.ItemHelpers;
using HamstarHelpers.PlayerHelpers;
using HamstarHelpers.UIHelpers;
using LosingIsFun.Buffs;
using LosingIsFun.NetProtocol;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace LosingIsFun {
	class MyPlayer : ModPlayer {
		public int Soreness = 0;
		public int MountHp = 1;

		private int EvacTimer = 0;
		private bool EvacInUse = false;
		private bool IsUsingNurse = false;
		private int GravChangeDelay = 0;
		private float PrevGravDir = 1f;
		private int MountHpRegenTimer = 0;


		////////////////

		public override void clientClone( ModPlayer clone ) {
			base.clientClone( clone );
			var myclone = (MyPlayer)clone;

			myclone.Soreness = this.Soreness;
			myclone.MountHp = this.MountHp;
			myclone.EvacTimer = this.EvacTimer;
			myclone.EvacInUse = this.EvacInUse;
			myclone.IsUsingNurse = this.IsUsingNurse;
			myclone.GravChangeDelay = this.GravChangeDelay;
			myclone.PrevGravDir = this.PrevGravDir;
			myclone.MountHpRegenTimer = this.MountHpRegenTimer;
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI == this.player.whoAmI ) {
				if( Main.netMode != 2 ) {   // Not server
					var mymod = (LosingIsFunMod)this.mod;
					if( !mymod.Config.LoadFile() ) {
						mymod.Config.SaveFile();
					}
				}

				if( Main.netMode == 1 ) {   // Client
					ClientPacketHandlers.SendModSettingsRequestFromClient( this.mod );
				}
			}
		}

		public override void Load( TagCompound tag ) {
			this.Soreness = tag.GetInt( "soreness" );
			if( this.Soreness > 0 ) {
				SorenessDebuff.UpdateIcon( this );
			}
		}

		public override TagCompound Save() {
			return new TagCompound { { "soreness", this.Soreness } };
		}



		////////////////

		public static readonly PlayerLayer MountHpBarLayer = new PlayerLayer(
			"LosingIsFun", "MountHpBar", PlayerLayer.MiscEffectsFront,
			delegate ( PlayerDrawInfo draw_info ) {
				Player player = draw_info.drawPlayer;
				var modplayer = player.GetModPlayer<MyPlayer>();
				var mymod = (LosingIsFunMod)modplayer.mod;
				int hp = modplayer.MountHp;
				int max_hp = mymod.Config.Data.MountMaxHp;
				if( hp == max_hp ) { return; }

				float x = player.position.X + (player.width / 2);
				float y = player.position.Y + 64;
				var pos = UIHelpers.ConvertToScreenPosition( new Vector2(x, y) );

				HudHealthBarHelpers.DrawHealthBar( Main.spriteBatch, pos.X, pos.Y, hp, max_hp, Color.White, 1f );

			}
		);

		public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			MyPlayer.MountHpBarLayer.visible = this.player.mount.Active;
			layers.Add( MyPlayer.MountHpBarLayer );
		}



		////////////////

		public override void PostHurt( bool pvp, bool quiet, double damage, int hitDirection, bool crit ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			if( !quiet && this.player.mount.Active ) {
				this.MountHpRegenTimer = mymod.Config.Data.MountHpRegenRate;
				if( this.MountHp > 0 ) { this.MountHp--; }

				if( this.MountHp == 0 ) {
					this.player.AddBuff( mymod.BuffType<BuckedDebuff>(), mymod.Config.Data.MountEjectDebuffTime );
				}
			}
		}

		public override bool PreItemCheck() {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return base.PreItemCheck(); }

			Item use_item = this.player.inventory[this.player.selectedItem];
			bool can_run_evac = false, evac_is_done = false;

			if( use_item.IsTheSameAs( Main.mouseItem ) ) {
				use_item = Main.mouseItem;
			}

			if( use_item != null && !use_item.IsAir ) { // Apply item effects
				switch( use_item.type ) {
				case 50:    // Magic Mirror
				case 3124:  // Cell Phone
				case 3199:  // Ice Mirror
				case 2350:  // Recall Potion
					if( mymod.Config.Data.EvacWarpChargeDurationFrames > 0 ) {
						if( this.player.itemTime > 0 ) {    // In use
							this.player.itemTime = use_item.useTime;
							can_run_evac = true;

							if( this.player.itemAnimation == 0 ) {
								if( use_item.type == 2350 ) {   // Recall Potion
									ItemHelpers.ReduceStack( use_item, 1 );
									this.EvacTimer += 30;   // Speed up warp by 0.5 seconds for Recall Potion
								}
								this.EvacInUse = true;
								this.player.itemTime = 0;
							}
						}
					}
					break;
				}
			}

			if( this.EvacInUse ) {
				can_run_evac = true;
			}
			
			if( !can_run_evac || !this.RunEvac( out evac_is_done ) ) {
				if( evac_is_done ) { this.player.itemTime = 0; }
				this.EvacTimer = 0;
				this.EvacInUse = false;
			}

			return true;
		}


		public override void PreUpdate() {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			// Detect nurse use + add soreness
			if( PlayerNPCHelpers.HasUsedNurse( this.player ) ) {
				if( !this.IsUsingNurse ) {
					this.IsUsingNurse = true;
					SorenessDebuff.GiveTo( this );
				}
			} else if( this.IsUsingNurse ) {
				this.IsUsingNurse = false;
			}

			// Restrict gravitation potion rapid changing
			if( this.player.gravControl && mymod.Config.Data.GravPotionFlipDelay > 0f ) {
				if( this.GravChangeDelay > 0 ) {
					if( this.PrevGravDir == -1 && this.player.gravDir == 1f ) {
						this.player.gravDir = -1f;
					} else if( this.PrevGravDir == 1 && this.player.gravDir == -1f ) {
						this.player.gravDir = 1f;
					}
					this.GravChangeDelay--;
				} else {
					if( this.PrevGravDir != this.player.gravDir ) {
						this.PrevGravDir = this.player.gravDir;
						this.GravChangeDelay = mymod.Config.Data.GravPotionFlipDelay;
					}
				}
			}

			// Add HP bar for mount
			if( this.MountHpRegenTimer > 0 ) {
				this.MountHpRegenTimer--;
			} else {
				if( this.MountHp < mymod.Config.Data.MountMaxHp ) {
					this.MountHp++;
					this.MountHpRegenTimer = mymod.Config.Data.MountHpRegenRate;
				}
			}
		}


		public override void PostUpdate() {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			// Apply soreness defense debuff (cannot use PreUpdateBuffs, PostUpdateBuffs, or ModBuff.Update for some reason)
			if( this.Soreness > 0 ) {
				if( this.player.FindBuffIndex(mymod.BuffType<SorenessDebuff>()) == -1 ) {
					this.Soreness = 0;
				} else {
					SorenessDebuff.ApplyDefenselessness( (LosingIsFunMod)this.mod, this.player, this.Soreness );
				}
			}
		}


		public override void UpdateEquips( ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			// Set fall immunity to work only 50% of the time
			if( this.player.noFallDmg ) {
				this.player.noFallDmg = Main.rand.NextFloat() <= mymod.Config.Data.LuckyHorseshoeFailChance;
			}
		}


		public override void PostUpdateEquips() {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			if( this.player.wings == 13 && mymod.Config.Data.LeafWingsTime >= 0 ) {
				this.player.wingTimeMax = mymod.Config.Data.LeafWingsTime;
			}
		}


		public override void PostUpdateRunSpeeds() {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			if( this.player.controlUseItem && this.player.itemTime > 1 ) {
				if( ItemIdentityHelpers.IsYoyo( this.player.inventory[this.player.selectedItem] ) ) {
					var fric = mymod.Config.Data.YoyoMoveSpeedClamp;

					if( this.player.velocity.X > fric ) {
						this.player.velocity.X -= 0.12f;
					} else if( this.player.velocity.X < -fric ) {
						this.player.velocity.X += 0.12f;
					}
				}
			}

			if( this.Soreness > 0 ) {
				SorenessDebuff.ApplyLameness( mymod, this.player, this.Soreness );
			}
		}


		public override void UpdateDead() {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			// No respawns during bosses
			if( this.player.respawnTimer > 0 && mymod.Config.Data.NoRespawnDuringBosses ) {
				for( int i=0; i<Main.npc.Length; i++ ) {
					NPC npc = Main.npc[i];
					if( npc != null && npc.active && npc.boss ) {
						this.player.respawnTimer++;
						break;
					}
				}
			}
		}


		////////////////

		private bool RunEvac( out bool is_interrupted ) {
			is_interrupted = false;

			if( this.player.velocity.X != 0 || this.player.velocity.Y != 0 ) {
				Main.NewText( "Recall interrupted by movement.", Color.Yellow );
				is_interrupted = true;
				return false;
			}

			var mymod = (LosingIsFunMod)this.mod;
			var duration = mymod.Config.Data.EvacWarpChargeDurationFrames;

			this.EvacTimer++;

			Dust.NewDust( this.player.position, this.player.width, this.player.height, 15, 0, 0, 150, Color.Cyan, 1.2f );

			if( this.EvacTimer > duration ) {
				PlayerHelpers.Evac( this.player );
				this.EvacTimer = 0;
				return false;
			}
			
			return true;
		}
	}
}
