using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Utils;


namespace LosingIsFun {
	class LosingIsFunPlayer : ModPlayer {
		private int EvacTimer = 0;
		private bool EvacPotConsumed = false;


		public override void PreUpdate() {
			var mymod = (LosingIsFunMod)this.mod;
			Item use_item = this.player.inventory[this.player.selectedItem];
			bool bad_use_item = true;

			if( use_item != null && !use_item.IsAir ) {
				switch( use_item.type ) {
				case 50:    // Magic Mirror
				case 2350:  // Recall Potion
				case 3124:  // Cell Phone
				case 3199:  // Ice Mirror
					if( this.player.itemTime > 0 ) {
						this.player.itemTime = use_item.useTime;
						bad_use_item = false;

						if( use_item.type == 2350 && this.player.itemAnimation == 0 ) {
							if( !this.EvacPotConsumed ) {
								this.EvacPotConsumed = true;
								use_item.stack--;
							}
						}

						if( mymod.Config.Data.EvacWarpChargeDurationFrames > 0 && !this.RunEvac() ) {
							this.player.itemTime = 0;
						}
					}
					break;
				default:
					bad_use_item = true;
					break;
				}
			}

			if( bad_use_item ) {
				this.EvacTimer = 0;
				this.EvacPotConsumed = false;
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
			if( this.player.controlUseItem && this.player.itemTime <= 1 ) {
				if( ItemClassifications.IsYoyo( this.player.inventory[this.player.selectedItem] ) ) {
					var mymod = (LosingIsFunMod)this.mod;
					this.player.velocity.X *= mymod.Config.Data.YoyoMoveSpeedMul;
				}
			}
		}


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
