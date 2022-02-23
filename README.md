# Chain Linked Entity Objects (CLEOs)

## What is this all about?

During development of a business application, I came accross some questions on how to design entities and their attached business logic best.

First, I followed the "Domain Driven Design" guidelines to design the entity classes which basicalls resulted in having public getter but only internal or private setter methods. Every manipulating action was convered by dedicated member functions, that did all the necessary validation before they alowed the entity data to be modified.

So far so good!

But then another question came up. After having built the entities with strong validation, what would be the best way to add authorization to the system?
I didn't want to tie the entities directly to the security system of the application because I wanted the entities to be usable in a flexible way in other applications too what led me to a chain link based concept.

**This was the birthhour of CLEO!** And to keep it in my own mind, it was the 23rd February 2022!

Now, a lot of thinking has to be done and I'm going to share every step with you in this repository!  
Please share your thoughts, comments and critics with me! If there is already a well developed concept out there, please point me to it.  
Thank you!

## Goals

Find here a list of goals that I'd like to achieve by creating the CLEO concept.

- The main goal is providing simple usage of CLEOs and hiding complex logic in chain links.
- Chain links should allow to be small simple pieces of code that a developer can easily overview.
- Chain links must be unit-testable.
- Chain links (at least their interfaces) should be usable for multiple CLEOs.  
  For example, if there is a chain type to modify a CLEOs name and there are multiple CLEOs supporting a name change, the validation chain link may be reusable if the validation rules for those CLEOs match.  
  If multiple CLEOs support the same chain type but need different chain link implementations, the chain builder should be aware of this and support CLEO type bound chain links.
- Chaining chain links to build the complete chain must be fast and stable.
- Every parts of a chain should be executable asynchronous.

The following goals came to my mind but are not of highest priority.  
I list them here to get feedback or at least don't forget them.

- CLEOs should be self-describing.  
  Which means there should be a way to query supported chain types from it.
- Limited execution chains/Chain saw  
  It should be possible, that executing an action through a chain is possible only a limited time. For that case, a CLEO should have some kind of chain saw to detach a grabbed chain at any time. If the caller wants to perform another action or the same action again, it has to grab a new chain.

## Concept of CLEOs

### Basic usage of a CLEO

A CLEO must be designed based on "Domain Driven Design" guidelines which means it has public getter but only internal or private setter methods.  
Not doing so (having public setters) will drop the benefits of this concept as a caller has complete freedom to modify the values.  
In addition a CLEO like DDD-entities can also consist of multiple classes which is named an "aggregate" in DDD. The mail type will then implement the CLEO interface and provide the required chains for manipulation.

```cs
public class SampleEntity: AbstractCleo<SampleEntity>
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
}
```

For any modification, the caller has to grab an according chain and execute the provided method.

```cs
// consider whe already have an instance of SampleEntity referenced through the variable "entity"
IChangeName chain = entity.GrabChain<IChangeName>();
chain.ChangeName("new name");
```

