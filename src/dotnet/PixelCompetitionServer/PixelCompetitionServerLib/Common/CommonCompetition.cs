using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelCompetitionServerLib.Common
{
	public class CommonCompetition : BaseCompetition
	{
		private readonly ICompetitionFactory _commonCompetitionFactory;
		private readonly List<ICompetitionFactory> _runningCompetitionFactory;
		private CompetitionMetaData _commonCompetitionMetaData;
		private List<CompetitionMetaData> _runningCompetitionMetaData;
		public CommonCompetition(
			ICanvas canvas,
			ResponseWriter responseChannel, 
			ICompetitionFactory commonCompetitionFactory,
			IEnumerable<ICompetitionFactory> runningCompetitionFactory):
			base(canvas, responseChannel)
		{
			_runningCompetitionFactory = new List<ICompetitionFactory>(runningCompetitionFactory);
			_runningCompetitionMetaData = 
				_runningCompetitionFactory.Select(f => f.getDefaultLanguage()).ToList();

			_commonCompetitionFactory = commonCompetitionFactory;
			_commonCompetitionMetaData = commonCompetitionFactory.getDefaultLanguage();

		}

		public override bool processInput(string[] command, ICompetition.ProcessState state)
		{
			switch (switchToken(command[0], new[] { "help", "lang", "competition" }, false, "command", state))
			{
					case 0:
					{
						ResponseChannel.writeLine(_commonCompetitionMetaData.Help);
						return true;
					}
					case 1:
					{
						if (command.Length == 1)
						{
							foreach (var displayLanguage in
								_runningCompetitionFactory.SelectMany(
									f => f.getLanguages()
										.Select(l => l.createLanguageDisplay())
										.Distinct()))
							{
								ResponseChannel.writeLine(displayLanguage);
							}
							return true;
						}
						else
						{
							string code = command[1].Trim();
							_commonCompetitionMetaData = _commonCompetitionFactory.getLanguage(code);
							ResponseChannel.writeLine(
								$"Set server language to {_commonCompetitionMetaData.createLanguageDisplay()}");
							_runningCompetitionMetaData = _runningCompetitionFactory.Select(c => c.getLanguage(code)).ToList();
							ResponseChannel.writeLine(
								"Set competition language to");
							foreach (var competitionMetaData in _runningCompetitionMetaData)
							{
								ResponseChannel.writeLine($"{competitionMetaData.Number} - {competitionMetaData.createLanguageDisplay()}");
							}
							return true;
						}
					}
					case 2:
					{
						if (command.Length == 1)
						{
							ResponseChannel.writeLine("Specify detail");
							return true;
						}
						else
						{
							switch (switchToken(command[1],
								new[] { "instruction", "urls", "name", "number" }, true, "competition detail", state))
							{
								case 0:
								{
									foreach (var m in _runningCompetitionMetaData)
									{
										ResponseChannel.writeLine($"{m.Number} - {m.Name}");
										ResponseChannel.writeLine();
										ResponseChannel.writeLine(m.Help);
										ResponseChannel.writeLine();
										ResponseChannel.writeLine(m.Urls);
									}
									return true;
								}
								case 1:
								{
									ResponseChannel.writeLine(
										String.Join(Environment.NewLine + Environment.NewLine,
											_runningCompetitionMetaData.Select(m => m.Urls)));
									return true;
								}
								case 2:
								{
									ResponseChannel.writeLine(
										String.Join(Environment.NewLine + Environment.NewLine,
											_runningCompetitionMetaData.Select(m => m.Name)));
									return true;
								}
								case 3:
								{
									ResponseChannel.writeLine(
										String.Join(Environment.NewLine + Environment.NewLine,
											_runningCompetitionMetaData.Select(m => m.Number)));
									return true;
								}
								default: return false; // should never happen
							}
						}
					}
					default: return false; // handled by running competition
			}
			// help
			// lang
			// lang code
			// competition instruction
			// competition 
			// competition name
			// competition 
		}

	}
}