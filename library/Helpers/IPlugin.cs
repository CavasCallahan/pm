namespace pm.Helpers
{
    public interface IPlugin
    {
        string PluginName { get; set; }
        string PluginDescription { get; set; }
        void Execute();
    }
}