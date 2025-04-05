#include <iostream>
#include <fstream>
#include <vector>
#include <cstdint>
#include <bitset>
#include <windows.h>  
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
                                 
struct DirEntry {                              
    uint8_t  name[11];           // Массив из 11 байт, который хранит имя файла или директории.
    uint8_t  attr;               // Атрибуты файла или директории. Это флаг, который может включать различные значения, такие как: 0x01 — атрибут "только для чтения" 0x02 — атрибут "скрытый файл"  0x04 — атрибут "системный файл"  0x08 — атрибут "метка тома" 0x10 — атрибут "директория" 0x20 — атрибут "архив"
    uint8_t  ntRes;              // Этот байт зарезервирован для использования в операционных системах, основанных на DOS. В современных системах его значение обычно игнорируется.
    uint8_t  crtTimeTenth;       // Десятая часть секунды при создании файла (значение от 0 до 99). Это поле дает точность до 10 миллисекунд.
    uint16_t crtTime;            // Время создания файла
    uint16_t crtDate;            // Дата создания файла
    uint16_t lstAccDate;         // Дата последнего доступа к файлу в таком же формате, как и для crtDate
    uint16_t fstClusHi;          // Старшая часть номера кластера первого кластера файла (старшие 16 бит). Для файлов, чьи данные не умещаются в одном кластере, эта часть указывает на номер первого кластера данных.
    uint16_t wrtTime;            // Время последней записи
    uint16_t wrtDate;            // Дата последней записи
    uint16_t fstClusLo;          // Младшая часть номера кластера первого кластера файла (младшие 16 бит).
    uint32_t fileSize;           // Размер файла в байтах.
};                               
                                 
struct LFNEntry {                // Эта структура описывает запись длинного имени файла, которое в FAT32 не может быть сохранено в одной записи DirEntry, поскольку FAT32 использует ограничение 8.3 для имени. 
    uint8_t order;               // Порядковый номер записи. Это значение определяет порядок, в котором части имени должны быть собраны. Оно начинается с младших значений и увеличивается для каждой новой записи LFN.
    uint16_t name1[5];           // Первые 5 символов длинного имени. Поскольку в FAT32 используется кодировка UTF-16, каждый символ занимает 2 байта.
    uint8_t attr;                // Атрибут записи, который всегда равен 0x0F для записи длинного имени.
    uint8_t type;                // Этот байт зарезервирован для использования в операционных системах, основанных на DOS, и обычно равен 0.
    uint8_t checksum;            // Контрольная сумма имени файла в формате 8.3. Это значение используется для проверки правильности собранного имени файла.
    uint16_t name2[6];           // Следующие 6 символов длинного имени, представленных в формате UTF-16.
    uint16_t zero;               // Два байта, которые всегда равны нулю. Это значение используется для выравнивания структуры.
    uint16_t name3[2];           // Последние два символа длинного имени, представленных в формате UTF-16.
};

#pragma pack(pop)

class Fat32 {

public:
    
    Fat32(const std::string filename)
        : filename_(filename)   // Инициализация const string   
    {
        diskOpener();
        readBootSector();
        readDirectory(bs_, fatTable_);
        printD(fatTable_);
    }

    ~Fat32() {
        if (disk_.is_open()) {
            disk_.close();
        }
    }

private:

    void diskOpener() {
        disk_.open(filename_, std::ios::binary);
        if (!disk_) {
            // вызвать деструктор
            throw std::runtime_error("file isnt open");     
        }
        std::cout << "file is open!";                                 
    }

    void readBootSector() {

        disk_.read(reinterpret_cast<char*>(&bs_), sizeof(BootSector));             //reinterpret_cast<char*>(&bs) преобразует указатель на структуру bs в указатель на массив байтов, то есть на char*. Почему именно char? Потому что в C++ char всегда представляет собой 1 байт данных.
        if (!disk_) {                                                              //После этого ты получаешь указатель на первый байт структуры BootSector, и таким образом можешь записать в эту структуру данные, прочитанные из файла.
            throw std::runtime_error("Error: Failed to read boot sector\n");
            return;
        }

        std::cout << "File system: " << std::string(bs_.fsType, 8) << "\n";
        std::cout << "Cluster size: " << (bs_.sectorsPerCluster * bs_.bytesPerSector) << " bytes\n";
        std::cout << "Root directory's first cluster: " << bs_.rootCluster << "\n";
        std::cout << "FAT size: " << (bs_.sectorsPerFAT32 * bs_.bytesPerSector) << " bytes\n";

        fatTable_ = readFAT(bs_);
    }

