using Terraria;


namespace Utils {
	public class TileHelper {
		public static bool IsAir( int world_x, int world_y ) {
			Tile tile = Main.tile[ world_x, world_y ];
			return tile == null || !tile.active();
		}

		public static bool IsSolid( int world_x, int world_y, bool is_platform_solid = false, bool is_actuated_solid = false ) {
			Tile tile = Main.tile[world_x, world_y];

			if( Main.tileSolid[(int)tile.type] ) {  // Solid
				if( !Main.tileSolidTop[(int)tile.type] || is_platform_solid ) {  // Non-platform
					if( !tile.inActive() || is_actuated_solid ) {  // Actuator not active
						return true;
					}
				}
			}
			return false;
		}

		public static bool IsDungeon( int world_x, int world_y ) {
			Tile tile = Main.tile[world_x, world_y];

			// Lihzahrd Brick Wall
			if( tile.wall == 87 ) {
				return true;
			}
			// Dungeon Walls
			if( (tile.wall >= 7 && tile.wall <= 9) || (tile.wall >= 94 && tile.wall <= 99) ) {
				return true;
			}
			return false;
		}
	}
}
