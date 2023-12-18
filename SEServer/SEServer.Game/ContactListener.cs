using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using SEServer.Game.Component;
using Phy2DWorld = nkast.Aether.Physics2D.Dynamics.World;

namespace SEServer.Game;

public class ContactListener : IDisposable
{
    public PhysicsSingletonComponent PhysicsComponent { get; set; }
    public Phy2DWorld Phy2DWorld => PhysicsComponent.Phy2DWorld;
    
    public ContactListener(PhysicsSingletonComponent physicsComponent)
    {
        PhysicsComponent = physicsComponent;
        InitContact();
    }

    private void InitContact()
    {
        Phy2DWorld.ContactManager.BeginContact += BeginContact;
        Phy2DWorld.ContactManager.EndContact += EndContact;
        Phy2DWorld.ContactManager.PreSolve += PreSolve;
        Phy2DWorld.ContactManager.PostSolve += PostSolve;
    }
    
    private void RemoveContact()
    {
        Phy2DWorld.ContactManager.BeginContact -= BeginContact;
        Phy2DWorld.ContactManager.EndContact -= EndContact;
        Phy2DWorld.ContactManager.PreSolve -= PreSolve;
        Phy2DWorld.ContactManager.PostSolve -= PostSolve;
    }

    private bool BeginContact(Contact contact)
    {
        var fixtureA = contact.FixtureA;
        var fixtureB = contact.FixtureB;
        var bodyA = fixtureA.Body;
        var bodyB = fixtureB.Body;
        
        if(bodyA.Tag is PhysicsData physicsDataA && bodyB.Tag is PhysicsData physicsDataB)
        {
            AddContact(physicsDataA, physicsDataB, contact);
            AddContact(physicsDataB, physicsDataA, contact, true);
        }
        
        return true;
    }

    private void EndContact(Contact contact)
    {
        var fixtureA = contact.FixtureA;
        var fixtureB = contact.FixtureB;
        var bodyA = fixtureA.Body;
        var bodyB = fixtureB.Body;
        
        if(bodyA.Tag is PhysicsData physicsDataA && bodyB.Tag is PhysicsData physicsDataB)
        {
            RemoveContact(physicsDataA, physicsDataB);
            RemoveContact(physicsDataB, physicsDataA);
        }
    }
    
    private void AddContact(PhysicsData physicsSource, PhysicsData physicsTarget, Contact contact, bool revertNormal = false)
    {
        contact.GetWorldManifold(out var normal, out var points);
        if (revertNormal)
        {
            normal = -normal;
        }
        var contactRecordData = new ContactRecordData()
        {
            OtherPhysicsData = physicsTarget,
            Normal = normal.ToSVector2(),
            ContactPoint = points[0].ToSVector2()
        };
        physicsSource.Contacts[physicsTarget.BindEntityId] = contactRecordData;
    }
    
    private void RemoveContact(PhysicsData physicsSource, PhysicsData physicsTarget)
    {
        physicsSource.Contacts.Remove(physicsTarget.BindEntityId);
    }

    private void PreSolve(Contact contact, ref Manifold oldmanifold)
    {
        
    }

    private void PostSolve(Contact contact, ContactVelocityConstraint impulse)
    {
        
    }


    public void Dispose()
    {
        RemoveContact();
    }
}