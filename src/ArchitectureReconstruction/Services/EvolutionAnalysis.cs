
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LibGit2Sharp;

namespace ArchitectureReconstructionPresentation.Services
{
    public static class EvolutionAnalysis
    {
        private static Dictionary<string, int> filesFrequency = new Dictionary<string, int>();

        public static void Timeline()
        {
            using (var repo = new Repository("/Users/bjergfelt/Documents/GitHub/ITU/ArchReconstrution/src/Blazorise"))
            {
                foreach (var log in repo.Commits.QueryBy(new CommitFilter() { SortBy = CommitSortStrategies.Time }))
                {
                    if (log.Parents.Any())
                    {
                        var oldTree = log.Parents.First().Tree;
                        var changes = repo.Diff.Compare<TreeChanges>(oldTree, log.Tree);
                        foreach (var change in changes)
                        {
                            UpdateFileFrequency(change.Path);
                        }
                    }
                    else
                    {
                        foreach (var entry in log.Tree)
                        {
                            CountUpdatedFilesFrequency(entry);
                        }
                    }
                }
            }

            foreach (var f in filesFrequency.Select(f => new { Frequency = f.Value, File = f.Key })
                .OrderByDescending(f => f.Frequency))
            {
                Console.WriteLine($"{f.Frequency}\t{f.File}");
            }
            
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


        public static void ResetToState(Repository repo)
        {
            //Date
            

            //    Commit c = repo.Commits.Last();

                Commit c = repo.Commits.Last(x => x.Author.When.Year == 2019 && x.Author.When.Month == 6);
                
            

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