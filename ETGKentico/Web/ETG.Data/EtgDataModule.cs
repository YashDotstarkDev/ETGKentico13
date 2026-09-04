using CMS;
using CMS.MacroEngine;
using ETG.Data;

[assembly: RegisterModule(typeof(EtgDataModule))]
namespace ETG.Data
{
    public class EtgDataModule : CMS.DataEngine.Module
    {
        public EtgDataModule() : base("ETG.DataModule")
        {
        }
        
        protected override void OnInit()
        {
            base.OnInit();

            MacroContext.GlobalResolver.SetNamedSourceData("ETGData", EtgDataModuleNamespace.Instance);
            MacroContext.GlobalResolver.AddAnonymousSourceData(EtgDataModuleNamespace.Instance);
        }
    }
}