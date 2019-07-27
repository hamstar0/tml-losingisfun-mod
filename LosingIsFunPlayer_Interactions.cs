using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Players;
using LosingIsFun.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun {
	partial class LosingIsFunPlayer : ModPlayer {
		public override void PostHurt( bool pvp, bool quiet, double damage, int hitDirection, bool crit ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

			if( !quiet && this.player.mount.Active ) {
				this.MountHpRegenTimer = mymod.Config.MountHpRegenRate;
				if( this.MountHp > 0 ) { this.MountHp--; }

				if( this.MountHp == 0 ) {
					this.player.AddBuff( mymod.BuffType<BuckedDebuff>(), mymod.Config.MountEjectDebuffTime );
				}
			}
		}

		public override bool PreItemCheck() {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Enabled ) { return base.PreItemCheck(); }

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
					if( mymod.Config.EvacWarpChargeDurationFrames > 0 ) {
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


		////////////////

		private bool RunEvac( out bool is_interrupted ) {
			is_interrupted = false;

			if( this.player.velocity.X != 0 || this.player.velocity.Y != 0 ) {
				Main.NewText( "Recall interrupted by movement.", Color.Yellow );
				is_interrupted = true;
				return false;
			}

			var mymod = (LosingIsFunMod)this.mod;
			var duration = mymod.Config.EvacWarpChargeDurationFrames;

			this.EvacTimer++;

			Dust.NewDust( this.player.position, this.player.width, this.player.height, 15, 0, 0, 150, Color.Cyan, 1.2f );

			if( this.EvacTimer > duration ) {
				PlayerWarpHelpers.Evac( this.player );
				this.EvacTimer = 0;
				return false;
			}
			
			return true;
		}
	}
}
