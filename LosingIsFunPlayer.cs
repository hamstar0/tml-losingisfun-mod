using LosingIsFun.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Utils;


namespace LosingIsFun {
	public class LosingIsFunPlayer : ModPlayer {
		public int Soreness = 0;

		private int EvacTimer = 0;
		private bool EvacInUse = false;
		private bool IsUsingNurse = false;
		private int GravChangeDelay = 0;
		private float PrevGravDir = 1f;


		////////////////

		public override void clientClone( ModPlayer clone ) {
			base.clientClone( clone );
			var myclone = (LosingIsFunPlayer)clone;

			myclone.Soreness = this.Soreness;
			myclone.EvacTimer = this.EvacTimer;
			myclone.EvacInUse = this.EvacInUse;
			myclone.IsUsingNurse = this.IsUsingNurse;
			myclone.GravChangeDelay = this.GravChangeDelay;
			myclone.PrevGravDir = this.PrevGravDir;
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
					LosingIsFunNetProtocol.SendModSettingsRequestFromClient( this.mod );
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

		public override bool PreItemCheck() {
			var mymod = (LosingIsFunMod)this.mod;
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
									ItemHelper.ReduceStack( use_item, 1 );
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

			// Detect nurse use + add soreness
			if( PlayerHelper.HasUsedNurse( this.player ) ) {
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
		}


		public override void PostUpdate() {
			// Apply soreness defense debuff (cannot use PreUpdateBuffs, PostUpdateBuffs, or ModBuff.Update for some reason)
			if( this.Soreness > 0 ) {
				SorenessDebuff.ApplyDefenselessness( (LosingIsFunMod)this.mod, this.player, this.Soreness );
			}
		}


		public override void UpdateEquips( ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff ) {
			var mymod = (LosingIsFunMod)this.mod;

			// Set fall immunity to work only 50% of the time
			if( this.player.noFallDmg ) {
				this.player.noFallDmg = Main.rand.NextFloat() <= mymod.Config.Data.LuckyHorseshoeFailChance;
			}
		}


		public override void PostUpdateRunSpeeds() {
			var mymod = (LosingIsFunMod)this.mod;

			if( this.player.controlUseItem && this.player.itemTime > 1 ) {
				if( ItemClassifications.IsYoyo( this.player.inventory[this.player.selectedItem] ) ) {
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
				PlayerHelper.Evac( this.player );
				this.EvacTimer = 0;
				return false;
			}
			
			return true;
		}
	}
}
