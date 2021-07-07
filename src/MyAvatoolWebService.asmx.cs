﻿/* PROJECT: MyAvatoolWebService (https://github.com/aprettycoolprogram/MyAvatoolWebService)
 *    FILE: MyAvatoolWebService.MyAvatoolWebService.asmx.cs
 * UPDATED: 7-7-2021-11:55 AM
 * LICENSE: Apache v2 (https://apache.org/licenses/LICENSE-2.0)
 *          Copyright 2021 A Pretty Cool Program All rights reserved
 */

/* Entry point for MAWS.
 */

using System.Collections.Generic;
using System.Reflection;
using System.Web.Services;
using NTST.ScriptLinkService.Objects;
using Logger;

namespace MyAvatoolWebService
{
    /// <summary>
    /// Entry point for MAWS.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class MyAvatoolWebService : WebService
    {
        //public Dictionary<string, string> MawsSetting;

        /// <summary>
        /// Returns the MAWS version.
        /// </summary>
        /// <returns>The MAWS version (e.g., "VERSION 1.0").</returns>
        [WebMethod]
        public string GetVersion()
        {
            // This line has to be commented out in production, otherwise MAWS will not work!
            ForceTest();

            return "VERSION 1.0";
        }

        /// <summary>
        /// Performs an MAWS Request.
        /// </summary>
        /// <param name="sentOptionObject">The OptionObject2015 object sent from myAvatar.</param>
        /// <param name="mawsRequest">     The MAWS Request to perform (e.g., "InptAdmitDate-ComparePreAdmitToAdmit")</param>
        /// <returns>A completed OptionObject2015 that MAWS will return to myAvatar.</returns>
        [WebMethod]
        public OptionObject2015 RunScript(OptionObject2015 sentOptionObject, string mawsRequest)
        {
            Dictionary<string, string> MawsSetting = Settings.GetSettings();
            var logSetting                         = MawsSetting["Logging"].ToLower();
            var assemblyName                       = Assembly.GetExecutingAssembly().GetName().Name;
            LogEvent.Timestamped(logSetting, "TRACE", assemblyName, $"Initial MAWS Request: {mawsRequest}");

            var mawsCommand = RequestSyntaxEngine.RequestComponent.GetCommand(mawsRequest);
            LogEvent.Timestamped(logSetting, "TRACE", assemblyName, $"Initial MAWS Command: {mawsCommand}");

            OptionObject2015 completedOptionObject;

            switch(mawsCommand)
            {
                case "inptadmitdate":
                    LogEvent.Timestamped(logSetting, "TRACE", assemblyName, "switch(mawsCommand) case: InptAdmitDate [{mawsCommand}]");
                    //completedOptionObject = InptAdmitDate.Execute.Action(sentOptionObject, mawsRequest);
                    break;

                case "dose":
                    LogEvent.Timestamped(logSetting, "TRACE", assemblyName, "switch(mawsCommand) case: Dose [{mawsCommand}]");
                    //completedOptionObject = Dose.Execute.Action(sentOptionObject, mawsRequest);
                    break;

                case "newdevelopment":
                    LogEvent.Timestamped(logSetting, "TRACE", assemblyName, "switch(mawsCommand) case: NewDevelopment [{mawsCommand}]");
                    //completedOptionObject = NewDevelopment.Execute.Action(sentOptionObject, mawsRequest);
                    break;

                default:
                    LogEvent.Timestamped(logSetting, "ERROR", assemblyName, $"Invalid MAWS Command: \"{mawsCommand}\".");
                    //completedOptionObject = sentOptionObject;
                    break;
            }

            completedOptionObject = new OptionObject2015(); // TEMP TESTING

            return completedOptionObject;
        }

        /// <summary>
        /// Force a bunch of MAWS functionality tests.
        /// </summary>
        public void ForceTest()
        {
            // Trace commands/actions/options.
            var emptyOptionObject = new OptionObject2015();
            _ = RunScript(emptyOptionObject, "InptAdmitDate-action-option");
            _ = RunScript(emptyOptionObject, "Dose-action-option");
            _ = RunScript(emptyOptionObject, "NewDevelopment-action-option");
            _ = RunScript(emptyOptionObject, "Fake-action-option");

            // Test against an partially initialized OptionObject.
            var testInptAdmitDateOptionObject= new OptionObject2015
            {
                ErrorCode = 0,
                ErrorMesg = "",
            };
            _ = RunScript(testInptAdmitDateOptionObject, "InptAdmitDate-ComparePreAdmitToAdmit");

            // Test RequestSyntaxEngine functionality.
            RequestSyntaxEngine.TestFunctionality.Force();
        }
    }
}

/* =================
 * DEVELOPMENT NOTES
 * =================
 * - The goal with this class is to keep it simple, with only the "GetVersion()", "RunScript()", and ForceTest()
 *   methods. All other logic will be in a class that corresponds to the MAWS Command (e.g., "InptAdmitDate")
 *
 * - Both GetVersion() and RunScript() are required by myAvatar, so don't remove them.
 *
 * - The "VERSION" will always be the target version that is being developed. For example, "VERSION 1.0" during v1.0
 *   development, "VERSION 1.1" during v1.1 development.
 *
 * - ForceTest() is here because otherwise it's a pain to get working, but eventually I would like to move it into its
 *   own class.
 *
 * ------------
 * GetVersion()
 * ------------
 * - Injecting code into GetVersion() is a Bad Idea, and if you don't comment-out the `ForceTest()` line, MAWS won't
 *   work. However I want an easy way to test some functionality on my local machine without having to publish the web
 *   service. This functionality will probably be depreciated further down development.
 *
 *   Here is how you can test MAWS:
 *      1. Make sure you have the proper testing settings configured in maws.settings
 *      2. Run MAWS
 *      3. Click "GetVersion"
 *      4. Click the "Invoke" button
 */