using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using ATOOL;

namespace UTEST
{
    [TestClass]
    public class TestConvertIntoCSRMatrix
    {
        string columnMapFileName = "columnMapNames.txt";
        string funcRelationsFileName = @"..\..\..\funcRelations.json";
        string inputCRinfoFileName = "inputCR.txt";
        string csrmatrixFileName = "csrmatrix.csr";
        string crdateFileName = "crdata.txt";

        ConvertIntoCSRMatrix CsrMatrixObj = null;


        public TestConvertIntoCSRMatrix(){
            CsrMatrixObj = new ConvertIntoCSRMatrix(funcRelationsFileName);
            var input = new List<string>{
                "21","11","22","12","31","23","32","EndOfModuleID",
                "345", "412","0983","EndOfDepID",
                "33"
                };

            using(var columnMapStream = new StreamWriter(columnMapFileName)){
                for(int i = 0; i < input.Count; ++ i){
                    columnMapStream.WriteLine(input[i]);
                }
            }

            var inputCRinfo = new List<string>{
                "6111111;20190101;[f11,f31,f22];983;31",
                "6222222;20190202;[f12,f32];345;32",
                "6333333;20190303;[f11;f23;f22];412;33"
            };
            using(var CRinfoStream = new StreamWriter(inputCRinfoFileName)){
                for(int i = 0; i < inputCRinfo.Count; ++ i){
                    CRinfoStream.WriteLine(inputCRinfo[i]);
                }
            }
        }

        [TestMethod]
        public void TestSetColumnMap()
        {
            CsrMatrixObj.SetColumnMap(columnMapFileName);

            Assert.AreEqual(CsrMatrixObj.ModulePosition(21),0);
            Assert.AreEqual(CsrMatrixObj.ModulePosition(11),1);
            Assert.AreEqual(CsrMatrixObj.ModulePosition(22),2);
            Assert.AreEqual(CsrMatrixObj.ModulePosition(12),3);
            Assert.AreEqual(CsrMatrixObj.ModulePosition(31),4);
            Assert.AreEqual(CsrMatrixObj.ModulePosition(23),5);
            Assert.AreEqual(CsrMatrixObj.ModulePosition(32),6);
            Assert.AreEqual(CsrMatrixObj.DepartmentPosition(345),7);
            Assert.AreEqual(CsrMatrixObj.DepartmentPosition(412),8);
            Assert.AreEqual(CsrMatrixObj.DepartmentPosition(983),9);
            Assert.AreEqual(CsrMatrixObj.YearPosition,10); 
        }
                //new HashSet<int>{11} - 11
                //new HashSet<int>{12} - 12
               // new HashSet<int>{11,12,21} - 21
               // new HashSet<int>{11,22} - 22
               // new HashSet<int>{11,12,23} - 23
               // new HashSet<int>{11,12,21,22,23,31} -31
               // new HashSet<int>{11,12,23,32} - 32
        [TestMethod]
        public void TestCreateCSRMatrix()
        {
            var masterDim = "3,10";
            var masterRows = "0,8,14,20";
            var masterColumns = "0,1,2,3,4,5,9,10,1,3,5,6,7,10,1,2,3,5,8,10";
            var masterValues = "1,3,2,1,1,1,1,31,1,2,1,1,1,32,3,1,1,1,1,33";
            var masterCRs = new List<string>{"6111111", "6222222", "6333333"};

            CsrMatrixObj.CreateCSRMatrix(inputCRinfoFileName,csrmatrixFileName,crdateFileName);

            using(var csrMatrixStream = new StreamReader(csrmatrixFileName)){
                Assert.AreEqual(csrMatrixStream.ReadLine().Equals(masterDim),true);
                Assert.AreEqual(csrMatrixStream.ReadLine().Equals(masterRows),true);
                Assert.AreEqual(csrMatrixStream.ReadLine().Equals(masterColumns),true);
                Assert.AreEqual(csrMatrixStream.ReadLine().Equals(masterValues),true);
            }

            using(var crDataStream = new StreamReader(crdateFileName)){
                Assert.AreEqual(crDataStream.ReadLine().Equals(masterCRs[0]),true);
                Assert.AreEqual(crDataStream.ReadLine().Equals(masterCRs[1]),true);
                Assert.AreEqual(crDataStream.ReadLine().Equals(masterCRs[2]),true);
            }
          
        }

        
    }
}