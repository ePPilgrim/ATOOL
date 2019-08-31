using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using ATOOL;

namespace UTEST
{
    [TestClass]
    public class TestModulesDependency
    {
        string functionNames = "funNames.txt";
        string functRefNames = "funRefNames.txt";
        string moduleIDs = "moduleIDs.txt";

        [TestMethod]
        public void CreateFunctionDependencyFromScratchTest1()
        {
            var inputFuncNameList = new List<string>{
                "f11","f11","f11","f11","f11",
                "f12","f12",
                "f21","f21",
                "f22",
                "f23","f23",
                "f31","f31",
                "f32"
                };
            var inputFuncRefNameList = new List<string>{
                "f21","s112","f22","s111","f23",
                "f23","f21",
                "s211","f31",
                "f31",
                "f31","f32",
                "s311","s312",
                "s321"
            };
            var inputFuncModulIDList = new List<string>{
                "11","11","11","11","11",
                "12","12",
                "21","21",
                "22",
                "23","23",
                "31","31",
                "32"
            };

            using(var funcNameStream = new StreamWriter(functionNames))
            using(var funcRefNameStream = new StreamWriter(functRefNames))
            using(var moduleIDStream = new StreamWriter(moduleIDs)){
                for(int i = 0; i < inputFuncNameList.Count; ++ i){
                    funcNameStream.WriteLine(inputFuncNameList[i]);
                    funcRefNameStream.WriteLine(inputFuncRefNameList[i]);
                    moduleIDStream.WriteLine(inputFuncModulIDList[i]);
                }
            }

            var funcNamesList = new List<string>{"f11", "f12", "f21", "f22", "f23", "f31", "f32"};
            var modulIDsList = new List<HashSet<int>>{
                new HashSet<int>{11},
                new HashSet<int>{12},
                new HashSet<int>{11,12,21},
                new HashSet<int>{11,22},
                new HashSet<int>{11,12,23},
                new HashSet<int>{11,12,21,22,23,31},
                new HashSet<int>{11,12,23,32}
            };

            var obj = new ModulesDependency();

            obj.SetRelation(functionNames, functRefNames, moduleIDs);

            Assert.AreEqual(modulIDsList[0].SetEquals(obj.GetTouchedModules(funcNamesList[0])),true);
            Assert.AreEqual(modulIDsList[1].SetEquals(obj.GetTouchedModules(funcNamesList[1])),true);
            Assert.AreEqual(modulIDsList[2].SetEquals(obj.GetTouchedModules(funcNamesList[2])),true);
            Assert.AreEqual(modulIDsList[3].SetEquals(obj.GetTouchedModules(funcNamesList[3])),true);
            Assert.AreEqual(modulIDsList[4].SetEquals(obj.GetTouchedModules(funcNamesList[4])),true);
            Assert.AreEqual(modulIDsList[5].SetEquals(obj.GetTouchedModules(funcNamesList[5])),true);
            Assert.AreEqual(modulIDsList[6].SetEquals(obj.GetTouchedModules(funcNamesList[6])),true);    
        }

        [TestMethod]
        public void CreateFunctionDependencyFromFile1()
        {
            var funcNamesList = new List<string>{"f11", "f12", "f21", "f22", "f23", "f31", "f32"};
            var modulIDsList = new List<HashSet<int>>{
                new HashSet<int>{11},
                new HashSet<int>{12},
                new HashSet<int>{11,12,21},
                new HashSet<int>{11,22},
                new HashSet<int>{11,12,23},
                new HashSet<int>{11,12,21,22,23,31},
                new HashSet<int>{11,12,23,32}
            };

            var obj = new ModulesDependency();

            obj.SetRelation();

            Assert.AreEqual(modulIDsList[0].SetEquals(obj.GetTouchedModules(funcNamesList[0])),true);
            Assert.AreEqual(modulIDsList[1].SetEquals(obj.GetTouchedModules(funcNamesList[1])),true);
            Assert.AreEqual(modulIDsList[2].SetEquals(obj.GetTouchedModules(funcNamesList[2])),true);
            Assert.AreEqual(modulIDsList[3].SetEquals(obj.GetTouchedModules(funcNamesList[3])),true);
            Assert.AreEqual(modulIDsList[4].SetEquals(obj.GetTouchedModules(funcNamesList[4])),true);
            Assert.AreEqual(modulIDsList[5].SetEquals(obj.GetTouchedModules(funcNamesList[5])),true);
            Assert.AreEqual(modulIDsList[6].SetEquals(obj.GetTouchedModules(funcNamesList[6])),true);    
        }

