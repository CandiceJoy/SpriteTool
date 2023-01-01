namespace SpriteTool
{
	using System.Collections.ObjectModel;
	using System.IO.Compression;
	using System.IO.Enumeration;
	using System.Text;
	using System.Text.RegularExpressions;

	public class DirMod : SpriteContainer
	{
		public static Regex[]      spriteRegexes = new Regex[] { new Regex( @"^sprites/.*?([^/]+)\.\w+$", RegexOptions.IgnoreCase ), new Regex( @"^filter/Game-\w+/Sprites/.*?([^/]+)\.\w+$", RegexOptions.IgnoreCase ) };
		public static Regex        texturesRegex = new Regex("^TEXTURES\\.", RegexOptions.IgnoreCase);
		private       string       path;
		private       List<string> dirs    = new List<string>();
		private       List<string> files   = new List<string>();
		private       PatchSet     patches = new PatchSet();
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
				string file          = entry.FullName;
				Match  texturesMatch = DirMod.texturesRegex.Match( file );

				if( texturesMatch.Success )
				{
					StreamReader stream   = new StreamReader(entry.Open());
					string       text     = stream.ReadToEnd();
					this.patches.addPatches( text );
				}

				files.Add( file );
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
					Match spriteMatch = regex.Match( file );

					if( spriteMatch.Success )
					{
						//Console.Write(" - IS SPRITE: " + match.Groups[1].Value );
						this.sprites.Add( spriteMatch.Groups[1].Value );
						break;
					}
				}
				//Console.WriteLine();
			}

			this.patches.exec( sprites );
		}

		public List<string> getSprites()
		{
			return this.sprites;
		}
	}
}
