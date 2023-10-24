using System;
using System.Collections;
using System.Collections.Generic;

namespace SEServer.Data;

public class ComponentDataCollection<T> : IEnumerable<T> where T : IComponent
{
    public List<T> Components { get; set; } = new();

    public IEnumerator<T> GetEnumerator()
    {
        return Components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class ComponentDataCollection<T, T1> : IEnumerable<ValueTuple<T, T1>>
    where T : IComponent
    where T1 : IComponent
{
    public List<ValueTuple<T, T1>> Components { get; set; } = new();

    public IEnumerator<(T, T1)> GetEnumerator()
    {
        return Components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class ComponentDataCollection<T, T1, T2> : IEnumerable<ValueTuple<T, T1, T2>>
    where T : IComponent
    where T1 : IComponent
    where T2 : IComponent
{
    public List<ValueTuple<T, T1, T2>> Components { get; set; } = new();

    public IEnumerator<(T, T1, T2)> GetEnumerator()
    {
        return Components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class ComponentDataCollection<T, T1, T2, T3> : IEnumerable<ValueTuple<T, T1, T2, T3>>
    where T : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
    public List<ValueTuple<T, T1, T2, T3>> Components { get; set; } = new();

    public IEnumerator<(T, T1, T2, T3)> GetEnumerator()
    {
        return Components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class ComponentDataCollection<T, T1, T2, T3, T4> : IEnumerable<ValueTuple<T, T1, T2, T3, T4>>
    where T : IComponent
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
    public List<ValueTuple<T, T1, T2, T3, T4>> Components { get; set; } = new();

    public IEnumerator<(T, T1, T2, T3, T4)> GetEnumerator()
    {
        return Components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}