﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;

[assembly: UsesFeature("android.hardware.location", Required = false)]
[assembly: UsesFeature("android.hardware.location.gps", Required = false)]
[assembly: UsesFeature("android.hardware.location.network", Required = false)]
[assembly: UsesFeature("android.hardware.camera", Required = false)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = false)]
// 底下的用法，可以參考 https://developer.xamarin.com/releases/android/mono_for_android_4/mono_for_android_4.0.0/
[assembly: UsesPermission(Name = Android.Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.AccessFineLocation)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.ReadExternalStorage)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.WriteExternalStorage)]
[assembly: UsesPermission(Android.Manifest.Permission.Flashlight)]
[assembly: UsesPermission(Android.Manifest.Permission.Camera)]
namespace LOBTask.Droid
{
    [Activity(Label = "派工與回報demo", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        #region MainActivity Instance
        // Create a new instance field for this activity.
        static MainActivity instance = null;

        // Return the current activity instance.
        public static MainActivity CurrentActivity
        {
            get
            {
                return instance;
            }
        }

        #endregion

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            #region 設定該應用程式的主要 Activity
            instance = this;
            #endregion

            base.OnCreate(bundle);

            #region 第三方套件／插件的初始化

            #region 進行 UserDialogs 套件的初始化
            Acr.UserDialogs.UserDialogs.Init(this);
            #endregion

            #region 進行 Iconize 套件的初始化
            Plugin.Iconize.Iconize
                .With(new Plugin.Iconize.Fonts.FontAwesomeModule());
            FormsPlugin.Iconize.Droid.IconControls.Init(Resource.Layout.Toolbar, Resource.Layout.Tabbar);
            #endregion

            #region 圓形圖片避免被 Linking 給自動移除
            var rendererAssemblies = new[] { typeof(ImageCircle.Forms.Plugin.Droid.ImageCircleRenderer) };
            #endregion

            #region QR Code 掃描套件初始化
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            #endregion

            #endregion

            #region 進行 Azure Mobile Client 套件初始化
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            #endregion

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            // Register any platform specific implementations
        }
    }
}

