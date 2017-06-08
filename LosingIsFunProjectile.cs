﻿using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun {
	class LosingIsFunProjectile : GlobalProjectile {
		public override void AI( Projectile projectile ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			Player owner = Main.player[projectile.owner];

			if( projectile.owner >= 0 && owner != null && owner.active ) {
				var info = projectile.GetGlobalProjectile<LosingIsFunGlobalProjectileInstanced>( this.mod );

				// Ranged attacks can crit only while player is standing still
				if( info.CanCrit == null ) {
					info.CanCrit = true;
					this.EvaluateRangedStillness( mymod, owner, info );
				}
			}
		}
		
		public override void ModifyHitNPC( Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			this.EvaluateCrit( projectile.GetGlobalProjectile<LosingIsFunGlobalProjectileInstanced>( this.mod ), ref crit );
		}
		public override void ModifyHitPlayer( Projectile projectile, Player target, ref int damage, ref bool crit ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			this.EvaluateCrit( projectile.GetGlobalProjectile<LosingIsFunGlobalProjectileInstanced>( this.mod ), ref crit );
		}
		public override void ModifyHitPvp( Projectile projectile, Player target, ref int damage, ref bool crit ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			this.EvaluateCrit( projectile.GetGlobalProjectile<LosingIsFunGlobalProjectileInstanced>( this.mod ), ref crit );
		}


		////////////////

		private void EvaluateRangedStillness( LosingIsFunMod mymod, Player owner, LosingIsFunGlobalProjectileInstanced info ) {
			if( mymod.Config.Data.RangedCritWithAimOnly ) {
				Item held_item = owner.inventory[owner.selectedItem];

				if( held_item != null && !held_item.IsAir ) {
					info.IsRanged = held_item.ranged;
					info.CanCrit = held_item.ranged && (owner.velocity.X == 0);
				}
			}
		}

		private void EvaluateCrit( LosingIsFunGlobalProjectileInstanced info, ref bool crit ) {
			if( info.CanCrit == false ) {
				crit = false;
			} else {
				var mymod = (LosingIsFunMod)this.mod;

				if( !crit && info.IsRanged && mymod.Config.Data.RangedCritWithAimOnly ) {
					crit = Main.rand.NextFloat() < mymod.Config.Data.RangedCritAddedAimPercentChance;
				}
			}
		}
	}



	class LosingIsFunGlobalProjectileInstanced : GlobalProjectile {
		public override bool InstancePerEntity { get { return true; } }
		public override bool CloneNewInstances { get { return true; } }

		public bool IsRanged;
		public bool? CanCrit = null;

		public override GlobalProjectile Clone() {
			var clone = new LosingIsFunGlobalProjectileInstanced();
			clone.IsRanged = this.IsRanged;
			clone.CanCrit = this.CanCrit;
			return clone;
		}
	}
}
