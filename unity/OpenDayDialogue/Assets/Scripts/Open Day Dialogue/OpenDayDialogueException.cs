using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OpenDayDialogue
{
	[Serializable]
	internal class OpenDayDialogueException : Exception
	{
		public OpenDayDialogueException()
		{
		}

		public OpenDayDialogueException(string message) : base(message)
		{
		}

		public OpenDayDialogueException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected OpenDayDialogueException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}