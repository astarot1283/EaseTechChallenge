using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EaseTechChallenge
{
    public static class FileGraphMapper
    {
        public static List<Node> ConvertToNodeList(this int[] lineValues, int lineFileIndex) 
        {            
            var nodes = new List<Node>(); 

            for (int i = 0; i < lineValues.Length; i++)
            {
                nodes.Add(
                    lineValues[i]
                    .ToString()
                    .ConvertToNode(lineFileIndex, i));
            }

            return nodes;
        }

        public static List<int> ConvertToIntegerNodeList(this string lineFile, string separator) 
        {
            return lineFile.Split(separator).Select(int.Parse).ToList();
        } 

        public static Node ConvertToNode(this string lineValue, int lineFileIndex, int lineValueIndex) 
        {
            return new Node(int.Parse(lineValue), new Point(lineFileIndex, lineValueIndex));
        }
    }
}
