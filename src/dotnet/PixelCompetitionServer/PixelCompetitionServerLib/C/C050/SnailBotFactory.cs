namespace PixelCompetitionServerLib.C.C050
{
    // ReSharper disable once UnusedMember.Global
    public class SnailBotFactory : BaseCompetitionFactory
	{
		public override ICompetition generateCompetition(ICanvas canvas, ResponseWriter responseWriter)
		{
			return new SnailBot(canvas, responseWriter);
		}
	}
}
