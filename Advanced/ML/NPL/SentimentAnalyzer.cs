using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.IO;
using System.Linq;
using static Microsoft.ML.DataOperationsCatalog;

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
                /// <summary>
                /// Sentiment Analysys class, designed using Microsoft's Tutorial: Analyze sentiment of website comments with binary classification in ML.NET[1]
                /// It's just an implementation of the tutorial's code as an object that can be reused.
                ///
                /// [1] https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/sentiment-analysis#next-steps
                /// </summary>
                /// <typeparam name="T1">A Type that inherits from <see cref="ISentimentData"/></typeparam>
                public class SentimentAnalyzer<T1> where T1 : ISentimentData, new()
                {
                    /// <summary>
                    /// Path to the dataset.
                    /// </summary>
                    private string datasetPath;

                    /// <summary>
                    /// Our ML Enviroment.
                    /// </summary>
                    private MLContext mlEnv;

                    /// <summary>
                    /// The dataset split between training data and testing data
                    /// </summary>
                    private TrainTestData splitDataView;

                    /// <summary>
                    /// The Model we will train.
                    /// </summary>
                    private ITransformer mlModel;

                    /// <summary>
                    /// Delegate void for the OnModelTrained event
                    /// </summary>
                    public delegate void ModelTrained();

                    /// <summary>
                    /// This event is triggered when the model finishes training.
                    /// </summary>
                    public event ModelTrained OnModelTrained;

                    /// <summary>
                    /// Delegate void for the OnModelEvaluated event
                    /// </summary>
                    public delegate void ModelEvaluated(string accuracy, string confidence, string f1);

                    public event ModelEvaluated OnModelEvaluated;

                    /// <summary>
                    /// Initializes a new instance of the Sentiment Analysis Class. It loads the specified Dataset from the Data directory.
                    /// </summary>
                    /// <param name="dataName">The dataset to load. It defaults to SentimentAnalysisDataset.txt if left empty</param>
                    public SentimentAnalyzer(string dataName = "SentimentAnalysisDataset.txt")
                    {
                        mlEnv = new MLContext();
                        datasetPath = Path.Combine(Environment.CurrentDirectory, "Data", dataName);
                        splitDataView = LoadDataset(0.1);
                    }

                    /// <summary>
                    /// This function loads the dataset and splits it in sets for training and testing
                    /// </summary>
                    /// <param name="percentage">The percentatge of the dataset that will be used for testing.</param>
                    /// <returns>The split training and testing dataset</returns>
                    public TrainTestData LoadDataset(double percentage)
                    {
                        IDataView dataView = mlEnv.Data.LoadFromTextFile<T1>(datasetPath, hasHeader: false);
                        TrainTestData splitDataView = mlEnv.Data.TrainTestSplit(dataView, testFraction: percentage);
                        return splitDataView;
                    }

                    public void BuildAndTrainModel()
                    {
                        var estimator = mlEnv.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(ISentimentData.SentimentText)).Append(mlEnv.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));
                        mlModel = estimator.Fit(splitDataView.TrainSet);
                        try
                        {
                            OnModelTrained();
                        }
                        catch (NullReferenceException)
                        {
                            throw new UnhandledEventException();
                        }
                    }

                    public void EvaluateModel()
                    {
                        IDataView predictions = mlModel.Transform(splitDataView.TrainSet);
                        CalibratedBinaryClassificationMetrics modelMetrics = mlEnv.BinaryClassification.Evaluate(predictions, "Label");
                        try
                        {
                            OnModelEvaluated(modelMetrics.Accuracy.ToString(), modelMetrics.AreaUnderRocCurve.ToString(), modelMetrics.F1Score.ToString());
                        }
                        catch (NullReferenceException)
                        {
                            throw new UnhandledEventException();
                        }
                    }

                    /// <summary>
                    /// This function predicts wheter a given string is positive or negative.
                    /// </summary>
                    /// <param name="sentence">The string to predict from.</param>
                    /// <returns>True if positive, flase if negative.</returns>
                    public (bool sentiment, double probablility, double score) CheckSingleSentiment(string sentence)
                    {
                        PredictionEngine<T1, SentimentPrediction> predictionFunction = mlEnv.Model.CreatePredictionEngine<T1, SentimentPrediction>(mlModel);
                        T1 sampleStatement = new T1 { SentimentText = sentence };
                        SentimentPrediction resultPrediction = predictionFunction.Predict(sampleStatement);
                        return (resultPrediction.Prediction, resultPrediction.Probability, resultPrediction.Score);
                    }

                    [Serializable]
                    public class UnhandledEventException : Exception
                    {
                        public override string Message => "You didn't handle an Event. Make sure to handle all events properly";
                    }
                }
            }
        }
    }
}