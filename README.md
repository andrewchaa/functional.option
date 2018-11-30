# functional.option

[![NuGet](https://buildstats.info/nuget/functional.option)](https://www.nuget.org/packages/functional.option/)

C# implementation of F# option

Null can be contagious and once a method returns null, the whole graph of related method calls requires null check. 
With Option<T>, you can make it explicit that this method should handle null type check. Coding becomes very pleasant when you do not have to worry of null return type.

### Option

```csharp
public async Task<Option<RestaurantEvents>> GetBy(
    Tenant tenant,
    RestaurantId restaurantId,
    RestaurantEventId restaurantEventId)
{
    using (var conn = _appSettings.ToTenantSpecificRestaurantEventsMySqlConnection(tenant))
    {
        await conn.OpenAsync();
        var restaurantEvent = await conn.QuerySingleOrDefaultAsync<RestaurantEvents>(
            @"SELECT *
                FROM RestaurantEvents
               WHERE EventId = @restaurantEventId
                 AND RestaurantId = @restaurantId",
            new
            {
                restaurantEventId = restaurantEventId.Id,
                restaurantId = restaurantId.Id
            });

        return restaurantEvent == null
            ? F.None
            : F.Some(restaurantEvent);
    }
}

var restaurantEvent = await _repository.GetBy(@event.Tenant.To<Tenant>(), 
        new RestaurantId(restaurantId), @event.RestaurantEventId);
await restaurantEvent.Match(
    None: () =>
    {
        _logger.LogWarning($"Cannot find an existing restaurant {restaurantId} for restaurant event {@event.RestaurantEventId}");
        return F.UnitAsync();
    },
    Some: e => _repository.Update(@event.Tenant.To<Tenant>(),
        e.EventId,
        e.RestaurantId,
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
