using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class ArithmeticOverflowVisitor : SqlFragmentVisitorBase
    {
        public override void ExplicitVisit(FunctionCall node)
        {
            analysisFunctionCall(node);
            base.ExplicitVisit(node);
        }

        private void analysisFunctionCall(FunctionCall functionCall)
        {
            if (functionCall == null)
                return;

            switch (functionCall.FunctionName.Value.ToLowerInvariant())
            {
                case "sum":
                    if (!hasCastAsBigInt(functionCall))
                    {
                        IsSuspected = true;
                    }
                    break;
                default:
                    return;
            }
        }

        private static bool hasCastAsBigInt(FunctionCall functionCall)
        {
            foreach (var parameter in functionCall.Parameters.OfType<CastCall>())
            {
                var dataType = parameter.DataType as SqlDataTypeReference;
                if (dataType == null)
                    continue;

                switch (dataType.SqlDataTypeOption)
                {
                    case SqlDataTypeOption.BigInt:
                        return true;
                }
            }

            return false;
        }
    }
}