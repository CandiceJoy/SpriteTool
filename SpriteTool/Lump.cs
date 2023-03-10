namespace SpriteTool
{
	using System.Text;

	public class Lump
	{
		private string name;
		private byte[] bytes;
		private uint   start;

		public Lump( string nameIn, uint startIn, byte[] bytesIn )
		{
			//Console.WriteLine("Lump " + nameIn);
			name  = nameIn.Trim().Replace( " ","" );
			start = startIn;
			bytes = bytesIn;
		}

		public String toString()
		{
			return Encoding.UTF8.GetString( bytes, 0, bytes.Length );
		}

		public string getName()
		{
			return this.name;
		}

		public uint getStart()
		{
			return this.start;
		}

		public uint getEnd()
		{
			return (uint)(start + this.bytes.Length);
		}
	}
}
