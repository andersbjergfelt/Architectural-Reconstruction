namespace ArchitectureReconstructionPresentation.Models
{
    public struct Edge
    {
        public string id { get; set; }

        public string source { get; set; }

        public string target { get; set; }

        public Edge(string id, string source, string target)
        {
            this.id = id;
            this.source = source;
            this.target = target;
        }

    }
}