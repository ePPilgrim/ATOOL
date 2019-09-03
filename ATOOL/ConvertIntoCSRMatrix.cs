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
            var rows = "0";
            var columns = "";
            var values = "";
            using(var crInfoStream = new StreamReader(crInfoFileName))
            {
                string str;
                int nextRowIndex = 1;
                while(!String.IsNullOrEmpty(str = crInfoStream.ReadLine().Trim())){
                    var list = str.Split(';');
                    crs.Add(list[0]); // just number of CR
                    var moduleIdHist = getModuleIdsHistogram(list[2]); // index 1 is for data
                    foreach(var key in moduleIdHist.Keys){ //key is index
                        columns += $",{key}";
                        values += $",{moduleIdHist[key]}";
                        nextRowIndex ++;
                    }
                    var depPosition = DepartmentPosition(Convert.ToInt32(list[3].Trim()));
                    columns += $",{depPosition},{YearPosition}";
                    values += $",1,{list[4].Trim()}";
                    nextRowIndex += 2;
                    rows += $"{nextRowIndex}";
                }

            }
             //using(var outputCRStream = new StreamWriter(outputCRsFileName))
            //using(var outputCSRMatrixStream = new StreamWriter(outputCSRMatrixFileName))
        }

        IDictionary<int,int> getModuleIdsHistogram(string functs){
            functs = functs.TrimStart('[');
            functs = functs.TrimEnd(']');
            var hist = new Dictionary<int,int>();
            foreach(var fun in functs.Split(',')){
                foreach(var id in funcRelations.GetTouchedModules(fun)){
                    var pos = ModulePosition(id);
                    if(hist.ContainsKey(pos)) hist[pos] ++;
                    else{
                        hist.Add(pos,1);
                    }
                }
            }
            return hist;
        }



    }
}