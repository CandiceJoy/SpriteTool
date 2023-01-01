namespace SpriteTool
{
	using System.Text.RegularExpressions;

	public class PatchSet
	{
		public  Regex                      patchRegex = new Regex("sprite\\s\"([a-z0-9]{1,8})\",\\s?\\d+,\\s?\\d+\\s*\\{[^\\}]*?patch\\s\"([a-z0-9]{1,8})\",\\s?\\d+,\\s?\\d+\\s*\\}", RegexOptions.IgnoreCase | RegexOptions.Singleline);
		private Dictionary<string, string> patches    = new Dictionary<string, string>();

		public PatchSet()
		{

		}

		public void addPatches(string contents)
		{
			MatchCollection matches = this.patchRegex.Matches( contents );

			foreach( Match match in matches )
			{
				string before = match.Groups[1].Value;
				string after  = match.Groups[2].Value;

				if( before == after )
				{
					continue;
				}

				this.patches.Add(before,after  );
			}
		}

		public void exec( List<string> src )
		{
			src.Sort();

			foreach( string before in this.patches.Keys )
			{
				string after = this.patches[before];

				while( src.Contains( before ) )
				{
					src[src.BinarySearch( before )] = after;
				}
			}
		}
	}
}
