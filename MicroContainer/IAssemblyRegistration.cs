namespace MicroContainer
{
	public interface IAssemblyRegistration
	{
		/// <summary>
		/// Register each type in the assembly as themselves
		/// </summary>
		IAssemblyRegistration AsSelf();

		/// <summary>
		/// Register each type in the assembly as the interface they implement.
		/// </summary>
		IAssemblyRegistration AsImplementedInterfaces();
	}
}