using Terraria;


namespace Utils {
	public class TileHelper {
		public static bool IsEmpty( int world_x, int world_y ) {
			Tile tile = Main.tile[world_x, world_y];

			if( tile != null && !tile.inActive() && tile.active() ) {
				if( Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type] ) {
					return false;
				}
			}
			// Special cases: Lava
			if( tile.lava() ) {
				return false;
			}
			// Special cases: Lihzahrd Brick Wall
			if( tile.wall == 87 ) {
				return false;
			}
			// Special cases: Dungeon Walls
			if( (tile.wall >= 7 && tile.wall <= 9) || (tile.wall >= 94 && tile.wall <= 99) ) {
				return false;
			}
			return true;
		}
	}
}
