
using Xunit;

namespace MicroContainer.Tests
{
	public class PerNamedContainerInstanceTest
	{
		[Fact]
		public void Throw_Exception_If_Not_Registered()
		{
			var container = Container.Create();
			container
				.Register<ExampleConcrete>()
				.As<IExampleInterface>()
				.InstancePerNamedContainer("helloWorld");

			Assert.Throws<DependencyException>(() => container.Resolve<IExampleInterface>());

			Assert.Throws<DependencyException>(() => container.Create().Resolve<IExampleInterface>());

			Assert.Throws<DependencyException>(() => container.Create("hello").Resolve<IExampleInterface>());
		}

		[Fact]
		public void Two_SameName_Containers_Still_Give_Different_Instances()
		{
			var rootContainer = Container.Create();

			rootContainer
					.Register<ExampleConcrete>()
					.AsImplementedInterfaces()
					.InstancePerNamedContainer("helloWorld");

			var container1 = rootContainer.Create("helloWorld");
			var container2 = rootContainer.Create("helloWorld");

			Assert.NotSame(
				container1.Resolve<IExampleInterface>(), 
				container2.Resolve<IExampleInterface>()
				);
		}

		[Fact]
		public void Named_Container_Returns_Same_Instance_Everytime()
		{
			var container = Container.Create();
			container
					.Register<ExampleConcrete>()
					.AsImplementedInterfaces()
					.InstancePerNamedContainer("helloWorld");
			var child = container.Create("helloWorld");
			Assert.Same(
				child.Resolve<IExampleInterface>(),
				child.Resolve<IExampleInterface>()
				);
		}

		[Fact]
		public void Child_Container_Of_Named_Container_Returns_Instance_From_Its_Parent()
		{
			var container = Container.Create();
			container
					.Register<ExampleConcrete>()
					.AsImplementedInterfaces()
					.InstancePerNamedContainer("helloWorld");
			var child = container.Create("helloWorld");

			Assert.Same(
				child.Create("notHelloWorld").Resolve<IExampleInterface>(),
				child.Resolve<IExampleInterface>());

			Assert.Same(
				child.Create(null).Resolve<IExampleInterface>(),
				child.Resolve<IExampleInterface>());

			Assert.NotSame(
				child.Create("helloWorld").Resolve<IExampleInterface>(),
				child.Resolve<IExampleInterface>());
		}

		class DependantOnExampeInterface
		{
			public IExampleInterface Dep
			{ get; private set; }

			public DependantOnExampeInterface(IExampleInterface dep) {
				Dep = dep;
			}
		}

		[Fact]
		public void Resolving_From_Dependency_Test()
		{
			var container = Container.Create();
			container
					.Register<ExampleConcrete>()
					.AsImplementedInterfaces()
					.InstancePerNamedContainer("helloWorld");

			container.Register<DependantOnExampeInterface>().AsSelf();

			Assert.Throws<DependencyException>(() => container.Resolve<DependantOnExampeInterface>());

			var helloWorldContainer = container.Create("helloWorld");

			Assert.Same(
				helloWorldContainer.Resolve<IExampleInterface>(),
				helloWorldContainer.Create("blah").Resolve<DependantOnExampeInterface>().Dep
				);
		}
	}
}
