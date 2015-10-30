using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskThreeDependeciesResolving
{
    class Dependencies
    {
        private static readonly HashSet<string> Visited = new HashSet<string>();
        private static HashSet<string> IncomingEdges = new HashSet<string>();
        private static Dictionary<string, List<string>> packs = new Dictionary<string, List<string>>();
        private static List<string> allKeys = new List<string>();
        private static Stack<string> installationOrder = new Stack<string>();
        static void Main(string[] args)
        {

            string GitHubAllPackages = "https://raw.githubusercontent.com/HackBulgaria/ApplicationFall2015/master/3-Dependencies-Resolving/all_packages.json";
            string GitHubDependecies = "https://raw.githubusercontent.com/HackBulgaria/ApplicationFall2015/master/3-Dependencies-Resolving/dependencies.json";

            // download and read json files
            WebClient wc = new System.Net.WebClient();
            string allPakcagesString = wc.DownloadString(GitHubAllPackages);
            string dependeciesAsString = wc.DownloadString(GitHubDependecies);

            // create Directory installed_modules in .exe dir and add installed packages there
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            Directory.CreateDirectory(exePath + "installed_modules");
            var localInstalledPackages = Directory.GetDirectories(exePath + "installed_modules");
            var installedPackages = new HashSet<string>();
            foreach (var dir in localInstalledPackages) // get all previously installed packages from installed_modules, if they exist
            {
                string folderName = dir.Split(new char[] { System.IO.Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).Last();
                installedPackages.Add(folderName);
            }

            // get which dependecy have to be installed from dependecies.js [works with only one element to download]
            var splittedDependecy = dependeciesAsString.Split(new char[] { '{', '\\', '}', '\n', }
               , StringSplitOptions.RemoveEmptyEntries);

            string packageToInstall = //"jQuery";
            splittedDependecy[1].Split(new char[] { '"', ':', '[', ']', ',', ' ' }
                , StringSplitOptions.RemoveEmptyEntries)[1]; // this is desired dependecy which will be given in DFS();


            // start transforming all_packages.json into Dictionary (oriented Graph)
            var splittedPackages = allPakcagesString.Split(new char[] { '{', '\\', '}', '\n', }
               , StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < splittedPackages.Length; i++)
            {
                var splitPair = splittedPackages[i].Split(new char[] { '"', ':', '[', ']', ',', ' ' } //[0] parent, rest - children
                    , StringSplitOptions.RemoveEmptyEntries);

                string parent = splitPair[0];
                List<string> children = new List<string>();

                if (splitPair.Length != 1)
                {
                    for (int j = 1; j < splitPair.Length; j++)
                    {
                        string child = splitPair[j];
                        IncomingEdges.Add(child); // all vertices with incoming edges
                        children.Add(child);
                    }
                }

                packs.Add(parent, children); // add element with it's children to Dictionary
            }

            if (installedPackages.Contains(packageToInstall)) // desired package already exists in installed_modules
            {
                Console.WriteLine("{0} is already installed", packageToInstall);
                return;

            }
            DFS(packageToInstall);

            Console.WriteLine("----------------------");
            string el = "";
            string installed_modulesDir = exePath + "installed_modules";

            // pop and install needed packages in dependecy order
            // while checking if they already exist in installed_modules 
            while (installationOrder.Count != 0)
            {
                el = installationOrder.Pop();
                if (installedPackages.Contains(el))
                {
                    Console.WriteLine("{0} is already installed on this machine", el);
                    continue;
                }
                // if package folder doesn't exist in installed_modules, create one 
                if (!Directory.Exists(installed_modulesDir + "/" + el))
                {
                    Directory.CreateDirectory(installed_modulesDir + "/" + el);
                }

                if (el != packageToInstall)
                {
                    Console.WriteLine("{0} installed", el);
                }
                else
                {
                    Console.WriteLine("{0} installed", el);
                    break;
                }
            }

            Console.WriteLine("{0} installed", packageToInstall);
            Directory.CreateDirectory(installed_modulesDir + "/" + packageToInstall);
            Console.WriteLine("----------------------");
        }


        public static void DFS(string node) // ready DFS from google, same result as TopologicalSort()
        {
            var nodes = new Stack<string>();
            nodes.Push(node);
            Visited.Add(node);

            while (nodes.Count != 0)
            {
                string currentNode = nodes.Pop();
                // allKeys.Add(currentNode); // go through all Graph and add all keys, for the Dictionary iteration in TopologicalSort()

                var list = packs[currentNode];
                if (list.Count != 0)
                {
                    Console.Write("In order to install {0} we need ", currentNode);

                    Console.WriteLine(string.Join((" and "), list));
                }
                for (int i = 0; i < packs[currentNode].Count; i++)
                {
                    if (!Visited.Contains(packs[currentNode][i]))
                    {
                        nodes.Push(packs[currentNode][i]);
                        installationOrder.Push(packs[currentNode][i]);

                        Visited.Add(packs[currentNode][i]);
                    }
                }
            }
        }

        //public static void TopologicalSort() // ownt try from pseudocode -> https://courses.cs.washington.edu/courses/cse326/03wi/lectures/RaoLect20.pdf
        //{

        //    for (int i = 0; i < allKeys.Count; i++)
        //    {
        //        var keyStr = allKeys[i];
        //        if (packs[keyStr].Count != 0)
        //            Console.Write("In order to install {0} we need ", keyStr);
        //        if (!IncomingEdges.Contains(keyStr))
        //        {
        //            installationOrder.Push(keyStr);

        //            foreach (var item in packs[keyStr])
        //            {
        //                if (packs[keyStr].Count != 0)
        //                {
        //                    Console.Write("{0}, ", item);
        //                }
        //                IncomingEdges.Remove(item);
        //            }
        //            Console.WriteLine();

        //            packs.Remove(keyStr);
        //            allKeys.Remove(keyStr);

        //            TopologicalSort();
        //        }
        //    }
        //}

    }
}