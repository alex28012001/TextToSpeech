using System.Collections.Generic;

namespace BLL.Services.Sound.VisualizeSound
{
    public interface IVisualizatorService
    {
        IEnumerable<int> Visualize(string audioPath, int amountProc);
    }
}
