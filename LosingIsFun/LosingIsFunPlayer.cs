﻿using LosingIsFun.Buffs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace LosingIsFun {
	partial class LosingIsFunPlayer : ModPlayer {
		public int Soreness = 0;
		public int MountHp = 1;

		private int EvacTimer = 0;
		private bool EvacInUse = false;
		private bool IsUsingNurse = false;
		private int GravChangeDelay = 0;
		private float PrevGravDir = 1f;
		private int MountHpRegenTimer = 0;
		
		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void clientClone( ModPlayer clone ) {
			base.clientClone( clone );
			var myclone = (LosingIsFunPlayer)clone;

			myclone.Soreness = this.Soreness;
			myclone.MountHp = this.MountHp;
			myclone.EvacTimer = this.EvacTimer;
			myclone.EvacInUse = this.EvacInUse;
			myclone.IsUsingNurse = this.IsUsingNurse;
			myclone.GravChangeDelay = this.GravChangeDelay;
			myclone.PrevGravDir = this.PrevGravDir;
			myclone.MountHpRegenTimer = this.MountHpRegenTimer;
		}


		////////////////

		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			var mymod = (LosingIsFunMod)this.mod;

			if( Main.netMode == 2 ) {
				if( toWho == -1 && fromWho == this.player.whoAmI ) {
					this.OnServerConnect();
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			if( Main.netMode == 0 ) {
				this.OnSingleConnect();
			}
			if( Main.netMode == 1 ) {
				this.OnClientConnect();
			}
		}


		private void OnSingleConnect() { }

		private void OnClientConnect() {
		}

		private void OnServerConnect() { }


		////////////////

		public override void Load( TagCompound tag ) {
			this.Soreness = tag.GetInt( "soreness" );
			if( this.Soreness > 0 ) {
				SorenessDebuff.UpdateIcon( this );
			}
		}

		public override TagCompound Save() {
			return new TagCompound { { "soreness", this.Soreness } };
		}
	}
}
