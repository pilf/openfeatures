using Coypu;
using System;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using OpenTable.Features.Core.Helpers;
using OpenTable.Features.Core.Interfaces;

namespace OpenTable.Features.Core.ScreenShotCapture
{
    public class ScreenShot
    {
        private static string _screenShotFolder;

        public static void TakeScreenshot(IScenarioContext scenarioContext, Driver staticDriverReference, string folderName)
        {
            _screenShotFolder = folderName;
            SaveScreenshotTo(Truncate(GetSaveLocation(scenarioContext), 245), staticDriverReference);
        }

        private static void SaveScreenshotTo(string imageFile, Driver staticDriverReference)
        {
            var screenshot = ((ITakesScreenshot)staticDriverReference.Native).GetScreenshot();
            screenshot.SaveAsFile(imageFile + ".png", ImageFormat.Png);
        }

        private static string GetSaveLocation(IScenarioContext scenarioContext)
        {
            var projectName = EnvironmentVariableHelper.Get("TEAMCITY_PROJECT_NAME", "projectName");
            var buildConfName = EnvironmentVariableHelper.Get("TEAMCITY_BUILDCONF_ID", "buildConfID");
            var buildNumber = EnvironmentVariableHelper.Get("BUILD_NUMBER", "buildNumber");
            var featureName = scenarioContext.CurrentFeatureTitle();
            
            var logFolder = GetLogFolder();

            var screenshotFolder = CombinePaths(logFolder, "buildScreenshots", projectName, buildConfName, buildNumber, _screenShotFolder, featureName);
            Directory.CreateDirectory(screenshotFolder);

            return Path.Combine(screenshotFolder, EscapeFilename(scenarioContext.CurrentScenarioTitle()));
        }

        private static string CombinePaths(string path1, params string[] paths)
        {
            if (path1 == null)
            {
                throw new ArgumentNullException("path1");
            }
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }
            return paths.Aggregate(path1, Path.Combine);
        }

        private static string GetLogFolder()
        {
            var currentDirInfo = new DirectoryInfo(Environment.CurrentDirectory);

            while (currentDirInfo.Parent != null)
            {
                currentDirInfo = currentDirInfo.Parent;
            }
            return currentDirInfo.FullName;
        }

        private static string EscapeFilename(string raw)
        {
            return Regex.Replace(raw, @"[\\\?:\+\*\s""<>|,]+", "_");
        }

        private static string Truncate(string value, int maxLength)
        {
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}