using SnippetVault.Core.Domain.Entities;

namespace SnippetVault.Core.Helpers
{
    public class QueryPivot
    {
        public DateTime PivotDateTime { get; private set; }

        public Guid PivotId { get; private set; }

        public QueryPivot(DateTime pivotDatetime, Guid pivotId)
        {
            PivotDateTime = pivotDatetime;
            PivotId = pivotId;
        }
    }

    public static class SnippetExtensions
    {
        public static QueryPivot GetQueryPivot(this Snippet snippet)
        {
            return new QueryPivot(snippet.SnippetCreatedDateTime.Value, snippet.SnippetId.Value);
        }
    }
}