using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Test1
{
    //public class AppInitializer
    //{
    //    public static IMyHomeAppSystem StartApp(Platform platform)
    //    {
    //        if (platform == Platform.Android){ 
    //            return new AndroidmyHome_App(
    //                ConfigureApp
    //                    .Android
    //                    .ApkFile("com.xamarin.samples.taskyandroid.apk")
    //                    .StartApp());
    //            }
    //        return new IOSmyHome_App(
    //            ConfigureApp
    //                .iOS
    //                .StartApp());
    //    }
    //}

    public class AppInitializer
    {
        public static IMyHomeAppSystem StartApp(Platform platform)
        {
                return new AndroidmyHome_App(
                    ConfigureApp
                        .Android
                        .ApkFile("com.xamarin.samples.taskyandroid.apk")
                        .StartApp());
        }
    }


}

