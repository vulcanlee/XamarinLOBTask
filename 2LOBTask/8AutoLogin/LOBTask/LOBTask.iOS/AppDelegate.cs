﻿using Foundation;
using UIKit;
using Microsoft.Practices.Unity;
using Prism.Unity;

namespace LOBTask.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            #region 第三方套件／插件的初始化

            #region QR Code 掃描套件初始化
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            #endregion

            #region 圓形圖片避免被 Linking 給自動移除
            var rendererAssemblies = new[] { typeof(ImageCircle.Forms.Plugin.iOS.ImageCircleRenderer) };
            #endregion

            #region 進行 Iconize 套件的初始化
            Plugin.Iconize.Iconize
                .With(new Plugin.Iconize.Fonts.FontAwesomeModule());
            FormsPlugin.Iconize.iOS.IconControls.Init();
            #endregion
            #endregion

            LoadApplication(new App(new iOSInitializer()));
            return base.FinishedLaunching(app, options);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {
            
        }
    }
}
