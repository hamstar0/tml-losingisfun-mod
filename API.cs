﻿namespace LosingIsFun {
	public static class LosingIsFunAPI {
		public static LosingIsFunConfigData GetModSettings() {
			return LosingIsFunMod.Instance.ConfigJson.Data;
		}
	}
}

