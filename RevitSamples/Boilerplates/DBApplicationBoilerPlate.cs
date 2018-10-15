#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
#endregion

namespace RevitSamples
{
    /// <summary>
    /// Sample Database Level Application
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    // Required Implementation
    public class DBApplicationBoilerPlate : IExternalDBApplication
    {
        /// <summary>
        /// Runs when Revit Shuts Down
        /// </summary>
        public ExternalDBApplicationResult OnShutdown(ControlledApplication application)
        {
            try
            {
                // Begin Code Here
                // Return Success
                return ExternalDBApplicationResult.Succeeded;
            }
            catch
            {
                // In Case of Failure
                return ExternalDBApplicationResult.Failed;

            }
        }
        /// <summary>
        /// Runs when Revit Starts Up
        /// </summary>
        public ExternalDBApplicationResult OnStartup(ControlledApplication application)
        {
            try
            {
                // Begin Code Here
                // Return Success
                return ExternalDBApplicationResult.Succeeded;
            }
            catch
            {
                // In Case of Failure
                return ExternalDBApplicationResult.Failed;
            }
        }
    }
}
