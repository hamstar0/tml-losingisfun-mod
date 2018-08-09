using LosingIsFun.Buffs;
using LosingIsFun.NetProtocol;
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

		public override bool CloneNewInstances { get { return false; } }

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

		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			var mymod = (LosingIsFunMod)this.mod;

			if( Main.netMode == 2 ) {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
					this.OnServerConnect();
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			var mymod = (LosingIsFunMod)this.mod;

			if( Main.netMode == 0 ) {
				if( !mymod.ConfigJson.LoadFile() ) {
					mymod.ConfigJson.SaveFile();
					ErrorLogger.Log( "Losing Is Fun config " + LosingIsFunConfigData.ConfigVersion.ToString() + " created (ModPlayer.OnEnterWorld())." );
				}
			}

			if( Main.netMode == 0 ) {
				this.OnSingleConnect();
			}
			if( Main.netMode == 1 ) {
				this.OnClientConnect();
			}
		}


		private void OnSingleConnect() { }

		private void OnClientConnect() {
			ClientPacketHandlers.SendModSettingsRequestFromClient( this.mod );
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
