using Microsoft.ML.Data;

namespace Engine
{
    namespace Advanced
    {
        /// <summary>
        /// This namespace contains everything related to Machine Learning
        /// Using the contents of this class might require lots of dependencies
        /// </summary>
        namespace ML
        {
            /// <summary>
            /// This namespace is dedicated to classes related to Natural Language Processing.
            /// </summary>
            namespace NPL
            {
                public class SentimentData : ISentimentData
                {
                }

                public class SentimentPrediction : SentimentData
                {
                    [ColumnName("PredictedLabel")]
                    public bool Prediction { get; set; }

                    public float Probability { get; set; }

                    public float Score { get; set; }
                }
            }
        }
    }
}