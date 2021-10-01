
#include "pch.h"
#include "CppUnitTest.h"

#include "../../SubTest/SubTest/CreateSubTestProject.h"
#include "../../SubTest/SubTest/SubTestCommon.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace MSUnitTest
{

	TEST_CLASS(MSUnitTest)
	{
	public:

		TEST_METHOD_INITIALIZE(test_method_initialize) 
		{
			setlocale(LC_ALL, "Japanese");// ���P�[���w��
		}

		TEST_METHOD(Boost�o�[�W�����`�F�b�N)
		{
			std::string ve = SubTest::CheckBoost();
			std::string vexp = "1.77.0";
			Assert::AreEqual(vexp, ve);
			Logger::WriteMessage(vexp.c_str());

		}
		TEST_METHOD(�X���b�h�v�[���e�X�g)
		{

			Assert::IsFalse(SubTest::QueryState());

			CALLBACK_MES cb = CALLBACK_MES();
						
			setlocale(LC_ALL, "Japanese");// ���P�[���w��
			char guidchar[MAX_PATH];
			sprintf_s(guidchar, MAX_PATH, "B08C972DA3C149439A55A58214B1E6BA");
			SubTest::CreateSubTestProject(guidchar, cb);
			Assert::IsFalse(!SubTest::QueryState());
		}
	};

	TEST_CLASS(DateTest)
	{
	public:

		TEST_METHOD(orz�N���X����)
		{
			//setlocale(LC_ALL, "Japanese");// ���P�[���w��
			//char guidchar[MAX_PATH];
			//sprintf_s(guidchar, MAX_PATH, __FUNCTION__);
			std::string vexp = "��������";


			Logger::WriteMessage(vexp.c_str());

			SubTest::DataSet(1, 2, 3);

			SubTest::Date dd = SubTest::GetData();

			Assert::AreEqual(1, dd.year());
			Assert::AreEqual(2, dd.month());
			Assert::AreEqual(3, dd.day());
			Assert::IsFalse(dd.quer());

			SubTest::DataSet(2012, 3, 31);
			dd = SubTest::GetData();

			Assert::AreEqual(2012, dd.year());
			Assert::AreEqual(3, dd.month());
			Assert::AreEqual(31, dd.day());

		}
		TEST_METHOD(orz�N���X����2)
		{

			SubTest::DataSet(1, 2, 3);

			SubTest::Date dd = SubTest::GetData();

			Assert::AreEqual(1, dd.year());
			Assert::AreEqual(2, dd.month());
			Assert::AreEqual(3, dd.day());
			Assert::IsTrue(dd.is_ok());

		}

	};
}

