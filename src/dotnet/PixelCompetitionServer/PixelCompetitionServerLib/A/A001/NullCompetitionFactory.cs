namespace PixelCompetitionServerLib.A.A001
{
    // ReSharper disable once UnusedMember.Global
    public class NullCompetitionFactory : BaseCompetitionFactory
	{
		public override ICompetition generateCompetition(ICanvas canvas, ResponseWriter responseChannel)
		{
			return new NullCompetition(canvas, responseChannel);
		}
	}
}
