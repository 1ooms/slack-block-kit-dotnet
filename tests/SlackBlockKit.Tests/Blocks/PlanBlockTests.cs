namespace SlackBlockKit.Tests.Blocks;

public class PlanBlockTests
{
    private static TaskCardBlock Task(string id = "t1") => new(id, "Task", TaskCardStatus.InProgress);

    [Fact]
    public void ListConstructor_SetsProperties()
    {
        var tasks = new List<TaskCardBlock> { Task() };

        var block = new PlanBlock("Plan", tasks, "plan_1");

        Assert.Equal("Plan", block.Title);
        Assert.Same(tasks, block.Tasks);
        Assert.Equal("plan_1", block.BlockId);
        Assert.Equal("plan", block.Type);
    }

    [Fact]
    public void ParamsConstructor_CollectsTasks()
    {
        Assert.Equal(2, new PlanBlock("Plan", Task("a"), Task("b")).Tasks.Count);
    }

    [Fact]
    public void ParamsConstructor_AllowsNoTasks()
    {
        Assert.Empty(new PlanBlock("Plan").Tasks);
    }

    [Fact]
    public void Serialize_WritesTasksWithDiscriminators()
    {
        var json = ToJson(new PlanBlock("Plan", [Task()], "plan_1"));

        AssertProperties(json, "type", "title", "tasks", "block_id");
        Assert.Equal("plan", json.GetProperty("type").GetString());
        Assert.Equal("task_card", json.GetProperty("tasks")[0].GetProperty("type").GetString());
    }

    [Fact]
    public void Title_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new PlanBlock(null!, Task()));
    }

    [Fact]
    public void Tasks_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new PlanBlock("Plan", (List<TaskCardBlock>)null!));
    }

    [Fact]
    public void Tasks_AllowsUpTo50()
    {
        var tasks = Enumerable.Range(0, 50).Select(i => Task($"t{i}")).ToList();

        Assert.Equal(50, new PlanBlock("Plan", tasks).Tasks.Count);

        tasks.Add(Task("extra"));
        Assert.Throws<ArgumentException>(() => new PlanBlock("Plan", tasks));
    }

    [Fact]
    public void RoundTrip_PreservesValues()
    {
        var block = RoundTrip(new PlanBlock("Plan", [Task("t1"), Task("t2")], "plan_1"));

        Assert.Equal("Plan", block.Title);
        Assert.Equal(["t1", "t2"], block.Tasks.Select(task => task.TaskId));
        Assert.Equal("plan_1", block.BlockId);
    }
}
