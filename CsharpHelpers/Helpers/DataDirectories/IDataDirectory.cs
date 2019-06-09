namespace CsharpHelpers.Helpers
{
    public interface IDataDirectory
    {
        string DirectoryPath { get; }
        string GetFilePath(string fileName);
    }
}
