using System;
using Xunit;

namespace MicroContainer
{
	public class RegistrationWithLambdaMethodTest
	{
		class MyCustomClass : IDisposable
		{
			public int DisposedCount
			{ get; set; }

			public string ExampleProperty
			{ get; set; }

			public void Dispose()
			{
				DisposedCount++;
			}
		}

		[Fact]
		public void Container_Uses_The_Lambda_Method_To_Activate_Instance()
		{
			var container = Container.Create();

			container
				.Register<MyCustomClass>(x => new MyCustomClass() { ExampleProperty = "CreatedUsingLambda" })
				.AsSelf();

			Assert.Equal("CreatedUsingLambda", container.Resolve<MyCustomClass>().ExampleProperty);
		}

		[Fact]
		public void Lambda_Created_Instances_Are_Disposed_With_Its_Container()
		{
			var container = Container.Create();
			var childContainer = container.Create("Child");

			childContainer
				.Register<MyCustomClass>(x => new MyCustomClass() { ExampleProperty = "CreatedUsingLambda" })
				.AsSelf();

			var inst = childContainer.Resolve<MyCustomClass>();
			childContainer.Dispose();
			Assert.Equal(1, inst.DisposedCount);
		}
	}
}
