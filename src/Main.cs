using BepInEx;

namespace Minimalism
{
    [BepInPlugin("goi.ext.minimalism", "Minimalism", "0.1.0")]
    public class Minimalism : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("GOI Minimalism is ready");
        }
    }
}
