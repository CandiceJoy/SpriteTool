namespace SpriteTool
{
	public class SpriteConflict
	{
		private string sprite1;
		private string sprite2;
		private string mod1;
		private string mod2;

		public SpriteConflict( string sprite1In, string sprite2In, string mod1In, string mod2In )
		{
			this.sprite1 = sprite1In;
			this.sprite2 = sprite2In;
			this.mod1    = mod1In;
			this.mod2    = mod2In;
		}

		public string toString()
		{
			return "[" + mod1 + "] " + sprite1 + " <---> " + sprite2 + " [" + mod2 + "]";
		}

		public bool matches( string sprite, string mod )
		{
			if( ( sprite == sprite1 && mod == mod1 )
			 || ( sprite == sprite2 && mod == mod2 ) )
			{
				return true;
			}

			return false;
		}

		public bool matches( SpriteConflict conflict )
		{
			return matches( conflict.sprite1, conflict.mod1 ) || matches( conflict.sprite2, conflict.mod2 );
		}

		public static bool doesConflict( string sprite1, string sprite2 )
		{
			if( sprite1.Equals(sprite2) )
			{
				return true;
			}

			if( ( sprite1.Length == 4 && sprite2.Length > 4 || sprite2.Length == 4 && sprite1.Length > 4 )
			 && sprite1.Substring( 0, 4 ) == sprite2.Substring( 0, 4 ) )
			{
				return true;
			}

			return false;
		}
	}
}
