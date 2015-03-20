using System.Net;
using System.Net.Http;
using System.Web.Http;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Common.Threading;

namespace DNTProfiler.Services
{
    public class CommandResultController : ApiController
    {
        public HttpResponseMessage Post([FromBody] CommandResult commandResult)
        {
            commandResult.ReceivedId = IdGenerator.GetId();
            commandResult.JsonContent = this.Request.Content.ReadAsStringAsync().Result.ToFormattedJson();
            DispatcherHelper.DispatchAction(()=>
                AppMessenger.Messenger.NotifyColleagues("AddCommandResult", commandResult));
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}