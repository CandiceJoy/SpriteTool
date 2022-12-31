namespace SpriteTool
{
	public class SpriteConflictList
	{
		private List<SpriteConflict> conflicts;

		public SpriteConflictList()
		{
			conflicts = new List<SpriteConflict>();
		}

		public SpriteConflictList( List<SpriteConflict> conflictsIn )
		{
			conflicts = conflictsIn;
		}

		public SpriteConflictList( List<SpriteContainer> containers )
		{
			conflicts = new List<SpriteConflict>();

			foreach( SpriteContainer container1 in containers )
			{
				addConflicts( container1.getConflicts() );

				foreach( SpriteContainer container2 in containers )
				{
					addConflicts( container1.getConflictsWith( container2 ) );
				}
			}
		}

		public void addConflict( SpriteConflict conflict )
		{
			if( !contains( conflict ) )
			{
				this.conflicts.Add( conflict  );
			}
		}

		public void addConflicts( List<SpriteConflict> conflicts )
		{
			foreach( SpriteConflict conflict in conflicts )
			{
				addConflict( conflict );
			}
		}

		private bool contains( SpriteConflict newConflict )
		{
			foreach( SpriteConflict conflict in conflicts )
			{
				if( conflict.matches( newConflict ) )
				{
					return true;
				}
			}

			return false;
		}

		public string toString()
		{
			string output = "";

			foreach( SpriteConflict conflict in this.conflicts )
			{
				output += conflict.toString() + "\n";
			}

			return output;
		}

		public bool hasConflicts()
		{
			return this.conflicts.Count > 0;
		}
	}
}
