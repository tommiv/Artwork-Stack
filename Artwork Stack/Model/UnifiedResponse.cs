using System.Collections.Generic;

namespace Artwork_Stack.Model
{
    internal class UnifiedResponse
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

    internal class Result
    {
        public string Request { get; set; }
        public string Artist  { get; set; }
        public string Album   { get; set; }
        public string Url     { get; set; }
    }
}
