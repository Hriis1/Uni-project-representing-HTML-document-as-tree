using System;
using System.IO;
using System.Collections.Generic;

namespace Kursova_Rabota_1_HTML
{
    using static Utils;
    class Program
    {
        public static MyList<TreeNode> PartOfHTMLToTree(string input, out string nodeData)
        {

            TreeNode htmlTree = new TreeNode("1stNode");
            TreeNode currentParentElement = htmlTree;

            bool InArrows = false;
            bool closingTag = false;
            bool NoClosingTag = false;
            bool inData = false;
            bool inTagName = true;
            string currentNodeName = "";
            string currentTagData = "";
            nodeData = "";

            for (int i = 0; i < input.Length; i++)
            {
                //Checks we are in a opening / closing tag
                if (InArrows == false)
                {
                    //Check if a tag opens
                    if (input[i] == '<')
                    {
                        InArrows = true;
                        inTagName = true;
                    }
                    else if(inData == false)
                    {
                        nodeData += input[i];
                    }
                    else if (input[i] != ' ' && input[i] != '\t' || inData == true)
                    {
                        currentParentElement.AddToData(input[i]);
                    }
                }
                else
                {
                    //Checks of the tag is a closing tag
                    if (input[i] == '/' && input[i-1] == '<')
                    {
                        closingTag = true;
                        inData = false;
                    }
                    else if (input[i] == '/' && input[i+1] == '>')
                    {
                        NoClosingTag = true;
                        closingTag = true;
                    }
                    //Checks if the tag ends
                    else if (input[i] == '>')
                    {
                        //Happens if we are in a opening tag
                        if (closingTag == false)
                        {
                            InArrows = false;
                            inData = true;
                            currentParentElement.AddChild(currentNodeName, currentTagData);
                            currentParentElement = currentParentElement.GetChildren()[currentParentElement.GetChildren().Count - 1];
                            currentNodeName = "";
                            currentTagData = "";
                        }
                        //Happens if we are in a tag without a closing tag
                        else if (closingTag == true && NoClosingTag == true)
                        {
                            InArrows = false;
                            closingTag = false;
                            NoClosingTag = false;
                            inData = false;
                            currentParentElement.AddChild(currentNodeName, currentTagData, true);
                            currentNodeName = "";
                            currentTagData = "";
                        }
                        //Happens if we are in an closing tag
                        else
                        {
                            closingTag = false;
                            InArrows = false;
                            inData = false;
                            currentParentElement = currentParentElement.GetParent();
                        }
                    }
                    else if (input[i] == ' ')
                    {
                        inTagName = false;
                    }
                    else if (closingTag == false && inTagName == true)
                    {
                        currentNodeName += input[i];
                    }
                    else if (closingTag == false && inTagName == false)
                    {
                        currentTagData += input[i];
                    }
                }


            }

            return htmlTree.GetChildren();
        }
        static TreeNode HTMLToTree(string filePath)
        {
            TreeNode htmlTree = new TreeNode("Node0");
            TreeNode currentParentElement = htmlTree;

            int openingEquelClosing = 0; //Check if there are equel amound of closing and opening tags

            //filePath = @"D:\uni stuff\TU\SAA\Kursova_Rabota_1_HTML\HTMLcodeOrig.txt";
            string[] lines = File.ReadAllLines(filePath);

            bool InArrows = false;
            bool closingTag = false;
            bool NoClosingTag = false;
            bool inTagName = true;
            string currentNodeName = "";
            string closingNodeNameCheck = "";
            string currentTagData = "";

            //Loop the lines
            for (int i = 0; i < lines.Length; i++)
            {
                //Loop the symbols
                for (int j = 0; j < lines[i].Length; j++)
                {
                    //Checks we are in a opening / closing tag
                    if (InArrows == false)
                    {
                        //Check if a tag opens
                        if (lines[i][j] == '<')
                        {
                            InArrows = true;
                            inTagName = true;
                            if(lines[i][j+1] != '/')
                                openingEquelClosing++;
                        }
                        else
                        {
                            //if (lines[i][j] != ' ' && lines[i][j] != '\t')
                            {
                                currentParentElement.AddToData(lines[i][j]);
                            }
                        }
                        
                    }
                    else
                    {
                        //Checks if the tag is a closing tag
                        if (lines[i][j] == '/' && lines[i][j - 1] == '<')
                        {
                            openingEquelClosing--;
                            if(openingEquelClosing < 0)
                            {
                                Console.WriteLine("Error in HTML more closing than opening tags");
                                return null;
                            }
                            closingTag = true;
                        }
                        //Checks if the tag doesn't have a closing tag
                        else if (lines[i][j] == '/' && lines[i][j + 1] == '>')
                        {
                            openingEquelClosing--;
                            NoClosingTag = true;
                            closingTag = true;
                        }
                        //Checks if the tag ends
                        else if (lines[i][j] == '>')
                        {
                            
                            //Happens if we are in a opening tag
                            if (closingTag == false)
                            {
                                InArrows = false;
                                currentParentElement.AddChild(currentNodeName, currentTagData);
                                currentParentElement = currentParentElement.GetChildren()[currentParentElement.GetChildren().Count - 1];
                                currentNodeName = "";
                                currentTagData = "";
                            }
                            //Happens if we are in a tag without a closing tag
                            else if (closingTag == true && NoClosingTag == true)
                            {
                                InArrows = false;
                                closingTag = false;
                                NoClosingTag = false;
                                currentParentElement.AddChild(currentNodeName, currentTagData, true);
                                currentNodeName = "";
                                currentTagData = "";
                            }
                            //Happens if we are in an closing tag
                            else
                            {
                                closingTag = false;
                                InArrows = false;
                                string sameTagCheck = currentParentElement.GetTagType();
                                if (sameTagCheck != closingNodeNameCheck)
                                {
                                    Console.WriteLine("A different tag was closed then the one last openned");
                                    return null;
                                }
                                currentParentElement = currentParentElement.GetParent();                             
                                currentNodeName = "";
                                closingNodeNameCheck = "";
                            }
                        }
                        else if (lines[i][j] == ' ')
                        {
                            inTagName = false;
                        }
                        else if (closingTag == false && inTagName == true)
                        {
                            currentNodeName += lines[i][j];
                        }
                        else if (closingTag == false && inTagName == false)
                        {
                            currentTagData += lines[i][j];
                        }
                        else if(closingTag == true)
                        {
                            closingNodeNameCheck += lines[i][j];
                        }
                    }


                }
            }

            if(openingEquelClosing != 0)
            {
                Console.WriteLine("Error in HTML: unequel amound of closing and opening tags");
                return null;
            }

            htmlTree.Trim();
            return htmlTree.GetChildren()[0];
        }

        
        static void Main(string[] args)
        {

            //char[] trimchars = { ' ', '\t' };
            //string z = MyTrim("  \t\t  Sa nkata\t \t  ", trimchars);

            TreeNode htmlTree = HTMLToTree(@"D:\uni stuff\TU\SAA\Kursova_Rabota_1_HTML\outputFile.txt");
            if(htmlTree == null)
            {
                return;
            }

            //htmlTree.SaveToFile(@"D:\uni stuff\TU\SAA\Kursova_Rabota_1_HTML\outputFile.txt");

            Console.WriteLine(htmlTree.GetDepth());
            htmlTree.PrintTree(htmlTree, "", true);
            while (true)
            {
                
                Console.WriteLine("Enter a command: PRINT, SET, COPY, SAVE");
                string command = Console.ReadLine();

                switch (command)
                {
                    case "PRINT":
                        command = Console.ReadLine();
                        htmlTree.PrintData(command);
                        break;
                    case "SET":
                        string parent = Console.ReadLine();
                        string childrenData = Console.ReadLine();
                        htmlTree.SetData(parent, childrenData);
                        break;
                    case "COPY":
                        string from = Console.ReadLine();
                        string to = Console.ReadLine();
                        htmlTree.CopyData(from, to);
                        break;
                    case "SAVE":
                        htmlTree.SaveToFile(@"D:\uni stuff\TU\SAA\Kursova_Rabota_1_HTML\outputFile.txt");
                        break;

                }
            }
        }
    }
}
