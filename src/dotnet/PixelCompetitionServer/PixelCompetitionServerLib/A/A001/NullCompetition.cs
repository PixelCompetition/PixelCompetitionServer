namespace PixelCompetitionServerLib.A.A001
{
	public class NullCompetition : BaseCompetition
	{
		public NullCompetition(ICanvas canvas, ResponseWriter responseWriter) : base(canvas, responseWriter)
		{
		}

		public override bool processInput(string[] command, ICompetition.ProcessState state)
		{
			ResponseChannel.writeLine("Does nothing");
			return true;
		}
	}
}