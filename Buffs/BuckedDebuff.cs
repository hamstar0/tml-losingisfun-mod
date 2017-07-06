using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun.Buffs {
	public class BuckedDebuff : ModBuff {
		public override void SetDefaults() {
			this.DisplayName.SetDefault( "Bucked" );
			this.Description.SetDefault( "Your mount has ejected you" );

			Main.debuff[ this.Type ] = true;
		}

		public override void Update( Player player, ref int buff_index ) {
			if( player.mount.Active ) {
				player.mount.Dismount( player );
			}
		}
	}
}
