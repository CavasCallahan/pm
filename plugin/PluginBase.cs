namespace PluginBase
{
    public interface IPlugin
    {
        string PluginName { get;}
        string PluginDescription { get; }

        int Execute();
    }
}