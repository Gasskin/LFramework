using System;
using YooAsset;

namespace GameFramework.Asset
{
    public class YooAssetsLogger:ILogger
    {
        public void Log(string message)
        {
            GameFrameworkLog.Info(message);
        }

        public void Warning(string message)
        {
            GameFrameworkLog.Warning(message);
        }

        public void Error(string message)
        {
            GameFrameworkLog.Error(message);
        }

        public void Exception(Exception exception)
        {
            GameFrameworkLog.Fatal(exception.Message);
        }
    }
}