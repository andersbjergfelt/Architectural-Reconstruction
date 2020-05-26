using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using ArchitectureReconstructionPresentation.Models;
using LibGit2Sharp;

namespace ArchitectureReconstructionPresentation.Services
{
   public class DataGathering
    {
        
        /*
        
            var startFolder = @"/Users/bjergfelt/Documents/GitHub/ITU/ArchReconstrution/src/Blazorise-master";

            var dir = new DirectoryInfo(startFolder);  

            IEnumerable<FileInfo> fileList = dir.GetFiles("*.cs", SearchOption.AllDirectories);
            
            
            /*
             *
             * 
            foreach (var str in fileList)
            {
                foreach (var info in extractImports(str))
                {
                    Console.WriteLine(info);
                }
            }
             

            foreach (var str in fileList)
            {
                foreach (var module in moduleFromFilePath(str))
                {
                    Console.WriteLine(module);
                }
            }
            
            /*
             *
            foreach(KeyValuePair<string, int> kvp in LOC(startFolder))
            {
                
                Console.WriteLine("Key = {0}, Value = {1}",
                    kvp.Key, kvp.Value);
            }
             * 
             */
        
        

        private List<KeyValuePair<string, int>> LOC(string folder)
        {
            FileInfo[] csFiles = new DirectoryInfo(folder.Trim()).GetFiles("*.razor", SearchOption.AllDirectories);
            List<KeyValuePair<string, int>> listOfKeyValuePairs = new List<KeyValuePair<string, int>>();

            foreach (var fo in csFiles)
            {
                
                listOfKeyValuePairs.Add( NumberOfLines(fo));
            };
            
            listOfKeyValuePairs.Sort((x,y) => (y.Value.CompareTo(x.Value)));
            
            return listOfKeyValuePairs;
        }

        private List<KeyValuePair<string, int>> NOM(string folder)
        {
            FileInfo[] csFiles = (new DirectoryInfo(folder.Trim()).GetFiles("*.cs", SearchOption.AllDirectories))
                .Concat(new DirectoryInfo(folder.Trim()).GetFiles("*.blazor", SearchOption.AllDirectories)).ToArray();
            List<KeyValuePair<string, int>> listOfKeyValuePairs = new List<KeyValuePair<string, int>>();
            
            foreach (var fo in csFiles)
            {
                listOfKeyValuePairs.Add( NumberOfMethods(fo));
            };
            
            listOfKeyValuePairs.Sort((x,y) => (y.Value.CompareTo(x.Value)));
            
            return listOfKeyValuePairs;
        }

        private KeyValuePair<string, int> NumberOfMethods(FileInfo fo)
        {
            var count = 0;
            
            using var sr = fo.OpenText();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                //if (line.Trim().StartsWith("public") || line.Trim().StartsWith("private") || line.Trim().StartsWith("protected"))
                if(line.Trim().StartsWith("return"))
                {
                    count++;
               
                }
            }

            KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>(fo.FullName, count);

            return keyValuePair;
        }

        private KeyValuePair<string, int> NumberOfLines(FileInfo fi)
        {
            var count = 0;
            
            using var sr = fi.OpenText();
            
            while ((sr.ReadLine()) != null)
            {
                count++;
            }

            KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>(fi.FullName, count);

            return keyValuePair;
        }

        
        private List<string> Imports(string folder)
        {
            FileInfo[] csFiles = new DirectoryInfo(folder.Trim()).GetFiles("*.cs", SearchOption.AllDirectories);
            List<string> listOfKeyValuePairs = new List<string>();

          
            
            return listOfKeyValuePairs;
        }
        
        private string module (FileInfo fileInfo)
        {
            return moduleFromFilePath(fileInfo);
        }
        public List<string> extractImports(FileInfo fi)
        {
            List<string> imports = new List<string>();         
            using var sr = fi.OpenText();
            string line;
            string importString; 
            while ((line = sr.ReadLine()) != null)
            {
                //if (line.Trim().StartsWith("public") || line.Trim().StartsWith("private") || line.Trim().StartsWith("protected"))
                if(line.Trim().StartsWith("using") || line.Trim().StartsWith("@using"))
                {
                    importString = line.Replace("@using", "").Replace("using", "").Replace(";", "").Trim();

                    imports.Add(importString);
                }
            }
            return imports;
        }


        

