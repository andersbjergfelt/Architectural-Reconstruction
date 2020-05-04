using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using ArchitectureReconstructionPresentation.Models;

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
        private List<string> extractImports(FileInfo fi)
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


        

        private string moduleFromFilePath(FileInfo fileInfo)
        {
            StringBuilder sb = new StringBuilder();
            using var sr = fileInfo.OpenText();
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
            return sb.ToString();
        }


        public void dependenciesGraph()
        {    
            var nodes = new List<Nodes>();
            var edges = new List<Edge>();
            var random = new Random();
            IEnumerable<FileInfo> fileList = new DirectoryInfo("/Users/bjergfelt/Documents/GitHub/ITU/ArchReconstrution/src/Blazorise").GetFiles("*.cs",SearchOption.AllDirectories).ToArray();

            foreach (var file in fileList)
            {
                var m = moduleFromFilePath(file);
                if (!m.Equals(string.Empty) && !nodes.Exists(x => x.id.Equals(m)))
                {
                    foreach (var each in extractImports(file).FindAll(x => !x.Contains("System")))
                    {
                        if (each.Contains(Dependency.COMPONENTS) || each.Contains(Dependency.JS) || each.Contains(Dependency.BLAZOR_COMPONENTS))
                        {
                            nodes.Add(new Nodes(m, random.Next(1000) , random.Next(1700), m, new Style(fill: "#FFFFFF"), "#FF0000"));
                            break;
                        }
                        else if (each.Contains(Dependency.NETCORE))
                        {
                            nodes.Add(new Nodes(m, random.Next(1000) , random.Next(1700), m, new Style(fill: "#FFFFFF"), "#0000FF"));
                            break;
                        }
                    }
                }
            }
            
            var data = new Data(nodes, edges);
            var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions(){WriteIndented = true});
            
            File.WriteAllText("wwwroot//allresults.json", jsonData);
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
