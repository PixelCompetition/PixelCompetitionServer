namespace PixelCompetitionServerLib.B.B001
{
    // ReSharper disable once UnusedMember.Global
    public class PixelFlutFactory : BaseCompetitionFactory
	{
		public override ICompetition generateCompetition(ICanvas canvas, ResponseWriter responseWriter)
		{
			return new PixelFlut(canvas, responseWriter);
		}
	}
}
