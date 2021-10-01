#include "pch.h"
#include "CppUnitTest.h"


using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace DummyTest
{


	TEST_CLASS(DummyTest1)
	{
	public:

		TEST_METHOD(test_method_1)
		{

		
		}

		TEST_METHOD(test_method_2) { Logger::WriteMessage(__FUNCTION__); }
	};
}