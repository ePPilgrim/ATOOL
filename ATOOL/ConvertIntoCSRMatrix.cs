using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ATOOL
{
    public class ConvertIntoCSRMatrix
    {
        private ModulesDependency funcRelations;
        private IDictionary<int,int> moduleIDColumnMap = null;// modID->position of column
        private IDictionary<int,int> departmentIDColumnMap = null; //depID->position of column
        public int YearPosition {get; private set;} // last position
        public int ModulePosition(int modulID){ return moduleIDColumnMap[modulID];}
        public int DepartmentPosition(int depID){return departmentIDColumnMap[depID];}

        public ConvertIntoCSRMatrix(string funcRelationFileName){
            funcRelations = new ModulesDependency();
            funcRelations.SetRelationFromFile(funcRelationFileName);
        }

        public void SetColumnMap(string columnFileName){
            YearPosition = 0;
            using(var columnStream = new StreamReader(columnFileName)){
                string str;

                moduleIDColumnMap = new Dictionary<int,int>();
                while(!(str = columnStream.ReadLine()).Equals("EndOfModuleID")){
                    moduleIDColumnMap.Add(Convert.ToInt32(str),YearPosition ++);
                }

                departmentIDColumnMap = new Dictionary<int,int>();
                while(!(str = columnStream.ReadLine()).Equals("EndOfDepID")){
                    departmentIDColumnMap.Add(Convert.ToInt32(str),YearPosition ++);
                }
            }
        }

        public void CreateCSRMatrix(string crInfoFileName, string outputCRsFileName, string outputCSRMatrixFileName){
            var crs = new List<string>(16000);
            var rows = "";
            var columns = "";
            var values = "";
            using(var crInfoStream = new StreamReader(crInfoFileName))
            {
                string str;
                while(!String.IsNullOrEmpty(str = crInfoStream.ReadLine().Trim())){
                    var list = str.Split(';');
                    crs.Add(list[0]); // just number of CR

                }

            }
             //using(var outputCRStream = new StreamWriter(outputCRsFileName))
            //using(var outputCSRMatrixStream = new StreamWriter(outputCSRMatrixFileName))
        }



    }
}