using System.Collections.Generic;

namespace PixelCompetitionServerLib
{
	public interface ICompetition
	{ 
		public class ProtocolError
		{
			public string Value;
			public string Message;
			public ProtocolError(string message, string value)
			{
				Message = message;
				Value = value;
			}
		}

		public class ProcessState
		{
			public bool HasError { get; private set; }
			private readonly IList<ProtocolError> _errors = new List<ProtocolError>();
			public IList<ProtocolError> Errors
			{
				get
				{
					if (HasError) return _errors;
					return null;
				}
			}
			public void protocolError(string message, string value)
			{
				HasError = true;
				Errors.Add(new ProtocolError(message, value));
			}

		}
		public bool processInput(string[] command, ProcessState state);
	}
}
