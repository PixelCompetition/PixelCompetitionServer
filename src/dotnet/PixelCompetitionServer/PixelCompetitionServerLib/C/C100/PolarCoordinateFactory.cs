namespace PixelCompetitionServerLib.C.C100
{
    // ReSharper disable once UnusedMember.Global
    public class PolarCoordinateFactory : BaseCompetitionFactory
	{
		public override ICompetition generateCompetition(ICanvas canvas, ResponseWriter responseWriter)
		{
			return new PolarCoordinate(canvas, responseWriter);
		}
	}
}
