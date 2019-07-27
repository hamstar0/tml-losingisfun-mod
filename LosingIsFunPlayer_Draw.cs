using HamstarHelpers.Helpers.HUD;
using HamstarHelpers.Helpers.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun {
	partial class LosingIsFunPlayer : ModPlayer {
		public static readonly PlayerLayer MountHpBarLayer = new PlayerLayer(
			"LosingIsFun", "MountHpBar", PlayerLayer.MiscEffectsFront,
			delegate ( PlayerDrawInfo drawInfo ) {
				Player player = drawInfo.drawPlayer;
				var modplayer = player.GetModPlayer<LosingIsFunPlayer>();
				var mymod = (LosingIsFunMod)modplayer.mod;
				int hp = modplayer.MountHp;
				int maxHp = mymod.Config.MountMaxHp;
				if( hp == maxHp ) { return; }

				float x = player.position.X + (player.width / 2);
				float y = player.position.Y + 64;
				var pos = UIHelpers.ConvertToScreenPosition( new Vector2(x, y) );

				HUDHealthBarHelpers.DrawHealthBar( Main.spriteBatch, pos.X, pos.Y, hp, maxHp, Color.White, 1f );

			}
		);

		public override void ModifyDrawLayers( List<PlayerLayer> layers ) {
			var mymod = (LosingIsFunMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

			LosingIsFunPlayer.MountHpBarLayer.visible = this.player.mount.Active;
			layers.Add( LosingIsFunPlayer.MountHpBarLayer );
		}
	}
}
