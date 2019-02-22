using BLL.Services.Sound.BuildSound;
using BLL.Services.Sound.Editor;
using BLL.Services.Sound.GetSound;
using BLL.Services.Sound.VisualizeSound;

namespace Voice.NinjectModules
{
    public interface ISoundServiceFactory
    {
        IBuilderSoundService CreateBuilderSoundService();
        ISoundEditorService CreateSoundEditorService();
        IGetSoundService CreateGetSoundService(); 
        IVisualizatorService CreateVisualizatorService();
    }
}
