namespace Artwork_Stack
{
    public class result
    { 
        public string GsearchResultClass;
        public string width;
        public string height;
        public string imageId;
        public string tbWidth;
        public string tbHeight;
        public string unescapedUrl;
        public string url;
        public string visibleUrl;
        public string title;
        public string titleNoFormatting;
        public string originalContextUrl;
        public string content;
        public string contentNoFormatting;
        public string tbUrl;
    }
    public class page
    {
        public string start;
        public string label;
    }
    public class cursor
    {
        public page[] pages;
        public string estimatedResultCount;
        public string currentPageIndex;
        public string moreResultsUrl;
    }

    public class responseData
    {
        public result[] results;
        public cursor cursor;
    }
    
    public class gImgAPI
    {
        public responseData responseData;
        public string responseDetails;
        public string responseStatus;
    }
}
