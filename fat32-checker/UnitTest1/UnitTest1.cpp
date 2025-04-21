#include "pch.h"
#include "CppUnitTest.h"
#define private public  // ���� ������ � ��������� ������
#include "../fat32-checker/fat32-checker.cpp"
#undef private

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace UnitTest

{
    void writeFilesToFile(const std::vector<File>& files, const std::string& outputPath) {
        // ��������� ���� ��� ������ (�������������, ���� ���� ����������)
        std::ofstream outFile("./HHHHHHHHHHHHHHHHHHHHHHH.txt");

        if (!outFile.is_open()) {
            std::cerr << "������ ��� �������� ����� ��� ������." << std::endl;
            return;
        }

        // �������� �� ���� ������ � ���������� �� � ����
        for (const auto& file : files) {
            // ���������� ������ ������� ����� � ����
            outFile << "��� �����: " << file.name << std::endl;
            outFile << "������ �������: " << file.firstClaster << std::endl;
            outFile << "������ �����: " << file.fileSize << std::endl;
            outFile << "-----------------------------------" << std::endl;
        }

        // ��������� ����
        outFile.close();
    }
    void writeBrokenFilesToFile(const std::vector<BrokenFileInfo>& brokenFiles, const std::string& filename) {
        std::ofstream outFile("./ffffffffffffffffff.txt");  // ��������� ���� ��� ������
        if (outFile.is_open()) {
            for (const auto& broken : brokenFiles) {
                outFile << broken.filename << " --- " << broken.errorMessage << "\n";  // ���������� ������ ������
            }
            outFile.close();  // ��������� ���� ����� ������
        }
        else {
            std::cerr << "Error opening file for writing.\n";
        }
    }

    void writeLostClustersToFile(const std::vector<uint32_t>& fatTable, const std::vector<File>& lostFiles) {
        std::ofstream outFile("PPPPPPPPPPPPPPPP.txt");
        if (!outFile.is_open()) {
            std::cerr << "Error opening file for writing.\n";
            return;
        }

        for (const auto& lostFile : lostFiles) {
            uint32_t currentCluster = lostFile.firstClaster;
            outFile << "Lost file starting with cluster " << currentCluster << " has clusters: ";

            while (currentCluster < fatTable.size() && fatTable[currentCluster] != 0x00000000) {
                outFile << currentCluster << " ";
                currentCluster = fatTable[currentCluster];
            }

            outFile << ".\n";
        }

        outFile.close();
    }

    TEST_CLASS(MyClassTest)
    {
    protected:
        Fat32* fs;

        // ������������� ������ ��� �����
        TEST_METHOD_INITIALIZE(SetUp)
        {
            fs = new Fat32("42.img", true);  // ������������� ������� Fat32
        }

        // ������� ����� �����
        TEST_METHOD_CLEANUP(TearDown)
        {
            delete fs;  // �������� ������� ����� �����
        }

    public:
        TEST_METHOD(UpdateFatHelperTableWorks)
        {
            // ��������� �������� �������� � fatTable_
            fs->fatTable_ = {
                0, 0, 0, 0,      // 0�3
                7, 6, 3, 9,      // 4�7
                5, 8, 4, 0,      // 8�11
                0x0FFFFFF7,      // 12 � bad cluster
                0x0FFFFFFF,      // 13 � EOF
                13, 14, 15,      // 14�16
                0, 0,            // 17�18
                20, 21, 22, 8,   // 19�22
                0,               // 23
                25, 26, 27, 28,  // 24�27
                26,              // 28
                0,               // 29
                0x0FFFFFFF,      // 30 � EOF
                0, 0             // 31�32
            };

            fs->files_ = {
                File{ "A.txt", "A.txt", "", 0, 10 },
                File{ "C.doc", "C.doc", "", 0, 19 },
                File{ "D.bin", "D.bin", "", 0, 24 }
            };

            // ������� �����, ������� ����� ��������������
            fs->updateFatHelperTable();
            fs->findLostFiles(fs->fatTable_);
            writeFilesToFile(fs->getFiles(), "output.txt");
            writeBrokenFilesToFile(fs->getBrokenFiles(), "./ooooo.txt");
            writeLostClustersToFile(fs->getFATTable(), fs->getLostFiles());
            // ��������: ����� �������� �������������� �������� � ASSERT ��� EXPECT
            // ��������:
            // Assert::IsTrue(fs->fatHelperTable_.size() > 0d);  // ��������, ��� ������� �� �����
        }
    };
}
