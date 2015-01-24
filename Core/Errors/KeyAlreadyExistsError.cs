namespace Opentable.Features.Core.Errors
{
	public class RememberKeyAlreadyExistsError : MementoExceptionBase
	{
		public RememberKeyAlreadyExistsError(string key)
			: base(string.Format("Cannot Remember because something already using key {0}", key))
		{
		}
	}
}