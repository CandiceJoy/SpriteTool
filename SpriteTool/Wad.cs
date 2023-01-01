//TODO: Add Texture check

namespace SpriteTool
{
	using System.ComponentModel;
	using System.Reflection.Metadata;
	using System.Text;
	using System.Text.RegularExpressions;

	public class Wad : SpriteContainer
	{
		public static Regex      markerRegex = new Regex( @"(\w\w?)_(START|END)" );
		private       string     path;
		private       byte[]     data;
		private       List<Lump> lumps   = new List<Lump>();
		List<Marker>             markers = new List<Marker>();
		private PatchSet         patches = new PatchSet();

		public Wad( string modNameIn, string pathIn )
		{
			modName = modNameIn;
			path         = pathIn;
		}

		public void load()
		{
			data = File.ReadAllBytes( path );
			//string[] hex   = BitConverter.ToString(File.ReadAllBytes( path )).Split( "-" );

			string wadType = getString( 0, 4 );
			Console.WriteLine( "Wad Type: '" + wadType + "'" );
			uint numLumps = getInt( 4 );
			Console.WriteLine( "numLumps: " + numLumps );
			uint directoryPtr = getInt( 8 );
			Console.WriteLine( "directoryPtr: " + directoryPtr );

			readLumps( directoryPtr, numLumps );
		}

		private void readLumps( uint directoryPtr, uint numLumps )
		{
			uint loc = directoryPtr;

			for( uint x = 0; x < numLumps; x++ )
			{
				uint   entryStart  = loc + ( x * 16 );
				uint   lumpStart   = getInt( entryStart );
				uint   lumpSize    = getInt( entryStart    + 4 );
				string lumpName    = getString( entryStart + 8, entryStart + 16 );
				Match  markerMatch = Wad.markerRegex.Match( lumpName );

				Console.WriteLine( "Lump Start: " + lumpStart );
				Console.WriteLine( "Lump Size: "  + lumpSize );
				Console.WriteLine( "Lump Name: "  + lumpName );

				if( markerMatch.Success )
				{
					string markerName  = markerMatch.Groups[1].Value;
					string markerType  = markerMatch.Groups[2].Value;
					Marker newMarker   = new Marker( markerName );
					bool   markerFound = false;

					//Console.WriteLine( "FOUND MARKER NAME: '" + markerName + "'" );

					foreach( Marker oldMarker in markers )
					{
						if( oldMarker.matches( newMarker ) )
						{
							if( markerType == "START" )
							{
								oldMarker.setStart( lumpStart );
							}
							else
							{
								oldMarker.setEnd( lumpStart );
							}

							markerFound = true;
							break;
						}
					}

					if( markerFound )
					{
						continue;
					}

					if( markerType == "START" )
					{
						newMarker.setStart( lumpStart );
					}
					else
					{
						newMarker.setEnd( lumpStart );
					}

					this.markers.Add( newMarker );
				}
				else
				{
					this.lumps.Add( new Lump( lumpName, lumpStart, getBytes( lumpStart, lumpStart + lumpSize ) ) );
				}
			}

			loadSprites();
		}

		public List<Lump> getLumps()
		{
			return this.lumps;
		}

		private void loadSprites()
		{
			foreach( Lump lump in lumps )
			{
				string lumpName = lump.getName();

				if( lumpName == "TEXTURES" )
				{
					this.patches.addPatches( lump.toString() );
				}
				else
				{
					foreach( Marker marker in markers )
					{
						if( marker.matches( "S" )
						 && marker.contains( lump ) )
						{
							sprites.Add( lumpName );
							break;
						}
					}
				}
			}

			this.patches.exec( sprites );
		}

		private string getString( uint start, uint end )
		{
			byte[] bytes = getBytes( start, end );
			return Encoding.UTF8.GetString( bytes, 0, bytes.Length );
		}

		private uint getInt( uint start )
		{
			byte[] bytes = getBytes( start, start + 4 );
			//BitConverter.
			return BitConverter.ToUInt32( bytes );
		}

		private byte[] getBytes( uint start, uint end )
		{
			return data.Skip( (int)start ).Take( (int)( end - start ) ).ToArray();
		}

		public List<string> getSprites()
		{
			return this.sprites;
		}
	}
}
