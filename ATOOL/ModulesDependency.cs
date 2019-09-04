using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ATOOL
{
    public class ModulesDependency
    {
        private IDictionary<string,Node> functionDependency;
        private ISet<string> functionNames;

        public void SetRelationInFile(string parrentFileName, string childFileName,
                                         string modulesIDsFileName, string outputFileName){
            functionNames = getUniqueFunctionNames(parrentFileName);
            functionDependency = new Dictionary<string,Node>();
            using(var parentFunctionStream = new StreamReader(parrentFileName))
            using(var childeFunctionStream = new StreamReader(childFileName))
            using(var modulesIDsStream = new StreamReader(modulesIDsFileName)){
               string parentFunc, childFunc, moduleIDStr;
               while((parentFunc = parentFunctionStream.ReadLine()) != null
                && (childFunc = childeFunctionStream.ReadLine()) != null
                && (moduleIDStr = modulesIDsStream.ReadLine()) != null){
                    Node parentNode, childNode;
                    if(!functionDependency.TryGetValue(parentFunc, out parentNode)){
                        parentNode = new Node(parentFunc, new HashSet<Node>());
                        functionDependency.Add(parentFunc, parentNode);
                    }
                    parentNode.ModuleID = Convert.ToInt32(moduleIDStr);
                    
                    if(functionNames.Contains(childFunc)){
                        if(!functionDependency.TryGetValue(childFunc, out childNode)){
                            childNode = new Node(childFunc, new HashSet<Node>());
                            functionDependency.Add(childFunc, childNode);
                        }
                        if(!parentNode.Relatives.Contains(childNode)){
                            parentNode.Relatives.Add(childNode);
                        }
                    }
                }
            }
            foreach(var node in functionDependency.Values){
                if(node.State == 0){
                    deepFirstSearchTree(null,node);
                }
            }
            saveInJsonFormat(outputFileName);
        }

        public void SetRelationFromFile(string jsonFuncRelationFileName){
            var serializer = new JsonSerializer();
            IList<JsonNode> jsonFunDependency; 
            using(var funcRelationStream = new StreamReader(jsonFuncRelationFileName)){
                jsonFunDependency = (List<JsonNode>) serializer.Deserialize(funcRelationStream,typeof(List<JsonNode>));
            }

            functionDependency = new Dictionary<string,Node>();
            Node parentNode, childNode;      
            foreach(var val in jsonFunDependency){
                if(!functionDependency.TryGetValue(val.FunctionName, out parentNode)){
                    parentNode = new Node(val.FunctionName, new List<Node>(val.Relatives.Count));
                    functionDependency.Add(val.FunctionName,parentNode);
                }
                parentNode.ModuleID = val.ModuleID;
                foreach(var childFunName in val.Relatives){
                    if(!functionDependency.TryGetValue(childFunName, out childNode)){
                        childNode = new Node(childFunName, new List<Node>(32));
                        functionDependency.Add(childFunName,childNode);
                    } 
                    parentNode.Relatives.Add(childNode);
                }
            }
        }

        public ISet<int> GetTouchedModules(string functionName){
            foreach(var node in functionDependency.Values){
                node.State = 0;
            }
            return getTouchedModules(functionName);
        }

        ISet<int> getTouchedModules(string functionName){
            Node node;
            if(functionDependency.TryGetValue(functionName, out node)){
                if(node.ModuleID is null) return null;
                var set = new HashSet<int>();
                if(node.State != 0) return set;
                node.State = 1;
                set.Add((int)node.ModuleID);
                foreach(var val in node.Relatives){
                    //if(val.ModuleID != null) set.Add((int)val.ModuleID);
                    var subset = getTouchedModules(val.FunctionName);
                    if(subset != null) set.UnionWith(subset);
                    //else return null;
                }
                return set;
            }
            return null;
        }

        private void deepFirstSearchTree(Node parentNode, Node node){
            node.State = 1;
            var childSet = node.Relatives;
            node.Relatives = new List<Node>(128);
            if(parentNode != null){
                node.Relatives.Add(parentNode);
            }
            foreach(var val in childSet){
                if(val.State != 0){
                    val.Relatives.Add(node);
                } else{
                    deepFirstSearchTree(node, val);
                }
            }
        } 
        private ISet<string> getUniqueFunctionNames(string funcNameFileName){
            var functionNames = new HashSet<string>();
            using(var funcNameStream = new StreamReader(funcNameFileName)){
                string funName;
                functionNames = new HashSet<string>();
                while((funName = funcNameStream.ReadLine()) != null){
                    functionNames.Add(funName);
                }
            }
            return functionNames;
        }

        private void saveInJsonFormat(string funcRelationFileName){
            var serializer = new JsonSerializer();
            using(var funcRelationStream = new StreamWriter(funcRelationFileName)){
                serializer.Serialize(funcRelationStream, functionDependency.Select(
                x => new JsonNode{
                    FunctionName = x.Value.FunctionName,
                    ModuleID = x.Value.ModuleID,
                    Relatives = x.Value.Relatives.Select(y=>y.FunctionName).ToList()
                }
                ).ToList());
            } 
        }

        private class Node{
           public string FunctionName;
           public int? ModuleID = null;
           public int State = 0;
           public ICollection<Node> Relatives; 

           public Node(string functionName, ICollection<Node> relatives){
               FunctionName = functionName;
               Relatives = relatives;
           }
        }

        private class JsonNode{
            public string FunctionName;
            public int? ModuleID;
            public IList<string> Relatives;
        }
    
    }
}