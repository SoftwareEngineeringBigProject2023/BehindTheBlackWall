using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
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
            physicsDataA.Contacts.Add(physicsDataB);
            physicsDataB.Contacts.Add(physicsDataA);
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
            physicsDataA.Contacts.Remove(physicsDataB);
            physicsDataB.Contacts.Remove(physicsDataA);
        }
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