using HamstarHelpers.Helpers.HudHelpers;
using HamstarHelpers.Helpers.UIHelpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun {
	partial class LosingIsFunPlayer : ModPlayer {
		public static readonly PlayerLayer MountHpBarLayer = new PlayerLayer(
			"LosingIsFun", "MountHpBar", PlayerLayer.MiscEffectsFront,
			delegate ( PlayerDrawInfo draw_info ) {
				Player player = draw_info.drawPlayer;
				var modplayer = player.GetModPlayer<LosingIsFunPlayer>();
				var mymod = (LosingIsFunMod)modplayer.mod;
				int hp = modplayer.MountHp;
				int max_hp = mymod.ConfigJson.Data.MountMaxHp;
				if( hp == max_hp ) { return; }

				float x = player.position.X + (player.width / 2);
				float y = player.position.Y + 64;
				var pos = UIHelpers.ConvertToScreenPosition( new Vector2(x, y) );

				HudHealthBarHelpers.DrawHealthBar( Main.spriteBatch, pos.X, pos.Y, hp, max_hp, Color.White, 1f );

			}
		);

		public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.ConfigJson.Data.Enabled ) { return; }

			LosingIsFunPlayer.MountHpBarLayer.visible = this.player.mount.Active;
			layers.Add( LosingIsFunPlayer.MountHpBarLayer );
		}
	}
}