        public string moduleFromFilePath(FileInfo fileInfo)
        {
            StringBuilder sb = new StringBuilder();
            if (fileInfo.Exists)
            {
                StreamReader sr = new StreamReader(fileInfo.FullName);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if(line.Trim().StartsWith("namespace"))
                    {
                        sb.Append(line.Replace("namespace", ""));
                        sb.Append($".{fileInfo.Name}");
                        sb.Replace(".cs", "");
                        sb.Replace(".razor", "");
                        sb.Replace(" ", "");

                    }
                }
            }
            return sb.ToString();
        }


        public void dependenciesGraph()
        {    
            var nodes = new List<Nodes>();
            var edges = new List<Edge>();
            var random = new Random();
            IEnumerable<FileInfo> fileList = new DirectoryInfo("/Users/bjergfelt/Documents/GitHub/ITU/Architectural-Reconstruction/Blazorise").GetFiles("*.cs",SearchOption.AllDirectories).ToArray();

            
            var count = 0;
            var redX = 0;
            var redY = 0;
            var blueY = 0;
            var blueX = 0;
            var innerCount = 0;
            
            foreach (var file in fileList)
            {   
               /*
                * 
                */
                if (count % 50 == 0)
                {
                    blueX = 50;
                    redX = 50 ;
                    redY += 100;
                    blueY += 100;
                }
                
                var m = moduleFromFilePath(file);
                if (!m.Equals(string.Empty) && !nodes.Exists(x => x.id.Equals(m)))
                {
                    foreach (var each in extractImports(file).FindAll(x => !x.Contains("System")))
                    {
                       
                        if (each.Contains(Dependency.COMPONENTS) || each.Contains(Dependency.JS))
                        {
                            redX += 70;
                            //nodes.Add(new Nodes(m, redY , redX, string.Empty, new Style(fill: "#FFFFFF"), "#FF0000"));
                            break;
                        }
                        else if (each.Contains(Dependency.NETCORE))
                        {
                            blueX += 70;
                            //nodes.Add(new Nodes(m, blueY, blueX, string.Empty, new Style(fill: "#00008B"), "#00008B"));
                            break;
                        }

                        innerCount++;
                    }
                }
                
                count++;
            }
            
            var data = new Data(nodes, edges);
            var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions(){WriteIndented = true});
            
            File.WriteAllText("wwwroot//allresults.json", jsonData);
        }
        
        public Data dependenciesGraphWithSize()
        {    
           // EvolutionAnalysis.ResetToState(new Repository("/Users/bjergfelt/Documents/GitHub/ITU/Architectural-Reconstruction/Blazorise"),0,0);
           // ResetToState()
            var nodes = new List<Nodes>();
            var edges = new List<Edge>();
            IEnumerable<FileInfo> fileList = new DirectoryInfo("/Users/bjergfelt/Documents/GitHub/ITU/Architectural-Reconstruction/Blazorise").GetFiles("*.cs",SearchOption.AllDirectories).ToArray();
            
            var blazorCount = 0;
            var redX = 0;
            var redY = 0;
            var blueY = 0;
            var blueX = 0;
            var dotNetCount = 0;
            
            foreach (var file in fileList)
            {
                var m = moduleFromFilePath(file);
                if (!m.Equals(string.Empty) && !nodes.Exists(x => x.id.Equals(m)))
                {
                    foreach (var each in extractImports(file).FindAll(x => !x.Contains("System")))
                    {
                       
                        if (each.Contains(Dependency.COMPONENTS) || each.Contains(Dependency.JS))
                        {
                            //redX += 70;
                            blazorCount++;
                         //   nodes.Add(new Nodes(m, redY , redX, string.Empty, new Style(fill: "#FFFFFF"), "#FF0000"));
                            break;
                        }
                        else if (each.Contains(Dependency.NETCORE))
                        {
                            //blueX += 70;
                            dotNetCount++;
                       //     nodes.Add(new Nodes(m, blueY, blueX, string.Empty, new Style(fill: "#00008B"), "#00008B"));
                            break;
                        }

                    }
                }
            }
            nodes.Add(new Nodes("hej", blueY, blueX, $"Blazor. Size: {blazorCount}",blazorCount, new Style(fill: "#FFFFFF"), "#FF0000"));
            nodes.Add(new Nodes("dotnet", blueY, blueX, $".NET CORE. Size: {dotNetCount}",dotNetCount, new Style(fill: "#FFFFFF"), "#00008B"));
            var data = new Data(nodes, edges);
            return data;
        }
        
        
        
        public void GenerateResultFile()
        {
            try
            {
                // No need to check if it exists.
                // If the folder does not exist yet, it will be created
                // If the folder exists already, the line will be ignored.
                using (new FileStream("wwwroot//allresults.json", FileMode.CreateNew))
                {
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
