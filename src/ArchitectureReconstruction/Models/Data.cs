using System.Collections.Generic;

namespace ArchitectureReconstructionPresentation.Models
{
    public struct Data
    {
        public List<Nodes> nodes { get; }

        public List<Edge> edges { get; }

        public Data(List<Nodes> nodes, List<Edge> edges)
        {
            this.edges = edges;
            this.nodes = nodes;
        }
        
        
    }
}