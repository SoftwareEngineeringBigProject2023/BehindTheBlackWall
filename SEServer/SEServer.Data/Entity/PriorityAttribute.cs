using System;

namespace SEServer.Data;

[AttributeUsage(AttributeTargets.Class)]
public class PriorityAttribute : Attribute
{
    public int Priority { get; }
    
    public PriorityAttribute(int priority)
    {
        Priority = priority;
    }
}