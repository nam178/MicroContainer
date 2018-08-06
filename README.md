# MicroContainer - IoC

Dependency injection container, written for .NET 3.5 (Unity3d Projects).

# Features
* Mutable container, can override registrations at runtime.
* Supports named registration.
* Supports child container (Unit Of Work).
* Supports global singleton, per unit-of-work singleton and per named unit-of-work singleton.
* Syntax inspired by Autofac.

# Quick Start

First of all, create the root container. Make sure you include the MicroContainer namespace to enable its extension methods.

```csharp
using MicroContainer;
...
var root = Container.Create();
```

Then, register, or bind a concrete type to a service interface:

```csharp
root.Register<MachineGun>().As<IWeapon>();
```

From now on, components those depend on IWeapon, are injected with MachineGun, for example, this prints out "MachineGun"

```csharp
class PlayerController
{
    public PlayerController(IWeapon weapon) {
        Console.WriteLine(weapon.GetType().FullName);
    }
}

root.Register<PlayerController>().AsSelf();
root.Resolve<PlayerController>();
```

# Child Containers - Unit Of Work

Unit Of Work design pattern means a block of work should have a container object that holds references to services and data you need. So, after finishing the work, killing the container also frees the data and resources, avoid memory leaks.

For example, you have a screen that displays data from a database connection:

```csharp
class MyScreen
{
    public MyScreen(MyDatabaseConnection connection) {
    }
}

class MyDatabaseConnection : IDisposable
{
    public void Dispose() {
        /* closes the database connection */
    }
}
```

To ensure resources used by the screen, such as MyDatabaseConnection above, are properly released when the user closes the screen, you create the screen from a child container like this:

```csharp
var screenContainer = root.Create();
var screen = screenContainer.Resolve<MyScreen>();
// Then, display the screen as normal
AddToViewHierarchy(screen);
```

Notes: registrations from parent container are copied to the child container unless you overwrite them. In this case, MyScreen already registered from "root", so you don't have to register it again.

Once the user is done with the screen, i.e. clicking the 'Close' button, dispose the container to release resources.

```csharp
void CloseButton_Click(object sender, EventArgs e) {
    RemoveFromViewHierarchy(screen);
    screenContainer.Dispose();
}
```

# Singletons

Singleton can be registered by appending Singleton(), InstancePerContainer() and InstancePerNamedContainer() to the registration.

```csharp
// Every resolve call to IDevice results in the same ILaptop instance.
container.Register<Laptop>().As<IDevice>().Singleton();

// Every Resolve() call to ISessionInfo from the same container results in the same instance.
container.Register<SessionInfo>().As<ISessionInfo>().InstancePerContainer();
// This should be true:
var session = container.Create();
Console.WriteLine(object.ReferenceEquals(session.Resolve<ISessionInfo>(), session.Resolve<ISessionInfo>()));

// Every Resolve() call to ISessionInfo from contains with the same name or parent name results in the same instance.
container.Register<ClientInfo>().As<IClientInfo>().InstancePerContainer();
// This should be true:
var clientViewContainer = container.Register<ClientInfo>().As<IClientInfo>().InstancePerNamedContainer("peter");
var clientChildViewContainer = clientViewContainer.Create();
Console.WriteLine(
  object.ReferenceEquals(
    clientViewContainer.Resolve<IClientInfo>(), 
    clientChildViewContainer.Resolve<IClientInfo>()));

```

# Parameters and Lambda registration.

You can explicitly supply parameter to an registration, specifing the parameter using a match function:

```csharp
var container = Container.Create();

// If MachineGune is registered as the default weapon,
// this registration overwrites it and the player given a Sniper instead:
container
    .Register<PlayerController>()
    .WithParameter(p => p.Name == "weapon", (p, c) => new Sniper());
```

Also, customizing how your weapon is contructed with lambda registration:

```csharp

// The Sniper is now given 150 ammo for paying players!
container
    .Register<Sniper>(x => new Sniper { Ammo = 150 })
    .AsSelf()
    .AsImplementedInterfaces()
    ;
 ```

Happy Resolving!
