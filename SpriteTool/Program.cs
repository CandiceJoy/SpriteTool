using System.Collections;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Text.RegularExpressions;
using SpriteTool;

class Program
{
	public static Regex wadRegex = new Regex(@"([^/]+)\.wad$",             RegexOptions.IgnoreCase);
	public static Regex zipRegex = new Regex(@"([^/]+)\.(?:pk3|zip|pk7)$", RegexOptions.IgnoreCase);
	public static Regex dirRegex = new Regex( "[^/]+([^/]+)$", RegexOptions.IgnoreCase);

	static void Main( string[] args )
	{
		foreach( string arg in args )
		{
			if( !File.Exists( arg )
			 && !Directory.Exists( arg ) )
			{
				throw new FileNotFoundException( arg + " does not exist" );
			}
		}

		List<SpriteConflict>          spriteConflicts  = new List<SpriteConflict>();
		List<SpriteContainer> containers = new List<SpriteContainer>();

		Console.WriteLine("Loading files...");

		foreach( string arg in args )
		{
			FileAttributes attributes = File.GetAttributes( arg );

			Match zipMatch = Program.zipRegex.Match( arg );
			Match wadMatch = Program.wadRegex.Match( arg );
			Match dirMatch = Program.dirRegex.Match( arg );

			if( ( attributes & FileAttributes.Directory ) == FileAttributes.Directory )
			{
				Console.WriteLine( "[Dir] " + arg );
				containers.Add( new DirMod( dirMatch.Groups[1].Value, arg ) );
			}
			else if( zipMatch.Success )
			{
				Console.WriteLine( "[Zip] " + arg );
				containers.Add( new DirMod( zipMatch.Groups[1].Value, ZipFile.OpenRead( arg ) ) );
			}
			else if( wadMatch.Success)
			{
				Console.WriteLine( "[Wad] " + arg );
				containers.Add( new Wad(wadMatch.Groups[1].Value,arg ) );
			}
			else
			{
				Console.WriteLine( "[???] " + arg );
			}
		}

		if( containers.Count == 1 )
		{
			List<string> sprites = containers[0].getSprites();

			foreach( string sprite in sprites )
			{
				Console.WriteLine(sprite);
			}
		}
		else
		{
			Console.WriteLine("Finding conflicts...");
			SpriteConflictList conflicts = new SpriteConflictList(containers);

			if( conflicts.hasConflicts() )
			{
				Console.WriteLine("-----Conflicts-----");
				Console.WriteLine(conflicts.toString());
			}

			Console.WriteLine("DONE");
		}
	}
}
