
    public static class WindowsNameValidator
    {
        private static readonly string[] ReservedNames =
        {
        "CON", "PRN", "AUX", "NUL",
        "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
        "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
    };

        public static bool IsInvalidWindowsName(string? path, out string reason)
        {
            reason = "";

            if (string.IsNullOrWhiteSpace(path))
            {
                reason = "Путь пустой или состоит из пробелов.";
                return true;
            }

            // Проверка на недопустимые символы в пути
            if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                reason = "Путь содержит недопустимые символы.";
                return true;
            }

            string fileName = Path.GetFileName(path); // только имя файла

            if (string.IsNullOrWhiteSpace(fileName))
            {
                reason = "Имя файла отсутствует в пути.";
                return true;
            }

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                reason = "Имя файла содержит недопустимые символы.";
                return true;
            }

            // Проверка на зарезервированные имена
            string baseName = Path.GetFileNameWithoutExtension(fileName).ToUpperInvariant();
            if (Array.Exists(ReservedNames, reserved => reserved.Equals(baseName, StringComparison.OrdinalIgnoreCase)))
            {
                reason = $"Имя \"{baseName}\" является зарезервированным в Windows.";
                return true;
            }

            return false;
        }

    }
