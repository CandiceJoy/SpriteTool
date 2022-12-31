namespace SpriteTool
{
	public class Marker
	{
		private string name;
		private uint   start;
		private uint   end;

		public Marker( string nameIn )
		{
			if( nameIn.Length == 1 )
			{
				nameIn = nameIn + nameIn;
			}

			name       = nameIn;
		}

		public string getName()
		{
			return name;
		}

		public uint getStart()
		{
			return this.start;
		}

		public uint getEnd()
		{
			return this.end;
		}

		public void setStart( uint startIn )
		{
			start = startIn;
		}

		public void setEnd( uint endIn )
		{
			if( name == "SS" )
			{
				Console.WriteLine("SPRITE MARKER: " + start + " to " + endIn);
			}
			end = endIn;
		}

		public bool matches( Marker marker )
		{
			return name == marker.name;
		}

		public bool matches( string markerName )
		{
			if( markerName.Length == 1 )
			{
				markerName = markerName + markerName;
			}

			return markerName == this.name;
		}

		public bool contains( Lump lump )
		{
			return lump.getStart() >= start && lump.getStart() < end;
		}
	}
}
