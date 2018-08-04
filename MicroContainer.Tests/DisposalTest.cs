using System;
using Xunit;

namespace MicroContainer.Tests
{
	public class DisposalTest
	{
		class MyDisposable : IDisposable
		{
			public bool IsDisposed
			{ get; private set; }

			public void Dispose()
			{
				IsDisposed = true;
			}
		}

		[Fact]
		public void Transient_Instances_Are_Disposed()
		{
			var container = Container.Create();
			container.Register<MyDisposable>().AsSelf().AsImplementedInterfaces();

			var disposable = container.Resolve<MyDisposable>();

			Assert.False(disposable.IsDisposed);
			container.Dispose();

			Assert.True(disposable.IsDisposed);
			
			Assert.Throws<ObjectDisposedException>(delegate
			{
				container.Create();
			});
			Assert.Throws<ObjectDisposedException>(delegate
			{
				container.AddDisposable(new MyDisposable());
			});
		}

		[Fact]
		public void Child_Containers_Are_Disposed_With_Parent()
		{
			var parent = Container.Create();
			parent.Register<MyDisposable>().AsSelf().AsImplementedInterfaces();

			var child = parent.Create();
			var instance = child.Resolve<MyDisposable>();

			parent.Dispose();
			Assert.True(instance.IsDisposed);
		}

		[Fact]
		public void Singleton_In_Parent_Are_Not_Disposed_Despite_Resolved_From_Child()
		{
			var parent = Container.Create();
			parent.Register<MyDisposable>().AsSelf().Singleton();

			var child = parent.Create();
			var instance = child.Resolve<MyDisposable>();

			child.Dispose();
			Assert.False(instance.IsDisposed);
		}
	}
}
