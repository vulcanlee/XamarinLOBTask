using LOBTask.ViewModels;
using LOBTask.Views;
using Microsoft.Practices.Unity;
using Prism.Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LOBTask
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            //將此行由專案樣板產生的程式碼，註解起來，修正第一個起始畫面為 SplashScreenPage
            //await NavigationService.NavigateAsync("NavigationPage/MainPage");

            // 這是 App 的第一個顯示的頁面，但不需要使用導航工具列
            await NavigationService.NavigateAsync("SplashScreenPage");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<UserLoginPage>();
            Container.RegisterTypeForNavigation<SplashScreenPage>();
            Container.RegisterTypeForNavigation<NaviPage>();
            Container.RegisterTypeForNavigation<MDPage>();
            Container.RegisterTypeForNavigation<HomePage>();
            Container.RegisterTypeForNavigation<TaskEditPage>();
            Container.RegisterTypeForNavigation<CodeScannerPage>();
        }
    }
}
