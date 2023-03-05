using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kursova_Rabota_1_HTML
{
    using static Utils;
    using static Program;
    public class TreeNode
    {
        private string _tagType;
        private string _tagData;
        private string _data;
        private MyList<TreeNode> _children;
        private TreeNode _parent;
        private bool _noClosingTag;


        //Private funcs
        private void SetParent(TreeNode node)
        {
            this._parent = node;
        }

        private int Depth(IEnumerable<TreeNode> nodes, int depth)
        {
            var nextLevelNodes = new MyList<TreeNode>();
            foreach (TreeNode node in nodes)
            {
                foreach (TreeNode item in node.GetChildren())
                {
                    nextLevelNodes.Add(item);
                }
            }
            if (nextLevelNodes.Count == 0)
                return depth;
            else
                return Depth(nextLevelNodes, depth + 1);
        }

        private void FixParentData()
        {
            foreach (TreeNode child in _children)
            {
                child._parent = this;
                if(child._children.Count >0)
                {
                    child.FixParentData();
                }
            }
        }

        private void PrintAllTagsAndData(TreeNode tn)
        {
            if (tn._noClosingTag == true) { }
            else
            {
                string temp0 = tn._tagData != "" ? " " + tn._tagData : "";
                Console.Write("<" + tn._tagType + temp0 + ">");
                Console.Write(_data);

                PrintAllChildrenDataAndTags(tn);
               
                Console.WriteLine("</" + tn._tagType + ">");
            }
        }

        private void PrintAllChildrenDataAndTags(TreeNode tn)
        {
            Console.Write(tn._data);
            if(tn._children.Count == 0)
                Console.Write(" ");
            
            foreach (TreeNode node in tn._children)
            {
                string temp = node._tagData != "" ? " " + node._tagData : "";
                if (node._noClosingTag == false)
                {
                    Console.Write("<" + node._tagType + temp + ">");
                    Console.Write(node._data != "" ? node._data : "");
                }
                else
                {
                    Console.Write("<" + node._tagType + temp + "/>");
                }


                if (node._children.Count > 0)
                    PrintAllChildrenDataAndTags(node);

                if (node._noClosingTag == false)
                    Console.Write("</" + node._tagType + ">");
            }
        }

        private void PrintAllData(TreeNode tn)
        {
            if (tn._noClosingTag == true) { }
            else
            {
                Console.Write(tn._data != "" ? tn._data + " " : "");
                foreach (TreeNode node in tn._children)
                {
                    Console.Write(node._data != "" ? node._data + " " : "");
                    if (node._children.Count > 0)
                        PrintAllData(node);
                }
            }
        }

        private void NodeToText(MyList<string> output, int level)
        {
            string tabs = "";
            for (int i = 0; i < level; i++)
            {
                tabs += "\t";
            }
            //output.Add(new string());
            if (this._noClosingTag == true)
            {
                string temp = this._tagData != "" ? " " + this._tagData : "";
                output.Add(new string(tabs + "<" + this._tagType + temp + "/>"));
            }
            else
            {
                string temp0 = this._tagData != "" ? " " + this._tagData : "";

                output.Add(new string(tabs + "<" + this._tagType + temp0 + ">"));

                if (_data != "")
                    output.Add(tabs +" " + _data);


                foreach (TreeNode child in this._children)
                {
                    child.NodeToText(output,level+1);
                }

                output.Add(new string(tabs + "</" + this._tagType + ">"));
            }
        }


        private void GetNoodesFromPath(MyList<TreeNode> nodes, MyList<string> path, int level, ref MyList<TreeNode> finalValidNodes)
        {
            if (level >= path.Count)
                return;

            MyList<TreeNode> validNodes = new MyList<TreeNode>();

            string currentPathElement = path[level];
            //Handle * case
            if (currentPathElement == "*")
            {
                validNodes = nodes;
            }
            else
            {
                string[] currentPathSplitElement = SplitData(currentPathElement);


                
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (currentPathSplitElement[1] == "" || int.TryParse(currentPathSplitElement[1], out int n))
                    {
                        if (nodes[i]._tagType == currentPathSplitElement[0])
                        {
                            validNodes.Add(nodes[i]);
                        }
                    }
                    else
                    {
                        Map SplitElementTagData = ProcessTagData(currentPathSplitElement[1]);
                        Map currentElementTagData = ProcessTagData(nodes[i]._tagData);
                        bool containsSameData = SplitElementTagData.ContainsSameData(currentElementTagData);

                        if (nodes[i]._tagType == currentPathSplitElement[0] && containsSameData == true) //&& nodes[i]._tagData == currentPathSplitElement[1]
                        {
                            validNodes.Add(nodes[i]);
                        }

                    }
                }
                //Handles tag[number] case
                int temp;
                if (int.TryParse(currentPathSplitElement[1], out temp) && temp >= 1)
                {

                    TreeNode element = validNodes[temp - 1];
                    validNodes.Clear();
                    validNodes.Add(element);
                }

            }
                if (level == path.Count - 1)
                {
                    foreach (TreeNode node in validNodes)
                    {
                        finalValidNodes.Add(node);
                    }
                }

                foreach (TreeNode vlaidNode in validNodes)
                {
                    GetNoodesFromPath(vlaidNode._children, path, level + 1, ref finalValidNodes);
                }
            

        }


        //Constructors
        public TreeNode()
        {
            _parent = null;
            _children = new MyList<TreeNode>();
            _tagType = "";
            _data = "";
            _tagData = "";
            _noClosingTag = false;
        }
        public TreeNode(string tagType)
        {
            _parent = null;
            _children = new MyList<TreeNode>();
            _tagType = tagType;
            _data = "";
            _tagData = "";
            _noClosingTag = false;
        }

        public TreeNode(string tagType, string tagData)
        {
            _parent = null;
            _children = new MyList<TreeNode>();
            _tagType = tagType;
            _data = "";
            _tagData = tagData;
            _noClosingTag = false;
        }

        public TreeNode(string tagType, string tagData, bool NoClosingTag)
        {
            _parent = null;
            _children = new MyList<TreeNode>();
            _tagType = tagType;
            _data = "";
            _tagData = tagData;
            _noClosingTag = NoClosingTag;
        }

        //Getters
        public string GetTagType() { return _tagType; }
        public TreeNode GetParent() { return _parent; }
        public MyList<TreeNode> GetChildren() { return _children; }

        //Setters
        public void SetTagType(string tagType)
        {
            _tagType = tagType;
        }

        public void SetData(string data)
        {
            _data = data;
        }

        //Copying

        public TreeNode GetDeepCopy()
        {
            TreeNode copy = GetCopyIntern();
            copy.FixParentData();
            return copy;
        }
        private TreeNode GetCopyIntern()
        {
            TreeNode copy = new TreeNode();
            copy._tagType = _tagType;
            copy._tagData = _tagData;
            copy._data = _data;
            copy._noClosingTag = _noClosingTag;
            copy._parent = null;
            MyList<TreeNode> copyOfChildren;
            if (this._children.Count > 0)
            {
                copyOfChildren = this.GetCopyOfChildrenIntern();
            }
            else
            {
                copyOfChildren = new MyList<TreeNode>();
            }
            copy._children = copyOfChildren;

            return copy;
        }

        public MyList<TreeNode> GetDeepCopyOfChildren()
        {
            MyList<TreeNode> copyOfChildren = GetCopyOfChildrenIntern();
            foreach (TreeNode child in copyOfChildren)
            {
                child.FixParentData();
            }
            return copyOfChildren;
        }
        private MyList<TreeNode> GetCopyOfChildrenIntern()
        {
            //_tagType;
            //_tagData;
            //_data;
            //_children;
            //_parent;
            //_noClosingTag;

            MyList<TreeNode> childrenCopies = new MyList<TreeNode>();

            foreach (TreeNode child in _children)
            {
                TreeNode childCopy = child.GetCopyIntern();
                childrenCopies.Add(childCopy);
            }

            return childrenCopies;
            
        }

        //Other funcs

        public void Trim()
        {
            char[] trimCharacters = { ' ', '\t' };

            _data = MyTrim(_data, trimCharacters);
            foreach (TreeNode child in _children)
            {
                child.Trim();
            }
        }
        public void AddToData(char symbol)
        {
            _data += symbol;
        }

        public void AddChild(string tagName)
        {
            TreeNode treeNode = new TreeNode(tagName);
            treeNode.SetParent(this);
            this._children.Add(treeNode);
        }

        public void AddChild(string tagName, string tagData)
        {
            TreeNode treeNode = new TreeNode(tagName, tagData);
            treeNode.SetParent(this);
            this._children.Add(treeNode);
        }
        public void AddChild(string tagName, string tagData, bool noClosingTag)
        {
            TreeNode treeNode = new TreeNode(tagName, tagData, noClosingTag);
            treeNode.SetParent(this);
            this._children.Add(treeNode);
        }

        public void AddChild(TreeNode child)
        {
            child.SetParent(this);
            this._children.Add(child);
        }
        public  void PrintTree(TreeNode tree, string indent, bool last)
        {
            Console.Write(indent + "+- " + tree._tagType);
            indent += last ? "   " : "|  ";

            Console.WriteLine();
            for (int i = 0; i < tree._children.Count; i++)
            {
                PrintTree(tree._children[i], indent, i == tree._children.Count - 1);
            }
        }
        public int GetDepth()
        {
            if (_children.Count == 0)
                return 0;
            else
                return Depth(_children, 1);
        }

        public void PrintData(string command)
        {
            if(command =="//")
            {
                PrintAllTagsAndData(this);
            }
            else if(command =="//*")
            {
                foreach (TreeNode item in this._children)
                {
                    PrintAllTagsAndData(item);
                }
            }
            else
            {
                
                MyList<string> path = Split(command, '/');
                

                //Process the '*' symbol
                //if(path[path.Count - 1] == "*")
                //{
                //    printTags = false;
                //    path.RemoveAt(path.Count - 1);
                //}

                MyList<TreeNode> nodes = ProcessPathAndGetNode(path);
                foreach (TreeNode node in nodes)
                {
                    PrintAllChildrenDataAndTags(node);
                }
                
            }

            Console.WriteLine();
        }
        public void SetData(string parent, string data)
        {
            if (parent == "//")
            {
                _children.Clear();
                string nodeData;
                MyList<TreeNode> childrenNodes = PartOfHTMLToTree(data, out nodeData);

                _data = nodeData;
                foreach (TreeNode item in childrenNodes)
                {
                    AddChild(item);
                } 
            }
            else
            {
                MyList<TreeNode> parentNodes;
                MyList<string> parentPath = Split(parent, '/');
                parentNodes = ProcessPathAndGetNode(parentPath);

                

                string nodeData;
                MyList<TreeNode> childrenNodes = PartOfHTMLToTree(data, out nodeData);

                foreach (TreeNode node in parentNodes)
                {
                    node._children.Clear();
                    node._data = nodeData;
                    foreach (TreeNode item in childrenNodes)
                    {
                        node.AddChild(item);
                    }
                }
            }
        }

        public void CopyData(string from, string to)
        {

            
            MyList<TreeNode> parentsToCopyFrom;
            

            if (from == "//")
            {
                parentsToCopyFrom = new MyList<TreeNode>();
                parentsToCopyFrom.Add(this);
            }
            else
            {
                MyList<string> splitFrom = Split(from,'/');
                parentsToCopyFrom = ProcessPathAndGetNode(splitFrom);
            }


            MyList<TreeNode> parentsToPasteTo;
            if (to =="//")
            {
                parentsToPasteTo = new MyList<TreeNode>();
                parentsToPasteTo.Add(this);
            }
            else
            {
                MyList<string> splitTo = Split(to, '/');
                parentsToPasteTo = ProcessPathAndGetNode(splitTo);
            }


            MyList<TreeNode> copiedChildren = new MyList<TreeNode>();

            //Create the copies
            string copiedData = "";
               foreach (TreeNode parent in parentsToCopyFrom)
               {
                   //Get the data
                   copiedData += parent._data;

                   //Get the children
                   MyList<TreeNode> children = parent.GetDeepCopyOfChildren();
                   foreach (TreeNode child in children)
                   {
                       copiedChildren.Add(child);
                   }
               }




                foreach (TreeNode parent in parentsToPasteTo)
                {
                    parent._data += copiedData;

                    foreach (TreeNode child in copiedChildren)
                    {
                        parent.AddChild(child);
                    }
                }

                
            }

        public void SaveToFile(string fileName)
        {
            MyList<string> lines = new MyList<string>();
            NodeToText(lines,0);
            File.WriteAllLines(fileName, lines);
            
        }

        public MyList<TreeNode> ProcessPathAndGetNode(MyList<string> input)
        {
            
            MyList<TreeNode> nodes = new MyList<TreeNode>();
            nodes.Add(this);
            MyList<TreeNode> valid = new MyList<TreeNode>();
            GetNoodesFromPath(nodes, input, 0, ref valid);
            return valid;
        }


        //Override
        public override string ToString()
        {
            return _tagType;
        }


    }

}