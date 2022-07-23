using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EaseTechChallenge
{
    public class Node
    {
        public int Value { get; set; }

        public Point Position { get; set; }

        public Node ParentNode { get; set; }

        public List<Node> Edges { get; set; }

        public Node(int value, Point position) 
        {
            Value = value;
            Position = position;
            Edges = new List<Node>();
        }
    }
}
