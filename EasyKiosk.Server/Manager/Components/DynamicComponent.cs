
namespace EasyKiosk.Server.Manager.Components;

public abstract class DynamicComponent
{
    public abstract Type ComponentType { get;}
    public Dictionary<string, object> ComponentParameters { get;}


    protected DynamicComponent()
    {
        
        if (ComponentType == null)
        {
            throw new NullReferenceException(
                "Component type Cannot be null, please add a property containing the component");
        }
        
        ComponentParameters = new();
    }
}