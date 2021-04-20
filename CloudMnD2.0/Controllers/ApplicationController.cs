using CloudMnD2._0.Core.ApplicationInsights;
using CloudMnD2._0.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Timers;

namespace CloudMnD2._0.Controllers
{
    public class ApplicationController : Controller
    {
        private static string RoleName = "Test_RoleName";
        private static Timer AvailabilityTimer;
        private static Timer RequestSenderTimer;
        private static StringBuilder Logs = new StringBuilder();

        public IActionResult Index()
        {
            var availabilitySenderState = false;
            var requestSenderState = false;
            if (AvailabilityTimer != null)
            {
                availabilitySenderState = AvailabilityTimer.Enabled;
            }

            if (RequestSenderTimer != null)
            {
                requestSenderState = RequestSenderTimer.Enabled;
            }

            return View(new ApplicationModel { RoleName = RoleName, AvailabilitySenderState = availabilitySenderState, RequestSenderState = requestSenderState});
        }

        public ActionResult GetLogs()
        {
            return Json(Logs.ToString());
        }

        public ActionResult CleanLogs()
        {
            Logs.Clear();
            return Json(Logs.ToString());
        }

        public ActionResult SetAppRoleName(string roleName)
        {
            RoleName = roleName;
            return Json(roleName);
        }

        public ActionResult GenerateException(int numberOfExceptions)
        {
            AppInsightsLogs.Initialize(RoleName);
            var operationId = Guid.NewGuid().ToString("N");
            for (var i = 0; i < numberOfExceptions; i++)
            {
                AppInsightsLogs.Instance.GenerateException(operationId);
                AppInsightsLogs.Instance.Flush();
            }
            Logs.AppendLine($"[{DateTime.Now}] Flush {numberOfExceptions} exception(s) for {RoleName}");
            return Json(numberOfExceptions);
        }

        public ActionResult StartGenerationAvailability()
        {
            AppInsightsLogs.Initialize(RoleName);
            InitializeAvailabilityTimer(60);
            return Json(new object());
        }

        public ActionResult StopGenerationAvailability()
        {
            StopAvailabilityTimer();
            return Json(string.Empty);
        }

        public ActionResult StartRequestSender()
        {
            AppInsightsLogs.Initialize(RoleName);
            InitializeRequestSenderTimer(10);
            return Json(new object());
        }

        public ActionResult StopRequestSender()
        {
            StopRequestSenderTimer();
            return Json(string.Empty);
        }

        private static void InitializeAvailabilityTimer(double intervalSec)
        {
            AvailabilityTimer = new Timer(1000 * intervalSec);
            AvailabilityTimer.Elapsed += AvailabilityMethod;
            AvailabilityTimer.Start();
            Logs.AppendLine($"[{DateTime.Now}] Run sending Availability requests with interval {intervalSec} sec for {RoleName}");
        }

        private static void StopAvailabilityTimer()
        {
            AvailabilityTimer.Stop();
            AvailabilityTimer.Dispose();
            Logs.AppendLine($"[{DateTime.Now}] Stop sending Availability requests for {RoleName}");
        }

        private static void AvailabilityMethod(Object source, ElapsedEventArgs e)
        {
            AppInsightsLogs.Instance.GenerateAvailability(true);
            AppInsightsLogs.Instance.Flush();
            Logs.AppendLine($"[{DateTime.Now}] Flush Availability for {RoleName}");
        }

        private static void InitializeRequestSenderTimer(double intervalSec)
        {
            RequestSenderTimer = new Timer(1000 * intervalSec);
            RequestSenderTimer.Elapsed += RequestSenderMethod;
            RequestSenderTimer.Start();
            Logs.AppendLine($"[{DateTime.Now}] Run sending requests with interval {intervalSec} sec for {RoleName}");
        }

        private static void StopRequestSenderTimer()
        {
            RequestSenderTimer.Stop();
            RequestSenderTimer.Dispose();
            Logs.AppendLine($"[{DateTime.Now}] Stop sending requests for {RoleName}");
        }

        private static void RequestSenderMethod(Object source, ElapsedEventArgs e)
        {
            var rng = new Random();
            var randomBool = rng.Next(0, 2) > 0;
            var numOfReq = rng.Next(1, 10);
          
            for (var i = 0; i < numOfReq; i++)
            {
                AppInsightsLogs.Instance.GenerateRequest(randomBool);
            }

            AppInsightsLogs.Instance.Flush();
            Logs.AppendLine($"[{DateTime.Now}] Flush {numOfReq} {randomBool} request for {RoleName}");
        }

    }
}