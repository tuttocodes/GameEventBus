# GameEventBus
A simple EventBus library in C# for Unity

# Basic Usage

```c#

// Create a unique class for each type of event
class PlayerMoveEvent : EventBase
{
  public Vector3 newPosition;

  public PlayerMoveEvent(Vector3 pos)
  {
    newPosition = pos;
  }

}

private void TestMethod()
{
  IEventBus eventBus = new EventBus();
  eventBus.Subscribe<PlayerMoveEvent>(OnPlayerMove); 
   
  eventBus.Publish(new PlayerMoveEvent(new Vector3(1,1,0)));// OnPlayerMove will be invoked

  eventBus.Unsubscribe(OnPlayerMove);
}

private void OnPlayerMove(PlayerMoveEvent playerMoveEvent)
{
  Console.WriteLine(playerMoveEvent.newPosition);
}

```
Each event subscription must be removed by calling EventBus.Unsubscribe(actionDelegate). This means in order to
unsubscribe from an event, you must keep a reference to your action delegate, or use a function reference.
Make sure to remove a subscription whenever it's script is going to be destroyed to avoid memory leaks.

# Example in Unity

```csharp



```
