using System;

namespace Bisner.ApiModels.General
{
    public class ApiImageModel
    {
        public Guid Id { get; set; }
       
        public string Small { get; set; }

        public string Medium { get; set; }

        public string Large { get; set; }

        public string OriginalFileName { get; set; }

        public string MimeType { get; set; }
    }
}