# functional.option

[![NuGet](https://buildstats.info/nuget/functional.option)](https://www.nuget.org/packages/functional.option/)

C# implementation of F# option

Null is contagious and once a method returns null, the whole graph of related method calls requires null check. 
With Option<T>, you can make it explicit that this method should handle null type check. Coding becomes very pleasant when you do not have to worry of null return type.

The symbolic definition is as follows

```csharp
Option<T> = None | Some(T)
```

* None: a special value that indicates the absence of a value. The Option is none, if it has no inner value.
* Some(T): a container that wraps the value of type T. The Option is Some if if has an inner value.

Some functional languages use Maybe for Option and it's the same concept.

### examples

Taking Option as parameter

```csharp
[Fact]
public void Should_handle_option_T()
{
    // arrange
    Option<string> _ = F.None;
    Option<string> batman = F.Some("Batman");

    // act
    var greet1 = greet(_);
    var greet2 = greet(batman);

    // assert
    Assert.Equal("Sorry, who?", greet1);
    Assert.Equal("Hello, Batman", greet2);
}

string greet(Option<string> greetee)
    => greetee.Match(
        None: () => "Sorry, who?",
        Some: (name) => $"Hello, {name}"
    );

```

Returning a value from the database wrapped in Option

```csharp
public async Task<Option<Events>> GetBy(
    tenant t,
    Id id,
    EventId eventId)
{
    using (var conn = _appSettings.ToSqlConnection(t))
    {
        await conn.OpenAsync();
        var event = await conn.QuerySingleOrDefaultAsync<Events>(
            @"SELECT *
                FROM Events
               WHERE EventId = @eventId
                 AND id = @id",
            new
            {
                eventId = eventId.Id,
                id = id.Id
            });

        return event == null
            ? F.None
            : F.Some(event);
    }
}

var event = await _repository.GetBy(@event.t.To<t>(), 
        new Id(id), @event.eventId);
await event.Match(
    None: () => 
    {
        return F.UnitAsync();
    },
    Some: e => _repository.Update(@event.t.To<t>(),
        e.EventId,
        e.Id,
        EventStatus.Completed));
}

```

You can use Map to apply func to the inner value.

```csharp
TempOfflineType = TempOffline.Map(v => v.To<TempOfflineType>());
```

### IsSome, IsNone

```csharp
duration.Match(
    None: () => { },
    Some: d =>
        DateTime.Today.AddDays(1).AddHours(5)
            .Pipe(tomorrow5Am => LocalDateTime.FromLocal(tenant, tomorrow5Am))
            .Pipe(localDateTime => new EventAction(localDateTime, localDateTime.ToUtc(), ActionType.BringOnline))
            .Pipe(eventAction => Actions.Add(eventAction))
    );

if (duration.IsSome || !endDateTime.HasValue)
    return;

```
