using System.Collections.Generic;
using UnityEngine;

namespace Game.Framework
{
    public class ABPackerCollection : ScriptableObject
    {
        public List<ABPackerSetting> settings = new List<ABPackerSetting>();
    }
}