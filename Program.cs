using System;
using System.Collections.Generic;
using System.Linq;

namespace EaseTechChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter file path and press enter key: ");
            string mapPathFile = Console.ReadLine();
            Console.WriteLine("");

            ExecuteForMultipleNodes(mapPathFile);

            Console.ReadLine();
        }

        static void ExecuteForMultipleNodes(string mapPathFile)
        {
            var mapBuilder = new GraphFileBuilder(mapPathFile, " ");
            mapBuilder.BuildMapFromFile();

            var graphMapManager = new GraphMapManager();
            var path = graphMapManager.GetLongestMapPathFromNodes(mapBuilder.Nodes);

            PrintExecutionData(graphMapManager, path);
        }

        static void ExecuteForSingleNode(string mapPathFile)
        {
            var mapMapper = new GraphFileBuilder(mapPathFile, " ");
            mapMapper.BuildMapFromFile();

            var graphMapManager = new GraphMapManager();
            var path = graphMapManager.GetLongestMapPathFromNode(
                mapMapper.Nodes.First(x => x.Position.X.Equals(999) &&
                x.Position.Y.Equals(999)), true);

            PrintExecutionData(graphMapManager, path);
        }

        static void ExecuteForMaximumNodes(string mapPathFile)
        {
            var mapBuilder = new GraphFileBuilder(mapPathFile, " ");
            mapBuilder.BuildMapWithMaximumNodesFromFile();

            var graphMapManager = new GraphMapManager();
            var path = graphMapManager.GetLongestMapPathFromNodes(
                mapBuilder.MaximumNodes);

            PrintExecutionData(graphMapManager, path);
        }

        static void PrintExecutionData(GraphMapManager graphMapManager, Stack<Node> path) 
        {
            Console.WriteLine("");
            Console.WriteLine("********* EXECUTION DATA *********");
            Console.WriteLine($"Length of calculated path: {path.Count}");
            Console.WriteLine($"Drop of calculated path: {graphMapManager.GetCalculatedDropPath(path)}");
            Console.WriteLine($"Calculated path: {graphMapManager.GetCalculatedPath(path)}");
        }
    }
}