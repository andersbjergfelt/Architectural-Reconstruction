namespace ArchitectureReconstructionPresentation.Models
{
    public struct Nodes
    {
        public string id { get; set; }
        
        public string label { get; set; }

        public int y { get; set; }

        public int x { get; set; }
        
        public Style style { get; set; }
        
        public string color { get; set; }
        

        
        public Nodes(string id, int y, int x, string label, Style style, string color)
        {
            this.id = id;
            this.label = label;
            this.y = y;
            this.x = x;
            this.style = style;
            this.color = color;
        }
    }
}