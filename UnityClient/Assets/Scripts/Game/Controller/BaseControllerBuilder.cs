using System;
using SEServer.Data.Interface;
using UnityEngine;

namespace Game.Controller
{
    public abstract class BaseControllerBuilder
    {
        public abstract BaseComponentController BuildController();
        public abstract Type BindType { get; }
    }
}