#include <iostream>
#include <fstream>
#include <vector>
#include <cstdint>
#include <bitset>
#include <windows.h>  // Для работы с кодировками Windows
#include <iomanip>


#pragma pack(push, 1)
struct BootSector {
    uint8_t  jumpInstruction[3]; // Содержит команду для перехода (jump instruction), которая используется в самом начале загрузочного сектора для выполнения кода при старте. Это обычно команда перехода к основному коду загрузчика, который будет выполнять дальнейшую инициализацию.
    char     OEMName[8];         // имя производителя
    uint16_t bytesPerSector;     // Количество байтов в одном секторе
    uint8_t  sectorsPerCluster;  // Количество секторов в одном кластере.
    uint16_t reservedSectors;    // Количество зарезервированных секторов перед файловой системой. Обычно это количество секторов, которые используются для хранения метаданных файловой системы, например, для области загрузчика и таблицы FAT.
    uint8_t  numFATs;            // Количество таблиц FAT. В FAT32 всегда используется 2 таблицы FAT для избыточности и защиты данных. Обычно это значение равно 2.
    uint16_t rootDirEntries;     // Количество записей в корневой директории. Для FAT16 это значение ограничено, но для FAT32 эта информация уже не имеет смысла, так как файлы и каталоги корневой директории могут располагаться в кластерах, а не фиксированно в таблице записей.
    uint16_t totalSectors16;     // Общее количество секторов на диске. Для дисков, объем которых не превышает 32 МБ, используется это 16-битное поле. Для более больших дисков используется поле totalSectors32.
    uint8_t  mediaType;          // Тип носителя
    uint16_t sectorsPerFAT16;    // Для FAT16 это поле указывает количество секторов в одной таблице FAT. В FAT32 это поле не используется, но может содержать значение для совместимости.
    uint16_t sectorsPerTrack;    // Количество секторов в одном треке жесткого диска. Это значение используется для геометрии дисков в старых системах.
    uint16_t numHeads;           // Количество головок для чтения/записи на диске.
    uint32_t hiddenSectors;      // Количество скрытых секторов перед началом раздела. Это может быть полезно, если перед основным разделом находится зарезервированное пространство, например, для загрузчика или для других целей
    uint32_t totalSectors32;     // Общее количество секторов на диске. Это поле используется для больших дисков, объем которых превышает 32 МБ. В отличие от totalSectors16, которое ограничено 16 битами, это поле позволяет работать с более крупными носителями.
                                 
    // FAT32-specific fields     
                                 
    uint32_t sectorsPerFAT32;    // Количество секторов, занимаемых одной таблицей FAT в файловой системе FAT32. Это поле имеет значение только для FAT32
    uint16_t extFlags;           // Флаги, специфичные для FAT32. Например, могут использоваться для указания наличия нескольких кластерных цепочек.
    uint16_t fsVersion;          // Версия файловой системы FAT32. Обычно для FAT32 это будет 0x00 0x00, что указывает на стандартную версию.
    uint32_t rootCluster;        // Номер первого кластера корневой директории. В отличие от FAT16, где корневая директория имеет фиксированный размер и расположение, в FAT32 корневая директория может занимать несколько кластеров, и этот параметр указывает на первый кластер, где начинается корневая директория.
    uint16_t fsInfoSector;       // Сектор, в котором хранится информация о файловой системе, включая информацию о свободных кластерах.
    uint16_t backupBootSector;   // Сектор, где хранится резервная копия загрузочного сектора. Используется для восстановления при повреждении основного загрузочного сектора.
    uint8_t  reserved[12];       // Зарезервированные байты. Эти байты обычно не используются, но они должны быть заняты для обеспечения совместимости.
    uint8_t  driveNumber;        // Номер диска, на котором находится файловая система (например, номер для виртуального диска или для физического устройства).
    uint8_t  reserved1;          // Зарезервированное поле. Обычно не используется и оставлено для совместимости с более старыми версиями.
    uint8_t  bootSignature;      // Подпись для проверки валидности загрузочного сектора. Это поле обычно содержит значение 0x29, которое указывает, что сектора являются действительными.
    uint32_t volumeID;           // Уникальный идентификатор тома. Это поле используется для идентификации тома и может быть сгенерировано случайным образом.
    char     volumeLabel[11];    // Метка тома (Volume Label). Это строка, которая может содержать метку для раздела (например, "NO_NAME "). Это поле не является обязательным.
    char     fsType[8];          // Тип файловой системы. Например, для FAT32 здесь будет строка "FAT32 ".
};



