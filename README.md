# functional.option

[![NuGet](https://buildstats.info/nuget/FunctionalWay)](https://www.nuget.org/packages/FunctionalWay/)

C# implementation of F# option

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

### Either

```csharp
Regex _bicRegex = new Regex("[A-Z]{11}");

Either<string, BookTransfer> Handle(BookTransfer transfer)
    => F.Right(transfer)
        .Bind(ValidateBic)
        .Bind(ValidateDate);

Either<string, BookTransfer> ValidateBic(BookTransfer transfer)
{
    if (!_bicRegex.IsMatch(transfer.Bic))
    {
        return "not in bic format";
    }

    return transfer;
}

Either<string, BookTransfer> ValidateDate(BookTransfer transfer)
{
    if (transfer.Date <= DateTime.Now)
    {
        return "Date is in the past";
    }

    return transfer;
}

[Fact]
public void Should_bind_function_calls_passing_correct_value()
{
    var transfer1 = new BookTransfer("ABCDEFGHIJK", DateTime.Now.AddMinutes(1));

    Assert.Equal(F.Right(transfer1).ToString(), Handle(transfer1).ToString());
}

```
