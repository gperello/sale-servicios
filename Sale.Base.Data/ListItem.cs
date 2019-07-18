namespace Sale.Base.Data
{
    public class ListItem
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class ListItemSelecteable : ListItem
    {
        public bool selected { get; set; }
    }
}
