namespace BubbleWar.Shared
{
    public static class SyncfusionServices
    {
        public static void InitializeSyncfusion()
        {
#if __IOS__
            Syncfusion.SfChart.XForms.iOS.Renderers.SfChartRenderer.Init();
#endif
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(SyncfusionConstants.LicenseKey);
        }
    }
}
