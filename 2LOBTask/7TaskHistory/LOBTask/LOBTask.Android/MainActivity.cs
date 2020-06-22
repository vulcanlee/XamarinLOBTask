using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Practices.Unity;
using Prism.Unity;

namespace LOBTask.Droid
{
    [Activity(Label = "派工與回報", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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

            base.OnCreate(bundle);

            #region 設定該應用程式的主要 Activity
            instance = this;
            #endregion

            global::Xamarin.Forms.Forms.Init(this, bundle);

            #region 第三方套件／插件的初始化

            #region QR Code 掃描套件初始化
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            #endregion

            #region 圓形圖片避免被 Linking 給自動移除
            var rendererAssemblies = new[] { typeof(ImageCircle.Forms.Plugin.Droid.ImageCircleRenderer) };
            #endregion

            #region 進行 UserDialogs 套件的初始化
            UserDialogs.Init(this);
            #endregion

            #region 進行 Iconize 套件的初始化
            Plugin.Iconize.Iconize
                .With(new Plugin.Iconize.Fonts.FontAwesomeModule());
            FormsPlugin.Iconize.Droid.IconControls.Init(Resource.Layout.Toolbar, Resource.Layout.Tabbar);
            #endregion
            #endregion

            #region 進行 Azure Mobile Client 套件初始化
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            #endregion

            LoadApplication(new App(new AndroidInitializer()));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {
            // Register any platform specific implementations
        }
    }
}

