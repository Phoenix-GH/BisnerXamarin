namespace Bisner.Constants
{
    public class ProviderNames
    {
        // Public providers
        public const string PublicTextPostProvider = "PublicTextPostProvider";
        public const string PublicImagePostProvider = "PublicImagePostProvider";

        // private providers
        public const string TextPostProvider = "TextPostProvider";
        public const string TextUrlPostProvider = "TextUrlPostProvider";
        public const string DocumentPostProviderNew = "DocumentPostProviderNew";
        public const string DocumentPostProviderNewVersion = "DocumentPostProviderNewVersion";
        public const string FileImagePostprovider = "FileImagePostprovider";
        public const string TaskPostProviderNew = "TaskPostProviderNew";
        public const string NotePostProviderNew = "NotePostProviderNew";

        // new collaboration feed models
        public const string CollaborationTaskPostProvider = "CollaborationTaskPostProvider";
        public const string CollaborationNotePostProvider = "CollaborationNotePostProvider";
        public const string CollaborationFilePostProvider = "CollaborationFilePostProvider";
        public const string CollaborationMultipleFilePostProvider = "CollaborationMultipleFilePostProvider";

        // whitelabel providers -> Seperate because we might want to merge feeds later on
        public const string WhitelabelTextPostProvider = "WhitelabelTextPostProvider";
        public const string WhitelabelTextUrlPostProvider = "WhitelabelTextUrlPostProvider";
        public const string WhitelabelImagePostProvider = "WhitelabelImagePostProvider";
        public const string WhitelabelEventPostProvider = "WhitelabelEventPostProvider";
        public const string WhitelabelArticlePostProvider = "WhitelabelArticlePostProvider";

        #region NOT USED IN CODE
        
        public const string ImagePostProvider = "ImagePostProvider";
        public const string ImageGalleryPostProvider = "ImageGalleryPostProvider";

        #endregion NOT USED IN CODE
    }
}