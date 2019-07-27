using LosingIsFun.Buffs;
using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun {
	partial class LosingIsFunMod : Mod {
		public static LosingIsFunMod Instance { get; private set; }



		////////////////

		public LosingIsFunConfig Config => this.GetConfig<LosingIsFunConfig>();


		////////////////

		public LosingIsFunMod() {
			LosingIsFunMod.Instance = this;
		}

		////////////////

		public override void Load() {
			if( !Main.dedServ ) {
				SorenessDebuff.LoadTextures( this );
			}
		}

		public override void Unload() {
			LosingIsFunMod.Instance = null;
		}
	}
}
