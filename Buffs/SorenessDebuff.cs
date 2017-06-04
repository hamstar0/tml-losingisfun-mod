﻿using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;


namespace LosingIsFun.Buffs {
	public class SorenessDebuff : ModBuff {
		public static int Stages = 8;
		public static Texture2D[] Textures { get; private set; }

		static SorenessDebuff() {
			SorenessDebuff.Textures = new Texture2D[ SorenessDebuff.Stages ];
		}


		public static void LoadTextures( LosingIsFunMod mymod ) {
			for( int i=0; i<SorenessDebuff.Stages; i++ ) {
				SorenessDebuff.Textures[i] = mymod.GetTexture( "Buffs/SorenessDebuff_" + (i + 1) );
			}
		}

		public static void UpdateIcon( LosingIsFunPlayer myplayer ) {
			LosingIsFunMod mymod = (LosingIsFunMod)myplayer.mod;
			int buff_type = mymod.BuffType<SorenessDebuff>();
			Main.buffTexture[buff_type] = SorenessDebuff.Textures[myplayer.Soreness - 1];
		}

		public static void GiveTo( LosingIsFunPlayer myplayer ) {
			LosingIsFunMod mymod = (LosingIsFunMod)myplayer.mod;
			int duration = mymod.Config.Data.SorenessDurationSeconds * 60;

			myplayer.player.AddBuff( mymod.BuffType("SorenessDebuff"), duration );

			if( myplayer.Soreness < SorenessDebuff.Stages ) {
				myplayer.Soreness++;
			}

			SorenessDebuff.UpdateIcon( myplayer );
		}



		public override void SetDefaults() {
			Main.buffName[this.Type] = "Soreness";
			Main.buffTip[this.Type] = "Defense and speed down";
			Main.debuff[this.Type] = true;
		}

		public override void Update( Player player, ref int buff_index ) {
			LosingIsFunPlayer myplayer = player.GetModPlayer<LosingIsFunPlayer>( this.mod );
			//SorenessDebuff.ApplyDefenselessness( (LosingIsFunMod)myplayer.mod, player, myplayer.Soreness );	?

			if( player.buffTime[buff_index] <= 2 ) {
				myplayer.Soreness = 0;
			}
		}


		public static void ApplyDefenselessness( LosingIsFunMod mymod, Player player, int soreness ) {
			float defenselessness = mymod.Config.Data.SorenessDefenselessnessPercent * (float)soreness;
			
			int def = (int)((float)player.statDefense * (1f - defenselessness));
			player.statDefense = def;
		}

		public static void ApplyLameness( LosingIsFunMod mymod, Player player, int soreness ) {
			float lameness = mymod.Config.Data.SorenessLamenessPercent * (float)soreness;

			//player.maxRunSpeed *= 1f - lameness;
			//player.accRunSpeed = player.maxRunSpeed;
			//player.moveSpeed *= 1f - lameness;
			player.maxRunSpeed *= 1f - lameness;
		}
	}
}
