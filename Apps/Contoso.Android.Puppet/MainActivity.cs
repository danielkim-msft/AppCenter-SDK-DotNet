﻿using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Microsoft.Azure.Mobile.Distribute;

namespace Contoso.Android.Puppet
{
    [Activity(Label = "SXPuppet", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        const string LogTag = "MobileCenterXamarinPuppet";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Mobile Center integration
            MobileCenterLog.Assert(LogTag, "MobileCenter.LogLevel=" + MobileCenter.LogLevel);
            MobileCenter.LogLevel = LogLevel.Verbose;
            MobileCenterLog.Info(LogTag, "MobileCenter.LogLevel=" + MobileCenter.LogLevel);
            MobileCenter.SetLogUrl("https://in-integration.dev.avalanch.es");
            Distribute.SetInstallUrl("http://install.asgard-int.trafficmanager.net");
            Distribute.SetApiUrl("https://asgard-int.trafficmanager.net/api/v0.1");
            MobileCenter.Start("bff0949b-7970-439d-9745-92cdc59b10fe", typeof(Analytics), typeof(Crashes), typeof(Distribute));
            Analytics.TrackEvent("myEvent", new Dictionary<string, string> { { "someKey", "someValue" } });
            MobileCenterLog.Info(LogTag, "MobileCenter.InstallId=" + MobileCenter.InstallId);
            MobileCenterLog.Info(LogTag, "MobileCenter.HasCrashedInLastSession=" + Crashes.HasCrashedInLastSession);
            Crashes.GetLastSessionCrashReportAsync().ContinueWith(report =>
            {
                MobileCenterLog.Info(LogTag, "MobileCenter.LastThrowable=" + report.Result?.AndroidDetails?.Throwable);
                MobileCenterLog.Info(LogTag, "MobileCenter.LastException=" + report.Result?.Exception);
            });
        }

        protected override void OnDestroy()
        {
            // This will crash with super not called exception, pure java exception that we want to test
        }
    }
}