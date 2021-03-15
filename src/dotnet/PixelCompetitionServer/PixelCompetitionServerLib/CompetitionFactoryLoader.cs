using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelCompetitionServerLib
{
	public class CompetitionFactoryLoader
	{
		private Dictionary<string, Type> _factoryTypes;

		private Dictionary<string, Type> FactoryTypes
		{
			get { return _factoryTypes ??= loadFactoryTypes(); }
		}

        private readonly Dictionary<string, ICompetitionFactory> Factories = new Dictionary<string, ICompetitionFactory>();

        private Dictionary<string, Type> loadFactoryTypes()
		{
			Dictionary<string, Type> res = new Dictionary<string, Type>();
			try
			{
				var iFaceType = typeof(ICompetitionFactory);
				var implTypes =
					GetType()
						.Assembly
						.GetTypes()
						.Where(p => iFaceType.IsAssignableFrom(p));
				foreach (var implType in implTypes)
				{
					string nameSpace = implType.Namespace;
					if (nameSpace == null) continue;
					var chunks = nameSpace.Split('.');
					if (chunks.Length < 2) continue;
					string number = chunks[^1];
					string group = chunks[^2];
					if (number.Length != 4) continue;
					if (group.Length != 1) continue;
					if (group[0] != number[0]) continue;
					var ctor = implType.GetConstructor(Type.EmptyTypes);
					if (ctor == null) continue;
					if (res.ContainsKey(number)) continue;
					res[number] = implType;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			return res;
		}

		public IEnumerable<ICompetitionFactory> createFactories(IEnumerable<string> numbers)
		{
			foreach (var number in numbers)
			{
				yield return createFactory(number);
			}
		}

		public ICompetitionFactory createFactory(string number)
		{
			try
			{
				number = number.Trim();
				Console.WriteLine($"Trying to find competition number {number}");

                if (Factories.ContainsKey(number)) return Factories[number];

				if (!FactoryTypes.ContainsKey(number))
				{
					Console.WriteLine($"Did not find {number}");
					foreach (string name in FactoryTypes.Keys)
					{
						Console.WriteLine(name);
					}
					return null;
				}

				Console.WriteLine($"Found competition number {number}");
				Type factoryType = FactoryTypes[number];
				ICompetitionFactory res = Activator.CreateInstance(factoryType) as ICompetitionFactory;
                if (res != null)
                {
                    res.Number = number;
                }
                Factories[number] = res; // Add also null to avoid retrying
                return res;
            }
            catch (Exception ex)
			{
				Console.WriteLine(ex);
				return null;
			}
		}
	}
}
