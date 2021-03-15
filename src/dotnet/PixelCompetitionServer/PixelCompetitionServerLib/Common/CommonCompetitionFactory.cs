using System.Collections.Generic;

namespace PixelCompetitionServerLib.Common
{
	public class CommonCompetitionFactory : BaseCompetitionFactory
	{
		private readonly List<ICompetitionFactory> _runningCompetitionFactory;

		public CommonCompetitionFactory(IEnumerable<ICompetitionFactory> runningCompetitionFactories)
		{
			_runningCompetitionFactory = new List<ICompetitionFactory>(runningCompetitionFactories);
		}
		public override ICompetition generateCompetition(ICanvas canvas, ResponseWriter responseChannel)
		{
			return new CommonCompetition(canvas, responseChannel, this, _runningCompetitionFactory);
		}
	}
}