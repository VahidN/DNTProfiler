using System.Net;
using System.Net.Http;
using System.Web.Http;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Common.Threading;

namespace DNTProfiler.Services
{
    public class CommandConnectionController : ApiController
    {
        public HttpResponseMessage Post([FromBody] CommandConnection commandConnection)
        {
            commandConnection.ReceivedId = IdGenerator.GetId();
            commandConnection.JsonContent = this.Request.Content.ReadAsStringAsync().Result.ToFormattedJson();
            DispatcherHelper.DispatchAction(() =>
                AppMessenger.Messenger.NotifyColleagues("AddCommandConnection", commandConnection));
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}