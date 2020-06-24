using EmotionPlatzi.Models;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web;

namespace EmotionPlatzi.Util
{
    public class EmotionHelper
    {
        public EmotionServiceClient emoClient;

        public EmotionHelper(string key)
        {
            emoClient = new EmotionServiceClient(key);
        }

        public async Task<EmoPicture> DetectAndExtractFacesAsync(Stream imageStream)
        {
            Emotion[] emotions = await emoClient.RecognizeAsync(imageStream);
            var emoPicture = new EmoPicture();
            emoPicture.Faces = ExtractFaces(emotions, emoPicture);

            return emoPicture;
        }

        private ObservableCollection<EmoFace> ExtractFaces(Emotion[] emotions, EmoPicture emoPicture)
        {
            //emoPicture.Faces = new ObservableCollection<EmoFace>();
            var facesList = new ObservableCollection<EmoFace>();
            foreach (var emotion in emotions)
            {
                var emoface = new EmoFace()
                {
                    X = emotion.FaceRectangle.Left,
                    Y = emotion.FaceRectangle.Top,
                    Width = emotion.FaceRectangle.Width,
                    Height = emotion.FaceRectangle.Height,
                    Picture = emoPicture
                };

                //emoPicture.Faces.Add(emoface);
                facesList.Add(emoface);
                emoface.Emotions = ProcessEmotions(emotion.Scores, emoface);
            }
            return facesList;
        }

        private ObservableCollection<EmoEmotion> ProcessEmotions(EmotionScores scores, EmoFace emoface)
        {
            var emotionList = new ObservableCollection<EmoEmotion>();

            var properties = scores.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var filterproperties = properties.Where(p => p.PropertyType == typeof(float));

            EmoEmotionEnum emotype = EmoEmotionEnum.Undetermined;
            foreach (var prop in filterproperties)
            {
                if (!Enum.TryParse<EmoEmotionEnum>(prop.Name, out emotype))
                {
                    emotype = EmoEmotionEnum.Undetermined;
                }

                var emoEmotion = new EmoEmotion();
                emoEmotion.Score = (float)prop.GetValue(scores);
                emoEmotion.EmotionType = emotype;
                emoEmotion.Face = emoface;

                emotionList.Add(emoEmotion);
            }
            return emotionList;
        }
    }
}