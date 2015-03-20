using DNTProfiler.Common.Models;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DNTProfiler.Infrastructure.ScriptDomVisitors
{
    public class CounterVisitor : TSqlFragmentVisitor
    {
        public CommandStatistics CommandStatistics { get; private set; }

        public CounterVisitor()
        {
            CommandStatistics = new CommandStatistics();
        }

        public override void ExplicitVisit(QualifiedJoin node)
        {
            CommandStatistics.JoinsCount++;
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(UnqualifiedJoin node)
        {
            CommandStatistics.JoinsCount++;
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(LikePredicate node)
        {
            CommandStatistics.LikesCount++;
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(SelectStatement node)
        {
            CommandStatistics.SelectsCount++;
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(InsertStatement node)
        {
            CommandStatistics.InsertsCount++;
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(UpdateStatement node)
        {
            CommandStatistics.UpdatesCount++;
            base.ExplicitVisit(node);
        }

        public override void ExplicitVisit(DeleteStatement node)
        {
            CommandStatistics.DeletesCount++;
            base.ExplicitVisit(node);
        }
    }
}