# Chain Linked Entity Objects (CLEOs)

Chain Link Extendable Objects

## What is this all about?

During development of a business application, I came accross some questions on how to design entities and their attached business logic best.

First, I followed the "Domain Driven Design" guidelines to design the entity classes which basically resulted in having public getter but only internal or private setter methods. Every manipulating action was convered by dedicated member functions, that do all the necessary validation before they allow the entity data to be modified.

So far so good!

But then another question came up. After having built the entities with strong validation, what would be the best way to add authorization to the system?
I didn't want to tie the entities directly to the security system of the application because I wanted the entities to be usable in a flexible way in other applications too what led me to a chain link based concept.

**This was the birthhour of CLEO!** And to keep it in my own mind, it was the 23rd February 2022!

Now, a lot of thinking has to be done and I'm going to share every step with you in this repository!  
You're welcome to share your thoughts, comments and critics with me! If there is already a well developed concept out there, please point me to it.  
Thank you!

## Goals

Find here a list of goals that I'd like to achieve by creating the CLEO concept.

- The main goal is providing simple usage of CLEOs and hiding complex logic in chain links.
- Chain links should allow to be small simple pieces of code that a developer can easily overview.
- Chain links must be unit-testable.
- Chain links (at least their interfaces) should be usable for multiple CLEOs.  
  For example, if there is a chain link type to modify a CLEOs name and there are multiple CLEOs supporting a name change, the validation chain link may be reusable if the validation rules for those CLEOs match.  
  If multiple CLEOs support the same chain type but need different chain link implementations, the chain builder should be aware of this and support CLEO type bound chain links.
- Chaining chain links to build the complete chain must be fast and stable.
- Every parts of a chain should be executable in an asynchronous way.
- It should be possible to attach chain links before and after the requested operation.
- It should be possible to define "generic chain links" that can be attached to every chain without the need to implement every interface or method.
- CLEOs should be able to create other CLEOs. This will be used to implement classes that access the data store and create CLEOs based on the loaded data. Creating new CLEOs is another use case.

The following goals came to my mind but are not of highest priority.  
I list them here to get feedback or at least don't forget them.

- CLEOs should be self-describing.  
  Which means there should be a way to query supported chain types from it.
- Limited execution chains/chain saw  
  It should be possible, that executing an action through a chain is possible only for a limited time. For that case, a CLEO should have some kind of chain saw to detach a grabbed chain at any time. If the caller wants to perform another action or the same action again, it has to grab a new chain.
- Chains should be allowed to add information to some kind of call context that another downstream chain link can read and use (for example to measure call times).

## Concept of CLEOs

### Basic usage of a CLEO

A CLEO must be designed based on "Domain Driven Design" guidelines which means it has public getter but only internal or private setter methods.  
Not doing so (having public setters) will drop the benefits of this concept as a caller has complete freedom to modify the values.  
In addition a CLEO like DDD-entities can also consist of multiple classes which is named an "aggregate" in DDD. The main type will then implement the CLEO interface and provide the required chains for manipulation.

```cs
public class SampleCleo: AbstractCleo<SampleCleo>
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
}
```

For any modification, the caller has to grab an according chain and execute the provided method.

```cs
// consider we already have an instance of SampleCleo referenced through the variable "cleo"
IChangeNameChain chain = await cleo.GrabChainAsync<IChangeNameChain>();
await chain.ChangeNameAsync("new name");
```

Behind this simple call to `ChangeName` on the `IChangeNameChain`, there may be multiple steps before the expected behavior will be executed.

Here are some examples:

1. The first chain link will check if a user is authenticated at all.
2. The second chain link will check, if the user has the necessary permission to change the name of the entity.
3. The third chain will check if the passed in name fulfills the naming rules.
4. The fourth chain will check if the entity is in a state that allows a name change.
5. The fifth chain link will write the name change to a protocol file.
6. Finally the sixth chain will perform the name change.

### Basic CLEO chain interface

```cs
public interface IChain
{
    Task<bool> CallingAsync();
    Task CalledAsync();
}

public interface IChangeNameChain : IChain
{
    Task<ChangeNameResult> ChangeNameAsync(string name);
}
```
















# Notes

- The chain builder will have to build a proxy object for every single chain link to direct the call through the chain. (`System.Reflection.DispatchProxy`)  
  https://devblogs.microsoft.com/dotnet/migrating-realproxy-usage-to-dispatchproxy/
- A chain should be built in a railway oriented way.

# Scratchpad

```cs
// add all types that implement chain links
services.AddChainLinks(assembly);
```

```cs
// Implements a chain link to any IDoSomething chain
public class DoSomethingChainLink : IDoSomething, IChainLink
{

}

// Implements a chain link that should be added to all chains
[UniversalChainLink]
public class DoSomethingForAllChainLink : IChainLink
{

}

// Implements a CLEO that doesn't accept universal chain links
[AttachUniversalChainLinks(false)]
public class MyEntityCleo : AbstractCleo<MyEntityCleo>
{

}

// Implements a CLEO that doesn't accept a specific universal chain links
[AttachUniversalChainLink(typeof(DoSomethingForAllChainLink), false)]
public class MyEntityCleo : AbstractCleo<MyEntityCleo>
{

}

// Implements a CLEO that only accepts a specific universal chain link
[AttachUniversalChainLinks(false)]
[AttachUniversalChainLink(typeof(DoSomethingForAllChainLink), true)]
public class MyEntityCleo : AbstractCleo<MyEntityCleo>
{

}
```

```cs
// add the data store CLEO to the DI container
services.AddScoped<IMyEntityDataStore, MyEntityDataStore>();

// The class MyEntityDataStore must call the base classes constructor
// that would setup the CLEO infrastructure for it.
// Creating a new CLEO from the data store will then need the caller to
// grab the ICreateMyEntity chain and call the according Create() method.
// As MyEntityDataStore and MyEntity will be implemented by the same developer,
// it's his responsibility to instanciate the MyEntity class and perform
// basic initialization.
// If any stored data should be loaded, the caller will grab the ILoadMyEntity
// chain and the logic behind the Load() method will be responsible
// to create the instance of MyEntity and set the loaded data to it's members.
```

