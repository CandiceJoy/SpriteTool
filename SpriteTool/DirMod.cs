namespace SpriteTool
{
	using System.Collections.ObjectModel;
	using System.IO.Compression;
	using System.Text.RegularExpressions;

	public class DirMod : SpriteContainer
	{
		public static Regex[]      spriteRegexes = new Regex[] { new Regex( @"^sprites/.*?([^/]+)\.\w+$", RegexOptions.IgnoreCase ), new Regex( @"^filter/Game-\w+/Sprites/.*?([^/]+)\.\w+$", RegexOptions.IgnoreCase ) };
		private       string       path;
		private       List<string> dirs    = new List<string>();
		private       List<string> files   = new List<string>();

		public DirMod( string modNameIn, string pathIn )
		{
			modName = modNameIn;
			path    = pathIn;
			this.dirs.Add( path );
			loadDir();
		}

		public DirMod(string modNameIn, ZipArchive archive)
		{
			modName = modNameIn;
			ReadOnlyCollection<ZipArchiveEntry> entries = archive.Entries;

			foreach( ZipArchiveEntry entry in entries )
			{
				files.Add( entry.FullName );
			}

			loadSprites();
		}

		private void loadDir()
		{
			int ptr = 0;

			while( ptr < this.dirs.Count )
			{
				List<string> newDirs = new List<string>( Directory.GetDirectories( this.dirs[ptr] ) );

				foreach( string dir in newDirs )
				{
					if( !this.dirs.Contains( dir ) )
					{
						this.dirs.Add( dir );
					}
				}

				ptr++;
			}

			loadFiles();
		}

		private void loadFiles()
		{
			foreach( string dir in dirs )
			{
				List<string> newFiles = new List<string>( Directory.GetFiles( dir ) );

				foreach( string newFile in newFiles )
				{
					if( !this.files.Contains( newFile ) )
					{
						this.files.Add( newFile.Replace( this.path, "" ).Substring( 1 ) );
					}
				}
			}

			loadSprites();
		}

		private void loadSprites()
		{
			foreach( string file in files )
			{
				//Console.Write( file );

				foreach( Regex regex in DirMod.spriteRegexes )
				{
					Match match = regex.Match( file );

					if( match.Success )
					{
						//Console.Write(" - IS SPRITE: " + match.Groups[1].Value );
						this.sprites.Add( match.Groups[1].Value );
						break;
					}
				}

				//Console.WriteLine();
			}
		}

		public List<string> getSprites()
		{
			return this.sprites;
		}
	}
}
