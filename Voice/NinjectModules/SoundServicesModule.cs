using BLL.Services.Comments;
using BLL.Services.Letters;
using BLL.Services.Likes;
using BLL.Services.Sound.BuildSound;
using BLL.Services.Sound.Editor;
using BLL.Services.Sound.GetSound;
using BLL.Services.Sound.VisualizeSound;
using BLL.Services.Subs;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Voice.NinjectModules
{
    public class SoundServicesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISoundEditorService>().To<WaveSoundEditorService>().InRequestScope();
            Bind<IBuilderSoundService>().To<BuilderWaveSoundService>().InRequestScope();
            Bind<IGetSoundService>().To<GetWaveSoundService>().InRequestScope();
            Bind<IVisualizatorService>().To<WaveVisualizatorService>().InRequestScope();
        
            Bind<ILikeService>().To<LikeService>().InRequestScope();
            Bind<ILetterService>().To<WaveLetterService>().InRequestScope();
            Bind<ISubService>().To<SubService>().InRequestScope();
            Bind<ICommentService>().To<CommentService>().InRequestScope();
              
            Bind<ISoundServiceFactory>().ToFactory().InRequestScope();         
        }
    }
}