struct FAT32_DIR_ENTRY_T {
    char DIR_Name[11];
    UINT8 DIR_Attr;
    UINT8 DIR_NTRes;
    UINT8 DIR_CrtTime_Tenth;
    UINT16 DIR_CrtTime;
    UINT16 DIR_CrtDate;
    UINT16 DIR_LstAccDate;
    UINT16 DIR_FstClusHI;
    UINT16 DIR_WrtTime;
    UINT16 DIR_WrtDate;
    UINT16 DIR_FstClusLO;
    UINT32 DIR_FileSize;
};

struct DirEntry {
    uint8_t  name[11];
    uint8_t  attr;
    uint8_t  ntRes;
    uint8_t  crtTimeTenth;
    uint16_t crtTime;
    uint16_t crtDate;
    uint16_t lstAccDate;
    uint16_t fstClusHi;
    uint16_t wrtTime;
    uint16_t wrtDate;
    uint16_t fstClusLo;
    uint32_t fileSize;
};

struct LFNEntry {
    uint8_t order;
    uint16_t name1[5];
    uint8_t attr;
    uint8_t type;
    uint8_t checksum;
    uint16_t name2[6];
    uint16_t zero;
    uint16_t name3[2];
};

#pragma pack(pop)




// Функция для чтения таблицы FAT
std::vector<uint32_t> readFAT(std::ifstream& disk, const BootSector& bs) {
    uint32_t fatSize = bs.sectorsPerFAT32 * bs.bytesPerSector;
    std::vector<uint32_t> fatTable(fatSize / sizeof(uint32_t));

    uint32_t fatOffset = bs.reservedSectors * bs.bytesPerSector;    // Адрес начала FAT
    disk.seekg(fatOffset, std::ios::beg);                           // Функция seekg перемещает указатель чтения в файл на заданное смещение. std::ios::beg означает, что отсчёт идет от начала файла (то есть, указатель будет перемещен на fatOffset байтов от начала файла). Это гарантирует, что мы начинаем читать таблицу FAT именно с того места, где она начинается на диске.
    disk.read(reinterpret_cast<char*>(fatTable.data()), fatSize);   // преобразует указатель на вектор в указатель на массив байтов (массив символов char). Это необходимо, потому что функция read работает с потоком байтов, а не с типизированными данными.

    if (!disk) {
        std::cerr << "Error: Failed to read FAT\n";
    }
    return fatTable;
}

// Функция для чтения загрузочного сектора
void readBootSector(const std::string& filename, BootSector& bs, std::vector<uint32_t>& fatTable) {
    std::ifstream disk(filename, std::ios::binary);
    if (!disk) {
        std::cerr << "Error: Failed to open file\n";
        return;
    }

    disk.read(reinterpret_cast<char*>(&bs), sizeof(BootSector));    //reinterpret_cast<char*>(&bs) преобразует указатель на структуру bs в указатель на массив байтов, то есть на char*. Почему именно char? Потому что в C++ char всегда представляет собой 1 байт данных.
    if (!disk) {                                                    //После этого ты получаешь указатель на первый байт структуры BootSector, и таким образом можешь записать в эту структуру данные, прочитанные из файла.
        std::cerr << "Error: Failed to read boot sector\n";
        return;
    }

    std::cout << "File system: " << std::string(bs.fsType, 8) << "\n";
    std::cout << "Cluster size: " << (bs.sectorsPerCluster * bs.bytesPerSector) << " bytes\n";
    std::cout << "Root directory's first cluster: " << bs.rootCluster << "\n";
    std::cout << "FAT size: " << (bs.sectorsPerFAT32 * bs.bytesPerSector) << " bytes\n";

    fatTable = readFAT(disk, bs);

    disk.close();
}

// Подсчет количества кластеров
void findClusterCount(const BootSector& bs) {
    uint32_t totalSectors = bs.totalSectors32;
    uint32_t fatSectors = bs.sectorsPerFAT32 * bs.numFATs;
    uint32_t dataSectors = totalSectors - (bs.reservedSectors + fatSectors);
    uint32_t totalClusters = dataSectors / bs.sectorsPerCluster;

    std::cout << "Total clusters: " << totalClusters << "\n";
}




bool isLFNEntry(const DirEntry& entry) {
    return (entry.attr & 0x0F) == 0x0F;
}

