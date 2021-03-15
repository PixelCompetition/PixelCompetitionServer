namespace PixelCompetitionServerLib.B.B003
{
    // ReSharper disable once UnusedMember.Global
    public class TurtleBotFactory : BaseCompetitionFactory
	{
		public override ICompetition generateCompetition(ICanvas canvas, ResponseWriter responseWriter)
		{
			return new TurtleBot(canvas, responseWriter);
		}
	}
}
