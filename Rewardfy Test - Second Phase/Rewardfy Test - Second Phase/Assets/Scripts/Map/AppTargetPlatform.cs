public abstract class AppTargetPlatform
{
    protected IMap map;

    public AppTargetPlatform(IMap map)
    {
        this.map = map;
    }
    public abstract IPathDrawer CreatePathDrawer();
    public abstract IMapDrawer CreateMapDrawer();
    public abstract IMapInputHandler CreateInputHandler();
}
