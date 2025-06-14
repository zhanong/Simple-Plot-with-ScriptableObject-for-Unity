![](img/logo.png)
# Introduction

This is a flexible tool for creating and managing sequential game events. It allows you to visually build tutorials, quests, and other guided experiences with ease.

**Use it to handle common scenarios like:**

  - **Interactive Tutorials:** Create a sequence of instructions that guide the player. The system will wait for the player to complete a specific action (like moving, aiming, or using an item) before automatically advancing to the next step.
  - **Multi-Stage Quests:** Design complex quests with multiple objectives and even nested sub-tasks. Trigger custom game reactions—like spawning rewards, opening doors, or playing a cutscene—as each task is completed.

This tool empowers you to manage these event-driven sequences intuitively. You can build out your entire plot, by simply creating and linking custom **ScriptableObjects** with drag-and-drop.

# Structure

Plots are organized using the following hierarchy:

![](img/Thread%20Structure.png)

`Thread`, `SubThread`, and `Event` all inherit from a common `ThreadBase` class, meaning their core logic is very similar.

The `ThreadBase` class is structured as follows:

![](img/ThreadBase.png)

Here, a `Thread` asset contains `SubThread` assets as its condition, a `SubThread` asset contains `Events` assets as its condition, and an `Event` asset contains `Condition` assets as its condition.

`Condition` and `Action` are implemented as customizable `ScriptableObjects` that you create to define your game's logic.

The entire system is managed by a `PlotBase` **MonoBehaviour** component. It is responsible for:

1. Initializing and storing all plot data (`Threads`, `Events`, etc.).
2. Continuously checking `Conditions` and executing `Actions`.
3. Providing helper utilities.

# Usage

## 1. Create a script that inherits from `PlotBase` and attach it to a `GameObject`

```csharp
public class Plot : PlotBase
{
    void Start()
    {
        // Set to true to begin the plot sequence
        sceneStarted = true;
    }
    
    // You can override virtual methods such as OnWin, OnLose, OnUpdate here
}
```

## 2. Create a `Thread` asset and  assign it to the `PlotBase` component

![](img/plot%20inspector.png)

You can also set the **Update period** here.

## 3. Create `SubThread` and `Event` assets to build your plot

Drag `SubThread` asset to the `Thread` asset's list.

Here you can also set the **Lose Condition**, which are checked throughout the scene.

![](img/Thread%20inspector.png)

>***Is Linear***
>- **Checked**: `SubThreads` start one by one, only after the previous `SubThread` has finished.
>- **Unchecked**: All `SubThread` in the list starts simultaneously.

<br>

Drag `Event` assets to `Subthread` assets's list.

![](img/SubThread%20inspector.png)

## 4. Customize your `Condition` classes

For each kind of condition, you need to define two classes: one inheriting from `ConditionAsset` (`ScriptableObject`) and one inheriting from `Condition` (which contains the logic).

Here is an example that checks if a certain amount of time has passed.

```c#
// This is the ScriptableObject class that appears in the Inspector
public class TimeUpAsset : ConditionAsset
{
    // Define custom fields to set in the Inspector 
    public float targetTime;

    protected override Condition GetCondition()
    {
        // Return a new instance of the logic class
        return new TimeUp();
    }
}

// This class contains the actual condition logic
public class TimeUp : Condition
{
    // Defind the main logic here
    public override bool CheckCondition(PlotBase plot)
    {
        // Access fields from the ConditionAsset via the `asset` property
        var a = asset as TimeUpAsset;

        // Return true if the condition is satisfined.
        return Time.time >= a.targetTime
    }
}
```

## 5. Customize your `Action` classes

Similarly, you need to define two classes for each kind of action. Here is an example that activates a target `GameObject`.

```c#
// This is the ScriptableObject class that appears in the Inspector
public class SetActiveAsset : ActionAsset 
{
    public string targetName;

    protected override Action GetAction()
    {
        // Return a new instance of the logic class
        return new SetActive();
    }
}

// This class contains the actual action logic
public class SetActive : Action
{
    // Define the main logic here
    public override void Act(PlotBase plot)
    {
        // Access fields from the ActionAsset via the `asset` property
        var a = asset as SetActiveAsset;

        // Find the target by name
        var go = plot.GetPlotObject(target.name);
        if (go != null)
            go.SetActive(true);
    }
}

```

To allow `PlotBase` to find GameObjects via `GetPlotObject`, simply attach the `PlotObject` component to those GameObjects. The `PlotBase` will register them automatically.

![](img/go%20inspector.png)



## 6. Create and assign your `Action` and `Condition` assets

![](img/Event%20inspector.png)

You can also drag `Action` assets to the **Pre Actions** fields of other assets.

# # Advanced Usage: Coroutine Actions

You can create `Actions` that run as Coroutines. This allows you to define long-running actions (e.g., animations, fades) and control whether subsequent `Actions` should wait for them to complete.

Here is an example that gradually sets a UI Slider's value to 0.

```c#
public class ResetSliderAsset : ActionAsset
{
    public string targetName;
    public float speed = 1f;

    protected override Action GetAction()
    {
        return new ResetSlider();
    }
}

public class ResetSlider : Action
{
    Task task;
    bool actionDone = false;

    public override void Act(PlotBase plot)
    {
        var a = asset as ResetSliderAsset;
        var go = plot.GetPlotObject(a.targetName);
        if (go != null && go.TryGetComponent(out Slider target))
        {
            task = new Task(Reset(target, a.speed));
            task.Start();
        }

    }

    // This is the Coroutine
    IEnumerator Reset(Slider target, float speed)
    {
        while (target.value > 0f)
        {
            target.value -= speed * Time.deltaTime;
            yield return null;
        }
        target.value = 0f;
        actionDone = true;
    }

    // Override `IsActionDone` to signal when the coroutine has finished.
    public override bool IsActionDone()
    {
        return actionDone;
    }
}
```
> To simplify coroutine management, this example uses the **Task** class. For more information, visit [krockot / Unity-TaskManager](https://github.com/krockot/Unity-TaskManager/tree/master).

To make subsequent `Actions` wait for this. Check the **Wait for Action** field.

![](img/action%20inspector.png)

# Acknowledge
This tool is made possible thanks to @krockot's [awesome TaskManager](https://github.com/krockot/Unity-TaskManager/tree/master)


