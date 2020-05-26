
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ArchitectureReconstructionPresentation.Models;
using LibGit2Sharp;
using Microsoft.AspNetCore.Components;

namespace ArchitectureReconstructionPresentation.Services
{
    public static class EvolutionAnalysis
    {
        private static Dictionary<string, int> filesFrequency = new Dictionary<string, int>();

        public static Data Timeline()
        {
            var nodes = new List<Nodes>();
            var edges = new List<Edge>();
            var random = new Random();
            DataGathering dg = new DataGathering();
            using (var repo = new Repository("/Users/bjergfelt/Documents/GitHub/ITU/ArchReconstrution/src/ArchitectureReconstruction/Blazorise"))
            {
                
                foreach (var log in repo.Commits.QueryBy(new CommitFilter() { SortBy = CommitSortStrategies.Time }))
                {
                    
                    if (log.Parents.Any())
                    {
                        var oldTree = log.Parents.First().Tree;
                        var changes = repo.Diff.Compare<TreeChanges>(oldTree, log.Tree);
                        foreach (var change in changes)
                        {
                            if (change.Path.EndsWith(".cs"))
                            {
                                UpdateFileFrequency(change.Path);    
                            }
                            
                        }
                    }
                    else
                    {
                        foreach (var entry in log.Tree)
                        {
                            if (entry.Path.EndsWith(".cs"))
                            {
                                CountUpdatedFilesFrequency(entry);
                            }
                            
                        }
                    }
                }
            }

            foreach (var f in filesFrequency.Select(f => new { Frequency = f.Value, File = f.Key })
                .OrderByDescending(f => f.Frequency).Take(15))
            {
                var m = string.Empty;

                if (new FileInfo(f.File.Insert(0,"Blazorise/")).Exists)
                {
                    m = dg.moduleFromFilePath(new FileInfo(f.File.Insert(0,"Blazorise/")));
                    foreach (var each in dg.extractImports(new FileInfo(f.File.Insert(0,"Blazorise/"))).FindAll(x => !x.Contains("System")))
                    {
                       
                        if (each.Contains(Dependency.COMPONENTS) || each.Contains(Dependency.JS) || each.Contains(Dependency.BLAZOR_COMPONENTS))
                        {
                            //nodes.Add(new Nodes(m, redY , redX, string.Empty, new Style(fill: "#FFFFFF"), "#FF0000"));
                            nodes.Add(new Nodes("hej", random.Next(100), random.Next(100), $"{m} with frequency: {f.Frequency}",350, new Style(fill: "#FFFFFF"), "#FF0000"));
                            break;
                        }

                        else if (each.Contains(Dependency.NETCORE))
                        {
                            Console.WriteLine($"found: {f.File}");
                            //nodes.Add(new Nodes("Hej", 50, 50, $"{m} \n with frequency: {f.Frequency}", 300, new Style(fill: "#FFFFFF"), "#0000FF"));
                            break;
                        }
                    
                    }
                }
                   
                

                Console.WriteLine($"{f.Frequency}\t{f.File}");
                
               
            }
            
            var data = new Data(nodes, edges);
            // var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions(){WriteIndented = true});
            
            // File.WriteAllText("wwwroot//allresults.json", jsonData);
           // Console.WriteLine(fileList.Count());
            return data;
        }
        
        private static void CountUpdatedFilesFrequency(TreeEntry file)
        {
            if (file.Mode == Mode.Directory)
            {
                foreach (var child in (file.Target as Tree))
                {
                    CountUpdatedFilesFrequency(child);
                }
            }
            else
                UpdateFileFrequency(file.Path);
        }
        private static void UpdateFileFrequency(string file)
        {
            int frequency = 0;
            filesFrequency.TryGetValue(file, out frequency);
            frequency++;
            filesFrequency[file] = frequency;
        }


        public static void ResetToState(Repository repo, int year, int month)
        {
                 Commit c = repo.Commits.Last();

                //Commit c = repo.Commits.Last(x => x.Author.When.Year == year && x.Author.When.Month == month);
            
                Console.WriteLine($"Author: {c.Author.Name} <{c.Author.Email}>");
                Console.WriteLine("Date:   {0}", c.Author.When.ToString());
                Console.WriteLine();
                Console.WriteLine(c.Message);
                Console.WriteLine();
                
                repo.Reset(ResetMode.Hard, c);

                //
                Console.WriteLine(repo.Head.Tip);
                Console.WriteLine();
        }


            //repo.Head.Commits.Select(x => x.)
            // Commit currentCommit = repo.Head.Tip;
            //repo.Reset(ResetMode.Mixed, currentCommit);
            
        
        
    }
}