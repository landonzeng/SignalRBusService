using Microsoft.Owin.Hosting;
using System;
using System.Configuration;
using System.ServiceProcess;
using Utilities;

namespace SignalRWindowsService
{
    public partial class SignalRBusService : ServiceBase
    {
        LogHelper log = LogFactory.GetLogger(typeof(SignalRBusService));

        private IDisposable _webApp;

        public SignalRBusService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string url = (args != null && args.Length > 0) ? args[0] : ConfigurationManager.AppSettings["SignalRBusAddress"].ToString();//"http://localhost:10086";
            try
            {
                _webApp = WebApp.Start<Startup>(url);
                log.Debug("\r\n服务地址：" + url + "\r\n-----------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                log.Debug("\r\n异常信息：" + ex + "\r\n-----------------------------------------------------------------------------");
            }
        }

        protected override void OnStop()
        {
            _webApp.Dispose();
        }
    }
}
