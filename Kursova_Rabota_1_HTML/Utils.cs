using System;
using System.Collections.Generic;
using System.Text;

namespace Kursova_Rabota_1_HTML
{
    public static class Utils
    {
        public static MyList<string> Split(string str, char splitter)
        {
            MyList<string> split = new MyList<string>();

            string currentStr = "";
            foreach (var symbol in str)
            {
                if (symbol == splitter && currentStr != "")
                {
                    split.Add(currentStr);
                    currentStr = "";
                }

                else if (symbol == splitter) { }
                else
                {
                    currentStr += symbol;
                }
            }

            split.Add(currentStr);

            return split;
        }

        public static string[] SplitData(string data)
        {
            string[] processedData = new string[2];
            processedData[0] = "";
            processedData[1] = "";

            bool inTagName = true;

            foreach (var symbol in data)
            {
                if(inTagName == true)
                {
                    if (symbol == '[')
                        inTagName = false;
                    else
                    {
                        processedData[0] += symbol;
                    }
                }
                else
                {
                    if (symbol != ']' && symbol !='@')
                        processedData[1] += symbol;
                }
            }

            return processedData;
        }

        public static Map ProcessTagData(string data)
        {
            bool inKey = false;
            bool inWord = false;
            string key = "";
            string value = "";

            Map map = new Map();
            foreach (var symbol in data)
            {
                if (inKey == false && inWord == false)
                {
                    if (symbol != ' ')
                        inKey = true;
                }


                if (symbol == '=') { }
                else if (symbol == '\'' || symbol =='\"')
                {
                    if (inKey == true && inWord == false)
                    { 
                        inKey = false;
                        inWord = true;
                    }
                    else if(inKey == false && inWord == true)
                    {
                        inKey = false;
                        inWord = false;
                        map.Add(key, value);
                        key = "";
                        value = "";
                    }
                }
                else
                {
                    if (inKey)
                        key += symbol;
                    else if(inWord)
                        value += symbol;
                }
            }
            

            return map;
        }

        public static bool newChildrenOrData(string input)
        {
            if (input[0] == '<')
                return true;

            return false;
        }
        
        public static string MyTrim(string input, char[] trimCharacters)
        {
            string output = "";

            int pFirst = 0;
            int pLast = input.Length - 1;
            bool characterFound = false;

            if (input == "")
                return "";

            while (!characterFound)
            {
                characterFound = true;
                for (int j = 0; j < trimCharacters.Length; j++)
                {
                    if (input[pFirst] == trimCharacters[j])
                    {
                        pFirst++;
                        characterFound = false;
                        break;
                    } 
                }

                if (pFirst == input.Length)
                    return "";
            }

            characterFound = false;
            while (!characterFound)
            {
                characterFound = true;
                for (int j = 0; j < trimCharacters.Length; j++)
                {
                    if (input[pLast] == trimCharacters[j])
                    {
                        pLast--;
                        characterFound = false;
                        break;
                    }
                   
                }

                if (pLast == -1)
                    return "";
            }

            for (int i = pFirst; i <= pLast; i++)
            {
                output += input[i];
            }

            return output;
        }
    }
}