    std::vector<uint32_t> readFAT( const BootSector& bs) {
        uint32_t fatSize = bs.sectorsPerFAT32 * bs.bytesPerSector;
        std::vector<uint32_t> fatTable(fatSize / sizeof(uint32_t));

        uint32_t fatOffset = bs.reservedSectors * bs.bytesPerSector;                // Адрес начала FAT
        disk_.seekg(fatOffset, std::ios::beg);                                      // Функция seekg перемещает указатель чтения в файл на заданное смещение. std::ios::beg означает, что отсчёт идет от начала файла (то есть, указатель будет перемещен на fatOffset байтов от начала файла). Это гарантирует, что мы начинаем читать таблицу FAT именно с того места, где она начинается на диске.
        disk_.read(reinterpret_cast<char*>(fatTable.data()), fatSize);              // преобразует указатель на вектор в указатель на массив байтов (массив символов char). Это необходимо, потому что функция read работает с потоком байтов, а не с типизированными данными.

        if (!disk_) {
            throw std::runtime_error("Error: Failed to read FAT\n");
        }
        return fatTable;
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

    void readDirectory(const BootSector& bs, const std::vector<uint32_t>& fatTable, uint32_t cluster = 0, const std::string& path = "/") { //кластер, с которого начинается чтение (по умолчанию 0)

        uint32_t dataBegin = bs.reservedSectors + (bs.numFATs * bs.sectorsPerFAT32);                        // определяет начало данных на диске, который включает все зарезервированные сектора и сектора, занятые таблицами FAT. Если cluster равен 0 (по умолчанию), это означает, что нужно начать с корневого кластера, указанный в rootCluster.
        if (cluster == 0) {
            cluster = bs.rootCluster;
        }

        const uint32_t bytesPerCluster = bs.bytesPerSector * bs.sectorsPerCluster;
        std::vector<LFNEntry> lfnBuffer;                                                                    // Вектор lfnBuffer используется для хранения записей длинных имён файлов (LFN, Long File Name).

        while (cluster < 0x0FFFFFF8 && cluster >= 2) {                                                      // Пока значение cluster не указывает на специальный маркер окончания цепочки кластеров (0x0FFFFFF8) и не меньше 2 (так как кластеры с номерами 0 и 1 зарезервированы).
            if (cluster >= fatTable.size()) {
                std::cerr << "Ошибка: кластер " << cluster << " выходит за границы FAT-таблицы.\n";
                return;
            }

            uint32_t sector = dataBegin + (cluster - 2) * bs.sectorsPerCluster;
            uint32_t offset = sector * bs.bytesPerSector;

            disk_.seekg(offset);
            std::vector<uint8_t> clusterData(bytesPerCluster);
            disk_.read(reinterpret_cast<char*>(clusterData.data()), bytesPerCluster);

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

                uint32_t firstCluster = (entry->fstClusHi << 16) | entry->fstClusLo; // сделать фат тэйбл ключ значенпе по значению кластера записывать название и провять на совподение

                if ((entry->attr & 0x10) && (fileEntryName == "." || fileEntryName == "..")) continue;

                std::string fullPath = path + fileEntryName;

                std::cout << (entry->attr & 0x10 ? "[DIR] " : "[FILE] ")
                    << std::setw(30) << fullPath
                    << " | Size: " << entry->fileSize
                    << " | Cluster: " << firstCluster << "\n";

                if ((entry->attr & 0x10) && firstCluster >= 2)
                    readDirectory(bs, fatTable, firstCluster, fullPath + "/");
            }

            cluster = fatTable[cluster];
        }
    }

    BootSector bs_;
    std::vector<uint32_t> fatTable_;                               // Вектор с таблицей FAT
    const std::string filename_;                                   // Диск с FAT32
    std::ifstream disk_;
    std::vector<uint32_t> bad_files_;                        // Вектор с какашкой добавить отдельную функцию для проверки


 public:

    void printD(const std::vector<uint32_t>& fatTable) {

        for (size_t i = 0; i < 50 && i < fatTable.size(); ++i) {
            if (fatTable[i] == 0x0FFFFFF7 || fatTable[i] == 0x0FFFFFF8) {
                std::cout << "FAT[" << i << "] = " << fatTable[i] << " (BAD CLUSTER)";
                //bad_files.push_back(i);
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
    }


};







int main() {

    std::vector<uint32_t> bad_files;                        // Вектор с какашкой
    std::string filename = R"(\\.\F:)";                     // Диск с FAT32
    Fat32 *disk;
    try
    {
        disk = new Fat32(filename);
    }
    catch (const std::exception& e) {
    
        std::cerr << "error: " << e.what() << '\n';
    }
    


    for (uint32_t file : bad_files) {
        std::cout << "bad clusters: " << file << "/n";
    }
    std::cin.get();                                           // Ожидает ввода, чтобы окно консоли не закрывалось сразу
    return 0;
}