        [TestMethod]
        public void CreateFunctionDependencyFromScratchTest2()
        {
            var inputFuncNameList = new List<string>{
                "f11","f11","f11","f11","f11",
                "f12","f12","f12",
                "f21","f21",
                "f22","f22",
                "f23","f23","f23",
                "f31","f31","f31",
                "f32","f32","f32"
                };
            var inputFuncRefNameList = new List<string>{
                "f21","s112","f22","s111","f23",
                "f23","f21","f12",
                "s211","f31",
                "f31", "f21",
                "f31","f32","f22", 
                "s311","s312","f22",
                "s321", "f31","f12"
            };
            var inputFuncModulIDList = new List<string>{
                "11","11","11","11","11",
                "12","12","12",
                "21","21",
                "22","22",
                "23","23","23",
                "31","31","31",
                "32","32","32"
            };

            using(var funcNameStream = new StreamWriter(functionNames))
            using(var funcRefNameStream = new StreamWriter(functRefNames))
            using(var moduleIDStream = new StreamWriter(moduleIDs)){
                for(int i = 0; i < inputFuncNameList.Count; ++ i){
                    funcNameStream.WriteLine(inputFuncNameList[i]);
                    funcRefNameStream.WriteLine(inputFuncRefNameList[i]);
                    moduleIDStream.WriteLine(inputFuncModulIDList[i]);
                }
            }

            var funcNamesList = new List<string>{"f11", "f12", "f21", "f22", "f23", "f31", "f32"};
            var modulIDsList = new List<HashSet<int>>{
                new HashSet<int>{11},
                new HashSet<int>{12,32,23,11},
                new HashSet<int>{11,12,21,22,23,31,32},
                new HashSet<int>{11,22,12,21,23,31,32},
                new HashSet<int>{11,12,23,32},
                new HashSet<int>{11,12,21,22,23,31,32},
                new HashSet<int>{11,12,23,32}
            };

            var obj = new ModulesDependency();

            obj.SetRelation(functionNames, functRefNames, moduleIDs);

            Assert.AreEqual(modulIDsList[0].SetEquals(obj.GetTouchedModules(funcNamesList[0])),true);
            Assert.AreEqual(modulIDsList[1].SetEquals(obj.GetTouchedModules(funcNamesList[1])),true);
            Assert.AreEqual(modulIDsList[2].SetEquals(obj.GetTouchedModules(funcNamesList[2])),true);
            Assert.AreEqual(modulIDsList[3].SetEquals(obj.GetTouchedModules(funcNamesList[3])),true);
            Assert.AreEqual(modulIDsList[4].SetEquals(obj.GetTouchedModules(funcNamesList[4])),true);
            Assert.AreEqual(modulIDsList[5].SetEquals(obj.GetTouchedModules(funcNamesList[5])),true);
            Assert.AreEqual(modulIDsList[6].SetEquals(obj.GetTouchedModules(funcNamesList[6])),true);    
        }

        [TestMethod]
        public void CreateFunctionDependencyFromFileTest2()
        {
            var funcNamesList = new List<string>{"f11", "f12", "f21", "f22", "f23", "f31", "f32"};
            var modulIDsList = new List<HashSet<int>>{
                new HashSet<int>{11},
                new HashSet<int>{12,32,23,11},
                new HashSet<int>{11,12,21,22,23,31,32},
                new HashSet<int>{11,22,12,21,23,31,32},
                new HashSet<int>{11,12,23,32},
                new HashSet<int>{11,12,21,22,23,31,32},
                new HashSet<int>{11,12,23,32}
            };

            var obj = new ModulesDependency();

            obj.SetRelation(functionNames, functRefNames, moduleIDs);

            Assert.AreEqual(modulIDsList[0].SetEquals(obj.GetTouchedModules(funcNamesList[0])),true);
            Assert.AreEqual(modulIDsList[1].SetEquals(obj.GetTouchedModules(funcNamesList[1])),true);
            Assert.AreEqual(modulIDsList[2].SetEquals(obj.GetTouchedModules(funcNamesList[2])),true);
            Assert.AreEqual(modulIDsList[3].SetEquals(obj.GetTouchedModules(funcNamesList[3])),true);
            Assert.AreEqual(modulIDsList[4].SetEquals(obj.GetTouchedModules(funcNamesList[4])),true);
            Assert.AreEqual(modulIDsList[5].SetEquals(obj.GetTouchedModules(funcNamesList[5])),true);
            Assert.AreEqual(modulIDsList[6].SetEquals(obj.GetTouchedModules(funcNamesList[6])),true);    
        }
    }
}
