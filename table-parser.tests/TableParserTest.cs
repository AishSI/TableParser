using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableParser;

namespace table_parser.tests
{
	[TestClass]
	public class TableParserTest
	{
		//void TestParseLine(string inputLine, List<string> expectedResult)
		void TestParseLine(string inputLine, List<string> expectedResult)
		{
			List<string> result = FieldsParserTask.ParseLine(inputLine);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void noFields()
		{
			TestParseLine(string.Empty, new List<string>{ });
		}

		[TestMethod]
		public void oneField()
		{
			//Arrange:
			var inputLine = "hello";
			//Act:
			var result = FieldsParserTask.ParseLine(inputLine);
			//Assert:
			Assert.AreEqual(new[] { "hello" }, result);
		}
	}
}

//public static void RunTests()
//{
	//Test(string.Empty, new string[0]); 									//	Нет полей
	//Test("hello", new[] { "hello"});										//	Одно поле
	//Test("hello world", new[] { "hello", "world" });						//	Больше одного поля  
	//																		//	Разделитель длиной в один пробел																			
	//Test("hello  world", new[] { "hello", "world" });						//	Разделитель длиной >1 пробела//
	//Test("\"bcd ef\"", new[] { "bcd ef" });								//	Пробел внутри кавычек
	//Test("a \"bcd\"", new[] { "a", "bcd"});								//	Поле в кавычках после простого поля
	//Test("\"bcd\" a", new[] { "bcd", "a"});								//	Простое поле после поля в кавычках
	//Test("\"a\"\"b\"", new[] { "a", "b"});								//	Разделитель без пробелов
	//Test(" \"a\"\"b\" ", new[] { "a", "b"});								//	Пробелы в начале или в конце строки
	//Test("\\\"a", new[] { "\\", "a"});									//	Нет закрывающей кавычки
	//Test("\"\\\\\"", new[] { "\\"});										//	Экранированный обратный слэш внутри кавычек
	//																		//	Экранированный обратный слэш перед закрывающей кавычкой
	//Test("'\\\''",new []{"\'"});											//	Экранированные одинарные кавычки внутри одинарных
	//Test("\"\\\"\"",new []{"\""});										// 	Экранированные двойные кавычки внутри двойных
	//Test("\"'a'\"",new []{"'a'"});										//	Одинарные кавычки внутри двойных
	//Test("\'\"\"\'",new []{"\"\""});										//	Двойные кавычки внутри одинарных
	//Test("''",new String[]{""});											//	Пустое поле
	//Test("' ",new String[]{" "});											//	Пробел в конце поля с незакрытой кавычкой 	//

//}