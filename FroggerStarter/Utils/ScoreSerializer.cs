using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using FroggerStarter.Model;

namespace FroggerStarter.Utils
{
    /// <summary>
    ///     Serializes and unserializes xml files
    /// </summary>
    public static class ScoreSerializer
    {
        #region Methods

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="highScores">The high scores.</param>
        public static async Task SerializeObject(IList<HighScore> highScores)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(GameSettings.HighScoresFilename,
                CreationCollisionOption.ReplaceExisting);
            var serializer = new XmlSerializer(typeof(List<HighScore>));
            var outStream = await file.OpenStreamForWriteAsync();
            using (outStream)
            {
                serializer.Serialize(outStream, highScores);
            }
        }

        /// <summary>
        ///     Deserializes this instance.
        /// </summary>
        /// <returns></returns>
        public static List<HighScore> Deserialize()
        {
            var highScores = new List<HighScore>();
            try
            {
                highScores = Task.Run(deserialize).Result;
            }
            catch (AggregateException e)
            {
                return highScores;
            }

            return highScores;
        }

        /// <summary>
        /// Deserializes the specified file name.
        /// </summary>
        /// <returns></returns>
        private static async Task<List<HighScore>> deserialize()
        {
            var folder = ApplicationData.Current.LocalFolder;
            StorageFile file;
            try
            {
                file = await folder.GetFileAsync(GameSettings.HighScoresFilename);
            }
            catch (FileNotFoundException)
            {
                return new List<HighScore>();
            }

            var deserializer = new XmlSerializer(typeof(List<HighScore>));
            var inStream = await file.OpenStreamForReadAsync();
            inStream.Position = 0;
            var highScoresFromXml = (List<HighScore>) deserializer.Deserialize(inStream);
            inStream.Dispose();

            return highScoresFromXml;
        }

        #endregion
    }
}