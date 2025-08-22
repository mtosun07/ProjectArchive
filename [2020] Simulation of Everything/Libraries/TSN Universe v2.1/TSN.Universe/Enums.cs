using System;

namespace TSN.Universe
{
    [Flags] public enum SimulationOptions : ulong
    {
        SpawnThingsFirst = 0,
        SpawnFoodsFirst = 1,
        SpawnAtTheSameTime = 2,
        ThingsGetSpawned = 4,
        FoodsGetSpawned = 8,
        AllGetSpawnedAtEverySimulation = ThingsGetSpawned | FoodsGetSpawned,
        ThingsGetKilled = 16,
        FoodsGetKilled = 32,
        AllGetKilledAtEverySimulation = ThingsGetKilled | FoodsGetKilled,
        ReturnFoodAfterDeath = 64,
        ReturnFoodIfNotHungry = 128 | ReturnFoodAfterDeath,
        DontDieIfHungry = 256,
        CanReproduce = 512,
        CanReproduceIfHungry = 1024 | CanReproduce,
        CanReproduceMoreThanOne = 2048 | CanReproduce,
        ReplaceThingWithExistingFood = 4096,
        ReplaceThingWithExistingThing = 8192,
        ReplaceThingWithAny = ReplaceThingWithExistingThing | ReplaceThingWithExistingFood,
        ReplaceFoodWithExistingThing = 16384,
        EatFoodIfExists = 32768 | ReplaceThingWithExistingFood,
        DecideLocationByIgnoringThings = 65536,
        DecideLocationByIgnoringFoods = 131072,
        DecideLocationByIgnoringExistings = DecideLocationByIgnoringThings | DecideLocationByIgnoringFoods,
        MoveToEat = 262144,
        GendersAvailable = 524288,
        Default = SpawnAtTheSameTime | AllGetSpawnedAtEverySimulation | AllGetKilledAtEverySimulation | ReturnFoodIfNotHungry | CanReproduce | EatFoodIfExists | DecideLocationByIgnoringFoods | MoveToEat | GendersAvailable
    }
    public enum FoodReturningOptions : ulong
    {
        DontReturn = 0,
        ReturnAlways = SimulationOptions.ReturnFoodAfterDeath,
        ReturnIfNotHungry = SimulationOptions.ReturnFoodIfNotHungry
    }
    public enum DeathReasons : byte
    {
        NONE = 0,
        ByChance = DeathReasonsForThings.ByChance,
        Starved = DeathReasonsForThings.Starved,
        ThingReplacedToThing = DeathReasonsForThings.ReplacedToThing,
        ThingReplacedToFood = DeathReasonsForThings.ReplacedToFood,
        DueToSimulation = DeathReasonsForFoods.DueToSimulation,
        Eaten = DeathReasonsForFoods.Eaten,
        FoodReplacedToThing = DeathReasonsForFoods.ReplacedToThing
    }
    public enum DeathReasonsForThings : byte
    {
        ByChance = 1,
        Starved = 2,
        ReplacedToThing = 3,
        ReplacedToFood = 4
    }
    public enum DeathReasonsForFoods : byte
    {
        DueToSimulation = 5,
        Eaten = 6,
        ReplacedToThing = 7
    }
    public enum MagnitudeTypes : byte
    {
        Summary = 0,
        Average = 1
    }
}