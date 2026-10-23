namespace Scriptorium_Nova.Web.Common;

public static class EntityValidation
{
    public static class EntryType
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 40;
        public const int DescriptionMaxLength = 300;
    }

    public static class Entry
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 40;
        public const int SummaryMinLength = 10;
        public const int SummaryMaxLength = 200;
        public const int DetailsMaxLength = 4000;
    }

    public static class Note
    {
        public const int TitleMinLength = 3;
        public const int TitleMaxLength = 40;
        public const int TextMinLength = 10;
        public const int TextMaxLength = 2000;
    }
}