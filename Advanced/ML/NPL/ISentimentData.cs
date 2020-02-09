using Microsoft.ML.Data;

namespace Engine.Advanced.ML.NPL
{
    public abstract class ISentimentData
    {
        [LoadColumn(0)]
        public string SentimentText;

        /// <summary>
        /// Comment
        /// </summary>
        [LoadColumn(1), ColumnName("Label")]
        public bool Sentiment;
    }
}