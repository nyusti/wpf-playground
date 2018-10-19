namespace WpfApp1.Configuration
{
    public interface IDependencyResolver : IDependencyScope
    {
        IDependencyScope BeginScope();
    }
}