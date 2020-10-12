using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun {
	class LosingIsFunProjectile : GlobalProjectile {
		public bool IsRanged;
		public bool? CanCrit = null;

		////////////////

		public override bool InstancePerEntity => true;
		public override bool CloneNewInstances => false;	// true;? TODO: Verify.



		////////////////
		
		public override void AI( Projectile projectile ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

			Player owner = Main.player[projectile.owner];

			if( projectile.owner >= 0 && owner != null && owner.active ) {
				// Ranged attacks can crit only while player is standing still
				if( this.CanCrit == null ) {
					this.CanCrit = true;
					this.EvaluateRangedStillness( owner );
				}
			}
		}
		
		public override void ModifyHitNPC( Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

			this.EvaluateCrit( ref crit );
		}
		public override void ModifyHitPlayer( Projectile projectile, Player target, ref int damage, ref bool crit ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

			this.EvaluateCrit( ref crit );
		}
		public override void ModifyHitPvp( Projectile projectile, Player target, ref int damage, ref bool crit ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

			this.EvaluateCrit( ref crit );
		}


		////////////////

		private void EvaluateRangedStillness( Player owner ) {
			var mymod = (LosingIsFunMod)this.mod;

			if( mymod.Config.RangedCritWithAimOnly ) {
				Item heldItem = owner.inventory[owner.selectedItem];

				if( heldItem != null && !heldItem.IsAir ) {
					this.IsRanged = heldItem.ranged;
					this.CanCrit = heldItem.ranged && (owner.velocity.X == 0);
				}
			}
		}

		private void EvaluateCrit( ref bool crit ) {
			if( this.CanCrit == false ) {
				crit = false;
			} else {
				var mymod = (LosingIsFunMod)this.mod;

				if( !crit && this.IsRanged && mymod.Config.RangedCritWithAimOnly ) {
					crit = Main.rand.NextFloat() < mymod.Config.RangedCritAddedAimPercentChance;
				}
			}
		}
	}
}
