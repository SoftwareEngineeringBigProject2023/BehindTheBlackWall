using System;
using SEServer.Data;
using SEServer.Data.Interface;
using UnityEngine;

namespace Game
{
    public abstract class BaseControllerBuilder
    {
        public abstract BaseController BuildController(GameObject gameObject, IComponent component);
        public abstract Type BindType { get; }
    }
}