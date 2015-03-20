using System.Net;
using System.Net.Http;
using System.Web.Http;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Common.Threading;
using DNTProfiler.Infrastructure.ScriptDomVisitors;

namespace DNTProfiler.Services
{
    public class CommandController : ApiController
    {
        public HttpResponseMessage Post([FromBody] Command command)
        {
            command.ReceivedId = IdGenerator.GetId();
            command.JsonContent = this.Request.Content.ReadAsStringAsync().Result.ToFormattedJson();
            command.SetCommandStatistics();
            DispatcherHelper.DispatchAction(() =>
                AppMessenger.Messenger.NotifyColleagues("AddCommand", command));
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}