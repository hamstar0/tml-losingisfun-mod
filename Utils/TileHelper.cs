using Terraria;


namespace Utils {
	public class TileHelper {
		public static bool IsEmpty( int world_x, int world_y,
				bool is_wall_solid=false, bool is_platform_solid=false, bool is_actuated_solid=false,
				bool is_lava_solid=true, bool is_dungeon_wall_solid=true ) {
			Tile tile = Main.tile[ world_x, world_y ];

			if( tile == null || !tile.active() ) {
				return true;
			}

			if( is_wall_solid && tile.wall > 0 ) {
				return false;
			}
			
			if( Main.tileSolid[ (int)tile.type ] ) {  // Solid
				if( !Main.tileSolidTop[(int)tile.type] || is_platform_solid ) {  // Non-platform
					if( !tile.inActive() || is_actuated_solid ) {  // Actuator not active
						return false;
					}
				}
			}

			// Special cases: Lava
			if( is_lava_solid && tile.lava() ) {
				return false;
			}

			if( is_dungeon_wall_solid ) {
				// Special cases: Lihzahrd Brick Wall
				if( tile.wall == 87 ) {
					return false;
				}
				// Special cases: Dungeon Walls
				if( (tile.wall >= 7 && tile.wall <= 9) || (tile.wall >= 94 && tile.wall <= 99) ) {
					return false;
				}
			}

			return true;
		}
	}
}
