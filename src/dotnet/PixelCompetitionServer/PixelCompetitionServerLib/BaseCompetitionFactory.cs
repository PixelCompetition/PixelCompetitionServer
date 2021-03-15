using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PixelCompetitionServerLib
{
	public abstract class BaseCompetitionFactory : ICompetitionFactory
	{
		protected PixelCompetitionServer Server { get; private set; }

		private const string DefaultLangCode = "en";
		private Dictionary<string, CompetitionMetaData> _metaDataByLangCode;

		protected Dictionary<string, CompetitionMetaData> MetaDataByLangCode
		{
			get { return _metaDataByLangCode ??= loadLanguages(); }
		}

		public Dictionary<string, CompetitionMetaData> loadLanguages()
		{
			Dictionary<string, CompetitionMetaData> res = new Dictionary<string, CompetitionMetaData>();
			Assembly assembly = this.GetType().Assembly;
			string nameSpace = GetType().Namespace;
			if (nameSpace == null) return res; // No namespace found

			string regEx = $"^{nameSpace.Replace(".","\\.")}\\.([a-z][a-z])\\.xml";
			Regex regex = new Regex(regEx);
			foreach (
				var element in assembly.GetManifestResourceNames()
					.Select(v => new {Name = v, Match = regex.Match(v)})
					.Where(o => o.Match.Success))
			{
				try
				{
					string code = element.Match.Groups[1].Value;
					if (res.ContainsKey(code)) continue;
					CompetitionMetaData newData = new CompetitionMetaData(
						nameSpace.Split('.').Last(),
						new CultureInfo(code));
					using Stream stream = assembly.GetManifestResourceStream(element.Name);
					if (stream == null) continue;
					using var reader = new StreamReader(stream);
					newData.assign(reader);
					res.Add(code, newData);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
			return res;
		}

        public string Number { get; set; }

        public CompetitionMetaData getLanguage(string code = null)
		{
			if (code == null) return getLanguage(DefaultLangCode);
			if (MetaDataByLangCode.ContainsKey(code)) return MetaDataByLangCode[code];
			if (MetaDataByLangCode.ContainsKey(DefaultLangCode)) return MetaDataByLangCode[DefaultLangCode];
			return new CompetitionMetaData();
		}

		public abstract ICompetition generateCompetition(ICanvas canvas, ResponseWriter responseChannel);
		public virtual void assignServer(PixelCompetitionServer server)
		{
			Server = server;
		}

		public CompetitionMetaData getDefaultLanguage()
		{
			return getLanguage();
		}

		public IEnumerable<CompetitionMetaData> getLanguages()
		{
			foreach (var competitionMetaData in MetaDataByLangCode.Values)
			{
				yield return competitionMetaData;
			}
		}
	}
}
