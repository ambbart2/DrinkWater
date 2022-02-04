
using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace DrinkWater.Configuration
{
    public class PluginConfig
    {
        public virtual bool EnablePlugin { get; set; } = true;
        public virtual bool ShowGIFs { get; set; } = true;
        public virtual int WaitDuration { get; set; } =  5;
        public virtual bool EnableByPlaytime { get; set; } = true;
        public virtual bool EnableByPlaycount { get; set; } = false;
        public virtual int PlaytimeBeforeWarning { get; set; } = 5;
        public virtual int PlaycountBeforeWarning { get; set; } = 2;
    }
}
