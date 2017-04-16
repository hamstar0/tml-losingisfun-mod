using LosingIsFun.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Utils;


namespace LosingIsFun {
	public class LosingIsFunPlayer : ModPlayer {
		public int Soreness = 0;

		private int EvacTimer = 0;
		private bool EvacPotInUse = false;
		private bool IsUsingNurse = false;


		////////////////

		public override void clientClone( ModPlayer clone ) {
			base.clientClone( clone );
			var myclone = (LosingIsFunPlayer)clone;

			myclone.EvacTimer = this.EvacTimer;
			myclone.EvacPotInUse = this.EvacPotInUse;
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI == this.player.whoAmI ) {
				if( Main.netMode != 2 ) {   // Not server
					var mymod = (LosingIsFunMod)this.mod;
					if( !mymod.Config.LoadFile() ) {
						mymod.Config.SaveFile();
					}
				}

				if( Main.netMode == 1 ) {	// Client
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
			Item use_item = this.player.inventory[ this.player.selectedItem ];
			bool can_run_evac = false;

			if( use_item.IsTheSameAs( Main.mouseItem ) ) {
				use_item = Main.mouseItem;
			}
			
			if( use_item != null && !use_item.IsAir ) {	// Apply item effects
				switch( use_item.type ) {
				case 50:    // Magic Mirror
				case 2350:  // Recall Potion
				case 3124:  // Cell Phone
				case 3199:  // Ice Mirror
					if( mymod.Config.Data.EvacWarpChargeDurationFrames > 0 ) {
						if( this.player.itemTime > 0 ) {    // In use
							this.player.itemTime = use_item.useTime;

							if( use_item.type == 2350 && this.player.itemAnimation == 0 ) {
								this.EvacPotInUse = true;
								ItemHelper.ReduceStack( use_item, 1 );
								this.player.itemTime = 0;
							}

							can_run_evac = true;
						}
					}
					break;
				}
			}

			if( this.EvacPotInUse ) {
				can_run_evac = true;
			}
			
			if( !can_run_evac || !this.RunEvac() ) {
				this.EvacTimer = 0;
				this.EvacPotInUse = false;
			}

			return true;
		}


		public override void PreUpdate() {
			// Detect nurse use + add soreness
			if( PlayerHelper.HasUsedNurse( this.player ) ) {
				if( !this.IsUsingNurse ) {
					this.IsUsingNurse = true;
					SorenessDebuff.GiveTo( this );
				}
			} else if( this.IsUsingNurse ) {
				this.IsUsingNurse = false;
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
		
		private bool RunEvac() {
			if( this.player.velocity.X != 0 || this.player.velocity.Y != 0 ) {
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
