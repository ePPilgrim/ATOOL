using System;
using System.Collections.Generic;
using System.IO;

namespace ATOOL
{
    public class ModulesDependency
    {
        private FileStream parentFunctionStream = null;
        private FileStream childeFunctionStream = null;
        private StreamReader modulesIDsStream = null;
        private IList<Node> grapsh = null;

        public IDictionary<string,Node> DependencyTable {get;private set;}

        public ModulesDependency(string parrentFileName, string childFileName, string ModulesIDsFileName){
            parentFunctionStream = File.Create(parrentFileName);
            childeFunctionStream = File.Create(childFileName);
            modulesIDsStream = new StreamReader(ModulesIDsFileName);
            DependencyTable = new Dictionary<string,Node>();
        }

        public class Node{
           public string FunctionName;
           public int? ModuleID = null;
           public int State = 0;
           public ICollection<Node> Relatives; 

           public Node(string functionName, ICollection<Node> relatives){
               FunctionName = functionName;
               Relatives = relatives;
           }
        }

        public void SetRelationFromFile(string parrentFileName, string childFileName, string ModulesIDsFileName){
            using(var parentFunctionStream = new StreamReader(parrentFileName))
            using(var childeFunctionStream = new StreamReader(childFileName))
            using(var modulesIDsStream = File.Create(ModulesIDsFileName)){
               string parentFunc = null;
               string childFunc = null;
               byte[] bytes = {0,0,0,0};
               while((parentFunc = parentFunctionStream.ReadLine()) != null
                    && (childFunc = childeFunctionStream.ReadLine()) != null
                    && modulesIDsStream.Read(bytes,0,bytes.Length) != 0){
                        var modulID = BitConverter.ToInt32(bytes,0);
                        Node parentNode, childeNode = null;
                        if(DependencyTable.TryGetValue(parentFunc, out parentNode)){
                            if(DependencyTable.TryGetValue(childFunc,out childeNode)){
                                if(!parentNode.Relatives.Contains(childeNode)){
                                    parentNode.Relatives.Add(childeNode);
                                }
                            }
                            else{
                                childeNode = new Node(childFunc, new HashSet<Node>());
                                parentNode.Relatives.Add(childeNode);
                                DependencyTable.Add(childFunc,childeNode);
                            } 
                        } else{
                                DependencyTable.Add(parentFunc,new Node(parentFunc, new HashSet<Node>()));
                        }
                        parentNode.ModuleID = modulID;
                    }
            }
            foreach(var node in DependencyTable.Values){
                if(node.State == 0){
                    deepFirstSearchTree(null,node);
                }
            }
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



        
    }
}