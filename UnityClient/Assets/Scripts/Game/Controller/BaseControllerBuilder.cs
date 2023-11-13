using System;
using SEServer.Data.Interface;
using UnityEngine;

namespace Game.Controller
{
    public abstract class BaseControllerBuilder
    {
        public abstract BaseController BuildController(GameObject gameObject, IComponent component);
        public abstract Type BindType { get; }
    }
}