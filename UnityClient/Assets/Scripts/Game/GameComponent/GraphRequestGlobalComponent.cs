using System.Collections.Generic;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.GameData.Component;
using SEServer.GameData.CTData;

namespace Game.GameComponent
{
    public class GraphRequestGlobalComponent : IComponent
    {
        public CId Id { get; set; }
        public EId EntityId { get; set; }
        public Queue<GraphRequest> Requests { get; set; } = new();
    }

    public abstract class GraphRequest
    {
        public TransformComponent Transform { get; set; }

        public GraphRequest(TransformComponent transform)
        {
            Transform = transform;
        }
    }

    public class ShootRequest : GraphRequest
    {
        public ShootRequest(TransformComponent transform, WeaponCTData weaponCtData) : base(transform)
        {
            WeaponCtData = weaponCtData;
        }

        public WeaponCTData WeaponCtData { get; set; }
    }

    public class PickupRequest : GraphRequest
    {
        public PickupRequest(TransformComponent transform) : base(transform)
        {
        }
    }

    public class InjuryRequest : GraphRequest
    {
        public InjuryRequest(TransformComponent transform, int damage) : base(transform)
        {
            Damage = damage;
        }

        public int Damage { get; set; }
    }

    public class ScoreChangeRequest : GraphRequest
    {
        public ScoreChangeRequest(TransformComponent transform, int score) : base(transform)
        {
            Score = score;
        }

        public int Score { get; set; }
    }
}