std::string decodeLFN(const std::vector<LFNEntry>& entries) {
    std::u16string name;
    for (auto it = entries.rbegin(); it != entries.rend(); ++it) {
        const LFNEntry& e = *it;
        for (auto ch : e.name1) if (ch != 0xFFFF && ch != 0) name += ch;
        for (auto ch : e.name2) if (ch != 0xFFFF && ch != 0) name += ch;
        for (auto ch : e.name3) if (ch != 0xFFFF && ch != 0) name += ch;
    }
    return std::string(name.begin(), name.end());
}

std::string decodeShortName(const uint8_t name[11]) {
    std::string shortName;
    for (int i = 0; i < 8 && name[i] != ' '; ++i)
        shortName += static_cast<char>(name[i]);
    if (name[8] != ' ')
        shortName += '.' + std::string(reinterpret_cast<const char*>(&name[8]), 3);
    return shortName;
}

void readDirectory(std::string filename, const BootSector& bs, const std::vector<uint32_t>& fatTable, uint32_t cluster = 0, const std::string& path = "/") {
    std::ifstream image(filename, std::ios::binary);
    if (!image) {
        std::cerr << "Ошибка: Не удалось открыть файл " << filename << "\n";
        return;
    }

    uint32_t dataBegin = bs.reservedSectors + (bs.numFATs * bs.sectorsPerFAT32);
    if (cluster == 0) {
        cluster = bs.rootCluster;
    }

    const uint32_t bytesPerCluster = bs.bytesPerSector * bs.sectorsPerCluster;
    std::vector<LFNEntry> lfnBuffer;

    while (cluster < 0x0FFFFFF8 && cluster >= 2) {
        uint32_t sector = dataBegin + (cluster - 2) * bs.sectorsPerCluster;
        uint32_t offset = sector * bs.bytesPerSector;

        image.seekg(offset);
        std::vector<uint8_t> clusterData(bytesPerCluster);
        image.read(reinterpret_cast<char*>(clusterData.data()), bytesPerCluster);

        for (size_t i = 0; i < bytesPerCluster; i += 32) {
            DirEntry* entry = reinterpret_cast<DirEntry*>(&clusterData[i]);

            if (entry->name[0] == 0x00) return;
            if (entry->name[0] == 0xE5) continue;

            if (isLFNEntry(*entry)) {
                lfnBuffer.push_back(*reinterpret_cast<LFNEntry*>(entry));
                continue;
            }

            std::string fileEntryName = lfnBuffer.empty() ? decodeShortName(entry->name)
                : decodeLFN(lfnBuffer);
            lfnBuffer.clear();

            uint32_t firstCluster = (entry->fstClusHi << 16) | entry->fstClusLo;

            if ((entry->attr & 0x10) && (fileEntryName == "." || fileEntryName == "..")) continue;

            std::string fullPath = path + fileEntryName;

            std::cout << (entry->attr & 0x10 ? "[DIR] " : "[FILE] ")
                << std::setw(30) << fullPath
                << " | Size: " << entry->fileSize
                << " | Cluster: " << firstCluster << "\n";

            if ((entry->attr & 0x10) && firstCluster >= 2)
                readDirectory(filename, bs, fatTable, firstCluster, fullPath + "/");
        }

        cluster = fatTable[cluster];
    }
}
int main() {
    BootSector bs;
    std::vector<uint32_t> fatTable;                         // Вектор с таблицей FAT
    std::vector<uint32_t> bad_files;                        // Вектор с какашкой
    std::string filename = R"(\\.\F:)";                     // Диск с FAT32

    readBootSector(filename, bs, fatTable);
    findClusterCount(bs);

    readDirectory(filename, bs, fatTable);


 
    for (size_t i = 0; i < 50 &&  i < fatTable.size(); ++i) {
        if (fatTable[i] == 0x0FFFFFF7 || fatTable[i] == 0x0FFFFFF8) {
            std::cout << "FAT[" << i << "] = " << fatTable[i] << " (BAD CLUSTER)";
            bad_files.push_back(i);
        }
        else if (fatTable[i] == 0x0FFFFFFF)
        {
            std::cout << "FAT[" << i << "] = " << fatTable[i] << " (EOF)";
        }
        else
        {
            std::cout << "FAT[" << i << "] = " << fatTable[i];
        }
        std::cout << "\n";
    }

    for (uint32_t file : bad_files) {
        std::cout << "bad clusters: " << file << "/n";
    }
    std::cin.get();                                           // Ожидает ввода, чтобы окно консоли не закрывалось сразу
    return 0;
}