using System.Collections.Generic;

namespace Artwork_Stack.Model
{
    public class UnifiedResponse
    {
        public int ResultsCount     { get; set; }
        public List<Result> Results { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }

        public UnifiedResponse()
        {
            ResultsCount = 0;
            Results = new List<Result>();
            Success = true;
            Error = string.Empty;
        }
    }

    public class Result
    {
        public string Request        { get; set; }
        public string Artist         { get; set; }
        public string Album          { get; set; }
        public string Url            { get; set; }
        public string Thumb          { get; set; }
        public object AdditionalInfo { get; set; }
        public int    Width          { get; set; }
        public int    Height         { get; set; }

        public Result()
        {
            Request = Artist = Album = Url = Thumb = string.Empty;
            AdditionalInfo = new object();
            Width = Height = 0;
        }
    }
}
