namespace NbSites.Core.AutoInject
{
    public interface IAutoInject
    {
    }

    public interface IAutoInjectAsTransient : IAutoInject
    {
    }

    public interface IAutoInjectAsScoped : IAutoInject
    {
    }

    public interface IAutoInjectAsSingleton : IAutoInject
    {
    }
}
