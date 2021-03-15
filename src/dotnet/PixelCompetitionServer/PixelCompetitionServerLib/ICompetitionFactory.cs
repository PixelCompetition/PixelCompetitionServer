using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace PixelCompetitionServerLib
{
	public class CompetitionMetaData
	{
		private readonly CultureInfo _culture;
		private readonly string _number;

		// internal
		[XmlIgnore()] public string Number => _number ?? "A001";

		[XmlIgnore()] public CultureInfo Culture => _culture ?? CultureInfo.InvariantCulture;

		//loaded
		public string Name { get; set; } = "undefined";
		public string Urls { get; set; } = "";
		public string Welcome { get; set; } = "This Competition does not have a welcome message";
		public string Help { get; set; } = "This Competition does not have a help text";

		public CompetitionMetaData()
		{

		}
		public CompetitionMetaData(string number = "", CultureInfo culture = null)
		{
			_number = number;
			_culture = culture ?? CultureInfo.InvariantCulture;
		}

		public void assign(StreamReader otherData)
		{
			var deserializer = new XmlSerializer(GetType());
			var loaded = deserializer.Deserialize(otherData) as CompetitionMetaData; assign(loaded);
		}

		public void assign(CompetitionMetaData other)
		{
			MergeHelper.copyValues(other, this);
		}

		public string createLanguageDisplay()
		{
			CultureInfo c = Culture;
			string code = c.TwoLetterISOLanguageName;
			string display = c.DisplayName;
			string name = c.EnglishName;
			return $"{code}-{display}({name})";
		}

	}
	public interface ICompetitionFactory
	{
        public string Number { get; set; }
		CompetitionMetaData getLanguage(string code);
		CompetitionMetaData getDefaultLanguage();
		IEnumerable<CompetitionMetaData> getLanguages();
		ICompetition generateCompetition(ICanvas canvas, ResponseWriter responseChannel);
		public void assignServer(PixelCompetitionServer server);
	}
}
