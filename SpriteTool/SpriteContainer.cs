namespace SpriteTool
{
	public class SpriteContainer
	{
		protected string       modName;
		protected List<string> sprites = new List<string>();

		public string getModName()
		{
			return this.modName;
		}

		public List<SpriteConflict> getConflictsWith(SpriteContainer container)
		{
			List<SpriteConflict> conflicts        = new List<SpriteConflict>();
			List<string> containerSprites = container.getSprites();

			for( int x = 0; x < this.sprites.Count; x++ )
			{
				for( int y = 0; y < containerSprites.Count; y++ )
				{
					if( container.getModName() == this.getModName()
					 && x         == y )
					{
						continue;
					}

					if( SpriteConflict.doesConflict( this.sprites[x], containerSprites[y] ))
					{
						conflicts.Add( new SpriteConflict( this.sprites[x], containerSprites[y], this.getModName(), container.getModName() ) );
					}
				}
			}

			return conflicts;
		}

		public List<string> getSprites()
		{
			return this.sprites;
		}

		public List<SpriteConflict> getConflicts()
		{
			return getConflictsWith( this );
		}
	}
}
