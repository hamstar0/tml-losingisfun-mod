using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.Items.Attributes;
using HamstarHelpers.Helpers.Players;
using LosingIsFun.Buffs;
using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun {
	partial class LosingIsFunPlayer : ModPlayer {
		public override void PreUpdate() {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

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
			if( this.player.gravControl && mymod.Config.GravPotionFlipDelay > 0f ) {
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
						this.GravChangeDelay = mymod.Config.GravPotionFlipDelay;
					}
				}
			}

			// Add HP bar for mount
			if( this.MountHpRegenTimer > 0 ) {
				this.MountHpRegenTimer--;
			} else {
				if( this.MountHp < mymod.Config.MountMaxHp ) {
					this.MountHp++;
					this.MountHpRegenTimer = mymod.Config.MountHpRegenRate;
				}
			}
		}


		public override void PostUpdate() {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

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
			if( !mymod.Config.Enabled ) { return; }

			// Set fall immunity to work only 50% of the time
			if( this.player.noFallDmg ) {
				this.player.noFallDmg = Main.rand.NextFloat() <= mymod.Config.LuckyHorseshoeFailChance;
			}
		}


		public override void PostUpdateEquips() {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

			if( this.player.wings == 13 && mymod.Config.LeafWingsTime >= 0 ) {
				this.player.wingTimeMax = mymod.Config.LeafWingsTime;
			}
		}


		public override void PostUpdateRunSpeeds() {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

			if( this.player.controlUseItem && this.player.itemTime > 1 ) {
				if( ItemAttributeHelpers.IsYoyo( this.player.inventory[this.player.selectedItem] ) ) {
					var fric = mymod.Config.YoyoMoveSpeedClamp;

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
			if( !mymod.Config.Enabled ) { return; }

			// No respawns during bosses
			if( this.player.respawnTimer > 0 && mymod.Config.NoRespawnDuringBosses ) {
				for( int i=0; i<Main.npc.Length; i++ ) {
					NPC npc = Main.npc[i];
					if( npc != null && npc.active && npc.boss ) {
						this.player.respawnTimer++;
						break;
					}
				}
			}
		}
	}
}
