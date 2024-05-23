namespace Storage.Options
{
    public sealed class StorageOptions()
    {
        public StorageOptions(string path)
            : this()
        {
            FolderPath = path;
        }

        public const string Position = "Storage";

        private string _folderPath = "./Storage";

        public string FolderPath
        {
            get => _folderPath;
            set
            {
                if (Path.Exists(value) == false)
                    throw new DirectoryNotFoundException();
                _folderPath = value;
            }
        }

        public string GetPath(string key) => Path.Combine(_folderPath, key);
    }
}
