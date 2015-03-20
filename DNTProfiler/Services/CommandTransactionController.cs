using System.Net;
using System.Net.Http;
using System.Web.Http;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Common.Threading;

namespace DNTProfiler.Services
{
    public class CommandTransactionController : ApiController
    {
        public HttpResponseMessage Post([FromBody] CommandTransaction commandTransaction)
        {
            commandTransaction.ReceivedId = IdGenerator.GetId();
            commandTransaction.JsonContent = this.Request.Content.ReadAsStringAsync().Result.ToFormattedJson();
            DispatcherHelper.DispatchAction(()=>
                AppMessenger.Messenger.NotifyColleagues("AddCommandTransaction", commandTransaction));
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}